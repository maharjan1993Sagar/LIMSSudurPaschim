using LIMS.Core;
using LIMS.Domain.Customers;
using LIMS.Domain.Media;
using LIMS.Framework.Components;
using LIMS.Services.Common;
using LIMS.Services.Customers;
using LIMS.Services.Directory;
using LIMS.Services.Helpers;
using LIMS.Services.Localization;
using LIMS.Services.Media;
using LIMS.Web.Models.Profile;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LIMS.Web.ViewComponents
{
    public class ProfileInfoViewComponent : BaseViewComponent
    {
        private readonly ICustomerService _customerService;
        private readonly IWorkContext _workContext;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly CustomerSettings _customerSettings;
        private readonly IPictureService _pictureService;
        private readonly MediaSettings _mediaSettings;
        private readonly ICountryService _countryService;

        public ProfileInfoViewComponent(ICustomerService customerService, 
            IWorkContext workContext, IDateTimeHelper dateTimeHelper,
            CustomerSettings customerSettings,
            IPictureService pictureService, MediaSettings mediaSettings,
            ICountryService countryService)
        {
            _customerService = customerService;
            _workContext = workContext;
            _dateTimeHelper = dateTimeHelper;
            _customerSettings = customerSettings;
            _pictureService = pictureService;
            _mediaSettings = mediaSettings;
            _countryService = countryService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string customerProfileId)
        {
            var customer = await _customerService.GetCustomerById(customerProfileId);
            if (customer == null)
            {
                return Content("");
            }

            //avatar
            var avatarUrl = "";
            if (_customerSettings.AllowCustomersToUploadAvatars)
            {
                avatarUrl = await _pictureService.GetPictureUrl(
                 customer.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.AvatarPictureId),
                 _mediaSettings.AvatarPictureSize,
                 _customerSettings.DefaultAvatarEnabled,
                 defaultPictureType: PictureType.Avatar);
            }

            //location
            bool locationEnabled = false;
            string location = string.Empty;
            if (_customerSettings.ShowCustomersLocation)
            {
                locationEnabled = true;

                var countryId = customer.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.CountryId);
                var country = await _countryService.GetCountryById(countryId);
                if (country != null)
                {
                    location = country.GetLocalized(x => x.Name, _workContext.WorkingLanguage.Id);
                }
                else
                {
                    locationEnabled = false;
                }
            }

            //registration date
            bool joinDateEnabled = false;
            string joinDate = string.Empty;

            if (_customerSettings.ShowCustomersJoinDate)
            {
                joinDateEnabled = true;
                joinDate = _dateTimeHelper.ConvertToUserTime(customer.CreatedOnUtc, DateTimeKind.Utc).ToString("f");
            }

            //birth date
            bool dateOfBirthEnabled = false;
            string dateOfBirth = string.Empty;
            if (_customerSettings.DateOfBirthEnabled)
            {
                var dob = customer.GetAttributeFromEntity<DateTime?>(SystemCustomerAttributeNames.DateOfBirth);
                if (dob.HasValue)
                {
                    dateOfBirthEnabled = true;
                    dateOfBirth = dob.Value.ToString("D");
                }
            }

            var model = new ProfileInfoModel
            {
                CustomerProfileId = customer.Id,
                AvatarUrl = avatarUrl,
                LocationEnabled = locationEnabled,
                Location = location,
                JoinDateEnabled = joinDateEnabled,
                JoinDate = joinDate,
                DateOfBirthEnabled = dateOfBirthEnabled,
                DateOfBirth = dateOfBirth,
            };

            return View(model);
        }
    }
}