using LIMS.Core;
using LIMS.Domain.Customers;
using LIMS.Domain.Seo;
using LIMS.Domain.Services;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Mvc.Filters;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Customers;
using LIMS.Services.Localization;
using LIMS.Services.LocalStructure;
using LIMS.Services.Security;
using LIMS.Services.Stores;
using LIMS.Services.User;
using LIMS.Web.Areas.Admin.Extensions;
using LIMS.Web.Areas.Admin.Extensions.Mapping;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Web.Areas.Admin.Interfaces;
using LIMS.Web.Areas.Admin.Models.Customers;
using LIMS.Web.Areas.Admin.Models.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class ServiceProviderController:BaseAdminController
    {
        #region fields
        private readonly IServiceProviderService _serviceProviderService;
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
        private readonly ILocalLevelService _localLevelService;

        #endregion
        public ServiceProviderController(IServiceProviderService serviceProviderService, ILanguageService languageService, ILocalizationService localizationService,
            IStoreService storeService, IWorkContext workContext, SeoSettings seoSettings,
             ICustomerService customerService, ICustomerViewModelService customerViewModelService,
            CustomerSettings customerSettings, ICustomerRegistrationService customerRegistrationService, IUserApiService userApiService, IEncryptionService encryptionService,
            ILocalLevelService localLevelService)
        {
            _serviceProviderService = serviceProviderService;
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
            _localLevelService = localLevelService;
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
            var createdby = _workContext.CurrentCustomer.Id;
            var ServiceProvider = await _serviceProviderService.GetServiceProvider(createdby, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult {
                Data = ServiceProvider,
                Total = ServiceProvider.TotalCount
            };
            return Json(gridModel);
        }

        [PermissionAuthorizeAction(PermissionActionName.Create)]
        public async Task<IActionResult> Create()
        {

            var provience = ProvinceHelper.GetProvince();
            provience.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.provience = provience;
            var serviceProviderType = new List<SelectListItem> {
                new SelectListItem { Text = "Vet-graduate", Value = "Vet-graduate" },
                new SelectListItem { Text = "Para-professional", Value = "Para-professional" },
            };
            serviceProviderType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            ViewBag.ServiceProviderType = serviceProviderType;
            var type = new List<SelectListItem> {
                new SelectListItem { Text = "Permanent", Value = "Permanent" },
                new SelectListItem { Text = "Temporary", Value = "Temporary" },
            };
            type.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            ViewBag.Type = type;

            var localLevels = await _localLevelService.GetLocalLevel("KATHMANDU");
            var localLevelSelect = new SelectList(localLevels).ToList();
            localLevelSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.LocalLevels = new SelectList(localLevelSelect, "Text","Text", ExecutionHelper.LocalLevel);

            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            ServiceProviderModel serviceProvider = new ServiceProviderModel();
            return View(serviceProvider);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(ServiceProviderModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrWhiteSpace(model.Email))
                {
                    var cust2 = await _customerService.GetCustomerByEmail(model.Email);
                    if (cust2 != null)
                        ModelState.AddModelError("", "Email is already registered");

                }
                //validate customer roles
                var allCustomerRoles = await _customerService.GetAllCustomerRoles(showHidden: true);
                var newCustomerRoles = new List<CustomerRole>();
                CustomerRole role = allCustomerRoles.Where(m => m.Name == RoleHelper.Technician).FirstOrDefault();

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
                    string entity = null;
                    if (model.IsPprs)
                    {
                        entity = "Nlbo";
                    }
                    else
                    {
                        entity = _workContext.CurrentCustomer.EntityId;
                    }

                        ServiceProvider serviceProvider = model.ToEntity();
                    serviceProvider.CreatedBy = _workContext.CurrentCustomer.Id;
                    await _serviceProviderService.InsertServiceProvider(serviceProvider);
                    CustomerModel customerModel = new CustomerModel();
                    customerModel.Email = model.Email;
                    customerModel.District = model.District;
                    customerModel.Province = model.Provience;
                    customerModel.Ward = model.Ward;
                    customerModel.Tole = model.Tole;
                    customerModel.Position = model.Designation;
                    customerModel.Active = true;
                    customerModel.OrgAddress = model.District + " " + model.LocalLevel;
                    customerModel.OrgName = model.NameEnglish;
                    customerModel.IDCardNo = model.CitizenshipNo;
                    customerModel.NameNepali = model.NameNepali;
                    customerModel.NameEnglish = model.NameNepali;
                    customerModel.Phone = model.MobileNo;
                    customerModel.CreatedBy = _workContext.CurrentCustomer.Email;
                    customerModel.Username = model.MobileNo;
                    customerModel.UsernamesEnabled = true;
                    customerModel.EntityId = entity;
                    customerModel.Active = model.IsActive;
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
                        var changePassRequest = new ChangePasswordRequest(model.Email, false, _customerSettings.DefaultPasswordFormat, model.Password);
                        var changePassResult = await _customerRegistrationService.ChangePassword(changePassRequest);
                        if (!changePassResult.Success)
                        {
                            foreach (var changePassError in changePassResult.Errors)
                                ErrorNotification(changePassError);
                        }
                    }
                    UserApiModel user = new UserApiModel();
                    user.Email = model.Email;
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
                    SuccessNotification("ServiceProvider added successfully");

                    return continueEditing ? RedirectToAction("Edit", new { id = serviceProvider.Id }) : RedirectToAction("List");
                }
            }
            var type = new List<SelectListItem> {
                new SelectListItem { Text = "Permanent", Value = "Permanent" },
                new SelectListItem { Text = "Temporary", Value = "Temporary" },
            };
            type.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            ViewBag.Type = type;
            var serviceProviderType = new List<SelectListItem> {
                new SelectListItem { Text = "Vet-graduate", Value = "Vet-graduate" },
                new SelectListItem { Text = "Para-professional", Value = "Para-professional" },
            };
            serviceProviderType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            ViewBag.ServiceProviderType = serviceProviderType;
            var provience = ProvinceHelper.GetProvince();
            provience.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.provience = provience;

            var localLevels = await _localLevelService.GetLocalLevel("KATHMANDU");
            var localLevelSelect = new SelectList(localLevels).ToList();
            localLevelSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.LocalLevels = new SelectList(localLevelSelect, "Text","Text", ExecutionHelper.LocalLevel);

            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);

            return View(model);
        }


        [PermissionAuthorizeAction(PermissionActionName.Preview)]
        public async Task<IActionResult> Edit(string id)
        {
            var ServiceProvider = await _serviceProviderService.GetServiceProviderById(id);
            if (ServiceProvider == null)
                return RedirectToAction("List");
            ServiceProviderModel model = ServiceProvider.ToModel();
            var customer = await _customerService.GetCustomerByEmail(ServiceProvider.Email);
            model.CustomerId = customer.Id;
            var provience = ProvinceHelper.GetProvince();
            provience.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.provience = provience;

            var localLevels = await _localLevelService.GetLocalLevel("KATHMANDU");
            var localLevelSelect = new SelectList(localLevels).ToList();
            localLevelSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.LocalLevels = new SelectList(localLevelSelect, "Text","Text", ExecutionHelper.LocalLevel);

            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            var serviceProviderType = new List<SelectListItem> {
                new SelectListItem { Text = "Vet-graduate", Value = "Vet-graduate" },
                new SelectListItem { Text = "Para-professional", Value = "Para-professional" },
            };
            serviceProviderType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            var type = new List<SelectListItem> {
                new SelectListItem { Text = "Permanent", Value = "Permanent" },
                new SelectListItem { Text = "Temporary", Value = "Temporary" },
            };
            type.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            ViewBag.Type = type;
            ViewBag.ServiceProviderType = serviceProviderType;
            model.IsActive = customer.Active;
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(ServiceProviderModel model, bool continueEditing)
        {
          
            string email = model.Email;
            var customer = await _customerService.GetCustomerByEmail(model.Email);
            var allCustomerRoles = await _customerService.GetAllCustomerRoles(showHidden: true);
            var newCustomerRoles = new List<CustomerRole>();
            CustomerRole role = allCustomerRoles.Where(m => m.Name == RoleHelper.Technician).FirstOrDefault();

            newCustomerRoles.Add(role);
            newCustomerRoles.Add(allCustomerRoles.Where(m => m.Name == "Registered").FirstOrDefault());
            var customerRolesError = _customerViewModelService.ValidateCustomerRoles(newCustomerRoles);
            if (!string.IsNullOrEmpty(customerRolesError))
            {
                ModelState.AddModelError("", customerRolesError);
                ErrorNotification(customerRolesError, false);
            }
            string entity = null;
            if (model.IsPprs)
            {
                entity = "Nlbo";
            }
            else
            {
                entity = _workContext.CurrentCustomer.EntityId;
            }

            CustomerModel customerModel = new CustomerModel();
            customerModel.Email = model.Email;
            customerModel.District = model.District;
            customerModel.Province = model.Provience;
            customerModel.Ward = model.Ward;
            customerModel.Tole = model.Tole;
            customerModel.Position = model.Designation;
            customerModel.Active = true;
            customerModel.OrgAddress = model.District + " " + model.LocalLevel;
            customerModel.OrgName = model.NameEnglish;
            customerModel.IDCardNo = model.CitizenshipNo;
            customerModel.NameNepali = model.NameNepali;
            customerModel.NameEnglish = model.NameEnglish;
            customerModel.Phone = model.MobileNo;
            customerModel.Username = model.MobileNo;
            customerModel.EntityId = entity;
            customerModel.Active = model.IsActive;

            var serviceProvider = await _serviceProviderService.GetServiceProviderById(model.Id);
            if (serviceProvider == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
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
                ServiceProvider m = model.ToEntity(serviceProvider);
                m.UpdatedBy = _workContext.CurrentCustomer.Id;
                await _serviceProviderService.UpdateServiceProvider(m);
                
                SuccessNotification("ServiceProvider updated");
                if (continueEditing)
                {
                    //selected tab
                    await SaveSelectedTabIndex();

                    return RedirectToAction("Edit", new { id = model.Id });
                }
                return RedirectToAction("List");
            }
            var serviceProviderType = new List<SelectListItem> {
                new SelectListItem { Text = "Vet-graduate", Value = "Vet-graduate" },
                new SelectListItem { Text = "Para-professional", Value = "Para-professional" },
            };
            serviceProviderType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            ViewBag.ServiceProviderType = serviceProviderType;
            var provience = ProvinceHelper.GetProvince();
            provience.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.provience = provience;
            var type = new List<SelectListItem> {
                new SelectListItem { Text = "Permanent", Value = "Permanent" },
                new SelectListItem { Text = "Temporary", Value = "Temporary" },
            };
            type.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            var localLevels = await _localLevelService.GetLocalLevel("KATHMANDU");
            var localLevelSelect = new SelectList(localLevels).ToList();
            localLevelSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.LocalLevels = new SelectList(localLevelSelect, "Text","Text", ExecutionHelper.LocalLevel);

            ViewBag.Type = type;
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Delete)]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var ServiceProvider = await _serviceProviderService.GetServiceProviderById(id);
            if (ServiceProvider == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                await _serviceProviderService.DeleteServiceProvider(ServiceProvider);

                SuccessNotification(_localizationService.GetResource("ServiceProvider deleted"));
                return RedirectToAction("List");
            }
            ErrorNotification(ModelState);
            return RedirectToAction("Edit", new { id = id });
        }

        public IActionResult PPrsTechList() => View();

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> PPRSTechList(DataSourceRequest command,string keyword)
        {
            var ServiceProvider = await _serviceProviderService.GetPPRSServiceProvider(keyword, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult {
                Data = ServiceProvider,
                Total = ServiceProvider.TotalCount
            };
            return Json(gridModel);
        }


    }
}
