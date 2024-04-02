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
using LIMS.Services.LocalStructure;
using LIMS.Services.MoAMAC;
using LIMS.Services.Security;
using LIMS.Services.Statisticaldata;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Web.Areas.Admin.Models.Livestock;
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
    [PermissionAuthorize(PermissionSystemName.LivestockUserAccess)]

    public class LivestockController : BaseAdminController
    {
        #region fields
        private readonly ILocalizationService _localizationService;
        private readonly ILivestockSpeciesService _speciesService;
        public readonly IFiscalYearService _fiscalYearService;
        public readonly IWorkContext _workContext;
        public readonly ILivestockService _livestockService;
        public readonly ICustomerService _customerService;
        public readonly IAnimalTypeService _ageCategory;
        public readonly IVhlsecService _vhlsecService;
        public readonly IDolfdService _dolfdService;
        public readonly ILocalLevelService _localLevelService;

        #endregion fields
        #region ctor
        public LivestockController(ILocalizationService localizationService,
            ILivestockSpeciesService speciesService,
              IFiscalYearService fiscalYearService,
              IWorkContext workContext,
              ILivestockService livestockService,
              ICustomerService customerService, 
               IAnimalTypeService ageCategory,
               IVhlsecService vhlsecService,
               IDolfdService dolfdService,
               ILocalLevelService localLevelService
             )
        {
            _localizationService = localizationService;
            _speciesService = speciesService;
            _fiscalYearService = fiscalYearService;
            _workContext = workContext;
            _livestockService = livestockService;
            _customerService = customerService;
            _ageCategory=ageCategory;
            _vhlsecService = vhlsecService;
            _dolfdService = dolfdService;
            _localLevelService = localLevelService;
        }
        #endregion ctor
        #region Livestock
        public IActionResult Index() => RedirectToAction("List");

        public async Task<IActionResult> List()
        {
            //var createdby = _workContext.CurrentCustomer.EntityId;
            //var createdbys = _customerService.GetCustomerByLssId(null, createdby);
            //var ids = createdbys.Select(m => m.Id).ToList();

            var localLevels = await _localLevelService.GetLocalLevel("KATHMANDU");
            var localLevelSelect = new SelectList(localLevels).ToList();
            localLevelSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.LocalLevels = new SelectList(localLevelSelect, "Text","Text", ExecutionHelper.LocalLevel);

            FarmListModel currentfiscal = new FarmListModel();
            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();
            currentfiscal.CurrentFiscalYear = fiscalyear.Id;
            var dropdownitem = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            dropdownitem.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.common.select"), ""));

            ViewBag.FiscalYearId = dropdownitem;
            var months = QuaterHelper.GetQuater();
            months.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Quater = months;
            var species = await _speciesService.GetBreed();

            //var speciesist = species.Where(m => m.EnglishName.ToLower()!= "fish").ToList();
            // var speci = new List<SelectListItem>();
            // try
            // {
            //      speci = new SelectList(speciesist, "Id", "NepaliName", species.Where(m => m.EnglishName.ToLower() == "cow").FirstOrDefault().Id).ToList();
            // }
            // catch
            // {
            //     speci = new SelectList(speciesist, "Id", "NepaliName").ToList();

            // }
            //speci.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            //ViewBag.Species = speci;
            var speciesist = species.Where(m => m.EnglishName.ToLower() != "fish").ToList();

            var speci = new List<SelectListItem>();
            try
            {
                speci = new SelectList(speciesist, "Id", "EnglishName", species.Where(m => m.EnglishName.ToLower() == "cow").FirstOrDefault().Id).ToList();
            }
            catch
            {
                speci = new SelectList(speciesist, "Id", "EnglishName").ToList();

            }
            speci.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Species = speci;
            WardHelper wardHelper = new WardHelper();
            var ward = wardHelper.GetWard();
            ward.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            ViewBag.Ward = ward;
            return View(currentfiscal);

        }

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command, string speciesId = "",string fiscalYear="", string district = "", string locallevel = "",string Ward = "")
        {
            if (!string.IsNullOrEmpty(fiscalYear))
            {
                List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();

                string createdby = null;
               
                    createdby = _workContext.CurrentCustomer.Id;
               
                var currenFiscal = await _fiscalYearService.GetCurrentFiscalYear();
                var livestocks = _livestockService.GetFilteredLivestock(createdby, speciesId, district, fiscalYear, "",locallevel,Ward,command.Page-1,command.PageSize).Result.ToList();
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
                    Data=livestocks,
                    Total=livestocks.Count()
                };
                return Json(gridModel);
            }
            else
            {
                List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();

                string createdby = null;
                
                    createdby = _workContext.CurrentCustomer.Id;
               
                var livestocks = new List<Livestock>();
                if (string.IsNullOrEmpty(fiscalYear))
                {
                 var  currenFiscal = await _fiscalYearService.GetCurrentFiscalYear();
                    livestocks =  _livestockService.GetLivestock(createdby).Result.ToList();

                }
                else
                {
                    livestocks =  _livestockService.GetLivestock(createdby).Result.ToList();

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
                    Data = livestocks,
                    Total = livestocks.Count()
                };
                return Json(gridModel);

            }
        }











        public async Task<IActionResult> Report()
        {
            var createdby = _workContext.CurrentCustomer.EntityId;
            //var createdbys = _customerService.GetCustomerByLssId(null, createdby);
            //var ids = createdbys.Select(m => m.Id).ToList();

            var localLevels = await _localLevelService.GetLocalLevel("KATHMANDU");
            var localLevelSelect = new SelectList(localLevels).ToList();
            localLevelSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.LocalLevels = new SelectList(localLevelSelect, "Text", "Text", ExecutionHelper.LocalLevel);


            FarmListModel currentfiscal = new FarmListModel();
            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();
            currentfiscal.CurrentFiscalYear = fiscalyear.Id;
            var dropdownitem = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            dropdownitem.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.common.select"), ""));

            ViewBag.FiscalYearId = dropdownitem;
            var months = QuaterHelper.GetQuater();
            months.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Quater = months;
            var roles = _workContext.CurrentCustomer.Id;
            //if(roles.Contains(RoleHelper.VhlsecAdmin)||roles.Contains(RoleHelper.VhlsecUser))
            //{
            //    ViewBag.District = _workContext.CurrentCustomer.District;
            //}
            var species = await _speciesService.GetBreed();

            var speciesist = species.Where(m => m.EnglishName.ToLower() != "fish").ToList();
            var speci = new List<SelectListItem>();
            try
            {
                speci = new SelectList(speciesist, "Id", "EnglishName", species.Where(m => m.EnglishName.ToLower() == "cow").FirstOrDefault().Id).ToList();
            }
            catch
            {
                speci = new SelectList(speciesist, "Id", "EnglishName").ToList();

            }
            speci.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Species = speci;
            return View(currentfiscal);

        }

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> Report(string speciesId = "", string fiscalYear = "",string district="",string locallevel="")
        {
            if (string.IsNullOrEmpty(speciesId) && string.IsNullOrEmpty(fiscalYear))
            {
                List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
                var livestocks = new List<Livestock>();

                // if (roles.Contains("MolmacAdmin") || roles.Contains("MolmacUser"))
                // {
                //     livestocks = _livestockService.GetLivestock().Result.ToList();

                // }
                //else if (roles.Contains("DolfdAdmin") || roles.Contains("DolfdUser") )
                // {

                //     livestocks = _livestockService.GetLivestock().Result.ToList();
                // }
                // else
                // {
                //     string createdby = null;

                //     createdby = _workContext.CurrentCustomer.Id;
                //     livestocks = _livestockService.GetLivestock(createdby).Result.ToList();


                // }
                     livestocks = _livestockService.GetLivestock("").Result.ToList();


                var currenFiscal = await _fiscalYearService.GetCurrentFiscalYear();
                livestocks.Where(m => m.FiscalYear.Id == currenFiscal.Id);

                var live = livestocks.Select(m => m.Ward).Distinct();
                List<LivestockModel> lives = new List<LivestockModel>();
                foreach (var item in live)
                {

                    var animal = livestocks.Where(m => m.Ward == item);
                    if (animal != null)
                    {
                        foreach (var items in animal)
                        {
                            lives.Add(new LivestockModel {
                                AgeCategory = _ageCategory.GetAnimalTypeById(items.AnimalType.Id).Result.Name,
                                Native = items.Local,
                                Improved = items.Improved,
                                Ward = item

                            });
                        }
                    }

                    if (live.LastOrDefault() == item)
                    {
                        lives.Add(new LivestockModel {
                            AgeCategory = "",
                            Native = Convert.ToString(livestocks.Select(m => Convert.ToInt32(m.Local)).Sum()),
                            Improved = Convert.ToString(livestocks.Select(m => Convert.ToInt32(m.Improved)).Sum()),
                            Ward = "Total"

                        });
                    }


                }
                    var gridModel = new DataSourceResult {
                        Data = lives,
                        Total = livestocks.Count
                    };
                    return Json(gridModel);
                
            }
            else
            {
                List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();

                string createdby = null;


                var livestocks = new List<Livestock>();
                //if (roles.Contains("MolmacAdmin") || roles.Contains("MolmacUser")|| roles.Contains("DolfdAdmin")|| roles.Contains("DolfdUserUser"))
                //{
                //    createdby = "molmac";
                //}
                //else
                //{
                //    createdby = _workContext.CurrentCustomer.Id;
                //}
                livestocks = _livestockService.GetFilteredLivestock("", speciesId, fiscalYear, locallevel, district).Result.ToList();


                //var live = livestocks.Select(m => m.Ward).Distinct();
                List<LivestockModel> lives = new List<LivestockModel>();


                var animal = livestocks.GroupBy(m => m.AnimalType.Id).Select(m =>
                  new LivestockModel {
                      AgeCategory = _ageCategory.GetAnimalTypeById(m.First().AnimalType.Id).Result.Name,
                      Native = Convert.ToString(m.Sum(m => Convert.ToInt32((int.TryParse(m.Local, out int n))?m.Local:"0"))),
                      Improved = Convert.ToString(m.Sum(m => Convert.ToInt32((int.TryParse(m.Improved, out int y))?m.Improved:"0"))),

                  }


                ).ToList();



                if (animal.Count() > 0)
                {
                    animal.Add(new LivestockModel {
                        AgeCategory = "Total",

                        Native = Convert.ToString(livestocks.Select(m => Convert.ToInt32((int.TryParse(m.Local, out int n)) ? m.Local : "0")).Sum()),
                        Improved = Convert.ToString(livestocks.Select(m => Convert.ToInt32((int.TryParse(m.Improved, out int n)) ? m.Improved : "0")).Sum()),

                    });
                }






                var gridModel = new DataSourceResult {
                    Data = animal,
                    Total = livestocks.Count
                };
                return Json(gridModel);

                //  var live = livestocks.Select(m => m.Ward).Distinct();
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


            }
        }



        public async Task<ActionResult> Create() {

             WardHelper wardHelper = new WardHelper();
            var ward = wardHelper.GetWard();
            ward.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Ward = ward;

            var provience = ProvinceHelper.GetProvince();
            provience.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.provience = provience;

            var spes = await _speciesService.GetBreed();
            var sf=spes.Where(m => m.EnglishName.ToLower() != "fish");
            var species = new SelectList(sf, "Id", "NepaliName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

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


            ViewBag.SpeciesId = species;
           // ViewBag.FiscalYearId = fiscalyear;

            var type =BreedTypeHelper.GetBreedType();
            type.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Type = type;

            var localLevels = await _localLevelService.GetLocalLevel("KATHMANDU");
            var localLevelSelect = new SelectList(localLevels).ToList();
            localLevelSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.LocalLevels = new SelectList(localLevelSelect, "Text", "Text", ExecutionHelper.LocalLevel);


            LivestockModel livestockModel = new LivestockModel();
            livestockModel.District = _workContext.CurrentCustomer.OrgAddress;
           
            return View(livestockModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(LivestockModel model, IFormCollection form)
        {
            var animalType = form["TypeId"].ToList();
            var units = form["Unit"].ToList();
            var Native = form["Native"].ToList();
            var Improved = form["Improved"].ToList();

            var existingLivestockDataIds = form["LivestockDataId"].ToList();
            var updateLivestocks = new List<Livestock>();
            var addLivestocks= new List<Livestock>();
            string createdby = null;
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
     
                createdby = _workContext.CurrentCustomer.Id;
            
            //else
            //{
            //    string adminemail = _workContext.CurrentCustomer.CreatedBy;
            //    var admin = await _customerService.GetCustomerByEmail(adminemail);
            //    createdby = adminemail;
            //}
            for (int i = 0; i < animalType.Count(); i++)
            {
                if (string.IsNullOrEmpty(Native[i])&& string.IsNullOrEmpty(Improved[i]))
                    continue;
                var livestock = (dynamic)null;
               
                    livestock = new Livestock {
                        AnimalType = await _ageCategory.GetAnimalTypeById(animalType[i]),
                        Improved = Improved[i],
                        Local = Native[i],

                        //Unit = await _unitService.GetUnitById(units[i]),
                        Ward = model.Ward,
                        FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYear),
                        Species = await _speciesService.GetBreedById(model.SpeciesName),
                        Provience = model.Provience,
                        District = model.District,
                        LocalLevel = _workContext.CurrentCustomer.LocalLevel,
                        Quater = model.Quater,
                        BreedType = model.BreedType,
                      
                        CreatedBy = createdby,
                        Month = model.Month

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


            var localLevels = await _localLevelService.GetLocalLevel("KATHMANDU");
            var localLevelSelect = new SelectList(localLevels).ToList();
            localLevelSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.LocalLevels = new SelectList(localLevelSelect, "Text", "Text", ExecutionHelper.LocalLevel);


            return RedirectToAction("List");
        }


        public async Task<ActionResult> CreateNew()
        {

                   WardHelper wardHelper = new WardHelper();
            var ward = wardHelper.GetWard();
            ward.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            ViewBag.Ward = ward;

            var provience = ProvinceHelper.GetProvince();
            provience.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.provience = provience;
            var spes = await _speciesService.GetBreed();
            var sf = spes.Where(m => m.EnglishName.ToLower() != "fish");
            //var species = new SelectList(sf, "Id", "NepaliName").ToList();
            //species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));


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


            // ViewBag.SpeciesId = species;
           // ViewBag.FiscalYearId = fiscalyear;

            
            var type = BreedTypeHelper.GetBreedType();
            type.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Type = type;

            var localLevels = await _localLevelService.GetLocalLevel("KATHMANDU");
            var localLevelSelect = new SelectList(localLevels).ToList();
            localLevelSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.LocalLevels = new SelectList(localLevelSelect, "Text", "Text", ExecutionHelper.LocalLevel);

            LivestockModel livestockModel = new LivestockModel();
            livestockModel.District = _workContext.CurrentCustomer.OrgAddress;
            List<SpeciesWithAnimal> speciesWithAnimals = new List<SpeciesWithAnimal>();
            foreach (var item in sf)
            {
                SpeciesWithAnimal sas = new SpeciesWithAnimal();
                sas.Species = item;
                sas.AgeCategory = await _ageCategory.GetAnimalTypeBySpeciesId(item.Id);
                if (item.EnglishName.ToLower() == "fowl")
                {

                    sas.AnimalType.Add("Native");

                    sas.AnimalType.Add("Broiler Commercial");
                    sas.AnimalType.Add("Layer commercial");
                    sas.AnimalType.Add("Giriraj");
                    sas.AnimalType.Add("Others");



                }

                else if (item.EnglishName.ToLower() != "fowl")
                {

                    sas.AnimalType.Add("Native");

                    sas.AnimalType.Add("Crossbred");
                    


                }
                else
                {
                    sas.AnimalType.Add(item.NepaliName);
                };

                speciesWithAnimals.Add(sas);


            }
            livestockModel.SpeciesWithAnimals = speciesWithAnimals;
            return View(livestockModel);
        }
        [HttpPost]
        public async Task<IActionResult> CreateNew(LivestockModel model, IFormCollection form)
        {
            var species = form["SpeciesId"].ToList();
            var animalType = form["AgeCategoryId"].ToList();
            var BreedType = form["AnimalType"].ToList();

            var noOfLivestock = form["LivestockQuantity"].ToList();
            //var existingLivestockDataIds = form["LivestockDataId"].ToList();
            var updateLivestocks = new List<Livestock>();
            var addLivestocks = new List<Livestock>();
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
                createdby = adminemail;
            }
            for (int i = 0; i < animalType.Count(); i++)
            {
                if (string.IsNullOrEmpty(noOfLivestock[i]))
                    continue;
                var livestock = (dynamic)null;
                if (string.IsNullOrEmpty(model.FarmId))
                {
                    livestock = new Livestock {
                        AnimalType = await _ageCategory.GetAnimalTypeById(animalType[i]),
                        NoOfLivestock = noOfLivestock[i],
                        //Unit = await _unitService.GetUnitById(units[i]),
                        Ward = model.Ward,
                        FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYear),
                        Species = await _speciesService.GetBreedById(species[i]),
                        Provience = model.Provience,
                        District = model.District,
                        LocalLevel = model.LocalLevel,
                      
                        Quater = model.Quater,
                        BreedType = BreedType[i],
                       
                        //PopulationInHouseHold = model.PopulationInHouseHold,
                        CreatedBy = createdby,
                        Month = model.Month

                    };
                }
               

                addLivestocks.Add(livestock);


            }
            if (updateLivestocks.Count > 0)
                await _livestockService.UpdateLivestockList(updateLivestocks);
            if (addLivestocks.Count > 0)
                await _livestockService.InsertLivestockList(addLivestocks);


            var localLevels = await _localLevelService.GetLocalLevel("KATHMANDU");
            var localLevelSelect = new SelectList(localLevels).ToList();
            localLevelSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.LocalLevels = new SelectList(localLevelSelect, "Text", "Text", ExecutionHelper.LocalLevel);


            return RedirectToAction("List");
        }



        public async Task<IActionResult> UpdateLivestock(String Id, String Local, String Improved)
        {
            var livestock =await _livestockService.GetLivestockById(Id);
            if(livestock!=null)
            {                livestock.Improved = Improved;
                livestock.Local = Local;
               await _livestockService.UpdateLivestock(livestock);
                return Json(true);
            }
            return Json(false);

        }
        [HttpPost]
        public async Task<IActionResult> GetLivestockData(string species,string fiscalYearId, string district,string locallevel,string ward)
        {
            string createdby = null;
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
            
                createdby = _workContext.CurrentCustomer.Id;
           
            var livestockData = (dynamic)null;
            
                livestockData = await _livestockService.GetFilteredLivestock(createdby, species, district, fiscalYearId, "",locallevel,ward);
            

            return Json(livestockData);
        }

        [HttpPost]
        public async Task<IActionResult> GetLivestocksData(string fiscalYearId, string district, string locallevel, string ward)
        {
            string createdby = null;
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();

            createdby = _workContext.CurrentCustomer.Id;

            var livestockData = (dynamic)null;

            livestockData = await _livestockService.GetFilteredsLivestock(createdby,  fiscalYearId, locallevel,"", ward );


            return Json(livestockData);
        }

        public async Task<List<SelectListItem>> GetAnimalType(string id)
        {
            var species = await _speciesService.GetBreedById(id);
            if (species.EnglishName.ToLower() == "fowl")
            {


                return new List<SelectListItem>() {
                     new SelectListItem {
                    Text = "Select",
                    Value = "",
                },
                     new SelectListItem {
                        Text="Native",
                        Value="Native",
                    },
                      new SelectListItem {
                        Text="Broiler Commercial",
                        Value="Broiler Commercial",
                    },
                      
                       new SelectListItem {
                        Text="Layer commercial",
                        Value="Layer commercial",
                    },
                       new SelectListItem {
                        Text="Giriraj",
                        Value="Giriraj",
                    },
                       new SelectListItem {
                        Text="Others",
                        Value="Others",
                    },


            };
            }
            else 
            {
               
                return new List<SelectListItem>() {
                     new SelectListItem {
                    Text = "Select",
                    Value = "",
                },
                new SelectListItem {
                    Text = "स्थानिय",
                    Value = "Native",
                },
                      new SelectListItem {
                          Text = "बर्णशंकर",
                          Value = "Crossbred",
                      },
                      new SelectListItem {
                          Text = "उन्नत",
                          Value = "Pure exotic breed",
                      },
                      };
            }
           
        }


        #endregion Livestock
    }
}
