using LIMS.Core;
using LIMS.Domain.StatisticalData;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Ainr;
using LIMS.Services.Basic;
using LIMS.Services.Breed;
using LIMS.Services.Customers;
using LIMS.Services.Localization;
using LIMS.Services.LocalStructure;
using LIMS.Services.MoAMAC;
using LIMS.Services.Security;
using LIMS.Services.Statisticaldata;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Web.Areas.Admin.Models.StatisticalData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class FishProductionController : BaseAdminController
    {
        #region fields
        private readonly ILocalizationService _localizationService;
        private readonly ISpeciesService _speciesService;
        private readonly IUnitService _unitService;
        private readonly IFiscalYearService _fiscalYearService;
        public readonly IBreedService _breedService;
        public readonly IWorkContext _workContext;
        public readonly IFishProductionService _fishProductionService;
        public readonly ILssService _lssService;
        public readonly ICustomerService _customerService;
        public readonly IAnimalTypeService _animalTypeService;
        public readonly IFarmService _farmService;
        public readonly ILocalLevelService _localLevelService;
        

        #endregion fields
        #region ctor
        public FishProductionController(ILocalizationService localizationService,
            ISpeciesService speciesService,
             IUnitService unitService,
              IFiscalYearService fiscalYearService,
              IBreedService breedService,
              IWorkContext workContext,
              IFishProductionService fishProductionService,
              ILssService lssService,
              ICustomerService customerService,
              IAnimalTypeService animalTypeService,
              IFarmService farmService,
              ILocalLevelService localLevelService
             )
        {
            _localizationService = localizationService;
            _speciesService = speciesService;
            _unitService = unitService;
            _fiscalYearService = fiscalYearService;
            _breedService = breedService;
            _workContext = workContext;
            _fishProductionService = fishProductionService;
            _lssService = lssService;
            _customerService = customerService;
            _animalTypeService = animalTypeService;
            _farmService = farmService;
            _localLevelService = localLevelService;
        }
        #endregion ctor
        #region FishProduction
        public IActionResult Index() => RedirectToAction("List");

        public IActionResult List() => View();

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command, string keyword = "")
        {
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
            var current = await _fiscalYearService.GetCurrentFiscalYear();
            string createdby = null;
            //if (roles.Contains(RoleHelper.LssAdmin) || roles.Contains(RoleHelper.VhlsecAdmin) || roles.Contains(RoleHelper.DolfdAdmin))
            //{
            //    createdby = _workContext.CurrentCustomer.Id;
            //}
            //else
            //{
            //    string adminemail = _workContext.CurrentCustomer.CreatedBy;
            //    var admin = await _customerService.GetCustomerByEmail(adminemail);
            //    createdby = adminemail;
            //}
            var fishProductions = await _fishProductionService.GetFishProduction("", command.Page - 1, command.PageSize, current.Id);

            var gridModel = new DataSourceResult {
                Data = fishProductions,
                Total = fishProductions.TotalCount
            };
            return Json(gridModel);
        }



        public async Task<ActionResult> Create()
        {

            WardHelper wardHelper = new WardHelper();
            var ward = wardHelper.GetWard();
            ward.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            ViewBag.Ward = ward;
            var current = await _fiscalYearService.GetCurrentFiscalYear();
           
            var provience = ProvinceHelper.GetProvince();
            provience.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.provience = provience;

            var localLevels = await _localLevelService.GetLocalLevel(ExecutionHelper.District);
            var localLevelSelect = new SelectList(localLevels).ToList();
            localLevelSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.LocalLevels = new SelectList(localLevelSelect, "Text","Text", ExecutionHelper.LocalLevel);

            var breeds = await _breedService.GetBreed();
            var breed = breeds.Where(m => m.Species.EnglishName.ToLower() == "fish").ToList();

            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;

            var quater = QuaterHelper.GetQuater();
            quater.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.QuaterId = quater;
           
            MonthHelper month = new MonthHelper();
            var months = month.GetMonths();
            months.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Month = months;

            ViewBag.BreedId = breed;
            //ViewBag.FiscalYearId = fiscalyear;

            var type = BreedTypeHelper.GetBreedType();
            type.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Type = type;

            var natureofprod = new List<SelectListItem> {
                new SelectListItem {Text=_localizationService.GetResource("Admin.select.Paddycumfish"),Value="Admin.select.Paddycumfish"  },
                new SelectListItem {Text=_localizationService.GetResource("Admin.select.Troutfishinraceway"),Value="Admin.select.Troutfishinraceway"  },
                new SelectListItem {Text=_localizationService.GetResource("Admin.select.Cagefishculture"),Value="Admin.select.Cagefishculture"  },
                new SelectListItem {Text=_localizationService.GetResource("Admin.select.Ghols"),Value="Admin.select.Ghols"  },
                new SelectListItem {Text=_localizationService.GetResource("Admin.select.Others"),Value="Admin.select.Others"  },

            };

            natureofprod.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.natureofprod = natureofprod;
            
            FishProductionModel fishProductionModel = new FishProductionModel();
            //  fishProductionModel.LocalLevel = _workContext.CurrentCustomer.OrgLocalLevel;
            fishProductionModel.District = _workContext.CurrentCustomer.OrgAddress;
            return View(fishProductionModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(FishProductionModel model, IFormCollection form)
        {
            var localLevels = await _localLevelService.GetLocalLevel(ExecutionHelper.District);
            var localLevelSelect = new SelectList(localLevels).ToList();
            localLevelSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.LocalLevels = new SelectList(localLevelSelect, "Text","Text", ExecutionHelper.LocalLevel);
            model.LocalLevel = _workContext.CurrentCustomer.LocalLevel;

            var wards = form["Ward"].ToList();
            var numberOfFish = form["NumberOfFish"].ToList();
            var natureOfProduction = form["NatureOfProduction"].ToList();
            var area = form["Area"].ToList();
            var quantity = form["Quantity"].ToList();
            var remarks = form["Remarks"].ToList();
            var livestockDataId = form["LivestockDataId"].ToList();
            var localLevel = form["LocalLevel"].ToList();
            var updateFishProductions = new List<FishProduction>();
            var addFishProductions = new List<FishProduction>();
            string createdby = null;
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();

            //if (roles.Contains(RoleHelper.LssAdmin) || roles.Contains(RoleHelper.VhlsecAdmin) || roles.Contains(RoleHelper.DolfdAdmin))
            //{
                createdby = _workContext.CurrentCustomer.Id;
            //}
            //else
            //{
            //    string adminemail = _workContext.CurrentCustomer.CreatedBy;
            //    var admin = await _customerService.GetCustomerByEmail(adminemail);
            //    createdby = admin.Id;
            //}
            for (int i = 0; i < wards.Count(); i++)
            {

                var fishProduction = (dynamic)null;

                if (string.IsNullOrEmpty(numberOfFish[i]))
                    continue;
                fishProduction = new FishProduction {
                    Area = area[i],
                    NumberOfFish = numberOfFish[i],
                    Ward = wards[i],
                    FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYearId),
                    Province = model.Province,
                    District = model.District,
                    LocalLevel = _workContext.CurrentCustomer.LocalLevel,
                    NatureOfProduction = natureOfProduction[i],
                    CreatedBy = createdby,
                    Remarks = remarks[i]

                };


                if (!string.IsNullOrEmpty(livestockDataId[i]))
                {
                    fishProduction.Id = livestockDataId[i];
                    updateFishProductions.Add(fishProduction);
                }
                else
                {
                    addFishProductions.Add(fishProduction);
                }

            }
            if (updateFishProductions.Count > 0)
                await _fishProductionService.UpdateFishProductionList(updateFishProductions);
            if (addFishProductions.Count > 0)
                await _fishProductionService.InsertFishProductionList(addFishProductions);



            WardHelper wardHelper = new WardHelper();
            var ward = wardHelper.GetWard();
            ward.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            ViewBag.Ward = ward;

            var provience = ProvinceHelper.GetProvince();
            provience.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.provience = provience;

            var breeds = await _breedService.GetBreed();
            var breeda = breeds.Where(m => m.Species.EnglishName.ToLower() == "fish").ToList();

            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;

            var quater = QuaterHelper.GetQuater();
            quater.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.QuaterId = quater;
            MonthHelper month = new MonthHelper();

            var months = month.GetMonths();
            months.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            ViewBag.Month = months;


            ViewBag.BreedId = breeda;
           // ViewBag.FiscalYearId = fiscalyear;

            var type = BreedTypeHelper.GetBreedType();
            type.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Type = type;

            var natureofprod = new List<SelectListItem> {

                new SelectListItem {Text=_localizationService.GetResource("Admin.select.Paddycumfish"),Value="Admin.select.Paddycumfish"  },
                new SelectListItem {Text=_localizationService.GetResource("Admin.select.Troutfishinraceway"),Value="Admin.select.Troutfishinraceway"  },
                new SelectListItem {Text=_localizationService.GetResource("Admin.select.Cagefishculture"),Value="Admin.select.Cagefishculture"  },
                new SelectListItem {Text=_localizationService.GetResource("Admin.select.Ghols"),Value="Admin.select.Ghols"  },
                                new SelectListItem {Text=_localizationService.GetResource("Admin.select.Others"),Value="Admin.select.Others"  },

            };
            natureofprod.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.natureofprod = natureofprod;
            FishProductionModel fishProductionModel = new FishProductionModel();
            //  fishProductionModel.LocalLevel = _workContext.CurrentCustomer.OrgLocalLevel;
            fishProductionModel.District = model.District;
            return View(fishProductionModel);

        }


        [HttpPost]
        public async Task<IActionResult> GetFishProductionData(string fiscalYearId, string localLevel = "")
        {
            string createdby = null;
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
            var fishProductionData = (dynamic)null;
            fishProductionData = await _fishProductionService.GetFilteredFishProduction(createdby, fiscalYearId, localLevel);

            //if (roles.Contains(RoleHelper.LssAdmin) || roles.Contains(RoleHelper.VhlsecAdmin) || roles.Contains(RoleHelper.DolfdAdmin))
            //{
            //    createdby = _workContext.CurrentCustomer.Id;
            //}
            //else
            //{
            //    string adminemail = _workContext.CurrentCustomer.CreatedBy;
            //    var admin = await _customerService.GetCustomerByEmail(adminemail);
            //    createdby = admin.Id;
            //}


            return Json(fishProductionData);
        }


        #endregion FishProduction
    }
}
