using LIMS.Core;
using LIMS.Domain.Customers;
using LIMS.Domain.Seo;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Mvc.Filters;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Customers;
using LIMS.Services.Localization;
using LIMS.Services.OtherOrganizations;
using LIMS.Services.Security;
using LIMS.Services.Stores;
using LIMS.Web.Areas.Admin.Extensions;
using LIMS.Web.Areas.Admin.Extensions.Mapping;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Web.Areas.Admin.Interfaces;
using LIMS.Web.Areas.Admin.Models.Organization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class OtherOrganizationController:BaseAdminController
    {
        #region fields
        private readonly IOtherOrganizationService _organizationService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;
        private readonly SeoSettings _seoSettings;
        private readonly ICustomerService _customerService;
        private readonly ICustomerViewModelService _customerViewModelService;
        private readonly CustomerSettings _customerSettings;
        private readonly ICustomerRegistrationService _customerRegistrationService;
        private readonly IUserApiService _userApiService;
        private readonly IEncryptionService _encryptionService;

        public ILocalizationService LocalizationService => _localizationService;
        #endregion
        public OtherOrganizationController(IOtherOrganizationService organizationService, ILanguageService languageService, ILocalizationService localizationService,
            IStoreService storeService, IWorkContext workContext, SeoSettings seoSettings,
             ICustomerService customerService, ICustomerViewModelService customerViewModelService,
            CustomerSettings customerSettings, ICustomerRegistrationService customerRegistrationService, IUserApiService userApiService, IEncryptionService encryptionService)
        {
            _organizationService = organizationService;
            _languageService = languageService;
            _localizationService = localizationService;
            _storeService = storeService;
            _workContext = workContext;
            _seoSettings = seoSettings;
            _customerService = customerService;
            _customerViewModelService = customerViewModelService;
            _customerService = customerService;
            _customerRegistrationService = customerRegistrationService;
            _customerSettings = customerSettings;
            _encryptionService = encryptionService;
            _userApiService = userApiService;
        }
       
        public IActionResult Index() => RedirectToAction("List");

        public IActionResult List() => View();

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command)
        {
            string createdby = null;
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
            if (roles.Contains(RoleHelper.LssAdmin) || roles.Contains(RoleHelper.VhlsecAdmin) || roles.Contains(RoleHelper.DolfdAdmin))
            {
                createdby = _workContext.CurrentCustomer.Id;
            }
            else
            {
                string adminemail = _workContext.CurrentCustomer.CreatedBy;
                var admin = await _customerService.GetCustomerByEmail(adminemail);
                createdby = admin.Id;
            }

            var Organization = await _organizationService.GetOtherOrganization(createdby, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult {
                Data = Organization,
                Total = Organization.TotalCount
            };
            return Json(gridModel);
        }

        [PermissionAuthorizeAction(PermissionActionName.Create)]
        public async Task<IActionResult> Create()
        {

            var provience = ProvinceHelper.GetProvince();
            provience.Insert(0, new SelectListItem(LocalizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.provience = provience;
            var type = OrganizationTypeHelper.GetOrganizationType();
            type.Insert(0, new SelectListItem(LocalizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Type = type;
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            var organization = new OtherOrganizationModel();
            return View(organization);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(OtherOrganizationModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                
              
                if (ModelState.IsValid)
                {
                    string createdby = null;
                    List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
                    if (roles.Contains(RoleHelper.LssAdmin) || roles.Contains(RoleHelper.VhlsecAdmin) || roles.Contains(RoleHelper.DolfdAdmin))
                    {
                        createdby = _workContext.CurrentCustomer.Id;
                    }
                    else
                    {
                        string adminemail = _workContext.CurrentCustomer.CreatedBy;
                        var admin = await _customerService.GetCustomerByEmail(adminemail);
                        createdby = admin.Id;
                    }
                    var Organization = model.ToEntity();
                    Organization.CreatedBy = createdby;
                    await _organizationService.InsertOtherOrganization(Organization);


                    SuccessNotification("Organization added successfully");

                    return continueEditing ? RedirectToAction("Edit", new { id = Organization.Id }) : RedirectToAction("List");
                }
            }
            var type = OrganizationTypeHelper.GetOrganizationType();
            type.Insert(0, new SelectListItem(LocalizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Type = type;
            var provience = ProvinceHelper.GetProvince();
            provience.Insert(0, new SelectListItem(LocalizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.provience = provience;
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);

            return View(model);
        }


        [PermissionAuthorizeAction(PermissionActionName.Preview)]
        public async Task<IActionResult> Edit(string id)
        {
            var organization = await _organizationService.GetOtherOrganizationById(id);
            if (organization == null)
                return RedirectToAction("List");
            var model = organization.ToModel();
          
            var provience = ProvinceHelper.GetProvince();
            provience.Insert(0, new SelectListItem(LocalizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.provience = provience;
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            var type = OrganizationTypeHelper.GetOrganizationType();
            type.Insert(0, new SelectListItem(LocalizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Type = type;

            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(OtherOrganizationModel model, bool continueEditing)
        {
            var organization = await _organizationService.GetOtherOrganizationById(model.Id);
            if (organization == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {

                var m = model.ToEntity(organization);
                m.UpdatedBy = _workContext.CurrentCustomer.Email;
                await _organizationService.UpdateOtherOrganization(m);
               
                SuccessNotification("Organization updated");
                if (continueEditing)
                {
                    //selected tab
                    await SaveSelectedTabIndex();

                    return RedirectToAction("Edit", new { id = model.Id });
                }
                return RedirectToAction("List");
            }
            var type = OrganizationTypeHelper.GetOrganizationType();
            type.Insert(0, new SelectListItem(LocalizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Type = type;
            var provience = ProvinceHelper.GetProvince();
            provience.Insert(0, new SelectListItem(LocalizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.provience = provience;
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Delete)]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var Organization = await _organizationService.GetOtherOrganizationById(id);
            if (Organization == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                await _organizationService.DeleteOtherOrganization(Organization);

                SuccessNotification(LocalizationService.GetResource("Organization deleted"));
                return RedirectToAction("List");
            }
            ErrorNotification(ModelState);
            return RedirectToAction("Edit", new { id = id });
        }

    }
}
