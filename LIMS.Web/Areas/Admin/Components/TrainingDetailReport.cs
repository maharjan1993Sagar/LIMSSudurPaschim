﻿using LIMS.Core;
using LIMS.Framework.Components;
using LIMS.Services.Ainr;
using LIMS.Services.Bali;
using LIMS.Services.Basic;
using LIMS.Services.Breed;
using LIMS.Services.Customers;
using LIMS.Services.Localization;
using LIMS.Services.LocalStructure;
using LIMS.Services.MoAMAC;
using LIMS.Web.Areas.Admin.Models.Breed;
using LIMS.Web.Areas.Admin.Models.Reports;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Components
{
    public class TrainingDetailReport : BaseViewComponent
    {
        #region fields
        private readonly ILocalizationService _localizationService;
        private readonly ISpeciesService _speciesService;
        private readonly IUnitService _unitService;
        public readonly IFiscalYearService _fiscalYearService;
        public readonly ILivestockBreedService _breedService;
        public readonly IWorkContext _workContext;
        public readonly IVarietyService _livestockService;
        public readonly ILssService _lssService;
        public readonly ICustomerService _customerService;
        public readonly IAnimalTypeService _animalTypeService;
        public readonly IFarmService _farmService;
        public readonly ICropsSeason _cropSeason;
        public readonly IFarmerService _farmerService;
        private readonly ILocalLevelService _localLevelService;
        private readonly ITalimService _talimService;
        private readonly IBudgetService _budgetService;


        #endregion fields
        #region ctor
        public TrainingDetailReport(ILocalizationService localizationService,
            ISpeciesService speciesService,
             IUnitService unitService,
              IFiscalYearService fiscalYearService,
              ILivestockBreedService breedService,
              IWorkContext workContext,
              IVarietyService livestockService,
              ILssService lssService,
              ICustomerService customerService,
              IAnimalTypeService animalTypeService,
              IFarmService farmService,
              ICropsSeason cropSeason,
              IFarmerService farmerService,
              ITalimService talimService,
              ILocalLevelService localLevelService,
              IBudgetService budgetService
             )
        {
            _localizationService = localizationService;
            _speciesService = speciesService;
            _unitService = unitService;
            _fiscalYearService = fiscalYearService;
            _breedService = breedService;
            _workContext = workContext;
            _livestockService = livestockService;
            _lssService = lssService;
            _customerService = customerService;
            _animalTypeService = animalTypeService;
            _farmService = farmService;
            _cropSeason = cropSeason;
            _farmerService = farmerService;
            _talimService = talimService;
            _localLevelService = localLevelService;
            _budgetService = budgetService;
        }
        #endregion ctor
        public async Task<IViewComponentResult> InvokeAsync(string fiscalyear, string budgetId, string locallevel, string xetra,string talimId)
        {
            var roles = _workContext.CurrentCustomer.CustomerRoles.Select(m => m.Name).ToList();


            if (roles.Contains("Agriculture"))
            {
                xetra = "कृषि विकास";
            }
            if (roles.Contains("Livestock"))
            {
                xetra = "पशु तथा मत्स्य विकास ";
            }
            if (roles.Contains("Administrators"))
            {
                xetra = "";
            }
           

            var t = await _farmerService.GetfarmerByPugigatType("", "", budgetId, fiscalyear, talimId, xetra);           

            //var objTrainingReport = new TrainingReportModel();
            //objTrainingReport.FiscalYear = talims.ToList().FirstOrDefault().FiscalYear.NepaliFiscalYear;
            //objTrainingReport.Address = "काठमाडौँ";
            //objTrainingReport.LocalLevel = locallevel;
            //objTrainingReport.Level = "नगर कार्यपालिकाको कार्यालय";
            //objTrainingReport.StartDate = "";
            //objTrainingReport.EndDate = "";
            //objTrainingReport.Ward = "";
            //var lstTrainingReportData = new List<TrainingRowData>();
            //var distinctTraining = talims.ToList().Select(m => m.TalimId).Distinct();
            //int i = 1;
            //foreach (var item in distinctTraining)
            //{
            //    var objTraining = talims.ToList().FirstOrDefault(m => m.TalimId == item);
            //    var objReportData = new TrainingRowData {
            //        BudgetTitle = objTraining.Budget.ActivityName,
            //        MainActivity = objTraining.Purpose,
            //        Remarks = objTraining.Remarks,
            //        SN = i.ToString(),
            //        TrainingTitle = objTraining.Talim.NameNepali
            //    };
            //    lstTrainingReportData.Add(objReportData);
            //    i++;
            //}
            //objTrainingReport.Rows = lstTrainingReportData;

            return View(t);


            //List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();

            // string createdby = null;
            // if (roles.Contains("MolmacAdmin") || roles.Contains("MolmacAdmin"))
            // {
            //     createdby = "molmac";
            // }
            // else
            // {
            //     createdby = _workContext.CurrentCustomer.Id;
            // }
            //     var currenFiscal = await _fiscalYearService.GetCurrentFiscalYear();
            //     var livestocks = await _livestockService.GetFilteredProduction(createdby,speciesId,fiscalyear,locallevel,district);
            // List<CropsProductionModel> cropsProductions = new List<CropsProductionModel>();
            // var live =livestocks.OrderBy(m => m.CropName.Id).GroupBy(m=>m.GrowingSeason).Select(m=>new CropsProductionModel {
            //     CropName = m.First().CropName.EnglishName,
            //     Season = m.First().GrowingSeason.GrowingSeason,
            //     Production = Convert.ToString(m.Sum(m=>Convert.ToDecimal(m.Production))),
            //     Area =Convert.ToString(m.Sum(m=>Convert.ToInt32(m.Area))),
            //     Yeald = Convert.ToString(Math.Round(Convert.ToDecimal(m.Sum(m => Convert.ToDecimal(m.Production))) / Convert.ToDecimal(m.Sum(m => Convert.ToInt32(m.Area)))))
            // }
            //     ).ToList();
            //foreach(var item in live)
            //{
            //    cropsProductions.Add(
            //      new CropsProductionModel {
            //        CropName=item.,
            //        Season=item.GrowingSeason.GrowingSeason,
            //        Production=item.Production,
            //        Area=item.Area,
            //        Yeald=Convert.ToString(Convert.ToDecimal(item.Production)/ Convert.ToDecimal(item.Area))
            //      });
            //}
        }
    }
}