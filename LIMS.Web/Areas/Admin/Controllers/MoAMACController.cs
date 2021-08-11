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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class MoAMACController : BaseAdminController
    {
        private readonly IMoAMACService _molmacService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;
        private readonly SeoSettings _seoSettings;
        private readonly ICustomerService _customerService;
        private readonly ICustomerViewModelService _customerViewModelService;
        private readonly CustomerSettings _customerSettings;
        private readonly ICustomerRegistrationService _customerRegistrationService;
            
        public MoAMACController(
            IMoAMACService molmacService,
            ILanguageService languageService,
            ILocalizationService localizationService,
            IStoreService storeService,
            IWorkContext workContext,
            SeoSettings seoSettings,
            ICustomerService customerService,
            ICustomerViewModelService customerViewModelService,
            CustomerSettings customerSettings,
            ICustomerRegistrationService customerRegistrationService
            )


        {
            _molmacService = molmacService;
            _languageService = languageService;
            _localizationService = localizationService;
            _storeService = storeService;
            _workContext = workContext;
            _seoSettings = seoSettings;
            _customerService = customerService;
            _customerViewModelService = customerViewModelService;
            _customerRegistrationService = customerRegistrationService;
            _customerSettings = customerSettings;
        }

        public IActionResult Index() => RedirectToAction("List");

        public IActionResult List() => View();

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command)
        {
            var molmac = await _molmacService.GetMoAMAC(command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult {
                Data = molmac,
                Total = molmac.TotalCount
            };
            return Json(gridModel);
        }

        [PermissionAuthorizeAction(PermissionActionName.Create)]
        public async Task<IActionResult> Create()
        {
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            MoAMACModel model = new MoAMACModel();
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(MoAMACModel model, bool continueEditing)
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
          
                role = allCustomerRoles.Where(m => m.Name == "MolmacAdmin").FirstOrDefault();
            
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
                var molmac = model.ToEntity();
                await _molmacService.InsertMoAMAC(molmac);

                CustomerModel customerModel = new CustomerModel();
                customerModel.Email = model.UserEmail;
                customerModel.District = model.UserDistrict;
                customerModel.Province = model.UserProvince;
                customerModel.LocalLevel = model.UserLocalLevel;
                customerModel.Ward = model.UserWard;
                customerModel.Tole = model.UserTole;
                customerModel.Position = model.Position;
                customerModel.OrgName = model.NameEnglish;
                customerModel.Active = true;
                customerModel.IDCardNo = model.IDCardNo;
                customerModel.NameNepali = model.UserNameNepali;
                customerModel.NameEnglish = model.UserNameEnglish;
                customerModel.PhoneNo = model.PhoneNo;
                customerModel.EntityId = molmac.Id;
                customerModel.CreatedBy = _workContext.CurrentCustomer.Email;
                customerModel.OrgAddress = molmac.LocalLevel +" "+ molmac.District;
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

                  SuccessNotification(_localizationService.GetResource("Admin.Molmac.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = molmac.Id }) : RedirectToAction("List");
            }

            //If we got this far, something failed, redisplay form
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            return View(model);
        }


        [PermissionAuthorizeAction(PermissionActionName.Preview)]
        public async Task<IActionResult> Edit(string id)
        {
            var molmac = await _molmacService.GetMoAMACById(id);
            if (molmac == null)
                return RedirectToAction("List");
            var model = molmac.ToModel();
            var customer = await _customerService.GetCustomerByEmail(molmac.UserEmail);
            model.CustomerId = customer.Id;
           
       
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
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
         
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(MoAMACModel model, bool continueEditing)
        {
            var customer = await _customerService.GetCustomerById(model.CustomerId);
            string email = customer.Email;
            var allCustomerRoles = await _customerService.GetAllCustomerRoles(showHidden: true);
            var newCustomerRoles = new List<CustomerRole>();
            CustomerRole role = new CustomerRole();

            role = allCustomerRoles.Where(m => m.Name == "MolmacAdmin").FirstOrDefault();

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
            customerModel.OrgName = model.NameEnglish;
            customerModel.Active = true;
            customerModel.IDCardNo = model.IDCardNo;
            customerModel.NameNepali = model.UserNameNepali;
            customerModel.NameEnglish = model.UserNameEnglish;
            customerModel.PhoneNo = model.PhoneNo;
            customerModel.CreatedBy = _workContext.CurrentCustomer.Email;
            customerModel.OrgAddress = model.LocalLevel+" " + model.District;
            var Vhlsec = await _molmacService.GetMoAMACById(model.Id);
            if (Vhlsec == null)
                //No blog post found with the specified id
                return RedirectToAction("List");
            if (ModelState.IsValid)
            {
                if (customer.EntityId == null)
                {
                    customerModel.EntityId = Vhlsec.Id;
                }
                customer = await _customerViewModelService.UpdateCustomerModel(customer, customerModel);

                var molmac = model.ToEntity(Vhlsec);
                await _molmacService.UpdateMoAMAC(molmac);

               
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

                SuccessNotification(_localizationService.GetResource("Admin.Molmac.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = molmac.Id }) : RedirectToAction("List");
            }

            //If we got this far, something failed, redisplay form
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            return View(model);
        }
    

    [PermissionAuthorizeAction(PermissionActionName.Delete)]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var molmac = await _molmacService.GetMoAMACById(id);
            if (molmac == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                await _molmacService.DeleteMoAMAC(molmac);

                SuccessNotification(_localizationService.GetResource("Admin.Molmac.Deleted"));
                return RedirectToAction("List");
            }
            ErrorNotification(ModelState);
            return RedirectToAction("Edit", new { id = id });
        }
    }
}
