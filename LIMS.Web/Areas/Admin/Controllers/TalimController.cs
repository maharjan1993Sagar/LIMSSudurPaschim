using LIMS.Core;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Mvc.Filters;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Bali;
using LIMS.Services.Basic;
using LIMS.Services.Breed;
using LIMS.Services.Localization;
using LIMS.Services.Security;
using LIMS.Web.Areas.Admin.Extensions.Mapping;
using LIMS.Web.Areas.Admin.Models.Bali;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class TalimController : BaseAdminController
    {
        private readonly ITalimService _animalRegistrationService;
        private readonly ISpeciesService _speciesService;
        private readonly IBreedService _breedService;
        private readonly ILocalizationService _localizationService;
        private readonly ILanguageService _languageService;
        private readonly IWorkContext _workContext;
        private readonly IFiscalYearService _fiscalYearService;
        private readonly IPujigatKharchaKharakramService _pujigatKharchaKharakramService;

        public TalimController(ILocalizationService localizationService,
            ITalimService animalRegistrationService,
            ILanguageService languageService,
            ISpeciesService speciesService,
            IBreedService breedService,
            IPujigatKharchaKharakramService pujigatKharchaKharakramService,
            IWorkContext workContext,
            IFiscalYearService fiscalYearService
            )
        {
            _localizationService = localizationService;
            _animalRegistrationService = animalRegistrationService;
            _languageService = languageService;
            _speciesService = speciesService;
            _breedService = breedService;
            _workContext = workContext;
            _fiscalYearService = fiscalYearService;
            _pujigatKharchaKharakramService = pujigatKharchaKharakramService;
        }

        public IActionResult Index() => RedirectToAction("List");

        public IActionResult List() => View();

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command)
        {
            var id = _workContext.CurrentCustomer.Id;
            var bali = await _animalRegistrationService.Gettalim(id, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult {
                Data = bali,
                Total = bali.TotalCount
            };
            return Json(gridModel);
        }


        public async Task<IActionResult> Create()
        {
            var id = _workContext.CurrentCustomer.Id;
            var currentFis = await _fiscalYearService.GetCurrentFiscalYear();

            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.SpeciesId = species;

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear").ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;

            //var pujigat = await _pujigatKharchaKharakramService.GetPujigatKharchaKharakram(id, currentFis.Id,"","","","");
            //var ddlPujigat =new SelectList(pujigat, "Id","Program").ToList();
            //ddlPujigat.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            //ViewBag.PujigatKharchaKaryakramId = ddlPujigat;

            TalimModel model = new TalimModel();

            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(TalimModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var animalRegistration = model.ToEntity();
                    animalRegistration.FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYearId);
                animalRegistration.PujigatKharchaKharakram = await _pujigatKharchaKharakramService.GetPujigatKharchaKharakramById(model.PujigatKharchaKaryakramId);

                animalRegistration.CreatedBy = _workContext.CurrentCustomer.Id;
                animalRegistration.PujigatKharchaKharakramId = model.PujigatKharchaKaryakramId;

                await _animalRegistrationService.Inserttalim(animalRegistration);

                SuccessNotification(_localizationService.GetResource("Admin.Create.successful"));
                return continueEditing ? RedirectToAction("Edit", new { id = animalRegistration.Id }) :RedirectToAction("TabView", "AanudanKaryakram");
                
            }
            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.SpeciesId = species;

            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear").ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;

            //var pujigat = await _pujigatKharchaKharakramService.GetPujigatKharchaKharakram(id, currentFis.Id, "", "", "", "");
            //var ddlPujigat = new SelectList(pujigat, "Id", "Program").ToList();
            //ddlPujigat.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            //ViewBag.PujigatKharchaKaryakramId = ddlPujigat;


            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var animalRegistration = await _animalRegistrationService.GettalimById(id);
            if (animalRegistration == null)
                return RedirectToAction("List");
            var model = animalRegistration.ToModel();
            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.SpeciesId = species;
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear").ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;
            model.PujigatKharchaKaryakramId = animalRegistration.PujigatKharchaKharakramId;
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(TalimModel model, bool continueEditing)
        {
            var animalRegistration = await _animalRegistrationService.GettalimById(model.Id);
            if (animalRegistration == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var m = model.ToEntity(animalRegistration);
                animalRegistration.FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYearId);
                animalRegistration.PujigatKharchaKharakram = await _pujigatKharchaKharakramService.GetPujigatKharchaKharakramById(model.PujigatKharchaKaryakramId);
                animalRegistration.PujigatKharchaKharakramId = model.PujigatKharchaKaryakramId;
                await _animalRegistrationService.Updatetalim(m);

                SuccessNotification(_localizationService.GetResource("Admin.Update.Successful"));
                if (continueEditing)
                {
                    //selected tab
                    await SaveSelectedTabIndex();
                  
                    return RedirectToAction("Edit", new { id = model.Id });
                }
                return RedirectToAction("TabView","AanudanKaryakram");
            }
            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear").ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;
            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.SpeciesId = species;



            //If we got this far, something failed, redisplay form
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);

            return RedirectToAction("Create");
        }


        public async Task<ActionResult> GetBreed(string species)
        {
            var breed = await _breedService.GetBreedBySpeciesId(species);

            return Json(breed.ToList());
        }
    }
}
