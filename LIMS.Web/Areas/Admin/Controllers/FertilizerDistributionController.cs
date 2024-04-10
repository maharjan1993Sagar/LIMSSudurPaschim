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
    public class FertilizerDistributionController:BaseAdminController
    {
        private readonly IFertilizerDistributionService _FertilizerDistributionService;
        private readonly IUnitService _unitService;
        private readonly IBreedService _breedService;
        private readonly ILocalizationService _localizationService;
        private readonly ILanguageService _languageService;
        private readonly IWorkContext _workContext;
        private readonly IFiscalYearService _fiscalYearService;
        private readonly ICategoryService _CategoryService;
        private readonly ILocalLevelService _localLevelService;
        private readonly IBudgetService _budgetService;

        public FertilizerDistributionController(ILocalizationService localizationService,
            IFertilizerDistributionService FertilizerDistributionService,
            ILanguageService languageService,
            IUnitService unitService,
            IBreedService breedService,
            IWorkContext workContext,
            IFiscalYearService fiscalYearService,
            ICategoryService CategoryService,
            ILocalLevelService localLevelService,
            ICategoryService categoryService,
            IBudgetService budgetService
            )
        {
            _localizationService = localizationService;
            _FertilizerDistributionService = FertilizerDistributionService;
            _languageService = languageService;
            _unitService = unitService;
            _breedService = breedService;
            _workContext = workContext;
            _fiscalYearService = fiscalYearService;
            _CategoryService = CategoryService;
            _localLevelService = localLevelService;
            _CategoryService = categoryService;
            _budgetService = budgetService;
        }

        public IActionResult Index() => RedirectToAction("List");

        public async Task<IActionResult> List() {

            //var localLevels = await _localLevelService.GetLocalLevel("KATHMANDU");
            //var localLevelSelect = new SelectList(localLevels).ToList();
            //localLevelSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            //ViewBag.LocalLevels = new SelectList(localLevelSelect, "Text","Text", ExecutionHelper.LocalLevel);

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
            var bali = await _FertilizerDistributionService.GetFertilizerDistribution("",fiscalYear,locallevel,district, command.Page - 1, command.PageSize);
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

            var localLevels = await _localLevelService.GetDistrict("");
            var localLevelSelect = new SelectList(localLevels).ToList();
            localLevelSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Districts = new SelectList(localLevelSelect, "Text", "Text", ExecutionHelper.LocalLevel);

            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;

            //ViewBag.Types = ExecutionHelper.GetXetras();

            //var month = new MonthHelper();
            //var months = month.GetMonths();
            //months.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            //ViewBag.Months = months;

            WardHelper wardHelper = new WardHelper();
            var ward = wardHelper.GetWard();
            ward.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Wards = ward;

            FertilizerDistributionModel model = new FertilizerDistributionModel();
            model.LocalLevel = _workContext.CurrentCustomer.LocalLevel;
            model.EnglishDate = DateTime.Now;

            return View(model);
        }


        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(FertilizerDistributionModel model, bool continueEditing)
        {
            var localLevels = await _localLevelService.GetDistrict("");
            var localLevelSelect = new SelectList(localLevels).ToList();
            localLevelSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Districts = new SelectList(localLevelSelect, "Text", "Text", ExecutionHelper.LocalLevel);

            if (ModelState.IsValid)
            {
                //if(String.IsNullOrEmpty(model.CategoryId))
                //{
                //    var category = await _CategoryService.GetCategoryByName(model.CategoryName);
                //    if(!String.IsNullOrEmpty(category.Id))
                //    {
                //        category.NameEnglish = model.CategoryName;
                //        category.Type = "FertilizerDistribution";
                //        await _CategoryService.InsertCategory(category);
                //        model.CategoryId = category.Id;
                //    }
                //}

                var FertilizerDistribution = model.ToEntity();
                FertilizerDistribution.Unit = await _unitService.GetUnitById(model.UnitId);
                FertilizerDistribution.FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYearId);
               

                FertilizerDistribution.CreatedBy = _workContext.CurrentCustomer.Id;

                await CreateCategory(FertilizerDistribution.FertilizerType, "Fertilizer Type");

                await _FertilizerDistributionService.InsertFertilizerDistribution(FertilizerDistribution);

                SuccessNotification(_localizationService.GetResource("Admin.Create.successful"));
                return continueEditing ? RedirectToAction("Edit", new { id = FertilizerDistribution.Id }) : RedirectToAction("Index");
            }
            var units = new SelectList(await _unitService.GetUnit(), "Id", "UnitNameEnglish").ToList();
            units.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Units = units;

            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;


            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var FertilizerDistribution = await _FertilizerDistributionService.GetFertilizerDistributionById(id);
            if (FertilizerDistribution == null)
                return RedirectToAction("List");
            var model = FertilizerDistribution.ToModel();

           
          
            var units = new SelectList(await _unitService.GetUnit(), "Id", "UnitNameEnglish").ToList();
            units.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Units = units;

            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;


            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(FertilizerDistributionModel model, bool continueEditing)
        {
            var FertilizerDistribution = await _FertilizerDistributionService.GetFertilizerDistributionById(model.Id);
            if (FertilizerDistribution == null)
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
                //        category.Type = "FertilizerDistribution";
                //        await _CategoryService.InsertCategory(category);
                //        model.CategoryId = category.Id;
                //    }
                //}
                m.Unit = await _unitService.GetUnitById(model.UnitId);
                m.FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYearId);
                await CreateCategory(m.FertilizerType, "Fertilizer Type");


                await _FertilizerDistributionService.UpdateFertilizerDistribution(m);

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
                    Type = type
                };

                await _CategoryService.InsertCategory(category);

            }

        }
    }
    }
