using LIMS.Core;
using LIMS.Domain.Customers;
using LIMS.Framework.Controllers;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Mvc.Filters;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Common;
using LIMS.Services.Customers;
using LIMS.Services.ExportImport;
using LIMS.Services.Localization;
using LIMS.Services.Media;
using LIMS.Services.Security;
using LIMS.Web.Areas.Admin.Extensions;
using LIMS.Web.Areas.Admin.Interfaces;
using LIMS.Web.Areas.Admin.Models.Customers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    [PermissionAuthorize(PermissionSystemName.Customers)]
    public partial class CustomerController : BaseAdminController
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
        private readonly IExportManager _exportManager;
        private readonly ICustomerAttributeParser _customerAttributeParser;
        private readonly ICustomerAttributeService _customerAttributeService;
        private readonly IAddressAttributeParser _addressAttributeParser;
        private readonly IAddressAttributeService _addressAttributeService;
   
        private readonly IDownloadService _downloadService;
        private readonly IPermissionService _permissionService;
        private readonly IUserApiService _userApiService;
        private readonly IEncryptionService _encryptionService;

        List<SelectListItem> lists = new List<SelectListItem> {
            new SelectListItem { Text = "Gandaki Province", Value = "Province 4" },
        };
        #endregion

        #region Constructors

        public CustomerController(ICustomerService customerService,
           
            ICustomerViewModelService customerViewModelService,
            IGenericAttributeService genericAttributeService,
            ICustomerRegistrationService customerRegistrationService,
            ILocalizationService localizationService,
            CustomerSettings customerSettings,
            IWorkContext workContext,
            IStoreContext storeContext,
            IExportManager exportManager,
            ICustomerAttributeParser customerAttributeParser,
            ICustomerAttributeService customerAttributeService,
            IAddressAttributeParser addressAttributeParser,
            IAddressAttributeService addressAttributeService,
          //  IWorkflowMessageService workflowMessageService,
            IDownloadService downloadService,
            IPermissionService permissionService,
            IUserApiService userApiService,
            IEncryptionService encryptionService)
        {
            _customerService = customerService;
           
            _customerViewModelService = customerViewModelService;
            _genericAttributeService = genericAttributeService;
            _customerRegistrationService = customerRegistrationService;
            _localizationService = localizationService;
            _customerSettings = customerSettings;
            _workContext = workContext;
            _storeContext = storeContext;
            _exportManager = exportManager;
            _customerAttributeParser = customerAttributeParser;
            _customerAttributeService = customerAttributeService;
            _addressAttributeParser = addressAttributeParser;
            _addressAttributeService = addressAttributeService;
          //  _workflowMessageService = workflowMessageService;
            _downloadService = downloadService;
            _permissionService = permissionService;
            _userApiService = userApiService;
            _encryptionService = encryptionService;
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
        public async Task<IActionResult> CustomerList(DataSourceRequest command, CustomerListModel model,
            string[] searchCustomerRoleIds, string[] searchCustomerTagIds)
        {
            var (customerModelList, totalCount) = await _customerViewModelService.PrepareCustomerList(model, searchCustomerRoleIds, searchCustomerTagIds, command.Page, command.PageSize);
            var gridModel = new DataSourceResult {
                Data = customerModelList.ToList(),
                Total = totalCount
            };

            return Json(gridModel);
        }

        [PermissionAuthorizeAction(PermissionActionName.Create)]
        public async Task<IActionResult> Create()
        {

            var provience = lists;
            provience.Insert(0, new SelectListItem("select", ""));
            ViewBag.provience = provience;
            var model = new CustomerModel();
            await _customerViewModelService.PrepareCustomerModel(model, null, false);
            //default value
            model.Active = true;
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]

        public async Task<IActionResult> Create(CustomerModel model, bool continueEditing, IFormCollection form)
        {
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
            foreach (var customerRole in allCustomerRoles)
                if (model.SelectedCustomerRoleIds != null && model.SelectedCustomerRoleIds.Contains(customerRole.Id))
                    newCustomerRoles.Add(customerRole);
            var customerRolesError = _customerViewModelService.ValidateCustomerRoles(newCustomerRoles);
            if (!string.IsNullOrEmpty(customerRolesError))
            {
                ModelState.AddModelError("", customerRolesError);
                ErrorNotification(customerRolesError, false);
            }

            if (ModelState.IsValid)
            {
                var customer = await _customerViewModelService.InsertCustomerModel(model);

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
            var provience = lists;
            provience.Insert(0, new SelectListItem("select", ""));
            ViewBag.provience = provience;
            //If we got this far, something failed, redisplay form
            await _customerViewModelService.PrepareCustomerModel(model, null, true);
            return View(model);
        }






        [PermissionAuthorizeAction(PermissionActionName.Preview)]
        public async Task<IActionResult> Edit(string id)
        {
            var customer = await _customerService.GetCustomerById(id);
            if (customer == null || customer.Deleted)
                //No customer found with the specified id
                return RedirectToAction("List");
            var provience = lists;
            provience.Insert(0, new SelectListItem("select", ""));
            ViewBag.provience = provience;
            var model = new CustomerModel();
            await _customerViewModelService.PrepareCustomerModel(model, customer, false);
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public async Task<IActionResult> Edit(CustomerModel model, bool continueEditing, IFormCollection form)
        {
            var customer = await _customerService.GetCustomerById(model.Id);
            string email = customer.Email;
            if (customer == null || customer.Deleted)
                //No customer found with the specified id
                return RedirectToAction("List");

            //validate customer roles
            var allCustomerRoles = await _customerService.GetAllCustomerRoles(showHidden: true);
            var newCustomerRoles = new List<CustomerRole>();
            foreach (var customerRole in allCustomerRoles)
                if (model.SelectedCustomerRoleIds != null && model.SelectedCustomerRoleIds.Contains(customerRole.Id))
                    newCustomerRoles.Add(customerRole);
            var customerRolesError = _customerViewModelService.ValidateCustomerRoles(newCustomerRoles);
            if (!String.IsNullOrEmpty(customerRolesError))
            {
                ModelState.AddModelError("", customerRolesError);
                ErrorNotification(customerRolesError, false);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    customer = await _customerViewModelService.UpdateCustomerModel(customer, model);

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
            var provience = lists;
            provience.Insert(0, new SelectListItem("select", ""));
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

        //[PermissionAuthorizeAction(PermissionActionName.Edit)]
        //[HttpPost, ActionName("Edit")]
        //[FormValueRequired("send-welcome-message")]
        //public async Task<IActionResult> SendWelcomeMessage(CustomerModel model)
        //{
        //    var customer = await _customerService.GetCustomerById(model.Id);
        //    if (customer == null)
        //        //No customer found with the specified id
        //        return RedirectToAction("List");

        //    await _workflowMessageService.SendCustomerWelcomeMessage(customer, _storeContext.CurrentStore, _workContext.WorkingLanguage.Id);

        //    SuccessNotification(_localizationService.GetResource("Admin.Customers.Customers.SendWelcomeMessage.Success"));

        //    return RedirectToAction("Edit", new { id = customer.Id });
        //}

        //[PermissionAuthorizeAction(PermissionActionName.Edit)]
        //[HttpPost, ActionName("Edit")]
        //[FormValueRequired("resend-activation-message")]
        //public async Task<IActionResult> ReSendActivationMessage(CustomerModel model)
        //{
        //    var customer = await _customerService.GetCustomerById(model.Id);
        //    if (customer == null)
        //        //No customer found with the specified id
        //        return RedirectToAction("List");

        //    //email validation message
        //    await _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.AccountActivationToken, Guid.NewGuid().ToString());
        //    await _workflowMessageService.SendCustomerEmailValidationMessage(customer, _storeContext.CurrentStore, _workContext.WorkingLanguage.Id);

        //    SuccessNotification(_localizationService.GetResource("Admin.Customers.Customers.ReSendActivationMessage.Success"));

        //    return RedirectToAction("Edit", new { id = customer.Id });
        //}

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

        #region Activity log and message contact form

        [PermissionAuthorizeAction(PermissionActionName.Preview)]
        [HttpPost]
        public async Task<IActionResult> ListActivityLog(DataSourceRequest command, string customerId)
        {
            var (activityLogModels, totalCount) = await _customerViewModelService.PrepareActivityLogModel(customerId, command.Page, command.PageSize);
            var gridModel = new DataSourceResult {
                Data = activityLogModels.ToList(),
                Total = totalCount
            };

            return Json(gridModel);
        }

         #endregion

        #region Export / Import

        [PermissionAuthorizeAction(PermissionActionName.Export)]
        [HttpPost, ActionName("List")]
        [FormValueRequired("exportexcel-all")]
        public async Task<IActionResult> ExportExcelAll(CustomerListModel model)
        {
            var customers = await _customerService.GetAllCustomers(
                customerRoleIds: model.SearchCustomerRoleIds.ToArray(),
                email: model.SearchEmail,
                username: model.SearchUsername,
                firstName: model.SearchFirstName,
                lastName: model.SearchLastName,
                company: model.SearchCompany,
                phone: model.SearchPhone,
                zipPostalCode: model.SearchZipPostalCode,
                loadOnlyWithShoppingCart: false);

            try
            {
                byte[] bytes = _exportManager.ExportCustomersToXlsx(customers);
                return File(bytes, "text/xls", "customers.xlsx");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("List");
            }
        }

        [PermissionAuthorizeAction(PermissionActionName.Export)]
        [HttpPost]
        public async Task<IActionResult> ExportExcelSelected(string selectedIds)
        {
            var customers = new List<Customer>();
            if (selectedIds != null)
            {
                var ids = selectedIds
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x)
                    .ToArray();
                customers.AddRange(await _customerService.GetCustomersByIds(ids));
            }

            byte[] bytes = _exportManager.ExportCustomersToXlsx(customers);
            return File(bytes, "text/xls", "customers.xlsx");
        }

        [PermissionAuthorizeAction(PermissionActionName.Export)]
        [HttpPost, ActionName("List")]
        [FormValueRequired("exportxml-all")]
        public async Task<IActionResult> ExportXmlAll(CustomerListModel model)
        {
            var customers = await _customerService.GetAllCustomers(
                customerRoleIds: model.SearchCustomerRoleIds.ToArray(),
                email: model.SearchEmail,
                username: model.SearchUsername,
                firstName: model.SearchFirstName,
                lastName: model.SearchLastName,
                company: model.SearchCompany,
                phone: model.SearchPhone,
                zipPostalCode: model.SearchZipPostalCode,
                loadOnlyWithShoppingCart: false);

            try
            {
                var xml = await _exportManager.ExportCustomersToXml(customers);
                return File(Encoding.UTF8.GetBytes(xml), "application/xml", "customers.xml");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("List");
            }
        }

        [PermissionAuthorizeAction(PermissionActionName.Export)]
        [HttpPost]
        public async Task<IActionResult> ExportXmlSelected(string selectedIds)
        {
            var customers = new List<Customer>();
            if (selectedIds != null)
            {
                var ids = selectedIds
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x)
                    .ToArray();
                customers.AddRange(await _customerService.GetCustomersByIds(ids));
            }

            var xml = await _exportManager.ExportCustomersToXml(customers);
            return File(Encoding.UTF8.GetBytes(xml), "application/xml", "customers.xml");
        }

        #endregion
    }
}
