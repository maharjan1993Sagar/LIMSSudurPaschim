using LIMS.Domain.Customers;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Common;
using LIMS.Services.Customers;
using LIMS.Services.Directory;
using LIMS.Services.Helpers;
using LIMS.Services.Localization;
using LIMS.Services.Security;
using LIMS.Web.Areas.Admin.Models.Customers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    [PermissionAuthorize(PermissionSystemName.Customers)]
    public partial class OnlineCustomerController : BaseAdminController
    {
        #region Fields

        private readonly ICustomerService _customerService;
        private readonly IGeoLookupService _geoLookupService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly CustomerSettings _customerSettings;
        private readonly ILocalizationService _localizationService;

        #endregion

        #region Constructors

        public OnlineCustomerController(ICustomerService customerService,
            IGeoLookupService geoLookupService, IDateTimeHelper dateTimeHelper,
            CustomerSettings customerSettings,
            ILocalizationService localizationService)
        {
            _customerService = customerService;
            _geoLookupService = geoLookupService;
            _dateTimeHelper = dateTimeHelper;
            _customerSettings = customerSettings;
            _localizationService = localizationService;
        }

        #endregion

        #region Methods

        public IActionResult List() => View();

        [HttpPost]
        [PermissionAuthorizeAction(PermissionActionName.List)]
        public async Task<IActionResult> List(DataSourceRequest command)
        {
            var customers = await _customerService.GetOnlineCustomers(DateTime.UtcNow.AddMinutes(-_customerSettings.OnlineCustomerMinutes),
                null, command.Page - 1, command.PageSize);
            var items = new List<OnlineCustomerModel>();
            foreach (var x in customers)
            {
                var item = new OnlineCustomerModel() {
                    Id = x.Id,
                    CustomerInfo = !string.IsNullOrEmpty(x.Email) ? x.Email : _localizationService.GetResource("Admin.Customers.Guest"),
                    LastIpAddress = x.LastIpAddress,
                    Location = _geoLookupService.LookupCountryName(x.LastIpAddress),
                    LastActivityDate = _dateTimeHelper.ConvertToUserTime(x.LastActivityDateUtc, DateTimeKind.Utc),
                    LastVisitedPage = _customerSettings.StoreLastVisitedPage ?
                        x.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.LastVisitedPage) :
                        _localizationService.GetResource("Admin.Customers.OnlineCustomers.Fields.LastVisitedPage.Disabled")
                };
                items.Add(item);
            }

            var gridModel = new DataSourceResult {
                Data = items,
                Total = customers.TotalCount
            };

            return Json(gridModel);
        }

        #endregion
    }
}
