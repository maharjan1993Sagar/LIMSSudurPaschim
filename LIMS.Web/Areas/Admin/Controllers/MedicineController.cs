using LIMS.Core;
using LIMS.Domain.Seo;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Mvc.Filters;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Customers;
using LIMS.Services.Localization;
using LIMS.Services.Security;
using LIMS.Services.Stores;
using LIMS.Services.Vaccination;
using LIMS.Web.Areas.Admin.Extensions.Mapping;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Web.Areas.Admin.Models.Vaccination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class MedicineController: BaseAdminController
    {
        private readonly IVaccinationTypeService _vaccinationTypeService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;
        private readonly SeoSettings _seoSettings;
        private readonly ICustomerService _customerService;


        public MedicineController(
            IVaccinationTypeService vaccinationTypeService,
            ILanguageService languageService,
            ILocalizationService localizationService,
            IStoreService storeService,
            IWorkContext workContext,
            SeoSettings seoSettings,
            ICustomerService customerService
            )
        {
            _vaccinationTypeService = vaccinationTypeService;
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
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
            //createdby = _context.CurrentCustomer.Id;
            var createdBy = "";
            var vaccination = await _vaccinationTypeService.GetVaccination(createdBy, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult {
                Data = vaccination,
                Total = vaccination.TotalCount
            };
            return Json(gridModel);

            //if (roles.Contains(RoleHelper.LssAdmin))
            //{
            //    var createdBy = _workContext.CurrentCustomer.Id;

               
            //}
            //else
            //{
            //    string adminemail = _workContext.CurrentCustomer.CreatedBy;
            //    var admin = await _customerService.GetCustomerByEmail(adminemail);
            //    string createdBy = admin.Id;

            //    var vaccination = await _vaccinationTypeService.GetVaccination(createdBy, command.Page - 1, command.PageSize);
            //    var gridModel = new DataSourceResult {
            //        Data = vaccination,
            //        Total = vaccination.TotalCount
            //    };
            //    return Json(gridModel);
            //}


        }

        [PermissionAuthorizeAction(PermissionActionName.Create)]
        public async Task<IActionResult> Create()
        {
           
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);

            return View();
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(VaccinationTypeModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var vaccinationType = model.ToEntity();
               
                var createdby = _workContext.CurrentCustomer.Id;
                vaccinationType.CreatedBy = createdby;
                vaccinationType.Type = "Medicine";
                await _vaccinationTypeService.InsertVaccinationType(vaccinationType);
                SuccessNotification(_localizationService.GetResource("Admin.Medicine.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = vaccinationType.Id }) : RedirectToAction("List");
            }
          
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            return View(model);
        }


        [PermissionAuthorizeAction(PermissionActionName.Preview)]
        public async Task<IActionResult> Edit(string id)
        {
            var vaccinationType = await _vaccinationTypeService.GetVaccinationTypeById(id);
            if (vaccinationType == null)
                return RedirectToAction("List");
            var model = vaccinationType.ToModel();
          
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(VaccinationTypeModel model, bool continueEditing)
        {
            var vaccinationType = await _vaccinationTypeService.GetVaccinationTypeById(model.Id);
            if (vaccinationType == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var m = model.ToEntity(vaccinationType);

                await _vaccinationTypeService.UpdateVaccinationType(m);

                SuccessNotification(_localizationService.GetResource("Admin.Medicine.Updated"));
                if (continueEditing)
                {
                    //selected tab
                    await SaveSelectedTabIndex();

                    return RedirectToAction("Edit", new { id = model.Id });
                }
                return RedirectToAction("List");
            }
         
            //If we got this far, something failed, redisplay form
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);

            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Delete)]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var vaccinationtype = await _vaccinationTypeService.GetVaccinationTypeById(id);
            if (vaccinationtype == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                await _vaccinationTypeService.DeleteVaccinationType(vaccinationtype);

                SuccessNotification(_localizationService.GetResource("Admin.Medicine.Deleted"));
                return RedirectToAction("List");
            }
            ErrorNotification(ModelState);
            return RedirectToAction("Edit", new { id = id });
        }


      
    }
}
