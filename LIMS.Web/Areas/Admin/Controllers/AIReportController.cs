﻿using LIMS.Core;
using LIMS.Services.Ainr;
using LIMS.Services.Basic;
using LIMS.Services.Breed;
using LIMS.Services.Customers;
using LIMS.Services.Localization;
using LIMS.Services.MoAMAC;
using LIMS.Services.Statisticaldata;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Web.Areas.Admin.Models.Reports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class AIReportController : BaseAdminController
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
        public readonly IFiscalYearService _fiscalYearService;
        public readonly IProductionionDataService _productionionDataService;
        public readonly ILssService _lssService;
        public readonly IServiceData _serviceData;
        public AIReportController(ILocalizationService localizationService,
            IAnimalRegistrationService animalRegistrationService,
            IFarmService farmService,
            ILanguageService languageService,
            ISpeciesService speciesService,
            IBreedService breedService,
            IAnimalTypeService animalTypeService,
            ILivestockService livestockService,
            ICustomerService customerService,
            IFiscalYearService fiscalYearService,
            IProductionionDataService productionionDataService,
            ILssService lssService,
            IWorkContext workContext,
            IServiceData serviceData

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
            _fiscalYearService = fiscalYearService;
            _productionionDataService = productionionDataService;
            _lssService = lssService;
            _serviceData = serviceData;
        }

        public async Task<IActionResult> Index()
        {
            var fiscalyear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear").ToList();
            fiscalyear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYear = fiscalyear;
            return View();

        }
        
        public async Task<IActionResult> Report(string FiscalYear)
        {
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
            if (roles.Contains(RoleHelper.VhlsecAdmin) || roles.Contains(RoleHelper.VhlsecUser))
          
            {
                string vhlsecid = _workContext.CurrentCustomer.EntityId;
                List<string> lssId = _lssService.GetLssByVhlsecId(vhlsecid).Result.Select(m => m.Id).ToList();
                var customers = _customerService.GetCustomerByLssId(lssId, vhlsecid);
                List<string> customerid = customers.Select(x => x.Id).ToList();
                var aidata = await _serviceData.GetService(customerid, "ai", FiscalYear);
                List<string> municipility = aidata.Select(m => m.LocalLevel).ToList();
               var report = new List<AIReport>();
                foreach(var item in municipility)
                {
                    report.Add(new AIReport {
                        Cow = aidata.Where(m => m.Species.EnglishName.ToLower() == "cow" &&m.LocalLevel==item).Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity)),
                       Goat = aidata.Where(m => m.Species.EnglishName.ToLower() == "goat" && m.LocalLevel == item).Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity)),
                        Pig = aidata.Where(m => m.Species.EnglishName.ToLower() == "pig" && m.LocalLevel == item).Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity)),
                        Buffalo = aidata.Where(m => m.Species.EnglishName.ToLower() == "buffalo" && m.LocalLevel == item).Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity)),
                        
                    }); 
                }
                return View(report);


            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ReportToHtml(string FiscalYear)
        {
            var livestockWardWiseReportHtml = RenderViewComponentToString("AiReport", new { fiscalyear = FiscalYear });

            return Json(new
            {
                success = true,
                message = string.Format(_localizationService.GetResource("Admin.Report.LivestockWardWiseReport.Success")),
                livestockWardWiseReportHtml
            });
        }
    

    }
}
