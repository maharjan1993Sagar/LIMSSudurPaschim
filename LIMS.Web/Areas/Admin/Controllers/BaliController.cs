﻿using LIMS.Core;
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
using LIMS.Web.Areas.Admin.Models.Livestock;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class BaliController:BaseAdminController
    {
        private readonly IBaliRegisterService _animalRegistrationService;
        private readonly ISpeciesService _speciesService;
        private readonly IBreedService _breedService;
        private readonly ILocalizationService _localizationService;
        private readonly ILanguageService _languageService;
        private readonly IWorkContext _workContext;
        private readonly IFiscalYearService _fiscalYearService;
        private readonly IUnitService _unitService;
        public BaliController(ILocalizationService localizationService, 
            IBaliRegisterService animalRegistrationService,
            ILanguageService languageService,
            ISpeciesService speciesService,
            IBreedService breedService, 
            IWorkContext workContext,
            IFiscalYearService fiscalYearService,
            IUnitService unitService
            
            )
        {
            _localizationService = localizationService;
            _animalRegistrationService = animalRegistrationService;
            _languageService = languageService;
            _speciesService = speciesService;
            _breedService = breedService;
            _workContext = workContext;
            _fiscalYearService = fiscalYearService;
            _unitService = unitService;
        }

        public IActionResult Index() => RedirectToAction("List");

        public IActionResult List() => View();

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command,string keyword)
        {
            var id = _workContext.CurrentCustomer.Id;
            var bali = await _animalRegistrationService.GetbaliRegister(id,keyword, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult {
                Data = bali,
                Total = bali.TotalCount
            };
            return Json(gridModel);
        }



        public async Task<IActionResult> BaliReport()
        {
            FarmListModel currentfiscal = new FarmListModel();

            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();
            currentfiscal.CurrentFiscalYear = fiscalyear.Id;
            var dropdownitem = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            dropdownitem.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.common.select"), ""));

            ViewBag.fiscalyear = dropdownitem;
            return View(currentfiscal);
        }

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> BaliReport(DataSourceRequest command, string Keyword)
        {
            var id = _workContext.CurrentCustomer.Id;
            var bali = (dynamic)null;
            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            if (!string.IsNullOrEmpty(Keyword))
            {
                 bali= await _animalRegistrationService.GetbaliRegister(id, command.Page - 1, command.PageSize, Keyword);

            }
            else
            {

                bali = await _animalRegistrationService.GetbaliRegister(id, command.Page - 1, command.PageSize, fiscalyear.Id);
            }
            var gridModel = new DataSourceResult {
                Data = bali,
                Total = bali.TotalCount
            };
            return Json(gridModel);
        }



        public async Task<IActionResult> Create()
        {
            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.SpeciesId = species;
            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear").ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;
            var unit = new SelectList(await _unitService.GetUnit(), "Id", "UnitShortName").ToList();
            unit.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Unit = unit;
            BaliModel model = new BaliModel();
           
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(BaliModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var animalRegistration = model.ToEntity();
                animalRegistration.Species = await _speciesService.GetSpeciesById(model.SpeciesId);
                animalRegistration.BreedReg = await _breedService.GetBreedById(model.BreedId);
                animalRegistration.FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYearId);
                animalRegistration.Unit = await _unitService.GetUnitById(model.UnitId);
                animalRegistration.Production =Convert.ToString(Convert.ToDecimal(model.Area) * Convert.ToDecimal(model.Productivity));
                animalRegistration.CreatedBy = _workContext.CurrentCustomer.Id;
                await _animalRegistrationService.InsertbaliRegister(animalRegistration);

                SuccessNotification(_localizationService.GetResource("Admin.Create.successful"));
                return continueEditing ? RedirectToAction("Edit", new { id = animalRegistration.Id }) : RedirectToAction("Index");
            }
            var unit = new SelectList(await _unitService.GetUnit(), "Id", "UnitShortName").ToList();
            unit.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Unit = unit;
            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.SpeciesId = species;
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear").ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;
            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var animalRegistration = await _animalRegistrationService.GetbaliRegisterById(id);
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
            var unit = new SelectList(await _unitService.GetUnit(), "Id", "UnitShortName").ToList();
            unit.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Unit = unit;
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(BaliModel model, bool continueEditing)
        {
            var animalRegistration = await _animalRegistrationService.GetbaliRegisterById(model.Id);
            if (animalRegistration == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var m = model.ToEntity(animalRegistration);
                animalRegistration.Species = await _speciesService.GetSpeciesById(model.SpeciesId);
                animalRegistration.BreedReg = await _breedService.GetBreedById(model.BreedId);
                animalRegistration.FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYearId);
                animalRegistration.Unit = await _unitService.GetUnitById(model.UnitId);
                animalRegistration.Production = Convert.ToString(Convert.ToDecimal(model.Area) * Convert.ToDecimal(model.Productivity));

                await _animalRegistrationService.UpdatebaliRegister(m);

                SuccessNotification(_localizationService.GetResource("Admin.Update.Successful"));
                if (continueEditing)
                {
                    //selected tab
                    await SaveSelectedTabIndex();

                    return RedirectToAction("Edit", new { id = model.Id });
                }
                return RedirectToAction("Index");
            }
            var unit = new SelectList(await _unitService.GetUnit(), "Id", "UnitShortName").ToList();
            unit.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Unit = unit;
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
