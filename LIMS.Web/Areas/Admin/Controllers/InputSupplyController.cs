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
    public class InputSupplyController:BaseAdminController
    {
        private readonly IInputSupplyService _inputSupplyService;
        private readonly IUnitService _unitService;
        private readonly IBreedService _breedService;
        private readonly ILocalizationService _localizationService;
        private readonly ILanguageService _languageService;
        private readonly IWorkContext _workContext;
        private readonly IFiscalYearService _fiscalYearService;
        private readonly ICategoryService _categoryService;
        private readonly ILocalLevelService _localLevelService;
        private readonly IBudgetService _budgetService;

        public InputSupplyController(ILocalizationService localizationService,
            IInputSupplyService inputSupplyService,
            ILanguageService languageService,
            IUnitService unitService,
            IBreedService breedService,
            IWorkContext workContext,
            IFiscalYearService fiscalYearService,
            ILocalLevelService localLevelService,
            ICategoryService categoryService,
            IBudgetService budgetService
            )
        {
            _localizationService = localizationService;
            _inputSupplyService = inputSupplyService;
            _languageService = languageService;
            _unitService = unitService;
            _breedService = breedService;
            _workContext = workContext;
            _fiscalYearService = fiscalYearService;
            _categoryService = categoryService;
            _localLevelService = localLevelService;
            _budgetService = budgetService;
        }

        public IActionResult Index() => RedirectToAction("List");

        public async Task<IActionResult> List() {

            var localLevels = await _localLevelService.GetLocalLevel(ExecutionHelper.District);
            var localLevelSelect = new SelectList(localLevels).ToList();
            localLevelSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.LocalLevels = new SelectList(localLevelSelect, "Text","Text", ExecutionHelper.LocalLevel);

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
            var bali = await _inputSupplyService.GetInputSupply("",fiscalYear,district,locallevel, command.Page - 1, command.PageSize);
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

            var localLevels = await _localLevelService.GetLocalLevel(ExecutionHelper.District);
            var localLevelSelect = new SelectList(localLevels).ToList();
            localLevelSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.LocalLevels = new SelectList(localLevelSelect, "Text","Text",ExecutionHelper.LocalLevel);

            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;

            ViewBag.Types = ExecutionHelper.GetXetras();

            var month = new MonthHelper();
            var months = month.GetMonths();
            months.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Month = months;

            WardHelper wardHelper = new WardHelper();
            var ward = wardHelper.GetWard();
            ward.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Wards = ward;

            InputSupplyModel model = new InputSupplyModel();

            return View(model);
        }


        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(InputSupplyModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                if(String.IsNullOrEmpty(model.CategoryId))
                {
                    var category = await _categoryService.GetCategoryByName(model.CategoryName);
                    if(String.IsNullOrEmpty(category?.Id))
                    {
                        category = new Category() {
                            NameNepali = model.CategoryName,
                            NameEnglish = model.CategoryName,
                            Type = "Input Supply"
                        };
                        await _categoryService.InsertCategory(category);
                        model.CategoryId = category.Id;
                    }
                }

                var inputSupply = model.ToEntity();
                inputSupply.Category = await _categoryService.GetCategoryById(model.CategoryId);
                inputSupply.Unit = await _unitService.GetUnitById(model.UnitId);
                inputSupply.FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYearId);
                if (!String.IsNullOrEmpty(model.BudgetId))
                {
                    inputSupply.Budget = await _budgetService.GetBudgetById(model.BudgetId);
                }

                inputSupply.CreatedBy = _workContext.CurrentCustomer.Id;
                await _inputSupplyService.InsertInputSupply(inputSupply);

                SuccessNotification(_localizationService.GetResource("Admin.Create.successful"));
                return continueEditing ? RedirectToAction("Edit", new { id = inputSupply.Id }) : RedirectToAction("Index");
            }
            var units = new SelectList(await _unitService.GetUnit(), "Id", "UnitNameEnglish").ToList();
            units.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Units = units;

            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;

            ViewBag.Types = ExecutionHelper.GetXetras();

            var month = new MonthHelper();
            var months = month.GetMonths();
            months.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Month = months;

            WardHelper wardHelper = new WardHelper();
            var ward = wardHelper.GetWard();
            ward.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Wards = ward;

            var localLevels = await _localLevelService.GetLocalLevel(ExecutionHelper.District);
            var localLevelSelect = new SelectList(localLevels).ToList();
            localLevelSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.LocalLevels = new SelectList(localLevelSelect, "Text","Text", ExecutionHelper.LocalLevel);

            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var inputSupply = await _inputSupplyService.GetInputSupplyById(id);
            if (inputSupply == null)
                return RedirectToAction("List");
            var model = inputSupply.ToModel();

            model.CategoryName = inputSupply.Category.NameEnglish;

            var units = new SelectList(await _unitService.GetUnit(), "Id", "UnitNameEnglish").ToList();
            units.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Units = units;

            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;

            ViewBag.Types = ExecutionHelper.GetXetras();

            var month = new MonthHelper();
            var months = month.GetMonths();
            months.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Month = months;

            WardHelper wardHelper = new WardHelper();
            var ward = wardHelper.GetWard();
            ward.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Wards = ward;


            var localLevels = await _localLevelService.GetLocalLevel(ExecutionHelper.District);
            var localLevelSelect = new SelectList(localLevels).ToList();
            localLevelSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.LocalLevels = new SelectList(localLevelSelect, "Text","Text", ExecutionHelper.LocalLevel);




            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(InputSupplyModel model, bool continueEditing)
        {
            var inputSupply = await _inputSupplyService.GetInputSupplyById(model.Id);
            if (inputSupply == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var m = model.ToEntity();
                if (String.IsNullOrEmpty(model.CategoryId))
                {
                    var category = await _categoryService.GetCategoryByName(model.CategoryName);
                    if (String.IsNullOrEmpty(category?.Id))
                    {
                        category = new Category() {
                            NameNepali = model.CategoryName,
                            NameEnglish = model.CategoryName,
                            Type = "Input Supply"
                        };
                       
                        await _categoryService.InsertCategory(category);
                        model.CategoryId = category.Id;
                    }
                }
                m.Category = await _categoryService.GetCategoryById(model.CategoryId);
                m.Unit = await _unitService.GetUnitById(model.UnitId);
                m.FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYearId);
                if (!String.IsNullOrEmpty(model.BudgetId))
                {
                    m.Budget = await _budgetService.GetBudgetById(model.BudgetId);
                }

                await _inputSupplyService.UpdateInputSupply(m);

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

            ViewBag.Types = ExecutionHelper.GetXetras();

            var month = new MonthHelper();
            var months = month.GetMonths();
            months.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Month = months;

            WardHelper wardHelper = new WardHelper();
            var ward = wardHelper.GetWard();
            ward.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Wards = ward;

            var localLevels = await _localLevelService.GetLocalLevel(ExecutionHelper.District);
            var localLevelSelect = new SelectList(localLevels).ToList();
            localLevelSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.LocalLevels = new SelectList(localLevelSelect, "Text","Text", ExecutionHelper.LocalLevel);

            //If we got this far, something failed, redisplay form
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);

            return RedirectToAction("Create");
        }


        public virtual async Task<IActionResult> CategoryAutoComplete(string term, string type)
        {
            var result = await _categoryService.GetCategoryByType(type, term);
            return Json(result);
        }
        public async Task<ActionResult> GetBudgetByFiscalYear(string fiscalYearId)
        {           
            var budget = await _budgetService.GetBudget("", fiscalYearId, "", "");

            return Json(budget);
        }
       

        }
    }
