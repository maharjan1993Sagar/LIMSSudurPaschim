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
using LIMS.Web.Areas.Admin.Models.Bali;
using LIMS.Web.Areas.Admin.Models.Breed;
using LIMS.Web.Areas.Admin.Models.Reports;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Components
{
    public class DeathVerificationReport : BaseViewComponent
    {
        #region fields
        private readonly ILocalizationService _localizationService;
        private readonly ISpeciesService _speciesService;
        private readonly IUnitService _unitService;
        public readonly IFiscalYearService _fiscalYearService;
        public readonly ILivestockBreedService _breedService;
        public readonly ILivestockSpeciesService _livestockSpeciesService;
        public readonly IWorkContext _workContext;
        public readonly IVarietyService _livestockService;
        public readonly ILssService _lssService;
        public readonly ICustomerService _customerService;
        public readonly IAnimalTypeService _animalTypeService;
        public readonly IFarmService _farmService;
        public readonly ICropsSeason _cropSeason;
        public readonly IDeathVerificationService _deathVerificationService;

        #endregion fields
        #region ctor
        public DeathVerificationReport(ILocalizationService localizationService,
            ISpeciesService speciesService,
             IUnitService unitService,
              IFiscalYearService fiscalYearService,
              ILivestockBreedService breedService,
              ILivestockSpeciesService livestockSpeciesService,
              IWorkContext workContext,
              IVarietyService livestockService,
              ILssService lssService,
              ICustomerService customerService,
              IAnimalTypeService animalTypeService,
              IFarmService farmService,
              ICropsSeason cropSeason,
              IDeathVerificationService deathVerificationService
             )
        {
            _localizationService = localizationService;
            _speciesService = speciesService;
            _unitService = unitService;
            _fiscalYearService = fiscalYearService;
            _breedService = breedService;
            _livestockSpeciesService = livestockSpeciesService;
            _workContext = workContext;
            _livestockService = livestockService;
            _lssService = lssService;
            _customerService = customerService;
            _animalTypeService = animalTypeService;
            _farmService = farmService;
            _cropSeason = cropSeason;
            _deathVerificationService = deathVerificationService;
        }
        #endregion ctor
        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            var customer = _workContext.CurrentCustomer;
            var roles = _workContext.CurrentCustomer.CustomerRoles.Select(m => m.Name).ToList();
            string localLevel = _workContext.CurrentCustomer.LocalLevel;
            string orgName = _workContext.CurrentCustomer.OrgName;
            string orgAddress = _workContext.CurrentCustomer.OrgAddress;
            string orgLevel = ExecutionHelper.LevelNepali;

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

            var filteredVerification = await _deathVerificationService.GetdeathVerificationById(id);
            var reportModel = new DeathVerificationReportModel();
            reportModel.DeathVerification = filteredVerification;
            reportModel.VerificationHeader = new VerificationHeader {
                LocalLevel = customer.OrgName,
                Level = ExecutionHelper.LevelNepali,
                Address = customer.OrgAddress,
                PhoneNumber = customer.PhoneNo,
                PaSa = "०८०/०८१",
                Department = ExecutionHelper.DepartmentLivestock,
                Today = DateTime.Now.ToString("yyyy/MM/dd")
            };

            return View(reportModel);
        }
    }
}
