﻿using LIMS.Core;
using LIMS.Domain.Breed;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Ainr;
using LIMS.Services.Basic;
using LIMS.Services.Breed;
using LIMS.Services.Customers;
using LIMS.Services.Localization;
using LIMS.Services.MoAMAC;
using LIMS.Services.Security;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Web.Areas.Admin.Models.Breed;
using LIMS.Web.Areas.Admin.Models.Livestock;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    [PermissionAuthorize(PermissionSystemName.AccessCropsProduction)]
    public class CropsProductionController : BaseAdminController
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
        public CropsProductionController(ILocalizationService localizationService,
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
        #region Livestock
        public IActionResult Index() => RedirectToAction("List");

        public async Task<IActionResult> List()
        {
            var createdby = _workContext.CurrentCustomer.EntityId;
            var createdbys = _customerService.GetCustomerByLssId(null, createdby);
            var ids = createdbys.Select(m => m.Id).ToList();

            FarmListModel currentfiscal = new FarmListModel();
            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();
            currentfiscal.CurrentFiscalYear = fiscalyear.Id;
            var dropdownitem = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            dropdownitem.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.common.select"), ""));

            ViewBag.fiscalyear = dropdownitem;
            var months = QuaterHelper.GetQuater();
            months.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Quater = months;
            var species = await _speciesService.GetSpecies();

            var speciesist = species.Where(m => m.EnglishName.ToLower() != "fish").ToList();
            var speci = new List<SelectListItem>();
            try
            {
                speci = new SelectList(speciesist, "Id", "NepaliName", species.Where(m => m.EnglishName.ToLower() == "cow").FirstOrDefault().Id).ToList();
            }
            catch
            {
                speci = new SelectList(speciesist, "Id", "NepaliName").ToList();

            }
            speci.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Species = speci;
            return View(currentfiscal);

        }

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command, string speciesId = "", string fiscalYear = "", string keyword = "")
        {
            if (string.IsNullOrEmpty(speciesId) && string.IsNullOrEmpty(fiscalYear))
            {
                List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();

                string createdby = null;

                createdby = _workContext.CurrentCustomer.Id;

                var currenFiscal = await _fiscalYearService.GetCurrentFiscalYear();

                var livestocks = await _livestockService.GetBreed(createdby,keyword);
                var live = livestocks.Select(m => m.Ward).Distinct();
                //List<LivestockListModel> lives = new List<LivestockListModel>();
                //foreach (var item in live)
                //{
                //    var animal = livestocks.Where(m => m.Ward == item);
                //    if (animal != null)
                //    {
                //        var species = animal.Where(m=>m.Species.EnglishName.ToLower()=="cow").Select(m => m.Species.Id).Distinct();
                //        foreach (var items in species)
                //        {
                //            lives.Add(new LivestockListModel {
                //                SpeciesName = animal.Where(m => m.Species.Id == items).FirstOrDefault().Species.NepaliName,
                //                Quantity = animal.Where(m => m.Species.Id == items).Sum(m => Convert.ToInt32(m.NoOfLivestock)),
                //                Ward = item
                //            });

                //        }
                //    }
                //}
                var gridModel = new DataSourceResult {
                    Data = livestocks,
                    Total = livestocks.TotalCount
                };
                return Json(gridModel);
            }
            else
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
                    createdby = adminemail;
                }
                var livestocks = new List<CropsProduction>();
                if (string.IsNullOrEmpty(fiscalYear))
                {
                    var currenFiscal = await _fiscalYearService.GetCurrentFiscalYear();
                    livestocks = _livestockService.GetBreed(createdby).Result.ToList();

                }
                else
                {
                    livestocks = _livestockService.GetBreed(createdby).Result.ToList();

                }

                var live = livestocks.Select(m => m.Ward).Distinct();
                //List<LivestockListModel> lives = new List<LivestockListModel>();
                //foreach (var item in live)
                //{
                //    var animal = livestocks.Where(m => m.Ward == item);
                //    if (animal != null)
                //    {
                //        var species = new List<string>();
                //        if (string.IsNullOrEmpty(speciesId))
                //        {
                //           species = animal.Select(m => m.Species.Id).Distinct().ToList();
                //        }
                //        else
                //        {

                //            species.Add(speciesId);
                //        }
                //        foreach (var items in species)
                //        {
                //            if (animal.Where(m => m.Species.Id == items).Count()>0)
                //            {
                //                lives.Add(new LivestockListModel {
                //                    SpeciesName = animal.Where(m => m.Species.Id == items).FirstOrDefault().Species.NepaliName,
                //                    Quantity = animal.Where(m => m.Species.Id == items).Sum(m => Convert.ToInt32(m.NoOfLivestock)),
                //                    Ward = item
                //                });
                //            }

                //        }
                // }
                //}
                var gridModel = new DataSourceResult {

                };
                return Json(gridModel);

            }
        }

        public async Task<IActionResult> Report()
        {
            var createdby = _workContext.CurrentCustomer.EntityId;
            var createdbys = _customerService.GetCustomerByLssId(null, createdby);
            var ids = createdbys.Select(m => m.Id).ToList();

            FarmListModel currentfiscal = new FarmListModel();
            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();
            currentfiscal.CurrentFiscalYear = fiscalyear.Id;
            var dropdownitem = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            dropdownitem.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.common.select"), ""));

            ViewBag.fiscalyear = dropdownitem;
            var months = QuaterHelper.GetQuater();
            months.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Quater = months;
            var species = await _speciesService.GetSpecies();

            var speciesist = species.Where(m => m.EnglishName.ToLower() != "fish").ToList();
            var speci = new List<SelectListItem>();
            try
            {
                speci = new SelectList(speciesist, "Id", "NepaliName", species.Where(m => m.EnglishName.ToLower() == "cow").FirstOrDefault().Id).ToList();
            }
            catch
            {
                speci = new SelectList(speciesist, "Id", "NepaliName").ToList();

            }
            speci.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Species = speci;
            return View();

        }

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> ReportHtml(string district = "",string locallevel="",string species=null, string fiscalYear = "")
        {
            var livestockWardWiseReportHtml = RenderViewComponentToString("CropProductionReport", new { fiscalyear = fiscalYear, species,district,locallevel });

            return Json(new
            {
                success = true,
                message = string.Format(_localizationService.GetResource("Admin.Report.LivestockWardWiseReport.Success")),
                livestockWardWiseReportHtml
            });
        }


        public async Task<ActionResult> Create()
        {

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
            var spes = await _speciesService.GetSpecies();
            var sf = spes.Where(m => m.EnglishName.ToLower() != "fish");
            var species = new SelectList(sf, "Id", "NepaliName").ToList();
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
            var type = BreedTypeHelper.GetBreedType();
            type.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Type = type;
            CropsProductionModel livestockModel = new CropsProductionModel();
            livestockModel.CropSeason = _cropSeason.GetBreed().Result.OrderBy(m=>m.Species.Id).ToList();
            return View(livestockModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CropsProductionModel model, IFormCollection form)
        {
            var GrowingSeason = form["GrowingSeasonId"].ToList();
            var Area = form["Area"].ToList();
            var Production = form["Production"].ToList();

            var existingLivestockDataIds = form["LivestockDataId"].ToList();
            var updateLivestocks = new List<CropsProduction>();
            var addLivestocks = new List<CropsProduction>();
            string createdby = null;
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();

            createdby = _workContext.CurrentCustomer.Id;

            //else
            //{
            //    string adminemail = _workContext.CurrentCustomer.CreatedBy;
            //    var admin = await _customerService.GetCustomerByEmail(adminemail);
            //    createdby = adminemail;
            //}
            for (int i = 0; i < GrowingSeason.Count(); i++)
            {
                if (string.IsNullOrEmpty(Area[i]) && string.IsNullOrEmpty(Production[i]))
                    continue;
                var livestock = (dynamic)null;

                livestock = new CropsProduction {
                    GrowingSeasonId = GrowingSeason[i],
                    Area = Area[i],
                    Production = Production[i],
                    GrowingSeason = await _cropSeason.GetBreedById(GrowingSeason[i]),
                    CropName =  _cropSeason.GetBreedById(GrowingSeason[i]).Result.Species,

                    //Unit = await _unitService.GetUnitById(units[i]),
                    FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYearId),
                    Provience = model.Provience,
                    District = model.District,
                    LocalLevel = model.LocalLevel,

                    CreatedBy = createdby,

                };

                if (!string.IsNullOrEmpty(existingLivestockDataIds[i]))
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
                await _livestockService.UpdateBreed(updateLivestocks);
            if (addLivestocks.Count > 0)
                await _livestockService.InsertBreed(addLivestocks);




            return RedirectToAction("List");
        }

        public async Task<IActionResult> UpdateCropProduction(string Id,string Area,string Production ) {
            var prod = await _livestockService.GetBreedById(Id);
            if(prod!=null)
            {
                prod.Area = Area;
                prod.Production = Production;
                await _livestockService.UpdateBreed(prod);
                return Json(true);
            }
            return Json(false);
        
        
        
        }
        [HttpPost]
        public async Task<IActionResult> GetLivestockData(string fiscalYearId, string district, string locallevel,string ward)
        {
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
            //    createdby = adminemail;
            //}
            var livestockData = (dynamic)null;

            livestockData = await  _livestockService.GetFilteredLivestock(createdby,  district, locallevel,fiscalYearId,Convert.ToInt32(ward));
            //}
            //else
            //{
            //     livestockData = await _livestockService.GetFilteredLivestock(createdby, species, breedType, fiscalYearId, quater,month, locallevel, ward,farmid);

            //}

            return Json(livestockData);
        }
        ////public List<SelectListItem> GetBreedType()
        //{
        //    return new List<SelectListItem>() {

        //             new SelectListItem {
        //                Text="Native",
        //                Value="Native",
        //            },
        //              new SelectListItem {
        //                Text="Crossbred",
        //                Value="CrossBred",
        //            },
        //              new SelectListItem {
        //                Text="Pure exotic Breed",
        //                Value="pureExoticBreed",
        //            },
        //    };
        //}


        #endregion Livestock
    }
}
