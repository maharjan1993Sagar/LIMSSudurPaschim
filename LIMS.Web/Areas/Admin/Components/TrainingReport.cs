using LIMS.Core;
using LIMS.Framework.Components;
using LIMS.Services.Ainr;
using LIMS.Services.Bali;
using LIMS.Services.Basic;
using LIMS.Services.Breed;
using LIMS.Services.Customers;
using LIMS.Services.Localization;
using LIMS.Services.LocalStructure;
using LIMS.Services.MoAMAC;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Web.Areas.Admin.Models.Breed;
using LIMS.Web.Areas.Admin.Models.Reports;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Components
{
    public class TrainingReport : BaseViewComponent
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
        private readonly IPujigatKharchaKharakramService _budgetService;


        #endregion fields
        #region ctor
        public TrainingReport(ILocalizationService localizationService,
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
              IPujigatKharchaKharakramService budgetService
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
        public async Task<IViewComponentResult> InvokeAsync(string fiscalyear, string budgetId, string locallevel, string xetra)
        {
            var createdBy = _workContext.CurrentCustomer.Id;
            var roles = _workContext.CurrentCustomer.CustomerRoles.Select(m => m.Name).ToList();
            string orgName = _workContext.CurrentCustomer.OrgName;
            string orgAddress = _workContext.CurrentCustomer.OrgAddress;
            string orgLevel = "नगर कार्यपालिकाको कार्यालय";
            //if (roles.Contains("Agriculture"))
            //{
            //    xetra = "कृषि विकास";
            //}
            //if (roles.Contains("Livestock"))
            //{
            //    xetra = "पशु तथा मत्स्य विकास ";
            //}
            //if (roles.Contains("Administrators"))
            //{
            //    xetra = "";
            //}
            var talims = await _farmerService.GetfarmerByPugigatType(createdBy, "", budgetId, fiscalyear, "");

            var objTrainingReport = new TrainingReportModel();

            if (!talims.Any())
            {
                return View(objTrainingReport);
            }
            objTrainingReport.FiscalYear = talims.ToList().FirstOrDefault().FiscalYear.NepaliFiscalYear;
            objTrainingReport.Address =orgAddress;
            objTrainingReport.LocalLevel = orgName;
            objTrainingReport.Level = orgLevel;
            objTrainingReport.StartDate = "";
            objTrainingReport.EndDate = "";
            objTrainingReport.Ward = "";
            var lstTrainingReportData = new List<TrainingRowData>();
            var distinctTraining = talims.ToList().Select(m => m.TalimId).Distinct();
            int i = 1;
            foreach (var item in distinctTraining)
            {
                var lstTraining = talims.ToList().Where(m => m.TalimId == item);
                var objTraining = talims.ToList().FirstOrDefault(m => m.TalimId == item);
                var objReportData = new TrainingRowData {
                    BudgetTitle = objTraining.pujigatKharchaKharakram.Program,
                    MainActivity = objTraining.Purpose,
                    Remarks = objTraining.Remarks,
                    SN = ExecutionHelper.EnglishToNepali(i.ToString()),
                    TrainingTitle = objTraining.Talim.NameNepali,                    
                    StartDate = objTraining.StartDate.ToString("yyyy-MM-dd"),                    
                    EndDate = objTraining.EndDate.ToString("yyyy-MM-dd"),
                    Male = ExecutionHelper.EnglishToNepali(lstTraining.Sum(m=>Convert.ToInt32(m.Male??"0")).ToString()),
                    Female = ExecutionHelper.EnglishToNepali(lstTraining.Sum(m => Convert.ToInt32(m.Male ?? "0")).ToString()),
                    Dalit = ExecutionHelper.EnglishToNepali(lstTraining.Sum(m => Convert.ToInt32(m.Dalit ?? "0")).ToString()),
                    Janajati = ExecutionHelper.EnglishToNepali(lstTraining.Sum(m => Convert.ToInt32(m.Janajati ?? "0")).ToString()),
                    Others = ExecutionHelper.EnglishToNepali(lstTraining.Sum(m => Convert.ToInt32(m.Others ?? "0")).ToString()),
                    Total = ExecutionHelper.EnglishToNepali(( lstTraining.Sum(m => Convert.ToInt32(m.Male ?? "0"))+ lstTraining.Sum(m => Convert.ToInt32(m.FeMale ?? "0"))).ToString()),
                    TotalExpense =ExecutionHelper.EnglishToNepali(objTraining.TotalExpenses.ToString()),
                    Logistics = objTraining.Logistics,
                    Purpose = objTraining.Purpose,
                    Address = objTraining.Address
                };
                lstTrainingReportData.Add(objReportData);
                i++;
            }
            objTrainingReport.Rows = lstTrainingReportData;

            return View(objTrainingReport);


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
