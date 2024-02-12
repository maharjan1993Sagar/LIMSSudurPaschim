using LIMS.Core;
using LIMS.Domain.Seo;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Mvc.Filters;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Customers;
using LIMS.Services.Localization;
using LIMS.Services.Security;
using LIMS.Services.Stores;
using LIMS.Services.User;
using LIMS.Web.Areas.Admin.Extensions.Mapping;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Web.Areas.Admin.Models.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class VaccinationUserController:BaseAdminController
    {
        private readonly IVaccinationUserService _vaccinationUserService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;
        private readonly SeoSettings _seoSettings;
        private readonly ICustomerService _customerService;

        public VaccinationUserController(
            IVaccinationUserService vaccinationUserService,
            ILanguageService languageService,
            ILocalizationService localizationService,
            IStoreService storeService,
            IWorkContext workContext,
            SeoSettings seoSettings,
            ICustomerService customerService
            )
        {
            _vaccinationUserService = vaccinationUserService;
            _languageService = languageService;
            _localizationService = localizationService;
            _storeService = storeService;
            _workContext = workContext;
            _seoSettings = seoSettings;
            _customerService = customerService;
        }

        public IActionResult Index() => RedirectToAction("List");

        public IActionResult List() => View();

        [PermissionAuthorizeAction(PermissionActionName.List)]

        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command)
        {
            var roles = _workContext.CurrentCustomer.CustomerRoles.Select(m=>m.Name);
            string createdby = null;
            //if (roles.Contains(RoleHelper.LssAdmin) || roles.Contains(RoleHelper.VhlsecAdmin) || roles.Contains(RoleHelper.DolfdAdmin))
            //{
            //    createdby = _workContext.CurrentCustomer.Id;
            //}
            //else
            //{
            //    string adminemail = _workContext.CurrentCustomer.CreatedBy;
            //    var admin = await _customerService.GetCustomerByEmail(adminemail);
            //    createdby = admin.Id;
            //}
            var vaccinationUser = await _vaccinationUserService.GetVaccinationUser(createdby,command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult {
                Data = vaccinationUser,
                Total = vaccinationUser.TotalCount
            };
            return Json(gridModel);
        }

        [PermissionAuthorizeAction(PermissionActionName.Create)]
        public async Task<IActionResult> Create()
        {
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            var Provience = ProvinceHelper.GetProvince();
            Provience.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Provience = Provience;
            return View();
        }

    
        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(VaccinationUserModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var roles = _workContext.CurrentCustomer.CustomerRoles.Select(m => m.Name);
                string createdby = null;
                //if (roles.Contains(RoleHelper.LssAdmin) || roles.Contains(RoleHelper.VhlsecAdmin) || roles.Contains(RoleHelper.DolfdAdmin))
                //{
                //    createdby = _workContext.CurrentCustomer.Id;
                //}
                //else
                //{
                //    string adminemail = _workContext.CurrentCustomer.CreatedBy;
                //    var admin = await _customerService.GetCustomerByEmail(adminemail);
                //    createdby = admin.Id;
                //}
                var vaccinationUser = model.ToEntity();
                vaccinationUser.CreatedBy = createdby;
                await _vaccinationUserService.InsertVaccinationUser(vaccinationUser);
                SuccessNotification(_localizationService.GetResource("Admin.VaccinationUser.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = vaccinationUser.Id }) : RedirectToAction("List");
            }
            var Provience = ProvinceHelper.GetProvince();
            Provience.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Provience = Provience;
            //If we got this far, something failed, redisplay form
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            return View(model);
        }


        [PermissionAuthorizeAction(PermissionActionName.Preview)]
        public async Task<IActionResult> Edit(string id)
        {
            var vaccinationUser = await _vaccinationUserService.GetVaccinationUserById(id);
            if (vaccinationUser == null)
                return RedirectToAction("List");
            var model = vaccinationUser.ToModel();
            var Provience = ProvinceHelper.GetProvince();
            Provience.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Provience = Provience;
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);

            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(VaccinationUserModel model, bool continueEditing)
        {
            var vaccinationUser = await _vaccinationUserService.GetVaccinationUserById(model.Id);
            if (vaccinationUser == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var m = model.ToEntity(vaccinationUser);
               
               
                await _vaccinationUserService.UpdateVaccinationUser(m);

                SuccessNotification(_localizationService.GetResource("Admin.VaccinationUser.Updated"));
                if (continueEditing)
                {
                    //selected tab
                    await SaveSelectedTabIndex();

                    return RedirectToAction("Edit", new { id = model.Id });
                }
                return RedirectToAction("List");
            }
            var Provience = ProvinceHelper.GetProvince();
            Provience.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Provience = Provience;
            //If we got this far, something failed, redisplay form
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);

            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Delete)]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var vaccinationUser = await _vaccinationUserService.GetVaccinationUserById(id);
            if (vaccinationUser == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                await _vaccinationUserService.DeleteVaccinationUser(vaccinationUser);

                SuccessNotification(_localizationService.GetResource("Admin.VaccinationUser.Deleted"));
                return RedirectToAction("List");
            }
            ErrorNotification(ModelState);
            return RedirectToAction("Edit", new { id = id });
        }

    }
}
