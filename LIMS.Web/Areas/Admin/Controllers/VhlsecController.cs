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
using LIMS.Web.Areas.Admin.Extensions.Mapping;
using LIMS.Web.Areas.Admin.Extensions;
using LIMS.Web.Areas.Admin.Helper;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class VhlsecController : BaseAdminController
    {
        private readonly IVhlsecService _vhlsecService;
        private readonly IDolfdService _dolfdService;
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

        public VhlsecController(IVhlsecService vhlsecService, ILanguageService languageService,
            ILocalizationService localizationService, IStoreService storeService,
            IWorkContext workContext, SeoSettings seoSettings,
            IDolfdService dolfdService, ICustomerService customerService,
            ICustomerViewModelService customerViewModelService, CustomerSettings customerSettings,
           ICustomerRegistrationService customerRegistrationService, IUserApiService userApiService,
           IEncryptionService encryptionService)
        {
            _vhlsecService = vhlsecService;
            _languageService = languageService;
            _localizationService = localizationService;
            _storeService = storeService;
            _workContext = workContext;
            _seoSettings = seoSettings;
            _dolfdService = dolfdService;
            _customerService = customerService;
            _customerViewModelService = customerViewModelService;
            _customerService = customerService;
            _customerRegistrationService = customerRegistrationService;
            _customerSettings = customerSettings;
            _userApiService = userApiService;
            _encryptionService = encryptionService;
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
            var Vhlsec = await _vhlsecService.GetVhlsec(command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult {
                Data = Vhlsec,
                Total = Vhlsec.TotalCount
            };
            return Json(gridModel);
        }

        [PermissionAuthorizeAction(PermissionActionName.Create)]
        public async Task<IActionResult> Create()
        {
            List<SelectListItem> Dolfd = new SelectList(await _dolfdService.GetDolfd(), "Id", "NameEnglish").ToList();
            Dolfd.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.DolfdId = Dolfd;
            var provience = ProvinceHelper.GetProvince();
            provience.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.provience = provience;
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            VhlsecModel vhlsecModel = new VhlsecModel();
            return View(vhlsecModel);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(VhlsecModel model, bool continueEditing)
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
                CustomerRole role = new CustomerRole();
              if (model.Type=="VHLSEC")
                {
                     role = allCustomerRoles.Where(m => m.Name == "VhlsecAdmin").FirstOrDefault();

                }
              else if(model.Type== "Fisheries development center")
                {
                    role = allCustomerRoles.Where(m => m.Name == "FisheriesAdmin").FirstOrDefault();
                }
                else if (model.Type == "Livestock service training center")
                {
                    role = allCustomerRoles.Where(m => m.Name == "LivestockAdmin").FirstOrDefault();
                }
                else if (model.Type == "Soil and fertilizer testing laboratory")
                {
                    role = allCustomerRoles.Where(m => m.Name == "SoilAdmin").FirstOrDefault();
                }
                else if (model.Type == "Plant protection laboratory")
                {
                    role = allCustomerRoles.Where(m => m.Name == "PlantAdmin").FirstOrDefault();
                }
                else if (model.Type == "seed testing laboratory")
                {
                    role = allCustomerRoles.Where(m => m.Name == "PlantAdmin").FirstOrDefault();
                }
                else if (model.Type == "AKC")
                {
                    role = allCustomerRoles.Where(m => m.Name == "AKCAdmin").FirstOrDefault();
                }
                else if (model.Type == "Dry fruits development center")
                {
                    role = allCustomerRoles.Where(m => m.Name == "DryAdmin").FirstOrDefault();
                }
                else if (model.Type == "Vegetable jarmaplajm enhancement and seed production center")
                {
                    role = allCustomerRoles.Where(m => m.Name == "VegitableAdmin").FirstOrDefault();
                }
                else if (model.Type == "Agri business promotion support and training center")
                {
                    role = allCustomerRoles.Where(m => m.Name == "AgriAdmin").FirstOrDefault();
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
                    Vhlsec Vhlsec = model.ToEntity();
                    Vhlsec.Dolfd = await _dolfdService.GetDolfdById(model.DolfdId);
                    await _vhlsecService.InsertVhlsec(Vhlsec);
                    CustomerModel customerModel = new CustomerModel();
                    customerModel.Email = model.UserEmail;
                    customerModel.District = model.UserDistrict;
                    customerModel.Province = model.UserProvince;
                    customerModel.LocalLevel = model.UserLocalLevel;
                    customerModel.Ward = model.UserWard;
                    customerModel.Tole = model.UserTole;
                    customerModel.Position = model.Position;
                    customerModel.OrgAddress = model.LocalLevel + " " + model.District  ;
                    customerModel.OrgName = model.NameEnglish;
                    customerModel.Active = true;
                    customerModel.IDCardNo = model.IDCardNo;
                    customerModel.NameNepali = model.UserNameNepali;
                    customerModel.NameEnglish = model.UserNameEnglish;
                    customerModel.PhoneNo = model.PhoneNo;
                    customerModel.EntityId = Vhlsec.Id;
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

                    SuccessNotification("Vhlsec added successfully");
                    return continueEditing ? RedirectToAction("Edit", new { id = Vhlsec.Id }) : RedirectToAction("List");
                }
            }

            var Dolfd = new SelectList(await _dolfdService.GetDolfd(), "Id", "NameEnglish").ToList();
            Dolfd.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.DolfdId = Dolfd;
            var provience = ProvinceHelper.GetProvince();
            provience.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.provience = provience;
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Preview)]
        public async Task<IActionResult> Edit(string id)
        {
            var Vhlsec = await _vhlsecService.GetVhlsecById(id);
            if (Vhlsec == null)
                return RedirectToAction("List");
            VhlsecModel model = Vhlsec.ToModel();
            var customer = await _customerService.GetCustomerByEmail(Vhlsec.UserEmail);
            model.CustomerId = customer.Id;
            List<SelectListItem> Dolfd = new SelectList(await _dolfdService.GetDolfd(), "Id", "NameEnglish").ToList();
            Dolfd.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.DolfdId = Dolfd;
            var provience = ProvinceHelper.GetProvince();
            provience.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.provience = provience;
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            model.UserNameEnglish = customer.NameEnglish;
            model.UserNameNepali = customer.NameNepali;
            model.IDCardNo = customer.IDCardNo;
            model.Position = customer.Position;
            model.PhoneNo = customer.PhoneNo;
            model.UserProvince = customer.Province;
            model.UserDistrict = customer.District;
            model.UserLocalLevel = customer.LocalLevel;
            model.UserWard = customer.Ward;
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(VhlsecModel model, bool continueEditing)
        {
            var customer = await _customerService.GetCustomerById(model.CustomerId);
            string email = customer.Email;

            var allCustomerRoles = await _customerService.GetAllCustomerRoles(showHidden: true);
            var newCustomerRoles = new List<CustomerRole>();
            CustomerRole role = allCustomerRoles.Where(m => m.Name == "VhlsecAdmin").FirstOrDefault();

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
            customerModel.LocalLevel = model.UserLocalLevel;
            customerModel.Ward = model.UserWard;
            customerModel.Tole = model.UserTole;
            customerModel.Position = model.Position;
            customerModel.Active = true;
            customerModel.OrgAddress = model.LocalLevel+" "+ model.District ;
            customerModel.OrgName = model.NameEnglish;
            customerModel.IDCardNo = model.IDCardNo;
            customerModel.NameNepali = model.UserNameNepali;
            customerModel.NameEnglish = model.UserNameEnglish;
            customerModel.PhoneNo = model.PhoneNo;
            
            var Vhlsec = await _vhlsecService.GetVhlsecById(model.Id);
            if (Vhlsec == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                
                    customerModel.EntityId = Vhlsec.Id;
                
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
                Vhlsec m = model.ToEntity(Vhlsec);
                m.Dolfd = await _dolfdService.GetDolfdById(model.DolfdId);
                await _vhlsecService.UpdateVhlsec(m);
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
                SuccessNotification("Vhlsec updated");
                if (continueEditing)
                {
                    //selected tab
                    await SaveSelectedTabIndex();

                    return RedirectToAction("Edit", new { id = model.Id });
                }
                return RedirectToAction("List");
            }

            var Dolfd = new SelectList(await _dolfdService.GetDolfd(), "Id", "NameEnglish").ToList();
            Dolfd.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.DolfdId = Dolfd;
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
            var Vhlsec = await _vhlsecService.GetVhlsecById(id);
            if (Vhlsec == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                await _vhlsecService.DeleteVhlsec(Vhlsec);

                SuccessNotification(_localizationService.GetResource("Vhlsec deleted"));
                return RedirectToAction("List");
            }
            ErrorNotification(ModelState);
            return RedirectToAction("Edit", new { id = id });
        }
    }
}

