using LIMS.Core;
using LIMS.Framework.Components;
using LIMS.Services.Ainr;
using LIMS.Services.Basic;
using LIMS.Services.Breed;
using LIMS.Services.Customers;
using LIMS.Services.Localization;
using LIMS.Services.MoAMAC;
using LIMS.Web.Areas.Admin.Models.Breed;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Components
{
    public class CropProductionReport : BaseViewComponent
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

        #endregion fields
        #region ctor
        public CropProductionReport(ILocalizationService localizationService,
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
              ICropsSeason cropSeason
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
        }
        #endregion ctor
        public async Task<IViewComponentResult> InvokeAsync(string fiscalyear,string speciesId, string district,string locallevel)
        {
          
                List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();

                string createdby = null;
            //if (roles.Contains("MolmacAdmin") || roles.Contains("MolmacAdmin"))
            //{
            //    createdby = "molmac";
            //}
            //else
            //{
            //    createdby = _workContext.CurrentCustomer.Id;
            //}
                var currenFiscal = await _fiscalYearService.GetCurrentFiscalYear();
                var livestocks = await _livestockService.GetFilteredProduction(createdby,speciesId,fiscalyear,locallevel,district);
                List<CropsProductionModel> cropsProductions = new List<CropsProductionModel>();
                var live =livestocks.OrderBy(m => m.CropName.Id).GroupBy(m=>m.GrowingSeason).Select(m=>new CropsProductionModel {
                CropName = m.First().CropName.EnglishName,
                Season = m.First().GrowingSeason.GrowingSeason,
                Production = Convert.ToString(m.Sum(m=>Convert.ToDecimal(m.Production))),
                Area =Convert.ToString(m.Sum(m=>Convert.ToInt32(m.Area))),
                Yeald = Convert.ToString(Math.Round(Convert.ToDecimal(m.Sum(m => Convert.ToDecimal(m.Production))) / Convert.ToDecimal(m.Sum(m => Convert.ToInt32(m.Area)))))
            }
                ).ToList();
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
                    
              

            return View(live);
        }
        }
}
