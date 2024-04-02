using LIMS.Core;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Mvc.Filters;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Bali;
using LIMS.Services.Basic;
using LIMS.Services.Breed;
using LIMS.Services.Localization;
using LIMS.Services.LocalStructure;
using LIMS.Services.Security;
using LIMS.Web.Areas.Admin.Extensions.Mapping;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Web.Areas.Admin.Models.Bali;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class SeedDistributionController:BaseAdminController
    {
        private readonly ISeedDistributionService _SeedDistributionService;
        private readonly IUnitService _unitService;
        private readonly IBreedService _breedService;
        private readonly ILocalizationService _localizationService;
        private readonly ILanguageService _languageService;
        private readonly IWorkContext _workContext;
        private readonly IFiscalYearService _fiscalYearService;
        private readonly ICategoryService _CategoryService;
        private readonly ILocalLevelService _localLevelService;
        private readonly IBudgetService _budgetService;
        private readonly ISpeciesService _speciesService;

        public SeedDistributionController(ILocalizationService localizationService,
            ISeedDistributionService SeedDistributionService,
            ILanguageService languageService,
            IUnitService unitService,
            IBreedService breedService,
            IWorkContext workContext,
            IFiscalYearService fiscalYearService,
            ICategoryService CategoryService,
            ILocalLevelService localLevelService,
            ICategoryService categoryService,
            IBudgetService budgetService,
            ISpeciesService speciesService
            )
        {
            _localizationService = localizationService;
            _SeedDistributionService = SeedDistributionService;
            _languageService = languageService;
            _unitService = unitService;
            _breedService = breedService;
            _workContext = workContext;
            _fiscalYearService = fiscalYearService;
            _CategoryService = CategoryService;
            _localLevelService = localLevelService;
            _CategoryService = categoryService;
            _budgetService = budgetService;
            _speciesService = speciesService;
        }

        public IActionResult Index() => RedirectToAction("List");

        public async Task<IActionResult> List() {

            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;

            return View();
                
                }

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command,  string fiscalYear = "", string district = "", string locallevel = "")
        {
            var id = _workContext.CurrentCustomer.Id;
            var bali = await _SeedDistributionService.GetSeedDistribution("",fiscalYear,locallevel,district, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult {
                Data = bali,
                Total = bali.TotalCount
            };
            return Json(gridModel);
        }


        public async Task<IActionResult> Create()
        {           
            var units = new SelectList(await _unitService.GetUnit(), "Id", "UnitNameEnglish").ToList();
            units.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Units = units;

            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Species = species;


            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;


            SeedDistributionModel model = new SeedDistributionModel();
            model.LocalLevel = _workContext.CurrentCustomer.LocalLevel;
            model.EnglishDate = DateTime.Now;

            return View(model);
        }


        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(SeedDistributionModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                //if(String.IsNullOrEmpty(model.CategoryId))
                //{
                //    var category = await _CategoryService.GetCategoryByName(model.CategoryName);
                //    if(!String.IsNullOrEmpty(category.Id))
                //    {
                //        category.NameEnglish = model.CategoryName;
                //        category.Type = "SeedDistribution";
                //        await _CategoryService.InsertCategory(category);
                //        model.CategoryId = category.Id;
                //    }
                //}

                var SeedDistribution = model.ToEntity();
                SeedDistribution.Species = await _speciesService.GetSpeciesById(model.SpeciesId);
                SeedDistribution.Breed = await _breedService.GetBreedById(model.BreedId);
                SeedDistribution.FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYearId);
                SeedDistribution.Unit = await _unitService.GetUnitById(model.UnitId);
               

                SeedDistribution.CreatedBy = _workContext.CurrentCustomer.Id;
                await _SeedDistributionService.InsertSeedDistribution(SeedDistribution);

                SuccessNotification(_localizationService.GetResource("Admin.Create.successful"));
                return continueEditing ? RedirectToAction("Edit", new { id = SeedDistribution.Id }) : RedirectToAction("Index");
            }
            var units = new SelectList(await _unitService.GetUnit(), "Id", "UnitNameEnglish").ToList();
            units.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Units = units;

            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;

            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Species = species;

            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var SeedDistribution = await _SeedDistributionService.GetSeedDistributionById(id);
            if (SeedDistribution == null)
                return RedirectToAction("List");
            var model = SeedDistribution.ToModel();
                       
            var units = new SelectList(await _unitService.GetUnit(), "Id", "UnitNameEnglish").ToList();
            units.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Units = units;

            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;

            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Species = species;

            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(SeedDistributionModel model, bool continueEditing)
        {
            var SeedDistribution = await _SeedDistributionService.GetSeedDistributionById(model.Id);
            if (SeedDistribution == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var m = model.ToEntity();
                //if (String.IsNullOrEmpty(model.CategoryId))
                //{
                //    var category = await _CategoryService.GetCategoryByName(model.CategoryName);
                //    if (!String.IsNullOrEmpty(category.Id))
                //    {
                //        category.NameEnglish = model.CategoryName;
                //        category.Type = "SeedDistribution";
                //        await _CategoryService.InsertCategory(category);
                //        model.CategoryId = category.Id;
                //    }
                //}
                m.Species = await _speciesService.GetSpeciesById(model.SpeciesId);
                SeedDistribution.Breed = await _breedService.GetBreedById(model.BreedId);
                m.FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYearId);
                m.Unit = await _unitService.GetUnitById(model.UnitId);

                await _SeedDistributionService.UpdateSeedDistribution(m);

                SuccessNotification(_localizationService.GetResource("Admin.Update.Successful"));
                if (continueEditing)
                {
                    //selected tab
                    await SaveSelectedTabIndex();

                    return RedirectToAction("Edit", new { id = model.Id });
                }
                return RedirectToAction("Index");
            }

           
            var units = new SelectList(await _unitService.GetUnit(), "Id", "UnitNameEnglish").ToList();
            units.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Units = units;

            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;

            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Species = species;

            //If we got this far, something failed, redisplay form
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);

            return RedirectToAction("Create");
        }


        public virtual async Task<IActionResult> CategoryAutoComplete(string term, string type)
        {
            var result = await _CategoryService.GetCategoryByType(type, term);
            return Json(result);
        }
      
        }
    }
