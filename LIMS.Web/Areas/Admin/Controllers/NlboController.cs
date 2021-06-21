using LIMS.Core;
using LIMS.Domain.Customers;
using LIMS.Domain.Seo;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Mvc.Filters;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Customers;
using LIMS.Services.Localization;
using LIMS.Services.MoAMAC;
using LIMS.Services.Security;
using LIMS.Services.Stores;
using LIMS.Web.Areas.Admin.Extensions;
using LIMS.Web.Areas.Admin.Extensions.Mapping;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Web.Areas.Admin.Interfaces;
using LIMS.Web.Areas.Admin.Models.Customers;
using LIMS.Web.Areas.Admin.Models.MoAMAC;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{

    public class NlboController : BaseAdminController
    {
        private readonly INlboService _nlboService;
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
        private readonly ICustomerRoleViewModelService _customerRoleViewModelService;
        private readonly IMoAMACService _moAMACService;
        public NlboController(
                INlboService nlboService,
                ILanguageService languageService,
                ILocalizationService localizationService,
                IStoreService storeService,
                IWorkContext workContext,
                SeoSettings seoSettings,
                IMoAMACService molmacService,
                ICustomerService customerService,
                ICustomerViewModelService customerViewModelService,
                CustomerSettings customerSettings,
               ICustomerRegistrationService customerRegistrationService,
               IUserApiService userApiService,
               IEncryptionService encryptionService,
               ICustomerRoleViewModelService customerRoleViewModelService
                )
        {
            _nlboService = nlboService;
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
            _userApiService = userApiService;
            _encryptionService = encryptionService;
            _customerRoleViewModelService = customerRoleViewModelService;
            _moAMACService = molmacService;
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
            var nlbo = await _nlboService.GetNlbo(command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult {
                Data = nlbo,
                Total = nlbo.TotalCount
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
            var moamac = new SelectList(await _moAMACService.GetMoAMAC(), "Id", "NameEnglish").ToList();
            moamac.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.MoamacId = moamac;
            NlboModel nlbo = new NlboModel();
            return View(nlbo);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(NlboModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrWhiteSpace(model.Email))
                {
                    var cust2 = await _customerService.GetCustomerByEmail(model.UserEmail);
                    if (cust2 != null)
                        ModelState.AddModelError("", _localizationService.GetResource("Admin.Nlbo.EmailExists"));
                }

                //validate customer roles
                var allCustomerRoles = await _customerService.GetAllCustomerRoles(showHidden: true);
                var newCustomerRoles = new List<CustomerRole>();
                var role = allCustomerRoles.Where(m => m.Name == "NlboAdmin").FirstOrDefault();
                try
                {
                    if (role == null)
                    {
                        CustomerRoleModel customerRole = new CustomerRoleModel();
                        customerRole.IsSystemRole = true;
                        customerRole.Name = "NlboAdmin";
                        customerRole.SystemName = "NlboAdmin";
                        customerRole.Active = true;
                        await _customerRoleViewModelService.InsertCustomerRoleModel(customerRole);
                    }
                }
                catch(Exception)
                {

                }
                newCustomerRoles.Add(role);
                newCustomerRoles.Add(allCustomerRoles.Where(m => m.Name == "Registered").FirstOrDefault());
                var customerRolesError = _customerViewModelService.ValidateCustomerRoles(newCustomerRoles);
                if (!string.IsNullOrEmpty(customerRolesError))
                {
                    ModelState.AddModelError("", customerRolesError);
                    ErrorNotification(customerRolesError, false);
                }

                if (ModelState.IsValid)
                {
                    var nlbo = model.ToEntity();
                    nlbo.Moald = await _moAMACService.GetMoAMACById(model.MOALDId);
                    await _nlboService.InsertNlbo(nlbo);
                    var customerModel = new CustomerModel();
                    customerModel.Email = model.UserEmail;
                    customerModel.District = model.UserDistrict;
                    customerModel.Province = model.UserProvince;
                    customerModel.Ward = model.UserWard;
                    customerModel.Tole = model.UserTole;
                    customerModel.Position = model.Position;
                    customerModel.IDCardNo = model.IDCardNo;
                    customerModel.Active = true;
                    customerModel.OrgAddress = model.District + model.Address;
                    customerModel.OrgName = model.NameEnglish;
                    customerModel.NameNepali = model.UserNameNepali;
                    customerModel.NameEnglish = model.UserNameEnglish;
                    customerModel.PhoneNo = model.PhoneNo;
                    
                    customerModel.EntityId = nlbo.Id;
                    var customer = await _customerViewModelService.InsertCustomerModel(customerModel);
                    foreach (var customerRole in newCustomerRoles)
                    {
                        //ensure that the current customer cannot add to "Administrators" system role if he's not an admin himself
                        if (customerRole.SystemName == SystemCustomerRoleNames.Administrators &&
                            !_workContext.CurrentCustomer.IsAdmin())
                            continue;

                        customer.CustomerRoles.Add(customerRole);
                        customerRole.CustomerId = customer.Id;

                        await _customerService.InsertCustomerRoleInCustomer(customerRole);
                    }
                    //password
                    if (!string.IsNullOrWhiteSpace(model.Password))
                    {
                        var changePassRequest = new ChangePasswordRequest(model.UserEmail, false, _customerSettings.DefaultPasswordFormat, model.Password);
                        var changePassResult = await _customerRegistrationService.ChangePassword(changePassRequest);
                        if (!changePassResult.Success)
                        {
                            foreach (var changePassError in changePassResult.Errors)
                                ErrorNotification(changePassError);
                        }
                    }
                    UserApiModel user = new UserApiModel();
                    user.Email = model.UserEmail;
                    user.Password = model.Password;
                    if (!string.IsNullOrEmpty(user.Password))
                    {
                        var userapi = user.ToEntity();
                        var keys = HashPassword(model.Password);
                        userapi.Password = keys.hashpassword;
                        userapi.PrivateKey = keys.privatekey;
                        userapi.IsActive = true;
                        await _userApiService.InsertUserApi(userapi);
                    }
                    SuccessNotification(_localizationService.GetResource("Admin.Nlbo.Added"));
                    return continueEditing ? RedirectToAction("Edit", new { id = nlbo.Id }) : RedirectToAction("List");
                }
            }
            var moamac = new SelectList(await _moAMACService.GetMoAMAC(), "Id", "NameEnglish").ToList();
            moamac.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.MoamacId = moamac;

            var provience = ProvinceHelper.GetProvince();
            provience.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.provience = provience;
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);

            return View(model);
        }


        [PermissionAuthorizeAction(PermissionActionName.Preview)]
        public async Task<IActionResult> Edit(string id)
        {
            var nlbo = await _nlboService.GetNlboById(id);
            if (nlbo == null)
                return RedirectToAction("List");
            var model = nlbo.ToModel();
            var customer = await _customerService.GetCustomerByEmail(nlbo.UserEmail);
            model.CustomerId = customer.Id;

            var provience = ProvinceHelper.GetProvince();
            var moamac = new SelectList(await _moAMACService.GetMoAMAC(), "Id", "NameEnglish").ToList();
            moamac.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.MoamacId = moamac;
            provience.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.provience = provience;
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            model.UserNameEnglish = customer.NameEnglish;
            model.UserNameNepali = customer.NameNepali;
            model.IDCardNo = customer.IDCardNo;
            model.PhoneNo = customer.PhoneNo;
            model.Position = customer.Position;
            model.UserProvince = customer.Province;
            model.UserDistrict = customer.District;
            model.LocalLevel = customer.LocalLevel;
            model.UserWard = customer.Ward;
            model.UserTole = customer.Tole;
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(NlboModel model, bool continueEditing)
        {
            var customer = await _customerService.GetCustomerById(model.CustomerId);
            string email = customer.Email;

            var allCustomerRoles = await _customerService.GetAllCustomerRoles(showHidden: true);
            var newCustomerRoles = new List<CustomerRole>();
            var role = allCustomerRoles.Where(m => m.Name == "NlboAdmin").FirstOrDefault();
            newCustomerRoles.Add(role);
            newCustomerRoles.Add(allCustomerRoles.Where(m => m.Name == "Registered").FirstOrDefault());
            var customerRolesError = _customerViewModelService.ValidateCustomerRoles(newCustomerRoles);
            if (!string.IsNullOrEmpty(customerRolesError))
            {
                ModelState.AddModelError("", customerRolesError);
                ErrorNotification(customerRolesError, false);
            }
            var customerModel = new CustomerModel();
            customerModel.Email = model.UserEmail;
            customerModel.District = model.UserDistrict;
            customerModel.Province = model.UserProvince;
            customerModel.Ward = model.UserWard;
            customerModel.Tole = model.UserTole;
            customerModel.Position = model.Position;
            customerModel.Active = true;
            customerModel.OrgAddress = model.Address;
            customerModel.OrgName = model.NameEnglish;
            customerModel.IDCardNo = model.IDCardNo;
            customerModel.NameNepali = model.UserNameEnglish;
            customerModel.NameEnglish = model.UserNameNepali;
            customerModel.PhoneNo = model.PhoneNo;
            var nlbo = await _nlboService.GetNlboById(model.Id);
            if (nlbo == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                if (customer.EntityId == null)
                {
                    customerModel.EntityId = nlbo.Id;
                }
                customer = await _customerViewModelService.UpdateCustomerModel(customer, customerModel);
                foreach (var customerRole in newCustomerRoles)
                {
                    //ensure that the current customer cannot add to "Administrators" system role if he's not an admin himself
                    if (customerRole.SystemName == SystemCustomerRoleNames.Administrators &&
                        !_workContext.CurrentCustomer.IsAdmin())
                        continue;

                    customer.CustomerRoles.Add(customerRole);
                    customerRole.CustomerId = customer.Id;

                    await _customerService.InsertCustomerRoleInCustomer(customerRole);
                }

                var m = model.ToEntity(nlbo);
                m.Moald = await _moAMACService.GetMoAMACById(model.MOALDId);
                await _nlboService.UpdateNlbo(m);
                if (!string.IsNullOrWhiteSpace(model.Password))
                {
                    var changePassRequest = new ChangePasswordRequest(model.UserEmail, false, _customerSettings.DefaultPasswordFormat, model.Password);
                    var changePassResult = await _customerRegistrationService.ChangePassword(changePassRequest);
                    if (!changePassResult.Success)
                    {
                        foreach (var changePassError in changePassResult.Errors)
                            ErrorNotification(changePassError);
                    }
                }
                UserApiModel user = new UserApiModel();
                var userapi = await _userApiService.GetUserByEmail(model.UserEmail);
                user.Email = model.UserEmail;
                user.Password = model.Password;
                if (!string.IsNullOrEmpty(user.Password))
                {
                    userapi = user.ToEntity(userapi);
                    var keys = HashPassword(model.Password);
                    userapi.Password = keys.hashpassword;
                    userapi.PrivateKey = keys.privatekey;
                    userapi.IsActive = true;
                    await _userApiService.UpdateUserApi(userapi);


                }
                SuccessNotification(_localizationService.GetResource("Admin.Nlbo.Updated"));
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
            var moamac = new SelectList(await _moAMACService.GetMoAMAC(), "Id", "NameEnglish").ToList();
            moamac.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.MoamacId = moamac;
            //If we got this far, something failed, redisplay form
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Delete)]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var nlbo = await _nlboService.GetNlboById(id);
            if (nlbo == null)
                //No blog post found with the specified id
                return RedirectToAction("List");
            if (ModelState.IsValid)
            {
                await _nlboService.DeleteNlbo(nlbo);
                SuccessNotification(_localizationService.GetResource("Admin.Nlbo.Deleted"));
                return RedirectToAction("List");
            }
            ErrorNotification(ModelState);
            return RedirectToAction("Edit", new { id = id });
        }
    }


}
