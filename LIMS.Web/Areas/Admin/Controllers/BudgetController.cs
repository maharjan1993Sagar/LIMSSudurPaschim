using LIMS.Core;
using LIMS.Domain.Bali;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Mvc;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Basic;
using LIMS.Services.Customers;
using LIMS.Services.ExportImport;
using LIMS.Services.Localization;
using LIMS.Services.Security;
using LIMS.Services.Stores;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using LIMS.Web.Areas.Admin.Models.Bali;
using LIMS.Web.Areas.Admin.Extensions.Mapping;
using LIMS.Web.Areas.Admin.Helper;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class BudgetController : BaseAdminController
    {
        #region fields
        //private readonly IPujigatKharchaKharakramService _pujigatKharchaKharakramService;
        private readonly IBudgetService _budgetService;
        private readonly ICustomerService _customerService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly IStoreService _storeService;
        //private readonly IExportManager _exportManager;
        private readonly IWorkContext _workContext;
        private readonly IImportManager _importManager;
        private readonly IFiscalYearService _fiscalYearService;
        private IHostingEnvironment _environment;
        #endregion
        #region ctor
        public BudgetController(
            IPujigatKharchaKharakramService pujigatKharchaKharakramService,
            IBudgetService budgetService,
            ICustomerService customerService,
            ILanguageService languageService,
             ILocalizationService localizationService,
             IStoreService storeService,
            //  IExportManager exportManager,
              IWorkContext workContext,
              IImportManager importManager,
              IFiscalYearService fiscalYearService,
              IHostingEnvironment environment

            )
        {
           // _pujigatKharchaKharakramService = pujigatKharchaKharakramService;
            _budgetService = budgetService;
            _customerService = customerService;
            _languageService = languageService;
            _localizationService = localizationService;
            _storeService = storeService;
           // _exportManager = exportManager;
            _workContext = workContext;
            _importManager = importManager;
            _fiscalYearService = fiscalYearService;
            _environment = environment;
        }
        #endregion
        public IActionResult Index() =>RedirectToAction("List");
        public IActionResult TabEntry() => View();

        public IActionResult List() => View();
        public IActionResult SubsidyEntry() => View();
        public IActionResult TrainingChonot() => View();
        public IActionResult InputSupply() => View();

        public async Task<IActionResult> ListTest(string keyword="",string category= "")
        {
           // var createdby = _workContext.CurrentCustomer.Id;
            var budgets = await _budgetService.GetBudget("", keyword, 0, 50);
            var b = budgets.ToList();

            b.ForEach(m => m.TypeOfExecution = ExecutionHelper.GetTypeOfExecution().Where(n => n.Value == m.TypeOfExecution).FirstOrDefault()?.Text);
            b.ForEach(m => m.PlanningProgram = ExecutionHelper.GetPlanningTypes().Where(n => n.Value == m.PlanningProgram).FirstOrDefault()?.Text);
            b.ForEach(m => m.TypeOfExpen = ExecutionHelper.GetPrakar().Where(n => n.Value == m.TypeOfExpen).FirstOrDefault()?.Text);
            b.ForEach(m => m.BudgetBiniyojanType = ExecutionHelper.GetPrakar().Where(n => n.Value == m.BudgetBiniyojanType).FirstOrDefault()?.Text);
            b.ForEach(m => m.Xetra = ExecutionHelper.GetXetras().Where(n => n.Value.ToString() == m.Xetra).FirstOrDefault()?.Text);
            b.ForEach(m => m.UpaXetra = ExecutionHelper.GetUpaXetras().Where(n => n.Value.ToString() == m.UpaXetra).FirstOrDefault()?.Text);
            //  b.ForEach(m => m.MukhyaKaryakram = _context.MukhyaKaryakrams.Where(n => n.Id.ToString() == m.MukhyaKaryakram).FirstOrDefault()?.MukhyaKaryakramKoName);
            b.ForEach(m => m.BudgetBiniyojanType = ExecutionHelper.GetPrakar().Where(n => n.Value == m.BudgetBiniyojanType).FirstOrDefault()?.Text);
            //   b.ForEach(m => m.FirstQuaterBudget = _context.Bhuktanis.Where(n => n.BudgetId == m.Id).FirstOrDefault()?.BudgetId.ToString());
           
            var lstBudget = b.ToList();
            if (!String.IsNullOrEmpty(category))
            {
                lstBudget = lstBudget.Where(m => m.ExpensesCategory == category).ToList();
            }

            var gridModel = new DataSourceResult {
                Data = b,
                Total = budgets.TotalCount
            };

            return View(lstBudget);
           // return Json(gridModel);

        }
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest  command,string keyword) {
            var createdby = _workContext.CurrentCustomer.Id;
            var budgets = await _budgetService.GetBudget("",keyword,command.Page-1, command.PageSize);
            var b = budgets.ToList();

         //   b.ForEach(m => m.TypeOfExecution = ExecutionHelper.GetTypeOfExecution().Where(n => n.Value == m.TypeOfExecution).FirstOrDefault()?.Text);
         //   b.ForEach(m => m.PlanningProgram = ExecutionHelper.GetPlanningTypes().Where(n => n.Value == m.PlanningProgram).FirstOrDefault()?.Text);
         //   b.ForEach(m => m.TypeOfExpen = ExecutionHelper.GetPrakar().Where(n => n.Value == m.TypeOfExpen).FirstOrDefault()?.Text);
         //   b.ForEach(m => m.BudgetBiniyojanType = ExecutionHelper.GetPrakar().Where(n => n.Value == m.BudgetBiniyojanType).FirstOrDefault()?.Text);
         //   b.ForEach(m => m.Xetra = ExecutionHelper.GetXetras().Where(n => n.Value.ToString() == m.Xetra).FirstOrDefault()?.Text);
         //   b.ForEach(m => m.UpaXetra =ExecutionHelper.GetUpaXetras().Where(n => n.Value.ToString() == m.UpaXetra).FirstOrDefault()?.Text);
         // //  b.ForEach(m => m.MukhyaKaryakram = _context.MukhyaKaryakrams.Where(n => n.Id.ToString() == m.MukhyaKaryakram).FirstOrDefault()?.MukhyaKaryakramKoName);
         //   b.ForEach(m => m.BudgetBiniyojanType = ExecutionHelper.GetPrakar().Where(n => n.Value == m.BudgetBiniyojanType).FirstOrDefault()?.Text);
         ////   b.ForEach(m => m.FirstQuaterBudget = _context.Bhuktanis.Where(n => n.BudgetId == m.Id).FirstOrDefault()?.BudgetId.ToString());
           
            var gridModel = new DataSourceResult {
                Data = b,
                Total = budgets.TotalCount
            };
            return Json(gridModel);


        }

        [HttpPost]
        public async Task<IActionResult> ListOther(DataSourceRequest command, string keyword, string category="")
        {
            var createdby = _workContext.CurrentCustomer.Id;
            var budgets = await _budgetService.GetBudget("", keyword, command.Page - 1, command.PageSize);
            var b = budgets.ToList();

            //b.ForEach(m => m.TypeOfExecution = ExecutionHelper.GetTypeOfExecution().Where(n => n.Value == m.TypeOfExecution).FirstOrDefault()?.Text);
            //b.ForEach(m => m.PlanningProgram = ExecutionHelper.GetPlanningTypes().Where(n => n.Value == m.PlanningProgram).FirstOrDefault()?.Text);
            //b.ForEach(m => m.TypeOfExpen = ExecutionHelper.GetPrakar().Where(n => n.Value == m.TypeOfExpen).FirstOrDefault()?.Text);
            //b.ForEach(m => m.BudgetBiniyojanType = ExecutionHelper.GetPrakar().Where(n => n.Value == m.BudgetBiniyojanType).FirstOrDefault()?.Text);
            //b.ForEach(m => m.Xetra = ExecutionHelper.GetXetras().Where(n => n.Value.ToString() == m.Xetra).FirstOrDefault()?.Text);
            //b.ForEach(m => m.UpaXetra = ExecutionHelper.GetUpaXetras().Where(n => n.Value.ToString() == m.UpaXetra).FirstOrDefault()?.Text);
            ////  b.ForEach(m => m.MukhyaKaryakram = _context.MukhyaKaryakrams.Where(n => n.Id.ToString() == m.MukhyaKaryakram).FirstOrDefault()?.MukhyaKaryakramKoName);
            //b.ForEach(m => m.BudgetBiniyojanType = ExecutionHelper.GetPrakar().Where(n => n.Value == m.BudgetBiniyojanType).FirstOrDefault()?.Text);
            ////   b.ForEach(m => m.FirstQuaterBudget = _context.Bhuktanis.Where(n => n.BudgetId == m.Id).FirstOrDefault()?.BudgetId.ToString());

            var lstBudget = b.ToList();
            if (!String.IsNullOrEmpty(category))
            {
                lstBudget  = lstBudget.Where(m => m.ExpensesCategory == category).ToList();
            }

            var gridModel = new DataSourceResult {
                Data = lstBudget,
                Total = lstBudget.Count
            };
            return Json(gridModel);


        }


        public async Task<IActionResult> Create() {
            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            ViewBag.ExpensesCategory = new SelectList(ExecutionHelper.GetExecTypes(), "Value", "Text");

            ViewBag.Chetra = new SelectList(ExecutionHelper.GetXetras(), "Value", "Text");
            //var c = ChetraId.ToList();
            //c.Insert(0, new SelectListItem { Text = "Select", Value = "" });
           // = c;

            ViewBag.UpaChetra = new SelectList(ExecutionHelper.GetUpaXetras(), "Value", "Text");

            //var selected = _context.FiscalYear.Where(m => m.status == true).FirstOrDefault().Id;
            // ViewBag.FiscalYearId = new SelectList(_contextabentyt.FiscalYear, "Id", "nepaliFY", selected);
            //r.FiscalYearId = selected;


            ViewBag.Expences = new SelectList(ExecutionHelper.GetPrakar(), "Value", "Text");

            ViewBag.BiniyojanType = new SelectList(ExecutionHelper.BiniyojanType(), "Value", "Text");

            ViewBag.TypeOFExecution = new SelectList(ExecutionHelper.GetTypeOfExecution(), "Value", "Text");

            ViewBag.Planning = new SelectList(ExecutionHelper.GetPlanningTypes(), "Value", "Text");

            ViewBag.Swrot = new SelectList(ExecutionHelper.Swrot(), "Value", "Text");

            WardHelper ward = new WardHelper();
            ViewBag.WardNo = ward.GetWard();


            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;


            var type = PujigatType();
            type.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Type = type;


            var programType = ProgramType();
            programType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.ProgramType = programType;


            BudgetModel model = new BudgetModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BudgetModel model)
        {
            try
            {
                var p = model.ToEntity();
                p.CreatedAt = DateTime.Now;
                p.CreatedBy = _workContext.CurrentCustomer.Id;
                await _budgetService.InsertBudget(p);
                SuccessNotification(_localizationService.GetResource("Admin.Catalog.Category.Imported"));
                return RedirectToAction("TabEntry");
            }
            catch (Exception ex)
            {
                SuccessNotification(_localizationService.GetResource("Admin.Catalog.Category.Imported"));
                return RedirectToAction("TabEntry");
            }
        }

            public FileResult Download() {

            string filePath= _environment.WebRootPath +"/Import_Budget.xlsx";
            byte[] bytes = System.IO.File.ReadAllBytes(filePath);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", "Import_Budget.xlsx");

        }

        [HttpPost]
        public async Task<IActionResult> ImportBudgetFromXlsx(IFormFile importexcelfile,string Type,string FiscalYear,string ProgramType)
        {
            //a vendor and staff cannot import categories           
            try
            {
                if (importexcelfile != null && importexcelfile.Length > 0)
                {
                    await _importManager.ImportBudgetFromXlsx(importexcelfile.OpenReadStream(),Type,FiscalYear,ProgramType);
                }
                else
                {
                    ErrorNotification(_localizationService.GetResource("Admin.Common.UploadFile"));
                    return RedirectToAction("TabEntry");
                }
                SuccessNotification(_localizationService.GetResource("Admin.Catalog.Category.Imported"));
                return RedirectToAction("TabEntry");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("TabEntry");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ImportFromXlsx(IFormFile importexcelfile, string Type, string FiscalYear, string ProgramType)
        {
            //a vendor and staff cannot import categories

            try
            {
                if (importexcelfile != null && importexcelfile.Length > 0)
                {
                    await _importManager.ImportCategoryFromXlsx(importexcelfile.OpenReadStream(), Type, FiscalYear, ProgramType);
                }
                else
                {
                    ErrorNotification(_localizationService.GetResource("Admin.Common.UploadFile"));
                    return RedirectToAction("TabEntry");
                }
                SuccessNotification(_localizationService.GetResource("Admin.Catalog.Category.Imported"));
                return RedirectToAction("TabEntry");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("TabEntry");
            }
        }
        public async Task<IActionResult> GetFiscalYear() {

            return Json(await _fiscalYearService.GetFiscalYear());

        }
        public List<SelectListItem> PujigatType()
        {

            return new List<SelectListItem>() {
                new SelectListItem {
                    Text="chalu",
                    Value="chalu"

                },
                 new SelectListItem {
                    Text="pujigat",
                    Value="pujigat"

                },
                // new SelectListItem {
                //    Text="kha si na.",
                //    Value="kha si na."

                //}
            };

        }

        public List<SelectListItem> ProgramType()
        {

            return new List<SelectListItem>() {
                new SelectListItem {
                    Text=_localizationService.GetResource("Lims.PujigatKharcha.SanghKoSasarthaAnudanAntargat"),
                    Value="Lims.PujigatKharcha.SanghKoSasarthaAnudanAntargat",

                },
                 new SelectListItem {
                    Text=_localizationService.GetResource("Lims.PujigatKharcha.PardeshKoBajetAntargat"),
                    Value="Lims.PujigatKharcha.PardeshKoBajetAntargat",


                },
                
            };

        }
        public async Task<IActionResult> Edit(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                var budget =await _budgetService.GetBudgetById(id);
                var model = budget.ToModel();


                var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

                ViewBag.ExpensesCategory = new SelectList(ExecutionHelper.GetExecTypes(), "Value", "Text", model.ExpensesCategory);

                ViewBag.Chetra = new SelectList(ExecutionHelper.GetXetras(), "Value", "Text", model.Xetra);
               

                ViewBag.UpaChetra = new SelectList(ExecutionHelper.GetUpaXetras(), "Value", "Text", model.UpaXetra);

              

                ViewBag.Expences = new SelectList(ExecutionHelper.GetPrakar(), "Value", "Text", model.TypeOfExpen);

                ViewBag.BiniyojanType = new SelectList(ExecutionHelper.BiniyojanType(), "Value", "Text", model.BudgetBiniyojanType);

                ViewBag.TypeOFExecution = new SelectList(ExecutionHelper.GetTypeOfExecution(), "Value", "Text",model.TypeOfExecution);

                ViewBag.Planning = new SelectList(ExecutionHelper.GetPlanningTypes(), "Value", "Text", model.PlanningProgram);

                ViewBag.Swrot = new SelectList(ExecutionHelper.Swrot(), "Value", "Text", model.SourceOfFund);

                WardHelper ward = new WardHelper();
                ViewBag.WardNo = ward.GetWard();


                var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
                fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
                ViewBag.FiscalYearId = fiscalYear;


                var type = PujigatType();
                type.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
                ViewBag.Type = type;


                var programType = ProgramType();
                programType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
                ViewBag.ProgramType = programType;


                return View(model);
            }
            else
            {
                return RedirectToAction("TabEntry");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id,BudgetModel model)
        {
            var budget = model.ToEntity();
            budget.CreatedBy = _workContext.CurrentCustomer.Id;
            budget.CreatedAt = DateTime.Now;


            var objBudget = await _budgetService.GetBudgetById(budget.Id);
            if (objBudget == null)
                throw new ArgumentException("No program found with the specified id ");
            if (ModelState.IsValid)
            {                
                await _budgetService.UpdateBudget(budget);
                SuccessNotification(_localizationService.GetResource("Admin.Catalog.Category.Imported"));
                return RedirectToAction("TabEntry");
            }


            ErrorNotification(_localizationService.GetResource("Admin.Common.UploadFile"));
            return RedirectToAction("TabEntry");
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, BudgetModel model)
        {
            var budget = model.ToEntity();

            var objBudget = await _budgetService.GetBudgetById(budget.Id);
            objBudget.ExpensesCategory = model.ExpensesCategory;
            objBudget.CreatedBy = _workContext.CurrentCustomer.Id;
            objBudget.CreatedAt = DateTime.Now;

            if (objBudget == null)
                throw new ArgumentException("No program found with the specified id ");
            if (ModelState.IsValid)
            {
                await _budgetService.UpdateBudget(objBudget);

                return new NullJsonResult();
            }


            return ErrorForKendoGridJson(ModelState);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var objBudget = await _budgetService.GetBudgetById(id);
            if (objBudget == null)
                throw new ArgumentException("No program found with the specified id ");
            if (ModelState.IsValid)
            {
                await _budgetService.DeleteBudget(objBudget);
                return new NullJsonResult();
            }
            return ErrorForKendoGridJson(ModelState);
        }

    }
}
