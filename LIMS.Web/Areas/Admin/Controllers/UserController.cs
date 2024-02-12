using LIMS.Core;
using LIMS.Domain.Customers;
using LIMS.Framework.Controllers;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Mvc.Filters;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Common;
using LIMS.Services.Customers;
using LIMS.Services.Localization;                                           
using LIMS.Services.LocalStructure;
using LIMS.Services.Media;
using LIMS.Services.Security;
using LIMS.Web.Areas.Admin.Extensions;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Web.Areas.Admin.Interfaces;
using LIMS.Web.Areas.Admin.Models.Customers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    [PermissionAuthorize(PermissionSystemName.User)]

    public class UserController : BaseAdminController
    {
        #region Fields
        private readonly ICustomerService _customerService;
        private readonly ICustomerViewModelService _customerViewModelService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ICustomerRegistrationService _customerRegistrationService;
        private readonly ILocalizationService _localizationService;
        private readonly CustomerSettings _customerSettings;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        //private readonly IExportManager _exportManager;
        private readonly ICustomerAttributeParser _customerAttributeParser;
        private readonly ICustomerAttributeService _customerAttributeService;
        private readonly IAddressAttributeParser _addressAttributeParser;
        private readonly IAddressAttributeService _addressAttributeService;
      //  private readonly IWorkflowMessageService _workflowMessageService;
        private readonly IDownloadService _downloadService;
        private readonly IPermissionService _permissionService;
        private readonly IUserApiService _userApiService;
        private readonly IEncryptionService _encryptionService;
        private readonly ICustomerRoleViewModelService _customerRoleViewModelService;
        private readonly ILocalLevelService _localLevelService;

        List<SelectListItem> lists = new List<SelectListItem> {
            new SelectListItem { Text = "Gandaki Province", Value = "Province 4" },
        };
        #endregion

        #region Constructors
        public UserController(ICustomerService customerService,
            
            ICustomerViewModelService customerViewModelService,
            IGenericAttributeService genericAttributeService,
            ICustomerRegistrationService customerRegistrationService,
            ILocalizationService localizationService,
            CustomerSettings customerSettings,
            IWorkContext workContext,
            IStoreContext storeContext,
           // IExportManager exportManager,
            ICustomerAttributeParser customerAttributeParser,
            ICustomerAttributeService customerAttributeService,
            IAddressAttributeParser addressAttributeParser,
            IAddressAttributeService addressAttributeService,
         //   IWorkflowMessageService workflowMessageService,
            IDownloadService downloadService,
            IPermissionService permissionService,
            IUserApiService userApiService,
            IEncryptionService encryptionService,
            ICustomerRoleViewModelService customerRoleViewModelService,
            ILocalLevelService localLevelService)
        {
            _customerService = customerService;
            
            _customerViewModelService = customerViewModelService;
            _genericAttributeService = genericAttributeService;
            _customerRegistrationService = customerRegistrationService;
            _localizationService = localizationService;
            _customerSettings = customerSettings;
            _workContext = workContext;
            _storeContext = storeContext;
          //  _exportManager = exportManager;
            _customerAttributeParser = customerAttributeParser;
            _customerAttributeService = customerAttributeService;
            _addressAttributeParser = addressAttributeParser;
            _addressAttributeService = addressAttributeService;
           // _workflowMessageService = workflowMessageService;
            _downloadService = downloadService;
            _permissionService = permissionService;
            _userApiService = userApiService;
            _encryptionService = encryptionService;
            _customerRoleViewModelService = customerRoleViewModelService;
            _localLevelService = localLevelService;
        }
        #endregion

        protected (string hashpassword, string privatekey) HashPassword(string password)
        {
            var pk = CommonHelper.GenerateRandomDigitCode(24);
            return (_encryptionService.EncryptText(password, pk), pk);
        }

        #region Customers

        public IActionResult Index() => RedirectToAction("List");

        public async Task<IActionResult> List()
        {
            var model = await _customerViewModelService.PrepareCustomerListModel();
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command, CustomerListModel model,
            string[] searchCustomerRoleIds, string[] searchCustomerTagIds)
        {
            var currentUserEmail = _workContext.CurrentCustomer.Id;

            var (customerModelList, totalCount) = await _customerViewModelService.PrepareCustomerList(model, searchCustomerRoleIds, searchCustomerTagIds, command.Page, command.PageSize, createdBy: currentUserEmail);
            var gridModel = new DataSourceResult {
                Data = customerModelList.ToList(),
                Total = totalCount
            };

            return Json(gridModel);
        }

        [PermissionAuthorizeAction(PermissionActionName.Create)]
        public async Task<IActionResult> Create()
        {
            //var provience = ProvinceHelper.GetProvince();
            //provience.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            //ViewBag.provience = provience;
            var lstRoles = new List<SelectListItem>(){
                            new SelectListItem{Text="Select",Value=""},
                            new SelectListItem{Text=RoleHelper.Agriculture, Value=RoleHelper.Agriculture},
                            new SelectListItem{Text=RoleHelper.Livestock, Value=RoleHelper.Livestock},
                            };

            ViewBag.Roles = new SelectList(lstRoles,"Value","Text");            

            var localLevels = await _localLevelService.GetLocalLevel("KATHMANDU");
            var localLevelSelect = new SelectList(localLevels).ToList();
            localLevelSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.LocalLevels = localLevelSelect;

            var model = new CustomerModel();
            await _customerViewModelService.PrepareCustomerModel(model, null, false);
            //default value
            model.Active = true;
            return View(model);
        }

        //[PermissionAuthorizeAction(PermissionActionName.Edit)]
        //[HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [HttpPost]
        public async Task<IActionResult> Create(CustomerModel model, bool continueEditing, IFormCollection form)
        {
            var localLevels = await _localLevelService.GetLocalLevel("KATHMANDU");
            var localLevelSelect = new SelectList(localLevels).ToList();
            localLevelSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.LocalLevels = localLevelSelect;

            var lstRoles = new List<SelectListItem>(){
                            new SelectListItem{Text="Select",Value=""},
                            new SelectListItem{Text=RoleHelper.Agriculture, Value=RoleHelper.Agriculture},
                            new SelectListItem{Text=RoleHelper.Livestock, Value=RoleHelper.Livestock},
                            };

            ViewBag.Roles = new SelectList(lstRoles,"Value","Text");

            if (!string.IsNullOrWhiteSpace(model.Email))
            {
                var cust2 = await _customerService.GetCustomerByEmail(model.Email);
                if (cust2 != null)
                    ModelState.AddModelError("", "Email is already registered");
            }



            if (!string.IsNullOrWhiteSpace(model.Username) & _customerSettings.UsernamesEnabled)
            {
                var cust2 = await _customerService.GetCustomerByUsername(model.Username);
                if (cust2 != null)
                    ModelState.AddModelError("", "Username is already registered");
            }

            //validate customer roles
            var allCustomerRoles = await _customerService.GetAllCustomerRoles(showHidden: true);
            var newCustomerRoles = new List<CustomerRole>();
            var currentCustomer = _workContext.CurrentCustomer;
            string[] customerRoles = currentCustomer.GetCustomerRoleName();
            string orgname = currentCustomer.OrgName;
            string orgAddress = currentCustomer.OrgAddress;

            CustomerRole role = new CustomerRole();

            newCustomerRoles.Add(allCustomerRoles.Where(m => m.Name == "Registered").FirstOrDefault());
            newCustomerRoles.Add(allCustomerRoles.FirstOrDefault(m => m.Name == model.Role));
            var customerRolesError = _customerViewModelService.ValidateCustomerRoles(newCustomerRoles);
            if (!string.IsNullOrEmpty(customerRolesError))
            {
                ModelState.AddModelError("", customerRolesError);
                ErrorNotification(customerRolesError, false);
            }

            if (ModelState.IsValid)
            {                
                model.Province = "BAGAMATI PROVINCE";
                model.District = "KATHMANDU";
                model.CreatedBy = _workContext.CurrentCustomer.Id;
                model.OrgAddress = orgAddress;
                model.OrgName = orgname;
                model.EntityId = _workContext.CurrentCustomer.EntityId;
                var customer = await _customerViewModelService.InsertCustomerModel(model);
                foreach (var customerRole in newCustomerRoles)
                {
                    //ensure that the current customer cannot add to "Administrators" system role if he's not an admin himself
                    if (customerRole.SystemName == SystemCustomerRoleNames.Administrators)
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
                    userapi.IsActive = model.Active;
                    await _userApiService.InsertUserApi(userapi);
                }
                SuccessNotification(_localizationService.GetResource("Admin.Customers.Customers.Added"));

                return continueEditing ? RedirectToAction("Edit", new { id = customer.Id }) : RedirectToAction("List");
            }
            var provience = ProvinceHelper.GetProvince();
            provience.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.provience = provience;
            //If we got this far, something failed, redisplay form
            await _customerViewModelService.PrepareCustomerModel(model, null, true);
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Preview)]
        public async Task<IActionResult> Edit(string id)
        {
            var localLevels = await _localLevelService.GetLocalLevel("KATHMANDU");
            var localLevelSelect = new SelectList(localLevels).ToList();
            localLevelSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.LocalLevels = localLevelSelect;

            var lstRoles = new List<SelectListItem>(){
                            new SelectListItem{Text="Select",Value=""},
                            new SelectListItem{Text=RoleHelper.Agriculture, Value=RoleHelper.Agriculture},
                            new SelectListItem{Text=RoleHelper.Livestock, Value=RoleHelper.Livestock},
                            };

            var customer = await _customerService.GetCustomerById(id);

            var roles = customer.CustomerRoles;

            if(roles.Select(m=>m.SystemName).Contains(RoleHelper.Livestock))
            {
                ViewBag.Roles = new SelectList(lstRoles,"Value","Text",RoleHelper.Livestock);
            }
            else if (roles.Select(m => m.SystemName).Contains(RoleHelper.Agriculture))
            {
                ViewBag.Roles = new SelectList(lstRoles, "Value", "Text",RoleHelper.Agriculture);
            }
            else
            {
                ViewBag.Roles = new SelectList(lstRoles, "Value", "Text");
            }

            if (customer == null || customer.Deleted)
                //No customer found with the specified id
                return RedirectToAction("List");
            //var provience = ProvinceHelper.GetProvince();
            //provience.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            //ViewBag.provience = provience;


            var model = new CustomerModel();
            await _customerViewModelService.PrepareCustomerModel(model, customer, false);
            
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(CustomerModel model, bool continueEditing, IFormCollection form)
        {
            var localLevels = await _localLevelService.GetLocalLevel("KATHMANDU");
            var localLevelSelect = new SelectList(localLevels).ToList();
            localLevelSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.LocalLevels = localLevelSelect;

            var lstRoles = new List<SelectListItem>(){
                            new SelectListItem{Text="Select",Value=""},
                            new SelectListItem{Text=RoleHelper.Agriculture, Value=RoleHelper.Agriculture},
                            new SelectListItem{Text=RoleHelper.Livestock, Value=RoleHelper.Livestock},
                            };

           // ViewBag.Roles = new SelectList(lstRoles,"Value","Text");

            var customer = await _customerService.GetCustomerById(model.Id);
            var roles = customer.CustomerRoles;

            if (roles.Select(m => m.SystemName).Contains(RoleHelper.Livestock))
            {
                ViewBag.Roles = new SelectList(lstRoles, "Value", "Text", RoleHelper.Livestock);
            }
            else if (roles.Select(m => m.SystemName).Contains(RoleHelper.Agriculture))
            {
                ViewBag.Roles = new SelectList(lstRoles, "Value", "Text", RoleHelper.Agriculture);
            }
            else
            {
                ViewBag.Roles = new SelectList(lstRoles, "Value", "Text");
            }

            string email = customer.Email;
            if (customer == null || customer.Deleted)
                //No customer found with the specified id
                return RedirectToAction("List");

            //validate customer roles

            if (ModelState.IsValid)
            {
                try
                {
                    var currentCustomer = _workContext.CurrentCustomer;
                    string[] customerRoles = currentCustomer.GetCustomerRoleName();
                    var allCustomerRoles = await _customerService.GetAllCustomerRoles(showHidden: true);
                    var newCustomerRoles = new List<CustomerRole>();
                    CustomerRole role = new CustomerRole();

                    newCustomerRoles.Add(allCustomerRoles.FirstOrDefault(m=>m.Name==model.Role));
                    newCustomerRoles.Add(allCustomerRoles.Where(m => m.Name == "Registered").FirstOrDefault());


                  
                        var customerRolesError = _customerViewModelService.ValidateCustomerRoles(newCustomerRoles);
                        if (!string.IsNullOrEmpty(customerRolesError))
                        {
                            ModelState.AddModelError("", customerRolesError);
                            ErrorNotification(customerRolesError, false);
                        }
                    



                    customer = await _customerViewModelService.UpdateCustomerModel(customer, model);
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
                    foreach (var customerRole in newCustomerRoles)
                    {
                        //ensure that the current customer cannot add to "Administrators" system role if he's not an admin himself
                        if (customerRole.SystemName == SystemCustomerRoleNames.Administrators)
                            continue;

                        customer.CustomerRoles.Add(customerRole);
                        customerRole.CustomerId = customer.Id;

                        await _customerService.InsertCustomerRoleInCustomer(customerRole);
                    }
                    var userapi = await _userApiService.GetUserByEmail(email);

                    UserApiModel user = new UserApiModel();
                    user.Email = model.Email;
                    user.Password = model.Password;
                    user.IsActive = model.Active;
                    if (!string.IsNullOrEmpty(user.Password))
                    {
                        userapi = user.ToEntity(userapi);
                        var keys = HashPassword(model.Password);
                        userapi.Password = keys.hashpassword;
                        userapi.PrivateKey = keys.privatekey;

                        await _userApiService.UpdateUserApi(userapi);
                    }

                    // var keys = HashPassword(model.Password);
                    //userapi.Password = keys.hashpassword;
                    //userapi.PrivateKey = keys.privatekey;


                    SuccessNotification(_localizationService.GetResource("Admin.Customers.Customers.Updated"));
                    if (continueEditing)
                    {
                        //selected tab
                        await SaveSelectedTabIndex();

                        return RedirectToAction("Edit", new { id = customer.Id });
                    }
                    return RedirectToAction("List");
                }
                catch (Exception exc)
                {
                    ErrorNotification(exc.Message, false);
                }
            }
            //If we got this far, something failed, redisplay form
            await _customerViewModelService.PrepareCustomerModel(model, customer, true);
            var provience = ProvinceHelper.GetProvince();
            provience.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.provience = provience;
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Preview)]
        public async Task<IActionResult> EditAdmin(string id)
        {
            var localLevels = await _localLevelService.GetLocalLevel("KATHMANDU");
            var localLevelSelect = new SelectList(localLevels).ToList();
            localLevelSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.LocalLevels = localLevelSelect;

            var lstRoles = new List<SelectListItem>(){
                            new SelectListItem{Text="Select",Value=""},
                            new SelectListItem{Text=RoleHelper.Agriculture, Value=RoleHelper.Agriculture},
                            new SelectListItem{Text=RoleHelper.Livestock, Value=RoleHelper.Livestock},
                            };

            var customer = await _customerService.GetCustomerById(id);
            

            if (customer == null || customer.Deleted)
                return RedirectToAction("List");
            
            var model = new CustomerModel();
            model.Role = RoleHelper.Administrators;

            await _customerViewModelService.PrepareCustomerModel(model, customer, false);

            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> EditAdmin(CustomerModel model, bool continueEditing, IFormCollection form)
        {
            var localLevels = await _localLevelService.GetLocalLevel("KATHMANDU");
            var localLevelSelect = new SelectList(localLevels).ToList();
            localLevelSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.LocalLevels = localLevelSelect;

            var lstRoles = new List<SelectListItem>(){
                            new SelectListItem{Text="Select",Value=""},
                            new SelectListItem{Text=RoleHelper.Agriculture, Value=RoleHelper.Agriculture},
                            new SelectListItem{Text=RoleHelper.Livestock, Value=RoleHelper.Livestock},
                            };

            // ViewBag.Roles = new SelectList(lstRoles,"Value","Text");

            var customer = await _customerService.GetCustomerById(model.Id);
            

            string email = customer.Email;
            if (customer == null || customer.Deleted)
                //No customer found with the specified id
                return RedirectToAction("List");

            //validate customer roles

            if (ModelState.IsValid)
            {
                try
                {
                    var currentCustomer = _workContext.CurrentCustomer;
                    string[] customerRoles = currentCustomer.GetCustomerRoleName();
                    var allCustomerRoles = await _customerService.GetAllCustomerRoles(showHidden: true);
                    var newCustomerRoles = new List<CustomerRole>();
                    CustomerRole role = new CustomerRole();

                    //newCustomerRoles.Add(allCustomerRoles.FirstOrDefault(m => m.Name == model.Role));
                    //newCustomerRoles.Add(allCustomerRoles.Where(m => m.Name == "Registered").FirstOrDefault());
                    //newCustomerRoles.Add(allCustomerRoles.Where(m => m.Name ==RoleHelper.Administrators).FirstOrDefault());

                    var customerRolesError = _customerViewModelService.ValidateCustomerRoles(newCustomerRoles);
                    if (!string.IsNullOrEmpty(customerRolesError))
                    {
                        ModelState.AddModelError("", customerRolesError);
                        ErrorNotification(customerRolesError, false);
                    }

                    customer = await _customerViewModelService.UpdateCustomerModel(customer, model);
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
                    //foreach (var customerRole in newCustomerRoles)
                    //{
                    //    //ensure that the current customer cannot add to "Administrators" system role if he's not an admin himself
                    //    if (customerRole.SystemName == SystemCustomerRoleNames.Administrators)
                    //        continue;

                    //    customer.CustomerRoles.Add(customerRole);
                    //    customerRole.CustomerId = customer.Id;

                    //    await _customerService.InsertCustomerRoleInCustomer(customerRole);
                    //}
                    var userapi = await _userApiService.GetUserByEmail(email);

                    UserApiModel user = new UserApiModel();
                    user.Email = model.Email;
                    user.Password = model.Password;
                    user.IsActive = model.Active;
                    if (!string.IsNullOrEmpty(user.Password))
                    {
                        userapi = user.ToEntity(userapi);
                        var keys = HashPassword(model.Password);
                        userapi.Password = keys.hashpassword;
                        userapi.PrivateKey = keys.privatekey;

                        await _userApiService.UpdateUserApi(userapi);
                    }

                    // var keys = HashPassword(model.Password);
                    //userapi.Password = keys.hashpassword;
                    //userapi.PrivateKey = keys.privatekey;


                    SuccessNotification(_localizationService.GetResource("Admin.Customers.Customers.Updated"));
                    if (continueEditing)
                    {
                        //selected tab
                        await SaveSelectedTabIndex();

                        return RedirectToAction("EditAdmin", new { id = customer.Id });
                    }
                    return RedirectToAction("List");
                }
                catch (Exception exc)
                {
                    ErrorNotification(exc.Message, false);
                }
            }
            //If we got this far, something failed, redisplay form
            await _customerViewModelService.PrepareCustomerModel(model, customer, true);
            var provience = ProvinceHelper.GetProvince();
            provience.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.provience = provience;
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ActionName("Edit")]
        [FormValueRequired("changepassword")]
        public async Task<IActionResult> ChangePassword(CustomerModel model)
        {
            var customer = await _customerService.GetCustomerById(model.Id);
            if (customer == null)
                //No customer found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var changePassRequest = new ChangePasswordRequest(model.Email,
                    false, _customerSettings.DefaultPasswordFormat, model.Password);
                var changePassResult = await _customerRegistrationService.ChangePassword(changePassRequest);
                if (changePassResult.Success)
                {
                    UserApiModel user = new UserApiModel();
                    var userapi = await _userApiService.GetUserByEmail(model.Email);

                    user.IsActive = userapi.IsActive;
                    user.Password = model.Password;
                    user.Email = model.Email;
                    if (!string.IsNullOrEmpty(user.Password))
                    {
                        userapi = user.ToEntity(userapi);
                        var keys = HashPassword(model.Password);
                        userapi.Password = keys.hashpassword;
                        userapi.PrivateKey = keys.privatekey;

                        await _userApiService.UpdateUserApi(userapi);
                    }


                    SuccessNotification(_localizationService.GetResource("Admin.Customers.Customers.PasswordChanged"));
                }
                else
                    foreach (var error in changePassResult.Errors)
                        ErrorNotification(error);
            }

            return RedirectToAction("Edit", new { id = customer.Id });
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ActionName("Edit")]
        [FormValueRequired("markVatNumberAsValid")]
        public async Task<IActionResult> MarkVatNumberAsValid(CustomerModel model)
        {
            var customer = await _customerService.GetCustomerById(model.Id);
            if (customer == null)
                //No customer found with the specified id
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = customer.Id });
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ActionName("Edit")]
        [FormValueRequired("markVatNumberAsInvalid")]
        public async Task<IActionResult> MarkVatNumberAsInvalid(CustomerModel model)
        {
            var customer = await _customerService.GetCustomerById(model.Id);
            if (customer == null)
                //No customer found with the specified id
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = customer.Id });
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ActionName("Edit")]
        [FormValueRequired("remove-affiliate")]
        public async Task<IActionResult> RemoveAffiliate(CustomerModel model)
        {
            var customer = await _customerService.GetCustomerById(model.Id);
            if (customer == null)
                //No customer found with the specified id
                return RedirectToAction("List");

            customer.AffiliateId = "";
            await _customerService.UpdateAffiliate(customer);
            return RedirectToAction("Edit", new { id = customer.Id });
        }

        [PermissionAuthorizeAction(PermissionActionName.Delete)]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var customer = await _customerService.GetCustomerById(id);
            if (customer == null)
                //No customer found with the specified id
                return RedirectToAction("List");
            if (customer.Id == _workContext.CurrentCustomer.Id)
            {
                ErrorNotification(_localizationService.GetResource("Admin.Customers.Customers.NoSelfDelete"));
                return RedirectToAction("List");
            }

            try
            {
                if (ModelState.IsValid)
                {
                    await _customerViewModelService.DeleteCustomer(customer);
                    SuccessNotification(_localizationService.GetResource("Admin.Customers.Customers.Deleted"));
                    return RedirectToAction("List");
                }
                ErrorNotification(ModelState);
                return RedirectToAction("Edit", new { id = customer.Id });
            }
            catch (Exception exc)
            {
                ErrorNotification(exc.Message);
                return RedirectToAction("Edit", new { id = customer.Id });
            }
        }

        [PermissionAuthorizeAction(PermissionActionName.Delete)]
        public async Task<IActionResult> DeleteSelected(ICollection<string> selectedIds)
        {
            if (selectedIds != null)
            {
                await _customerViewModelService.DeleteSelected(selectedIds.ToList());
            }

            return Json(new { Result = true });
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ActionName("Edit")]
        [FormValueRequired("impersonate")]
        public async Task<IActionResult> Impersonate(string id)
        {
            var customer = await _customerService.GetCustomerById(id);
            if (customer == null)
                //No customer found with the specified id
                return RedirectToAction("List");

            //ensure that a non-admin user cannot impersonate as an administrator
            //otherwise, that user can simply impersonate as an administrator and gain additional administrative privileges
            if (!_workContext.CurrentCustomer.IsAdmin() && customer.IsAdmin())
            {
                ErrorNotification("A non-admin user cannot impersonate as an administrator");
                return RedirectToAction("Edit", customer.Id);
            }

            await _genericAttributeService.SaveAttribute<string>(_workContext.CurrentCustomer,
                SystemCustomerAttributeNames.ImpersonatedCustomerId, customer.Id);

            return RedirectToAction("Index", "Home", new { area = "" });
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ActionName("Edit")]
        [FormValueRequired("send-welcome-message")]
        public async Task<IActionResult> SendWelcomeMessage(CustomerModel model)
        {
            var customer = await _customerService.GetCustomerById(model.Id);
            if (customer == null)
                //No customer found with the specified id
                return RedirectToAction("List");

            //await _workflowMessageService.SendCustomerWelcomeMessage(customer, _storeContext.CurrentStore, _workContext.WorkingLanguage.Id);

            SuccessNotification(_localizationService.GetResource("Admin.Customers.Customers.SendWelcomeMessage.Success"));

            return RedirectToAction("Edit", new { id = customer.Id });
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ActionName("Edit")]
        [FormValueRequired("resend-activation-message")]
        public async Task<IActionResult> ReSendActivationMessage(CustomerModel model)
        {
            var customer = await _customerService.GetCustomerById(model.Id);
            if (customer == null)
                //No customer found with the specified id
                return RedirectToAction("List");

            //email validation message
            await _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.AccountActivationToken, Guid.NewGuid().ToString());
          //  await _workflowMessageService.SendCustomerEmailValidationMessage(customer, _storeContext.CurrentStore, _workContext.WorkingLanguage.Id);

            SuccessNotification(_localizationService.GetResource("Admin.Customers.Customers.ReSendActivationMessage.Success"));

            return RedirectToAction("Edit", new { id = customer.Id });
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        public async Task<IActionResult> SendEmail(CustomerModel model)
        {
            var customer = await _customerService.GetCustomerById(model.Id);
            if (customer == null)
                //No customer found with the specified id
                return RedirectToAction("List");

            try
            {
                if (String.IsNullOrWhiteSpace(customer.Email))
                    throw new LIMSException("Customer email is empty");
                if (!CommonHelper.IsValidEmail(customer.Email))
                    throw new LIMSException("Customer email is not valid");
                if (String.IsNullOrWhiteSpace(model.SendEmail.Subject))
                    throw new LIMSException("Email subject is empty");
                if (String.IsNullOrWhiteSpace(model.SendEmail.Body))
                    throw new LIMSException("Email body is empty");

                await _customerViewModelService.SendEmail(customer, model.SendEmail);

                SuccessNotification(_localizationService.GetResource("Admin.Customers.Customers.SendEmail.Queued"));
            }
            catch (Exception exc)
            {
                ErrorNotification(exc.Message);
            }

            return RedirectToAction("Edit", new { id = customer.Id });
        }


        #endregion
    }

}

