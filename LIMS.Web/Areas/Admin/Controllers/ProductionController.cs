using LIMS.Core;
using LIMS.Domain.StatisticalData;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Ainr;
using LIMS.Services.Basic;
using LIMS.Services.Breed;
using LIMS.Services.Customers;
using LIMS.Services.Localization;
using LIMS.Services.MoAMAC;
using LIMS.Services.Security;
using LIMS.Services.Statisticaldata;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Web.Areas.Admin.Models.StatisticalData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    [PermissionAuthorize(PermissionSystemName.ProductionData)]

    public class ProductionController : BaseAdminController
    {
        private readonly ILocalizationService _localizationService;
        private readonly ILivestockSpeciesService _speciesService;
        private readonly IUnitService _unitService;
        private readonly IFiscalYearService _fiscalYearService;
        private readonly IProductionionDataService _productionionDataService;
        private readonly IWorkContext _workContext;
        private readonly IBreedService _breedService;
        private readonly ILssService _lssService;
        private readonly ICustomerService _customerService;
        private readonly IFarmService _farmService;
        public ProductionController(ILocalizationService localizationService, ILivestockSpeciesService speciesService, IUnitService unitService, IFiscalYearService fiscalYearService, IProductionionDataService productionionDataService, IWorkContext workContxt,
           IBreedService breedService,
             ILssService lssService,
             ICustomerService customerService,
             IFarmService farmService
            )
        {
            _localizationService = localizationService;
            _speciesService = speciesService;
            _unitService = unitService;
            _fiscalYearService = fiscalYearService;
            _productionionDataService = productionionDataService;
            _workContext = workContxt;
            _breedService = breedService;
            _lssService = lssService;
            _customerService = customerService;
            _farmService = farmService;
        }
        public IActionResult Index() => RedirectToAction("List");

        public IActionResult List() => View();

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command, string keyword = "")
        {
            string user = _workContext.CurrentCustomer.Id;
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();

            string createdby = null;
           
                createdby = _workContext.CurrentCustomer.Id;
           
            var production = await _productionionDataService.GetProduction(createdby,command.Page - 1,command.PageSize);

            var gridModel = new DataSourceResult {
                Data = production,
                Total = production.TotalCount
            };
            return Json(gridModel);
        }



        public async Task<IActionResult> Report()
        {
            var provience = ProvinceHelper.GetProvince();
            provience.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.provience = provience;
            var roles = _workContext.CurrentCustomer.CustomerRoles.Select(m=>m.Name).ToList();
            var species = new SelectList(await _speciesService.GetBreed(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            var productionType = new List<SelectListItem>() {
                new SelectListItem{
                    Text="Milk",
                    Value="Milk"
                },
                new SelectListItem{
                    Text="Meat",
                    Value="Meat"
                },
                  new SelectListItem{
                    Text="Wool",
                    Value="Wool"
                },
                   new SelectListItem{
                    Text="Egg",
                    Value="Egg"
                },
                    new SelectListItem{
                    Text="Pasmina",
                    Value="Pasmina"
                },
            };
            productionType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Type = productionType;
            var getfiscalyear = await _fiscalYearService.GetCurrentFiscalYear();
            var CurrentFiscalYear = getfiscalyear.Id;

            var fiscalyear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear",CurrentFiscalYear).ToList();
            fiscalyear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Fiscalyear = fiscalyear;
            return View();
        }

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> Report(string type , string fiscalYear, string district , string locallevel)
        {
            string user = _workContext.CurrentCustomer.Id;
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();

            string createdby = null;
            if (roles.Contains("MolmacAdmin") || roles.Contains("MolmacAdmin"))
            {
                createdby = "molmac";
            }
            else
            {
                createdby = _workContext.CurrentCustomer.Id;
            }
         

            var production = await _productionionDataService.GetFilteredProduction(createdby,type,fiscalYear,locallevel,district);
           // var prod=production.GroupBy(m=>m.ProductionType)
            var gridModel = new DataSourceResult {
                Data = production,
                Total = production.TotalCount
            };
            return Json(gridModel);
        }

















        public async Task<IActionResult> GetSpeciesProductionType(string productionType)
        {
            var species = await _speciesService.GetBreed();
            var speciesist = species.ToList();
            if (productionType.ToLower() == "meat")
            {
                speciesist=species.Where(m => m.Purposes.Contains("Meat")).ToList();
            }
            if (productionType.ToLower() == "egg")
            {
                speciesist = species.Where(m => m.Purposes.Contains("Egg")).ToList();
            }
            if (productionType.ToLower() == "milk")
            {
                speciesist = species.Where(m => m.Purposes.Contains("Milk")).ToList();
            }
            if (productionType.ToLower() == "pasmina")
            {
                speciesist = species.Where(m => m.Purposes.Contains("Pasmina")).ToList();
            }
            if (productionType.ToLower() == "wool")
            {
                speciesist = species.Where(m => m.Purposes.Contains("Wool")).ToList();
            }
            return Json(speciesist);

        }


        public async Task<IActionResult> Create()
        {
            var unit = new SelectList(await _unitService.GetUnit(), "Id", "UnitShortName").ToList();
            unit.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            var provience = ProvinceHelper.GetProvince();
            provience.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.provience = provience;
            var species = new SelectList(await _speciesService.GetBreed(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            var productionType = new List<SelectListItem>() {
                new SelectListItem{
                    Text="Milk",
                    Value="Milk"
                },
                new SelectListItem{
                    Text="Meat",
                    Value="Meat"
                },
                  new SelectListItem{
                    Text="Wool",
                    Value="Wool"
                },
                   new SelectListItem{
                    Text="Egg",
                    Value="Egg"
                },
                    new SelectListItem{
                    Text="Pasmina",
                    Value="Pasmina"
                },
            };
            productionType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            var fiscalyear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear").ToList();
            fiscalyear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            var quater = QuaterHelper.GetQuater();
            quater.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.QuaterId = quater;

            ViewBag.SpeciesId = species;
            ViewBag.FiscalYearId = fiscalyear;

            ViewBag.UnitId = unit;
            ViewBag.ProductionTypeId = productionType;
            WardHelper wardHelper = new WardHelper();

            var ward = wardHelper.GetWard();
            ward.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            ViewBag.Ward = ward;
            ProductionModel model = new ProductionModel();
            model.Species = await _speciesService.GetBreed();
            return View(model);
        }




        [HttpPost]
        public async Task<IActionResult> Create(ProductionModel model, IFormCollection form)
        {
            var speciesIds = form["SpeciesId"].ToList();
            var quantities = form["Quantity"].ToList();
           
            var existingProductionDataIds = form["ProductionDataId"].ToList();
            var updateproductions = new List<Production>();
            var addproduction = new List<Production>();
            string createdby = null;
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
            
                createdby = _workContext.CurrentCustomer.Id;
           
            for (int i = 0; i < speciesIds.Count(); i++)
            {
                if (string.IsNullOrEmpty(quantities[i])) continue;
                var production = new Production {
                    Quantity = quantities[i],
                
                    FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYear),
                    Species = await _speciesService.GetBreedById(speciesIds[i]),
                    ProductionType = model.ProductionType,
                    Provience = model.Provience,
                    District = model.District,
                    LocalLevel = model.LocalLevel,
                    CreatedBy = createdby,
                    Ward=model.Ward,
                    Farm = await _farmService.GetFarmById(model.FarmId),
                    FarmId=model.FarmId
                };

                if (!string.IsNullOrEmpty(existingProductionDataIds[i]))
                {
                    production.Id = existingProductionDataIds[i];
                    updateproductions.Add(production);
                }
                else
                {
                    addproduction.Add(production);
                }

            }
            var customerId = _workContext.CurrentCustomer.Id;
            //  var productiondata = await _productionionDataService.GetFilteredProduction(model.FiscalYear, model.Quater, model.ProductionType, customerId);

            if (updateproductions.Count > 0)
                await _productionionDataService.UpdateProductionList(updateproductions);
            if (addproduction.Count > 0)
                await _productionionDataService.InsertProductionList(addproduction);

            return RedirectToAction("Index");
        }



        public async Task<ActionResult> UpdateProduction(string Id,string Quantity) {
            var production = await _productionionDataService.GetProductionById(Id);
            if(production!=null)
            {
                production.Quantity =Quantity;
               await _productionionDataService.UpdateProduction(production);
                return Json(true);
            }
            return Json(false);
        }
        [HttpPost]
        public async Task<IActionResult> GetProductionData(string fiscalyearId, string district,string locallevel,string ward, string productionType)
        {
            string createdby = null;
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
           
                createdby = _workContext.CurrentCustomer.Id;
            var productiondata = await _productionionDataService.GetFilteredProduction(fiscalyearId, productionType, createdby, district, locallevel,ward);
            return Json(productiondata);
        }

        [AllowAnonymous]
        public async Task<List<SelectListItem>> GetUnit()
        {
            var unit = new SelectList(await _unitService.GetUnit(), "Id", "UnitShortName").ToList();
            unit.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            return unit;
        }

        [AllowAnonymous]
        public async Task<List<SelectListItem>> GetFarm()
        {
            var farm = new SelectList(await _farmService.SearchFarm(), "Id", "NameEnglish").ToList();
            farm.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            return farm;
        }
    }
}