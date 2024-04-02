using LIMS.Core;
using LIMS.Domain.MoAMAC;
using LIMS.Framework.Controllers;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Ainr;
using LIMS.Services.Bali;
using LIMS.Services.Basic;
using LIMS.Services.Breed;
using LIMS.Services.Common;
using LIMS.Services.Customers;
using LIMS.Services.Localization;
using LIMS.Services.LocalStructure;
using LIMS.Services.MedicineInventory;
using LIMS.Services.MoAMAC;
using LIMS.Services.Security;
using LIMS.Services.Statisticaldata;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Web.Areas.Admin.Models.Reports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Wkhtmltopdf.NetCore;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class ReportController : BaseAdminController
    {
        private readonly ISpeciesService _speciesService;
        private readonly IBreedService _breedService;
        private readonly IAnimalTypeService _animalTypeService;
        private readonly ILivestockService _livestockService;
        private readonly IFarmService _farmService;
        private readonly ILocalizationService _localizationService;
        private readonly ILanguageService _languageService;
        private readonly IWorkContext _workContext;
        public readonly ICustomerService _customerService;
        private readonly IReceivedMedicineService _receivedMedicineService;
        private readonly IMedicineProgressService _medicineProgressService;
        public readonly IFiscalYearService _fiscalYearService;
        public readonly ILssService _lssService;
        public readonly IVhlsecService _vhlsecService;
        public readonly ILocalLevelService _localLevelService;
        public readonly IBudgetService _budgetService;
        public readonly IGeneratePdf _generatePdf;
        public readonly IPdfService _pdfService;
        public readonly IAnudanService _anudanService;
        public readonly IWorkContext workContext;
        public readonly ITalimService _talimService;
        // public readonly IMonthlyPragatiService _MonthlyPragatiService;

        public ReportController(ILocalizationService localizationService,
            IAnimalRegistrationService animalRegistrationService,
            IFarmService farmService,
            ILanguageService languageService,
            ISpeciesService speciesService,
            IBreedService breedService,
            IAnimalTypeService animalTypeService,
            ILivestockService livestockService,
            ICustomerService customerService,
            IReceivedMedicineService receivedMedicineService,
            IMedicineProgressService medicineProgressService,
            IFiscalYearService fiscalYearService,
            ILssService lssService,
            IWorkContext workContext,
            IVhlsecService vhlsecService,
            ILocalLevelService localLevelService,
            IBudgetService budgetService,
            IGeneratePdf generatePdf,
            IPdfService pdfService,
            IAnudanService anudanService,
            ITalimService talimService
            )
        {
            _localizationService = localizationService;

            _languageService = languageService;
            _farmService = farmService;
            _speciesService = speciesService;
            _breedService = breedService;
            _workContext = workContext;
            _animalTypeService = animalTypeService;
            _livestockService = livestockService;
            _customerService = customerService;
            _receivedMedicineService = receivedMedicineService;
            _medicineProgressService = medicineProgressService;
            _fiscalYearService = fiscalYearService;
            _lssService = lssService;
            _vhlsecService = vhlsecService;
            _localLevelService = localLevelService;
            _budgetService = budgetService;
            _generatePdf = generatePdf;
            _pdfService = pdfService;
            _anudanService = anudanService;
            _talimService = talimService;

        }

        public async Task<IActionResult> LivestockReport()
        {
            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;
            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "NepaliName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Species = species;
            string entityId = _workContext.CurrentCustomer.EntityId;
            var LssIds = new List<Lss>();
          
              LssIds.AddRange(await _lssService.GetLssByVhlsecId(entityId));
            
            var lss = new SelectList(LssIds, "Id", "NameEnglish").ToList();
            lss.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.LocalLevel = lss;
            return View();


        }
        public async Task<IActionResult> LivestockReportNepali()
        {
            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;
            string entityId = _workContext.CurrentCustomer.EntityId;
            var LssIds = new List<Lss>();

            LssIds.AddRange(await _lssService.GetLssByVhlsecId(entityId));

            var lss = new SelectList(LssIds, "Id", "NameNepali").ToList();
            lss.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.LocalLevel = lss;

            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "NepaliName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Species = species;
            return View();


        }


        public async Task<IActionResult> SubsidyReportNepali()
        {
            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;
            string entityId = _workContext.CurrentCustomer.EntityId;
            var LssIds = new List<Lss>();

            LssIds.AddRange(await _lssService.GetLssByVhlsecId(entityId));

            var lss = new SelectList(LssIds, "Id", "NameNepali").ToList();
            lss.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.LocalLevel = lss;
           
            return View();


        }
        
         [HttpPost]
        public virtual IActionResult SubsidyReportNepaliHtml(string FiscalYear, string LocalLevel = null)
        {

            var livestockWardWiseReportHtml = RenderViewComponentToString("SubsidyReportNepali", new { fiscalyear = FiscalYear });

            return Json(new
            {
                success = true,
                message = string.Format(_localizationService.GetResource("Admin.Report.LivestockWardWiseReport.Success")),
                livestockWardWiseReportHtml
            });
        }

        public async Task<IActionResult> OrgwiseSubsidyReportNepali()
        {
            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;
            string entityId = _workContext.CurrentCustomer.EntityId;
            var LssIds = new List<Lss>();

            LssIds.AddRange(await _lssService.GetLssByVhlsecId(entityId));

            var lss = new SelectList(LssIds, "Id", "NameNepali").ToList();
            lss.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.LocalLevel = lss;

            return View();


        }

        [HttpPost]
        public virtual IActionResult OrgWiseSubsidyReportNepaliHtml(string FiscalYear, string LocalLevel = null)
        {

            var livestockWardWiseReportHtml = RenderViewComponentToString("OrgWiseSubsidyReportNepali", new { fiscalyear = FiscalYear });

            return Json(new
            {
                success = true,
                message = string.Format(_localizationService.GetResource("Admin.Report.LivestockWardWiseReport.Success")),
                livestockWardWiseReportHtml
            });
        }


        public async Task<IActionResult> LivestockReportDolfd()
        {
            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;
            string entityId = _workContext.CurrentCustomer.EntityId;
            List<Vhlsec> dolfdid = _vhlsecService.GetVhlsecByDolfdId(entityId).Result.ToList();

            var lss = new SelectList(dolfdid, "Id", "NameEnglish").ToList();
            lss.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.LocalLevel = lss;
            return View();


        }
        public async Task<IActionResult> LivestockReportDolfdNepali()
        {
            var f = await _fiscalYearService.GetCurrentFiscalYear();
            string entityId = "610769807e6b0bcd5beed52c";
            var fiscalyear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear",f.Id).ToList();
            fiscalyear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalyear;
          
            List<Vhlsec> dolfdid = _vhlsecService.GetVhlsecByDolfdId(entityId).Result.ToList();

            var lss = new SelectList(dolfdid, "Id", "NameNepali").ToList();
            lss.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.LocalLevel = lss; 
            return View();


        }


        [HttpPost]
        public virtual IActionResult LivestockDolfdReportNepaliHtml(string FiscalYear, string LocalLevel = null)
        {
            
            var livestockWardWiseReportHtml = RenderViewComponentToString("LivestockDolfdReportNepali", new { fiscalyear = FiscalYear, lssid = LocalLevel });

            return Json(new
            {
                success = true,
                message = string.Format(_localizationService.GetResource("Admin.Report.LivestockWardWiseReport.Success")),
                livestockWardWiseReportHtml
            });
        }

        [HttpPost]
        public virtual IActionResult LivestockDolfdReportHtml(string FiscalYear,string LocalLevel=null)
        {
            var livestockWardWiseReportHtml = RenderViewComponentToString("LivestockDolfdReport", new { fiscalyear = FiscalYear,lssid=LocalLevel });

            return Json(new
            {
                success = true,
                message = string.Format(_localizationService.GetResource("Admin.Report.LivestockWardWiseReport.Success")),
                livestockWardWiseReportHtml
            });
        }

        public async Task<IActionResult> TrainingReport()
        {
            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;

            var localLevels = await _localLevelService.GetLocalLevel("KATHMANDU");
            var localLevelSelect = new SelectList(localLevels).ToList();
            //localLevelSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.LocalLevels = new SelectList(localLevelSelect, "Text", "Text", ExecutionHelper.LocalLevel);

            ViewBag.Xetras = new SelectList(ExecutionHelper.GetXetras(), "Value", "Text");


            string entityId = _workContext.CurrentCustomer.EntityId;
            var LssIds = new List<Lss>();

            LssIds.AddRange(await _lssService.GetLssByVhlsecId(entityId));

            //var lss = new SelectList(LssIds, "Id", "NameNepali").ToList();
            //lss.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            //ViewBag.LocalLevel = lss;

            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "NepaliName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Species = species;

            var model = new TrainingReportModel();

            return View(model);


        }

        public async Task<IActionResult> SubsidyReport()
        {
            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;

            var localLevels = await _localLevelService.GetLocalLevel("KATHMANDU");
            var localLevelSelect = new SelectList(localLevels).ToList();
            //localLevelSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.LocalLevels =new SelectList(localLevelSelect.ToList(),"Text","Text",ExecutionHelper.LocalLevel);

            ViewBag.Xetras = new SelectList(ExecutionHelper.GetXetras(), "Value", "Text");

            string entityId = _workContext.CurrentCustomer.EntityId;

            //var LssIds = new List<Lss>();
            //LssIds.AddRange(await _lssService.GetLssByVhlsecId(entityId));
            //var lss = new SelectList(LssIds, "Id", "NameNepali").ToList();
            //lss.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            //ViewBag.LocalLevel = lss;


            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "NepaliName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Species = species;

            var model =new SubsidyReportModel();

            return View(model);


        }
        public async Task<IActionResult> ProgressReport()
        {
            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;

            ViewBag.Xetras = new SelectList(ExecutionHelper.GetXetras(), "Value", "Text");

            var month = new MonthHelper();
            var months = month.GetMonths();
            months.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.AllMonths"), ""));
            ViewBag.Month = months;

            string entityId = _workContext.CurrentCustomer.EntityId;
            var LssIds = new List<Lss>();

            LssIds.AddRange(await _lssService.GetLssByVhlsecId(entityId));

            //var lss = new SelectList(LssIds, "Id", "NameNepali").ToList();
            //lss.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            //ViewBag.LocalLevel = lss;

            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "NepaliName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Species = species;

            var model = new ProgressReportModel();

            return View(model);


        }
        [HttpPost]
        public virtual IActionResult TrainingReportHtml(string FiscalYear,string BudgetId, string LocalLevel = "", string xetra="")
        {
           
            var livestockWardWiseReportHtml = RenderViewComponentToString("TrainingReport", new { fiscalyear = FiscalYear, budgetId = BudgetId, localLevel = LocalLevel, xetra = xetra });

            return Json(new
            {
                success = true,
                message = string.Format(_localizationService.GetResource("Admin.Report.LivestockWardWiseReport.Success")),
                livestockWardWiseReportHtml
            });
        }
        [HttpPost]
        public virtual IActionResult SubsidyReportHtml(string FiscalYear, string LocalLevel = "",string xetra="")
        {

            var livestockWardWiseReportHtml = RenderViewComponentToString("SubsidyReport", new { fiscalyear = FiscalYear, LocalLevel = LocalLevel, xetra= xetra });

            return Json(new
            {
                success = true,
                message = string.Format(_localizationService.GetResource("Admin.Report.LivestockWardWiseReport.Success")),
                livestockWardWiseReportHtml
            });
        }
        [HttpPost]
        public virtual IActionResult ProgressReportHtml(string FiscalYear,  string xetra = "", string month ="")
        {
            var livestockWardWiseReportHtml = RenderViewComponentToString("ProgressReport", new { fiscalyear = FiscalYear, xetra = xetra, month = month });

            return Json(new
            {
                success = true,
                message = string.Format(_localizationService.GetResource("Admin.Report.LivestockWardWiseReport.Success")),
                livestockWardWiseReportHtml
            });
        }


        public async Task<IActionResult> TrainingDetailReport()
        {
            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;

            var localLevels = await _localLevelService.GetLocalLevel("KATHMANDU");
            var localLevelSelect = new SelectList(localLevels).ToList();
            //localLevelSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.LocalLevels = new SelectList(localLevelSelect, "Text", "Text", ExecutionHelper.LocalLevel);

            // var trainings = await _training

            string entityId = _workContext.CurrentCustomer.EntityId;
            var c = await _fiscalYearService.GetCurrentFiscalYear();
           

            string createdby = _workContext.CurrentCustomer.Id;
           
            var talim = new SelectList(await _talimService.Gettalim(createdby), "Id", "NameEnglish").ToList();
            talim.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Talim = talim;

            ViewBag.Xetras = new SelectList(ExecutionHelper.GetXetras(), "Value", "Text");


            var model = new TrainingReportModel();

            return View(model);


        }
        public async Task<IActionResult> SubsidyDetailReport()
        {
            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;

            var localLevels = await _localLevelService.GetLocalLevel("KATHMANDU");
            var localLevelSelect = new SelectList(localLevels).ToList();
           // localLevelSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.LocalLevels = new SelectList(localLevelSelect, "Text", "Text", ExecutionHelper.LocalLevel);

            var budget = await _budgetService.GetBudget("");
            var anudan = budget.Where(m => m.ExpensesCategory == "Subsidy").ToList();

            var budgetSelect = new SelectList(anudan, "Id", "ActivityName").ToList();
            budgetSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Budget = budgetSelect;

            ViewBag.Xetras = new SelectList(ExecutionHelper.GetXetras(), "Value", "Text");


            string entityId = _workContext.CurrentCustomer.EntityId;

            var model = new SubsidyReportModel();

            return View(model);


        }
        public async Task<IActionResult> AgricultureCoOperativeReport()
        {
            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;

            var model = new SubsidyReportModel();

            return View(model);


        }
        [HttpPost]
        public virtual IActionResult AgricultureCoOperativeReportHtml(string FiscalYear)
        {

            var livestockWardWiseReportHtml = RenderViewComponentToString("AgricultureCoOperativeReport", new { fiscalyear = FiscalYear});

            return Json(new
            {
                success = true,
                message = string.Format(_localizationService.GetResource("Admin.Report.AgricultureCoOperativeReport.Success")),
                livestockWardWiseReportHtml
            });
        }
        public async Task<IActionResult> FertilizerShopReport()
        {
            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;

            var model = new SubsidyReportModel();

            return View(model);


        }
        [HttpPost]
        public virtual IActionResult FertilizerShopReportHtml(string FiscalYear)
        {
            var livestockWardWiseReportHtml = RenderViewComponentToString("FertilizerShopReport", new { fiscalyear = FiscalYear});

            return Json(new
            {
                success = true,
                message = string.Format(_localizationService.GetResource("Admin.Report.FertilizerShopReport.Success")),
                livestockWardWiseReportHtml
            });
        }
        [HttpPost]
        public virtual IActionResult TrainingDetailReportHtml(string FiscalYear, string BudgetId, string LocalLevel = "",string xetra="",string talimId="")
        {
            var livestockWardWiseReportHtml = RenderViewComponentToString("TrainingDetailReport", new { fiscalyear = FiscalYear, budgetId = BudgetId, localLevel = LocalLevel, xetra =xetra,talimId = talimId });

            return Json(new
            {
                success = true,
                message = string.Format(_localizationService.GetResource("Admin.Report.LivestockWardWiseReport.Success")),
                livestockWardWiseReportHtml
            });
        }
        [HttpPost]
        public virtual IActionResult SubsidyDetailReportHtml(string FiscalYear, string budgetId, string LocalLevel = "",string xetra="")
        {

            var livestockWardWiseReportHtml = RenderViewComponentToString("SubsidyDetailReport", new { fiscalyear = FiscalYear, budgetId =budgetId, LocalLevel = LocalLevel,xetra =xetra });

            return Json(new
            {
                success = true,
                message = string.Format(_localizationService.GetResource("Admin.Report.LivestockWardWiseReport.Success")),
                livestockWardWiseReportHtml
            });
        }
        [PermissionAuthorizeAction(PermissionActionName.Export)]
        [HttpPost]
        //[FormValueRequired("download-subsidy-pdf")]
        public async Task<IActionResult> DownloadSubsidyPdf(string FiscalYear, string LocalLevel = "")
        {
            var reportModel =await _anudanService.GetSubsidyReportModel("",FiscalYear,LocalLevel,"");
                //RenderViewComponentToString("SubsidyReport", new { fiscalyear = FiscalYear, LocalLevel = LocalLevel });
                       
            try
            {
                byte[] bytes;
                using (var stream = new MemoryStream())
                {
                   await _pdfService.PrintSubsidyPdf(stream,reportModel);
                   bytes = stream.ToArray();
                }
                return File(bytes, "application/octet-stream", "subsidy.pdf");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("SubsidyReport");
            }
        }

        [PermissionAuthorizeAction(PermissionActionName.Export)]
        [HttpPost]
        //[FormValueRequired("download-talim-pdf")]
        public async Task<IActionResult> DownloadTrainingPdf(string FiscalYear, string BudgetId, string LocalLevel = "")
        {
            var livestockWardWiseReportHtml = RenderViewComponentToString("TrainingReport", new { fiscalyear = FiscalYear, budgetId = BudgetId, localLevel = LocalLevel });

            try
            {
                byte[] bytes;
                using (var stream = new MemoryStream())
                {
                  //  await pdfService.PrintToPdf(stream, livestockWardWiseReportHtml);
                    bytes = _generatePdf.GetPDF(livestockWardWiseReportHtml);
                   // bytes = stream.ToArray();
                }
                return File(bytes, "application/pdf", "training.pdf");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("TrainingReport");
            }
        }
        //[PermissionAuthorizeAction(PermissionActionName.Export)]
        //[HttpPost]
        //[FormValueRequired("download-training-excel")]
        //public async Task<IActionResult> DownloadSubsidyExcel( string FiscalYear, string LocalLevel = "")
        //{
        //    var earTags = await _earTagService.GetEarTags(model.From, model.To);
        //    var newEarTags = earTags.Select(x => new LIMS.Domain.AInR.EarTag { EarTagNo = Convert.ToInt32(x.EarTagNo).ToString() }).ToList();
        //    newEarTags.Reverse();
        //    try
        //    {
        //        byte[] bytes = exportManager.ExpertEarTagToXlsx(newEarTags);

        //        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "eartags.xlsx");
        //    }
        //    catch (Exception exc)
        //    {
        //        ErrorNotification(exc);
        //        return RedirectToAction("EarTag");
        //    }
        //}


    }
}
