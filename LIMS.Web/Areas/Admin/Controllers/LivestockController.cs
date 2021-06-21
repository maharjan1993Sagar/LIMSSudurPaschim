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
    [PermissionAuthorize(PermissionSystemName.LivestockData)]

    public class LivestockController : BaseAdminController
    {
        #region fields
        private readonly ILocalizationService _localizationService;
        private readonly ISpeciesService _speciesService;
        private readonly IUnitService _unitService;
        public readonly IFiscalYearService _fiscalYearService;
        public readonly IBreedService _breedService;
        public readonly IWorkContext _workContext;
        public readonly ILivestockService _livestockService;
        public readonly ILssService _lssService;
        public readonly ICustomerService _customerService;
        public readonly IAnimalTypeService _animalTypeService;
        public readonly IFarmService _farmService;
        #endregion fields
        #region ctor
        public LivestockController(ILocalizationService localizationService,
            ISpeciesService speciesService,
             IUnitService unitService,
              IFiscalYearService fiscalYearService,
              IBreedService breedService,
              IWorkContext workContext,
              ILivestockService livestockService,
              ILssService lssService,
              ICustomerService customerService, 
              IAnimalTypeService animalTypeService,
              IFarmService farmService
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
        }
        #endregion ctor
        #region Livestock
        public IActionResult Index() => RedirectToAction("List");

        public IActionResult List() => View();

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command, string keyword = "")
        {
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();

               string createdby = null;
                if (roles.Contains(RoleHelper.LssAdmin) || roles.Contains(RoleHelper.VhlsecAdmin) || roles.Contains(RoleHelper.DolfdAdmin))
                {
                    createdby = _workContext.CurrentCustomer.Id;
                }
                else
                {
                    string adminemail = _workContext.CurrentCustomer.CreatedBy;
                    var admin = await _customerService.GetCustomerByEmail(adminemail);
                    createdby = admin.Id;
                }
            var livestocks = await _livestockService.GetLivestock(createdby, command.Page-1, command.PageSize);

            var gridModel = new DataSourceResult {
                    Data = livestocks,
                    Total = livestocks.TotalCount
                };
                return Json(gridModel);
            }
        


        public async Task<ActionResult> Create() {

            var unit = new SelectList(await _unitService.GetUnit(), "Id", "UnitShortName").ToList();
            unit.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            WardHelper wardHelper = new WardHelper();
            var ward = wardHelper.GetWard();
            ward.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            ViewBag.Ward = ward;
            unit.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            var provience = ProvinceHelper.GetProvince();
            provience.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.provience = provience;
            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            var fiscalyear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear").ToList();
            fiscalyear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            var quater = QuaterHelper.GetQuater();
            quater.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.QuaterId = quater;
            MonthHelper month = new MonthHelper();

            var months = month.GetMonths();
            months.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            ViewBag.Month = months;


            ViewBag.SpeciesId = species;
            ViewBag.FiscalYearId = fiscalyear;

            ViewBag.UnitId = unit;
            var type =BreedTypeHelper.GetBreedType();
            type.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Type = type;
            LivestockModel livestockModel = new LivestockModel();

            return View(livestockModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(LivestockModel model, IFormCollection form)
        {
            var animalType = form["TypeId"].ToList();
            var units = form["Unit"].ToList();
            var toles = form["Tole"].ToList();
            var noOfLivestock = form["NoOfLivestock"].ToList();
            var existingLivestockDataIds = form["LivestockDataId"].ToList();
            var updateLivestocks = new List<Livestock>();
            var addLivestocks= new List<Livestock>();
            string createdby = null;
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
            if (roles.Contains(RoleHelper.LssAdmin) || roles.Contains(RoleHelper.VhlsecAdmin) || roles.Contains(RoleHelper.DolfdAdmin))
            {
                createdby = _workContext.CurrentCustomer.Id;
            }
            else
            {
                string adminemail = _workContext.CurrentCustomer.CreatedBy;
                var admin = await _customerService.GetCustomerByEmail(adminemail);
                createdby = admin.Id;
            }
            for (int i = 0; i < animalType.Count(); i++)
            {
                if (string.IsNullOrEmpty(noOfLivestock[i]))
                    continue;

                var livestock = new Livestock {
                    AnimalType = await _animalTypeService.GetAnimalTypeById(animalType[i]),
                    NoOfLivestock = noOfLivestock[i],
                    Unit = await _unitService.GetUnitById(units[i]),
                    Ward =model.Ward,
                    Tole = toles[i],
                    FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYear),
                    Species = await _speciesService.GetSpeciesById(model.SpeciesName),
                    Provience = model.Provience,
                    District = model.District,
                    LocalLevel = model.LocalLevel,
                    Quater = model.Quater,
                    BreedType=model.BreedType,
                    FarmId=model.FarmId,
                    Farm= await _farmService.GetFarmById(model.FarmId),
                    CreatedBy = createdby,
                    Month=model.Month

                };
                if(!string.IsNullOrEmpty(existingLivestockDataIds[i]))
                {
                    livestock.Id = existingLivestockDataIds[i];
                    updateLivestocks.Add(livestock);
                }
                else
                {
                    addLivestocks.Add(livestock);
                }

            }
            if (updateLivestocks.Count > 0)
                await _livestockService.UpdateLivestockList(updateLivestocks);
            if (addLivestocks.Count > 0)
                await _livestockService.InsertLivestockList(addLivestocks);




            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> GetLivestockData(string species,string fiscalYearId, string quater, string breedType,string ward,string month)
        {
            string createdby = null;
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
            if (roles.Contains(RoleHelper.LssAdmin) || roles.Contains(RoleHelper.VhlsecAdmin) || roles.Contains(RoleHelper.DolfdAdmin))
            {
                createdby = _workContext.CurrentCustomer.Id;
            }
            else
            {
                string adminemail = _workContext.CurrentCustomer.CreatedBy;
                var admin = await _customerService.GetCustomerByEmail(adminemail);
                createdby = admin.Id;
            }
            var livestockData = (dynamic)null;
            if (roles.Contains(RoleHelper.LssAdmin) || roles.Contains(RoleHelper.LssUser))
            {
                livestockData = await _livestockService.GetFilteredLivestock(createdby, species, breedType, fiscalYearId, quater,ward,month);
            }
            else
            {
                 livestockData = await _livestockService.GetFilteredLivestock(createdby, species, breedType, fiscalYearId, quater,month);

            }

            return Json(livestockData);
        }
        public List<SelectListItem> GetBreedType()
        {
            return new List<SelectListItem>() {

                     new SelectListItem {
                        Text="Native",
                        Value="Native",
                    },
                      new SelectListItem {
                        Text="Crossbred",
                        Value="CrossBred",
                    },
                      new SelectListItem {
                        Text="Pure exotic Breed",
                        Value="pureExoticBreed",
                    },
            };
        }
        #endregion Livestock
    }
}
