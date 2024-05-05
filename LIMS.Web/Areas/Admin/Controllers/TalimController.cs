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
        private readonly ITalimService _talimService;
        private readonly ISpeciesService _speciesService;
        private readonly IBreedService _breedService;
        private readonly ILocalizationService _localizationService;
        private readonly ILanguageService _languageService;
        private readonly IWorkContext _workContext;
        private readonly IFiscalYearService _fiscalYearService;
        private readonly IPujigatKharchaKharakramService _pujigatKharchaKharakramService;
        private readonly IBudgetService _budgetService;

        public TalimController(ILocalizationService localizationService,
            ITalimService talimService,
            ILanguageService languageService,
            ISpeciesService speciesService,
            IBreedService breedService,
            IPujigatKharchaKharakramService pujigatKharchaKharakramService,
            IBudgetService budgetService,
            IWorkContext workContext,
            IFiscalYearService fiscalYearService
            )
        {
            _localizationService = localizationService;
            _talimService = talimService;
            _languageService = languageService;
            _speciesService = speciesService;
            _breedService = breedService;
            _workContext = workContext;
            _fiscalYearService = fiscalYearService;
            _pujigatKharchaKharakramService = pujigatKharchaKharakramService;
            _budgetService = budgetService;
        }

        public IActionResult Index() => RedirectToAction("List");

        public IActionResult List() => View();

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command)
        {
            var id = _workContext.CurrentCustomer.Id;
            var bali = await _talimService.Gettalim("","","" ,command.Page - 1, command.PageSize);
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

            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;

            var budgetId = new SelectList(await _budgetService.GetBudget("", "", "", "", "Training"), "Id", "ActivityName").ToList();
            budgetId.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.BudgetId = budgetId;

            TalimModel model = new TalimModel();

            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(TalimModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var talim = model.ToEntity();
                talim.FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYearId);
                talim.Budget = await _budgetService.GetBudgetById(model.BudgetId);

                talim.CreatedBy = _workContext.CurrentCustomer.Id;
                await _talimService.Inserttalim(talim);

                SuccessNotification(_localizationService.GetResource("Admin.Create.successful"));
                return continueEditing ? RedirectToAction("Edit", new { id = talim.Id }) :RedirectToAction("TabView", "AanudanKaryakram");
                
            }
            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.SpeciesId = species;
           
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);

            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;

            var budgetId = new SelectList(await _budgetService.GetBudget("","","","","Training"), "Id", "ActivityName", model.BudgetId).ToList();
            budgetId.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.BudgetId = budgetId;

            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var talim = await _talimService.GettalimById(id);
            if (talim == null)
                return RedirectToAction("List");
            var model = talim.ToModel();
           
            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.SpeciesId = species;
           
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);

            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;

            var budgetId = new SelectList(await _budgetService.GetBudget("", "", "", "", "Training"), "Id", "ActivityName", model.BudgetId).ToList();
            budgetId.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.BudgetId = budgetId;

            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(TalimModel model, bool continueEditing)
        {
            var talim = await _talimService.GettalimById(model.Id);
            if (talim == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var m = model.ToEntity(talim);
                talim.FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYearId);
                talim.Budget = await _budgetService.GetBudgetById(model.BudgetId);

                await _talimService.Updatetalim(m);

                SuccessNotification(_localizationService.GetResource("Admin.Update.Successful"));
                if (continueEditing)
                {
                    //selected tab
                    await SaveSelectedTabIndex();
                  
                    return RedirectToAction("Edit", new { id = model.Id });
                }
                return RedirectToAction("Index");
            }
            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;

            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.SpeciesId = species;

            var budgetId = new SelectList(await _budgetService.GetBudget("", "", "", "", "Training"), "Id", "ActivityName", model.BudgetId).ToList();
            budgetId.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.BudgetId = budgetId;

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
