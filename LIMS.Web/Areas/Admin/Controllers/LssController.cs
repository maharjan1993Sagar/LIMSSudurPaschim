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
using LIMS.Web.Areas.Admin.Extensions.Mapping;
using LIMS.Web.Areas.Admin.Interfaces;
using LIMS.Web.Areas.Admin.Models.Customers;
using LIMS.Web.Areas.Admin.Models.MoAMAC;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LIMS.Domain.MoAMAC;
using LIMS.Web.Areas.Admin.Extensions;
using LIMS.Web.Areas.Admin.Helper;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class LssController : BaseAdminController
    {
        private readonly ILssService _lssService;
        private readonly IVhlsecService _VhlsecService;
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
        public LssController(ILssService lssService, ILanguageService languageService, ILocalizationService localizationService,
            IStoreService storeService, IWorkContext workContext, SeoSettings seoSettings,
            IVhlsecService VhlsecService, ICustomerService customerService, ICustomerViewModelService customerViewModelService,
            CustomerSettings customerSettings, ICustomerRegistrationService customerRegistrationService, IUserApiService userApiService, IEncryptionService encryptionService)
        {
            _lssService = lssService;
            _languageService = languageService;
            _localizationService = localizationService;
            _storeService = storeService;
            _workContext = workContext;
            _seoSettings = seoSettings;
            _VhlsecService = VhlsecService;
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
            var Lss = await _lssService.GetLss(command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult {
                Data = Lss,
                Total = Lss.TotalCount
            };
            return Json(gridModel);
        }

        [PermissionAuthorizeAction(PermissionActionName.Create)]
        public async Task<IActionResult> Create()
        {
            var vhlsec = new SelectList(await _VhlsecService.GetVhlsec(), "Id", "NameEnglish").ToList();
            vhlsec.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.VhlsecId = vhlsec;
            var provience = ProvinceHelper.GetProvince();
            provience.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.provience = provience;
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            LssModel lss = new LssModel();
            return View(lss);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(LssModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrWhiteSpace(model.UserEmail))
                {
                    var cust2 = await _customerService.GetCustomerByEmail(model.UserEmail);
                    if (cust2 != null)
                        ModelState.AddModelError("", "Email is already registered");
                }
                //validate customer roles
                var allCustomerRoles = await _customerService.GetAllCustomerRoles(showHidden: true);
                var newCustomerRoles = new List<CustomerRole>();
                CustomerRole role = allCustomerRoles.Where(m => m.Name == "LssAdmin").FirstOrDefault();

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
                    Lss Lss = model.ToEntity();
                    Lss.Vhlsec = await _VhlsecService.GetVhlsecById(model.VhlsecId);
                    await _lssService.InsertLss(Lss);
                    CustomerModel customerModel = new CustomerModel();
                    customerModel.Email = model.UserEmail;
                    customerModel.District = model.UserDistrict;
                    customerModel.Province = model.UserProvince;
                    customerModel.LocalLevel = model.UserLocalLevel;
                    customerModel.Ward = model.UserWard;
                    customerModel.Tole = model.UserTole;
                    customerModel.Position = model.Position;
                    customerModel.Active = true;
                    customerModel.OrgAddress = model.District +" "+ model.LocalLevel;
                    customerModel.OrgName = model.NameEnglish;
                    customerModel.IDCardNo = model.IDCardNo;
                    customerModel.NameNepali = model.UserNameNepali;
                    customerModel.NameEnglish = model.UserNameEnglish;
                    customerModel.Phone = model.PhoneNo;
                    customerModel.EntityId = Lss.Id;
                    customerModel.CreatedBy = _workContext.CurrentCustomer.Email;
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
                    SuccessNotification("Lss added successfully");

                    return continueEditing ? RedirectToAction("Edit", new { id = Lss.Id }) : RedirectToAction("List");
                }
            }
            List<SelectListItem> Vhlsec = new SelectList(await _VhlsecService.GetVhlsec(), "Id", "NameEnglish").ToList();
            Vhlsec.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.VhlsecId = Vhlsec;
            var provience = ProvinceHelper.GetProvince();
            provience.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.provience = provience;
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            
            return View(model);
        }


        [PermissionAuthorizeAction(PermissionActionName.Preview)]
        public async Task<IActionResult> Edit(string id)
        {
            var Lss = await _lssService.GetLssById(id);
            if (Lss == null)
                return RedirectToAction("List");
            LssModel model = Lss.ToModel();
            var customer = await _customerService.GetCustomerByEmail(Lss.UserEmail);
            model.CustomerId = customer.Id;
            List<SelectListItem> Vhlsec = new SelectList(await _VhlsecService.GetVhlsec(), "Id", "NameEnglish").ToList();
            Vhlsec.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.VhlsecId = Vhlsec;
            var provience = ProvinceHelper.GetProvince();
            provience.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.provience = provience;
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            model.UserNameEnglish = customer.NameEnglish;
            model.UserNameNepali = customer.NameNepali;
            model.IDCardNo = customer.IDCardNo;
            model.Position = customer.Position;
            model.PhoneNo = customer.PhoneNo;
            model.UserDistrict = customer.District;
            model.UserLocalLevel = customer.LocalLevel;
            model.UserProvince = customer.Province;
            model.UserWard= customer.Ward ;
            model.UserTole = customer.Tole;
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(LssModel model, bool continueEditing)
        {
            var customer = await _customerService.GetCustomerById(model.CustomerId);
            string email = customer.Email;

            var allCustomerRoles = await _customerService.GetAllCustomerRoles(showHidden: true);
            var newCustomerRoles = new List<CustomerRole>();
            CustomerRole role = allCustomerRoles.Where(m => m.Name == "LssAdmin").FirstOrDefault();

            newCustomerRoles.Add(role);
            newCustomerRoles.Add(allCustomerRoles.Where(m => m.Name == "Registered").FirstOrDefault());
            var customerRolesError = _customerViewModelService.ValidateCustomerRoles(newCustomerRoles);
            if (!string.IsNullOrEmpty(customerRolesError))
            {
                ModelState.AddModelError("", customerRolesError);
                ErrorNotification(customerRolesError, false);
            }
            CustomerModel customerModel = new CustomerModel();
            customerModel.Email = model.UserEmail;
            customerModel.District = model.UserDistrict;
            customerModel.Province = model.UserProvince;
            customerModel.Ward = model.UserWard;
            customerModel.Tole = model.UserTole;
            customerModel.Position = model.Position;
            customerModel.Active = true;
            customerModel.OrgAddress = model.District+" " + model.LocalLevel;
            customerModel.OrgName = model.NameEnglish;
            customerModel.IDCardNo = model.IDCardNo;
            customerModel.NameNepali = model.UserNameNepali;
            customerModel.NameEnglish = model.UserNameEnglish;
            customerModel.Phone = model.PhoneNo;
            
            var lss = await _lssService.GetLssById(model.Id);
            if (lss == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                if (customer.EntityId == null)
                {
                    customerModel.EntityId = lss.Id;
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
                Lss m = model.ToEntity(lss);
                m.Vhlsec = await _VhlsecService.GetVhlsecById(model.VhlsecId);
                await _lssService.UpdateLss(m);
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
                SuccessNotification("Lss updated");
                if (continueEditing)
                {
                    //selected tab
                    await SaveSelectedTabIndex();

                    return RedirectToAction("Edit", new { id = model.Id });
                }
                return RedirectToAction("List");
            }
            List<SelectListItem> Vhlsec = new SelectList(await _VhlsecService.GetVhlsec(), "Id", "NameEnglish").ToList();
            Vhlsec.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.VhlsecId = Vhlsec;
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
            var Lss = await _lssService.GetLssById(id);
            if (Lss == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                await _lssService.DeleteLss(Lss);

                SuccessNotification(_localizationService.GetResource("Lss deleted"));
                return RedirectToAction("List");
            }
            ErrorNotification(ModelState);
            return RedirectToAction("Edit", new { id = id });
        }
    }
}
