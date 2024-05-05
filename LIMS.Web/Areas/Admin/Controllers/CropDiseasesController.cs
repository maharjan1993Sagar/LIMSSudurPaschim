using LIMS.Core;
using LIMS.Domain.BasicSetup;
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
    public class CropDiseasesController : BaseAdminController
    {
        private readonly ICropDiseasesService _CropDiseasesService;
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

        public CropDiseasesController(ILocalizationService localizationService,
            ICropDiseasesService CropDiseasesService,
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
            _CropDiseasesService = CropDiseasesService;
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

        public async Task<IActionResult> List()
        {
            var localLevels = await _localLevelService.GetLocalLevel("KATHMANDU");
            var localLevelSelect = new SelectList(localLevels).ToList();
            localLevelSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.LocalLevels = new SelectList(localLevelSelect, "Text", "Text", ExecutionHelper.LocalLevel);

            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;

            return View();
        }

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command, string fiscalYear = "", string district = "", string locallevel = "")
        {
            var id = _workContext.CurrentCustomer.Id;
            var bali = await _CropDiseasesService.GetCropDiseases("", fiscalYear, locallevel, district, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult {
                Data = bali,
                Total = bali.TotalCount
            };
            return Json(gridModel);
        }


        public async Task<IActionResult> Create()
        {
            var localLevels = await _localLevelService.GetDistrict("");
            var localLevelSelect = new SelectList(localLevels).ToList();
            localLevelSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Districts = new SelectList(localLevelSelect, "Text", "Text", ExecutionHelper.LocalLevel);


            var units = new SelectList(await _unitService.GetUnit(), "Id", "UnitNameEnglish").ToList();
            units.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Units = units;

            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Species = species;

            //var localLevels = await _localLevelService.GetLocalLevel("KATHMANDU");
            //var localLevelSelect = new SelectList(localLevels).ToList();
            //localLevelSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            //ViewBag.LocalLevels = new SelectList(localLevelSelect, "Text","Text", ExecutionHelper.LocalLevel);

            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;


            WardHelper wardHelper = new WardHelper();
            var ward = wardHelper.GetWard();
            ward.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Wards = ward;

            CropDiseasesModel model = new CropDiseasesModel();
            model.LocalLevel = _workContext.CurrentCustomer.LocalLevel;
            model.EnglishDate = DateTime.Now;

            return View(model);
        }


        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(CropDiseasesModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var CropDiseases = model.ToEntity();
                CropDiseases.Species = await _speciesService.GetSpeciesById(model.SpeciesId);
                CropDiseases.Breed = await _breedService.GetBreedById(model.BreedId);
                CropDiseases.FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYearId);

                CropDiseases.CreatedBy = _workContext.CurrentCustomer.Id;

                await CreateCategory(CropDiseases.DiseaseName, "Crop Disease");
                await CreateCategory(CropDiseases.LabName, "Lab");
                await CreateCategory(CropDiseases.Technician, "Technician");
                await CreateCategory(CropDiseases.TechDesignation, "Designation");



                await _CropDiseasesService.InsertCropDiseases(CropDiseases);

                SuccessNotification(_localizationService.GetResource("Admin.Create.successful"));
                return continueEditing ? RedirectToAction("Edit", new { id = CropDiseases.Id }) : RedirectToAction("Index");
            }
            var localLevels = await _localLevelService.GetDistrict("");
            var localLevelSelect = new SelectList(localLevels).ToList();
            localLevelSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Districts = new SelectList(localLevelSelect, "Text", "Text", ExecutionHelper.LocalLevel);


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


            WardHelper wardHelper = new WardHelper();
            var ward = wardHelper.GetWard();
            ward.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Wards = ward;

            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var CropDiseases = await _CropDiseasesService.GetCropDiseasesById(id);
            if (CropDiseases == null)
                return RedirectToAction("List");
            var model = CropDiseases.ToModel();

            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Species = species;

            var units = new SelectList(await _unitService.GetUnit(), "Id", "UnitNameEnglish").ToList();
            units.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Units = units;

            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;

            WardHelper wardHelper = new WardHelper();
            var ward = wardHelper.GetWard();
            ward.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Wards = ward;



            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(CropDiseasesModel model, bool continueEditing)
        {
            var CropDiseases = await _CropDiseasesService.GetCropDiseasesById(model.Id);
            if (CropDiseases == null)
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
                //        category.Type = "CropDiseases";
                //        await _CategoryService.InsertCategory(category);
                //        model.CategoryId = category.Id;
                //    }
                //}
                m.Species = await _speciesService.GetSpeciesById(model.SpeciesId);
                m.Breed = await _breedService.GetBreedById(model.BreedId);
                m.FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYearId);

                await CreateCategory(m.DiseaseName, "Crop Disease");
                await CreateCategory(m.LabName, "Lab");
                await CreateCategory(m.Technician, "Technician");
                await CreateCategory(m.TechDesignation, "Designation");

                await _CropDiseasesService.UpdateCropDiseases(m);

                SuccessNotification(_localizationService.GetResource("Admin.Update.Successful"));
                if (continueEditing)
                {
                    //selected tab
                    await SaveSelectedTabIndex();

                    return RedirectToAction("Edit", new { id = model.Id });
                }
                return RedirectToAction("Index");
            }
            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Species = species;

            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;

            WardHelper wardHelper = new WardHelper();
            var ward = wardHelper.GetWard();
            ward.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Wards = ward;

            //If we got this far, something failed, redisplay form
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);

            return RedirectToAction("Create");
        }


        public virtual async Task<IActionResult> CategoryAutoComplete(string term, string type)
        {
            var result = await _CategoryService.GetCategoryByType(type, term);
            return Json(result);
        }


        public async Task CreateCategory(string term, string type)
        {
            var category = await _CategoryService.GetCategoryByNameType(term, type);
            if (category == null)
            {
                category = new Category {
                    NameEnglish = term.Trim().ToString(),
                    NameNepali = term.Trim().ToString(),
                    Type=type
                };

                await _CategoryService.InsertCategory(category);
                
            }
            
            }
        }
    }
