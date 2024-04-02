using LIMS.Core;
using LIMS.Framework.Components;
using LIMS.Services.Ainr;
using LIMS.Services.Bali;
using LIMS.Services.Basic;
using LIMS.Services.Breed;
using LIMS.Services.Customers;
using LIMS.Services.Localization;
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
    public class SubsidyReport : BaseViewComponent
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
        public readonly IAnudanService _anudanService;

        #endregion fields
        #region ctor
        public SubsidyReport(ILocalizationService localizationService,
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
              IAnudanService anudanService
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
            _anudanService = anudanService;
        }
        #endregion ctor
        public async Task<IViewComponentResult> InvokeAsync(string fiscalyear, string locallevel, string xetra)
        {
            var roles = _workContext.CurrentCustomer.CustomerRoles.Select(m => m.Name).ToList();
            string localLevel = _workContext.CurrentCustomer.LocalLevel;
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

            var filteredAnudan = await _anudanService.GetFilteredSubsidy("", fiscalyear, "", "", xetra);

            var distinctBudget = filteredAnudan.ToList().Select(m => m.BudgetId).Distinct();

            var subsidy = new SubsidyReportModel();
            if (!filteredAnudan.Any())
            {
                return View(subsidy);
            }
            subsidy.FiscalYear = ExecutionHelper.EnglishToNepali(filteredAnudan.ToList().FirstOrDefault().FiscalYear.NepaliFiscalYear);
            subsidy.LocalLevel = orgName;
            subsidy.Level = orgLevel;
            subsidy.StartDate = "";
            subsidy.EndDate = "";
            subsidy.Address = orgAddress;
            subsidy.Ward = "";
            var lstRowData = new List<SubsidyRowData>();
            int i = 1;
            foreach (var item in distinctBudget)
            {
                var objData = filteredAnudan.ToList().Where(m => m.BudgetId == item);
                var objFirst = objData.FirstOrDefault();
                var objSubsidyData = new SubsidyRowData { 
                    Upalabdhiharu = objFirst.ExpectedOutput,
                    BudgetTitle = objFirst.Budget.ActivityName,
                    MainActivity = objFirst.ExpectedOutput,
                    Remarks = objFirst.Remaks,
                    Purpose = objFirst.ExpectedOutput,
                    Male = ExecutionHelper.EnglishToNepali(objData.Sum(m => m.MaleMember??0).ToString()),
                    Female = ExecutionHelper.EnglishToNepali(objData.Sum(m => m.FemaleMember??0).ToString()),
                    Janajati = ExecutionHelper.EnglishToNepali(objData.Sum(m => m.JanajatiMember??0).ToString()),
                    Dalit = ExecutionHelper.EnglishToNepali(objData.Sum(m => m.DalitMember??0).ToString()),
                    Others = ExecutionHelper.EnglishToNepali(objData.Sum(m => m.Others??0).ToString()),
                    Total = ExecutionHelper.EnglishToNepali((objData.Sum(m => m.MaleMember ?? 0) + objData.Sum(m => m.FemaleMember ?? 0)).ToString()),
                    SN = ExecutionHelper.EnglishToNepali(i.ToString())
                };
                lstRowData.Add(objSubsidyData);
                i++;
            }

            subsidy.Rows = lstRowData;            

            return View(subsidy);
        }
        }
}
