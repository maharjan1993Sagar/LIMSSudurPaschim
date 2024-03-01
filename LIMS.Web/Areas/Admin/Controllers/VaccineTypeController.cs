using LIMS.Core;
using LIMS.Domain.Breed;
using LIMS.Domain.Seo;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Mvc.Filters;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Breed;
using LIMS.Services.Localization;
using LIMS.Services.Security;
using LIMS.Services.Stores;
using LIMS.Services.Vaccination;
using LIMS.Web.Areas.Admin.Extensions.Mapping;
using LIMS.Web.Areas.Admin.Models.Vaccination;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class VaccineTypeController : BaseAdminController
    {
        private readonly IVaccinationTypeService _vaccinationTypeService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;
        private readonly SeoSettings _seoSettings;
        private readonly ISpeciesService _speciesService;
        private readonly ILivestockSpeciesService _livestockSpeciesService;
        public VaccineTypeController(
            IVaccinationTypeService vaccinationTypeService,
            ILanguageService languageService,
            ILocalizationService localizationService,
            IStoreService storeService,
            IWorkContext workContext,
            SeoSettings seoSettings,
            ISpeciesService speciesService,
            ILivestockSpeciesService livestockSpeciesService
            )
        {
            _vaccinationTypeService = vaccinationTypeService;
            _languageService = languageService;
            _localizationService = localizationService;
            _storeService = storeService;
            _workContext = workContext;
            _seoSettings = seoSettings;
            _speciesService = speciesService;
            _livestockSpeciesService = livestockSpeciesService;
        }

        public IActionResult Index() => RedirectToAction("List");

        public IActionResult List() => View();

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command)
        {
            var createdby = _workContext.CurrentCustomer.Id;
            var vaccination = await _vaccinationTypeService.GetVaccination("",command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult {
                Data =vaccination,
                Total = vaccination.TotalCount
            };
            return Json(gridModel);
        }

        [PermissionAuthorizeAction(PermissionActionName.Create)]
        public async Task<IActionResult> Create()
        {
            var vaccineType = new List<SelectListItem>() {
                new SelectListItem{Text="Medicine", Value="Medicine" },
                new SelectListItem{Text="Vaccine", Value="Vaccine" },
            };
            vaccineType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Purpose = await _livestockSpeciesService.GetBreed();
            ViewBag.Type = vaccineType;
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);

            return View();
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(VaccinationTypeModel model,IFormCollection form, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var vaccinationType = model.ToEntity();
               var createdby= _workContext.CurrentCustomer.Id;
                vaccinationType.CreatedBy = createdby;
                var purposes = form["Species"].ToList();
                vaccinationType.Species = purposes;
                await _vaccinationTypeService.InsertVaccinationType(vaccinationType);

                SuccessNotification(_localizationService.GetResource("Admin.VaccineType.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = vaccinationType.Id }) : RedirectToAction("List");
            }
            var vaccineType = new List<SelectListItem>() {
                new SelectListItem{Text="Medicine", Value="Medicine" },
                new SelectListItem{Text="Vaccine", Value="Vaccine" },
            };
            vaccineType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Type = vaccineType;
            ViewBag.Purpose = await _livestockSpeciesService.GetBreed();

            //If we got this far, something failed, redisplay form
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            return View(model);
        }


        [PermissionAuthorizeAction(PermissionActionName.Preview)]
        public async Task<IActionResult> Edit(string id)
        {
            var vaccinationType = await _vaccinationTypeService.GetVaccinationTypeById(id);

            if (vaccinationType == null)
                //No blog post found with the specified id
                return RedirectToAction("List");
            var model = vaccinationType.ToModel();
            model.Species = vaccinationType.Species;
            var vaccineType = new List<SelectListItem>() {
                new SelectListItem{Text="Medicine", Value="Medicine" },
                new SelectListItem{Text="Vaccine", Value="Vaccine" },
            };
            ViewBag.Purpose = await _livestockSpeciesService.GetBreed();

            vaccineType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Type = vaccineType;
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(VaccinationTypeModel model,IFormCollection form, bool continueEditing)
        {
            var vaccinationType = await _vaccinationTypeService.GetVaccinationTypeById(model.Id);
            if (vaccinationType == null)
                //No blog post found with the specified id
                return RedirectToAction("List");           

            if (ModelState.IsValid)
            {
                var m = model.ToEntity(vaccinationType);
                var purposes = form["Species"].ToList();
                m.Species = purposes;
                await _vaccinationTypeService.UpdateVaccinationType(m);

                SuccessNotification(_localizationService.GetResource("Admin.VaccineType.Updated"));
                if (continueEditing)
                {
                    //selected tab
                    await SaveSelectedTabIndex();

                    return RedirectToAction("Edit", new { id = model.Id });
                }
                return RedirectToAction("List");
            }
            var vaccineType = new List<SelectListItem>() {
                new SelectListItem{Text="Medicine", Value="Medicine" },
                new SelectListItem{Text="Vaccine", Value="Vaccine" },
            };
            ViewBag.Purpose = await _livestockSpeciesService.GetBreed();

            vaccineType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            ViewBag.Type = vaccineType;
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

                SuccessNotification(_localizationService.GetResource("Admin.VaccineType.Deleted"));
                return RedirectToAction("List");
            }
            ErrorNotification(ModelState);
            return RedirectToAction("Edit", new { id = id });
        }
    }
}
