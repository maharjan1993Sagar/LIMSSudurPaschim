using LIMS.Core;
using LIMS.Domain;
using LIMS.Domain.Customers;
using LIMS.Domain.Seo;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Mvc.Filters;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Customers;
using LIMS.Services.Localization;
using LIMS.Services.Organizations;
using LIMS.Domain.Organizations;
using LIMS.Services.Security;
using LIMS.Services.Stores;
using LIMS.Web.Areas.Admin.Extensions;
using LIMS.Web.Areas.Admin.Interfaces;
using LIMS.Web.Areas.Admin.Models.Customers;
using LIMS.Web.Areas.Admin.Models.Organization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LIMS.Web.Areas.Admin.Helper;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class OrganizationController : BaseAdminController
    {
        #region fields
        private readonly IOrganizationService _organizationService;
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
        #endregion
        public OrganizationController(IOrganizationService organizationService, ILanguageService languageService, ILocalizationService localizationService,
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
        protected (string hashpassword, string privatekey) HashPassword(string password)
        {
            var pk = CommonHelper.GenerateRandomDigitCode(24);
            return (_encryptionService.EncryptText(password, pk), pk);
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

            var Organization = await _organizationService.GetOrganization(createdby, command.Page - 1, command.PageSize);
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
            provience.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.provience = provience;
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            OrganizationModel organization = new OrganizationModel();
            return View(organization);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(OrganizationModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                //if (!string.IsNullOrWhiteSpace(model.Email))
                //{
                //    var cust2 = await _customerService.GetCustomerByEmail(model.UserEmail);
                //    if (cust2 != null)
                //        ModelState.AddModelError("", "Email is already registered");
                //}
                //validate customer roles
                //var allCustomerRoles = await _customerService.GetAllCustomerRoles(showHidden: true);
                //var newCustomerRoles = new List<CustomerRole>();
                //CustomerRole role = allCustomerRoles.Where(m => m.Name == RoleHelper.OrganizationAdmin).FirstOrDefault();

                //newCustomerRoles.Add(role);
                //newCustomerRoles.Add(allCustomerRoles.Where(m => m.Name == "Registered").FirstOrDefault());
                //var customerRolesError = _customerViewModelService.ValidateCustomerRoles(newCustomerRoles);
                //if (!string.IsNullOrEmpty(customerRolesError))
                //{
                //    ModelState.AddModelError("", customerRolesError);
                //    ErrorNotification(customerRolesError, false);
                //}

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
                    Organization Organization = model.ToEntity();
                    Organization.CreatedBy = createdby;
                    await _organizationService.InsertOrganization(Organization);


                    SuccessNotification("Organization added successfully");

                    return continueEditing ? RedirectToAction("Edit", new { id = Organization.Id }) : RedirectToAction("List");
                }
            }

            var provience = ProvinceHelper.GetProvince();
            provience.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.provience = provience;
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);

            return View(model);
        }


        [PermissionAuthorizeAction(PermissionActionName.Preview)]
        public async Task<IActionResult> Edit(string id)
        {
            var Organization = await _organizationService.GetOrganizationById(id);
            if (Organization == null)
                return RedirectToAction("List");
            OrganizationModel model = Organization.ToModel();
            var customer = await _customerService.GetCustomerByEmail(Organization.UserEmail);
          //  model.CustomerId = customer.Id;
            var provience = ProvinceHelper.GetProvince();
            provience.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.provience = provience;
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            //model.UserNameEnglish = customer.NameEnglish;
            //model.UserNameNepali = customer.NameNepali;
            //model.IDCardNo = customer.IDCardNo;
            //model.Position = customer.Position;
            //model.PhoneNo = customer.PhoneNo;

            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(OrganizationModel model, bool continueEditing)
        {
            var organization = await _organizationService.GetOrganizationById(model.Id);
            if (organization == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {

                Organization m = model.ToEntity(organization);
                m.UpdatedBy = _workContext.CurrentCustomer.Email;
                await _organizationService.UpdateOrganization(m);
                //if (!string.IsNullOrWhiteSpace(model.Password))
                //{
                //    var changePassRequest = new ChangePasswordRequest(model.UserEmail, false, _customerSettings.DefaultPasswordFormat, model.Password);
                //    var changePassResult = await _customerRegistrationService.ChangePassword(changePassRequest);
                //    if (!changePassResult.Success)
                //    {
                //        foreach (var changePassError in changePassResult.Errors)
                //            ErrorNotification(changePassError);
                //    }
                //}
                //UserApiModel user = new UserApiModel();
                //var userapi = await _userApiService.GetUserByEmail(model.UserEmail);
                //user.Email = model.UserEmail;
                //user.Password = model.Password;
                //if (!string.IsNullOrEmpty(user.Password))
                //{
                //    userapi = user.ToEntity(userapi);
                //    var keys = HashPassword(model.Password);
                //    userapi.Password = keys.hashpassword;
                //    userapi.PrivateKey = keys.privatekey;
                //    userapi.IsActive = true;
                //    await _userApiService.UpdateUserApi(userapi);


                //}

                SuccessNotification("Organization updated");
                if (continueEditing)
                {
                    //selected tab
                    await SaveSelectedTabIndex();

                    return RedirectToAction("Edit", new { id = model.Id });
                }
                return RedirectToAction("List");
            }

            var provience = ProvinceHelper.GetProvince();
            provience.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.provience = provience;
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Delete)]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var Organization = await _organizationService.GetOrganizationById(id);
            if (Organization == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                await _organizationService.DeleteOrganization(Organization);

                SuccessNotification(_localizationService.GetResource("Organization deleted"));
                return RedirectToAction("List");
            }
            ErrorNotification(ModelState);
            return RedirectToAction("Edit", new { id = id });
        }

        public async Task<IActionResult> GetAllOrganization()
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
            var Organization = await _organizationService.GetOrganization(createdby);
           
            return Json(Organization);
        }
    } 
}
