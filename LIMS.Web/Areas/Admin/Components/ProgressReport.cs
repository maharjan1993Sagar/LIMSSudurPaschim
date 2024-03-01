using LIMS.Core;
using LIMS.Domain.Bali;
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
    public class ProgressReport : BaseViewComponent
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
        private readonly IMonthlyPragatiService _pragatiservice;
        private readonly IBudgetService _budgetService;
        private readonly IMonthlyPragatiService _monthlyProgressService;


        #endregion fields
        #region ctor
        public ProgressReport(ILocalizationService localizationService,
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
              IMonthlyPragatiService pragatiservice,
              ILocalLevelService localLevelService,
              IBudgetService budgetService,
              IMonthlyPragatiService monthlyProgressService
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
            _pragatiservice = pragatiservice;
            _localLevelService = localLevelService;
            _budgetService = budgetService;
            _monthlyProgressService = monthlyProgressService;
        }
        #endregion ctor
        public async Task<IViewComponentResult> InvokeAsync(string fiscalyear, string month,string xetra)
        {
            var roles = _workContext.CurrentCustomer.CustomerRoles.Select(m => m.Name).ToList();
            string orgName = _workContext.CurrentCustomer.OrgName;
            string orgAddress = _workContext.CurrentCustomer.OrgAddress;
            string orgLevel = "नगर कार्यपालिकाको कार्यालय";
            if (roles.Contains("Agriculture"))
            {
                xetra = "कृषि विकास";
            }
            if (roles.Contains("Livestock"))
            {
                xetra = "पशु तथा मत्स्य विकास ";
            }
            //if (roles.Contains("Administrators"))
            //{
            //    xetra = "";
            //}
            var pragatis = await _monthlyProgressService.GetFilteredMonthlyPragati("", fiscalyear, "", "", month,"",xetra);

            var allBudget = await _budgetService.GetBudget(new List<string>(), fiscalyear);

            var budget = new List<Budget>();
            budget = allBudget.ToList(); 

            if(!String.IsNullOrEmpty(xetra))
            {
                budget = allBudget.Where(m => m.Xetra == xetra).ToList();
            }

            var objProgressReport = new ProgressReportModel();

            if (!pragatis.Any())
            {
                return View(objProgressReport);
            }
            objProgressReport.FiscalYear = pragatis.ToList().FirstOrDefault().FiscalYear.NepaliFiscalYear;
            objProgressReport.Address =orgAddress;
            objProgressReport.LocalLevel = orgName;
            objProgressReport.Level = orgLevel;
            objProgressReport.Ward = "";
            var lstPragatiVayeka = new List<ProgressRowData>();
            //var lstPragatiNavayeka = new List<ProgressRowData>();
            int i = 1;

            foreach (var item in budget)
            {
                var lstprogress = pragatis.Where(m => m.BudgetId == item.Id);

                if (lstprogress.Count() > 0)
                {
                    var objReportDatas = lstprogress.Select(m => new ProgressRowData {
                        BudgetTitle = m.Budget.ActivityName,
                        Anugaman = m.Anugaman,
                        BitiyaPragati = m.BitiyaPragati,
                        VautikPragati = m.VautikPragati,
                        UpalbdiHaru = m.UpalbdiHaru,
                        Remarks = m.Remarks
                    });
                    lstPragatiVayeka.AddRange(objReportDatas);

                }
                else
                {
                    var objReportData = new ProgressRowData {
                        BudgetTitle = item.ActivityName,
                        Anugaman = "",
                        BitiyaPragati = "0",
                        VautikPragati ="0",
                        UpalbdiHaru = "",
                        Remarks = ""
                    };
                    lstPragatiVayeka.Add(objReportData);

                }

                //var objReportData = new ProgressRowData {
                //    BudgetTitle = item.Budget.ActivityName,
                //    Anugaman = item.Anugaman,
                //    BitiyaPragati = item.BitiyaPragati,
                //    VautikPragati = item.VautikPragati,
                //    UpalbdiHaru = item.UpalbdiHaru,
                //    Remarks = item.Remarks,
                //    SN = i.ToString()
                //};
                //if ((!String.IsNullOrEmpty(item.BitiyaPragati) && Convert.ToDecimal(item.BitiyaPragati) > 0)
                //    && (!String.IsNullOrEmpty(item.VautikPragati) && Convert.ToDecimal(item.VautikPragati) > 0))
                //{
                //    lstPragatiVayeka.Add(objReportData);
                //}
                //else
                //{ 
                //    lstPragatiNavayeka.Add(objReportData);
                //}
                // lstProgressReportData.Add(objReportData);
                //i++;
            }

            objProgressReport.RowsPragatiVayeka = lstPragatiVayeka;
            //objProgressReport.RowsPragatiNavayeka = lstPragatiNavayeka;

            return View(objProgressReport);


        }
    }
}
