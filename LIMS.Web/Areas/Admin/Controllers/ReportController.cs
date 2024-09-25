using Google.Apis.AnalyticsReporting.v4.Data;
using LIMS.Core;
using LIMS.Domain.MoAMAC;
using LIMS.Domain.StatisticalData;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Ainr;
using LIMS.Services.Basic;
using LIMS.Services.Breed;
using LIMS.Services.Customers;
using LIMS.Services.Localization;
using LIMS.Services.MedicineInventory;
using LIMS.Services.MoAMAC;
using LIMS.Services.Security;
using LIMS.Services.Statisticaldata;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Web.Areas.Admin.Models;
using LIMS.Web.Areas.Admin.Models.Reports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static LIMS.Web.Areas.Admin.Models.Reports.LivestockReport;

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
            IVhlsecService vhlsecService
            
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
        }

        public async Task<IActionResult> ProgressReport()
        {
            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;

            //ViewBag.Xetras = new SelectList(ExecutionHelper.GetXetras(), "Value", "Text");

            var month = new MonthHelper();
            var months = month.GetMonths();
            months.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.AllMonths"), ""));
            ViewBag.Month = months;

            string entityId = _workContext.CurrentCustomer.EntityId;

            //var species = new SelectList(await _speciesService.GetSpecies(), "Id", "NepaliName").ToList();
            //species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            //ViewBag.Species = species;

            var model = new ProgressReportModel();

            return View(model);


        }

        [HttpPost]
        public virtual IActionResult ProgressReportHtml(string FiscalYear, string xetra = "", string month = "")
        {
            var livestockWardWiseReportHtml = RenderViewComponentToString("ProgressReport", new { fiscalyear = FiscalYear, xetra = xetra, month = month });

            return Json(new
            {
                success = true,
                message = string.Format(_localizationService.GetResource("Admin.Report.LivestockWardWiseReport.Success")),
                livestockWardWiseReportHtml
            });
        }


        public async Task<IActionResult> LivestockReport()
        {
            var fiscalyear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear").ToList();
            fiscalyear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYear = fiscalyear;
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
            var fiscalyear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear").ToList();
            fiscalyear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYear = fiscalyear;
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
            var fiscalyear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear").ToList();
            fiscalyear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYear = fiscalyear;
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
            var fiscalyear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear").ToList();
            fiscalyear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYear = fiscalyear;
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
            var fiscalyear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear").ToList();
            fiscalyear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYear = fiscalyear;
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
            ViewBag.FiscalYear = fiscalyear;
          
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

     
      

    }
}
