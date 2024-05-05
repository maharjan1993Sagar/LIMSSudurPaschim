using LIMS.Core;
using LIMS.Domain.StatisticalData;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Ainr;
using LIMS.Services.AnimalHealth;
using LIMS.Services.Basic;
using LIMS.Services.Breed;
using LIMS.Services.Customers;
using LIMS.Services.Localization;
using LIMS.Services.LocalStructure;
using LIMS.Services.MoAMAC;
using LIMS.Services.Security;
using LIMS.Services.Statisticaldata;
using LIMS.Services.Vaccination;
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
    [PermissionAuthorize(PermissionSystemName.ServiceData)]
    public class ServicesController : BaseAdminController
    {

        #region fields
        private readonly ILocalizationService _localizationService;
        private readonly ISpeciesService _speciesService;
        private readonly IUnitService _unitService;
        public readonly IFiscalYearService _fiscalYearService;
        public readonly ILivestockBreedService _breedService;
        public readonly IWorkContext _workContext;
        public readonly IServiceData _serviceData;
        public readonly ILssService _lssService;
        public readonly ICustomerService _customerService;
        public readonly IFarmService _farmService;
        public readonly IVaccinationTypeService _vaccinationService;
        public readonly ILocalLevelService _localLevelService;
        #endregion fields
        #region ctor
        public ServicesController(ILocalizationService localizationService,
            ISpeciesService speciesService,
             IUnitService unitService,
              IFiscalYearService fiscalYearService,
              ILivestockBreedService breedService,
              IWorkContext workContext,
              IServiceData serviceData,
              ILssService lssService,
              ICustomerService customerService,
              IFarmService farmService,
               IVaccinationTypeService vaccinationService,
               ILocalLevelService localLevelService
            )
        {
            _localizationService = localizationService;
            _speciesService = speciesService;
            _unitService = unitService;
            _fiscalYearService = fiscalYearService;
            _breedService = breedService;
            _workContext = workContext;
            _serviceData = serviceData;
            _lssService = lssService;
            _customerService = customerService;
            _farmService = farmService;
            _vaccinationService = vaccinationService;
            _localLevelService = localLevelService;
        }
        #endregion ctor
        #region service
        public IActionResult Index() => RedirectToAction("List");

        public IActionResult List() => View();

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command, string keyword = "")
        {
            string user = _workContext.CurrentCustomer.Id;
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
            string createdby = null;
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

            var service = await _serviceData.GetService("");
            var gridModel = new DataSourceResult {
                Data = service.Where(m => m.CreatedBy == createdby),
                Total = service.Where(m => m.CreatedBy == createdby).Count()
            };
            return Json(gridModel);

        }

        public async Task<ActionResult> Create()
        {

            var unit = new SelectList(await _unitService.GetUnit(), "Id", "UnitShortName").ToList();
            unit.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            MonthHelper month = new MonthHelper();
            var months = month.GetMonths();
            months.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Months = months;

            var provience = ProvinceHelper.GetProvince();
            provience.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.provience = provience;
            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
            species=species.Where(m => m.Text.ToLower() == "cow" || m.Text.ToLower() == "buffalo" || m.Text.ToLower() == "goat").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;
            var quater = QuaterHelper.GetQuater();
            quater.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            var ServicesType = new List<SelectListItem>() {
                new SelectListItem{
                    Text="AI",
                    Value="AI"
                },
                new SelectListItem{
                    Text="Vaccination",
                    Value="Vaccination"
                },
                new SelectListItem{
                    Text="Treatment",
                    Value="Treatment"
                },
                  new SelectListItem{
                    Text="Animal health",
                    Value="Animal health"
                },
                new SelectListItem {
                    Text="Drenching",
                    Value="Drenching"
                }
            };
            ServicesType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.ServiceTypeId = ServicesType;

            ViewBag.QuaterId = quater;
            ViewBag.SpeciesId = species;
            //ViewBag.FiscalYearId = fiscalyear;

            ViewBag.UnitId = unit;
            var vaccination = new SelectList(await _vaccinationService.FiletrVaccinationType("Vaccine"), "Id", "MedicalName").ToList();
            vaccination.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.vaccinationId = vaccination;

            var treatmentType = new List<SelectListItem>() {
                new SelectListItem{
                    Text="Medical treatment",
                    Value="Medical treatment"
                },
                new SelectListItem{
                    Text="Minor surgical",
                    Value="Minor surgical"
                },
                new SelectListItem{
                    Text="Gynecological Treatment",
                    Value="Gynecological Treatment"
                }
                  
            };
            var animalHealthService = new List<SelectListItem>() {
                new SelectListItem{
                    Text="Disease preventation",
                    Value="Disease preventation"
                },
                new SelectListItem{
                    Text="Fecal Examination",
                    Value="Fecal Examination"
                },
                new SelectListItem{
                    Text="Sample collection",
                    Value="Sample collection"
                },
                 new SelectListItem{
                    Text="Swab examination",
                    Value="Swab examination"
                }

            };
            animalHealthService.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.AnimalHealthService = animalHealthService;

            treatmentType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.TreatmentType = treatmentType;
            WardHelper wardHelper = new WardHelper();
            var ward = wardHelper.GetWard();
            ward.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Ward = ward;

            var localLevels = await _localLevelService.GetLocalLevel("KATHMANDU");
            var localLevelSelect = new SelectList(localLevels).ToList();
            //localLevelSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.LocalLevels = new SelectList(localLevelSelect, "Text", "Text", ExecutionHelper.LocalLevel);


            ServicesModel model = new ServicesModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ServicesModel model, IFormCollection form)
        {
            var breedIds = form["BreedId"].ToList();
            var speciesids = form["SpeciesId"].ToList();
            var quantities = form["Quantity"].ToList();
            var units = form["Unit"].ToList();
            var toles = form["Tole"].ToList();
            var dates = form["Date"].ToList();
            var existingServiceId = form["ServiceDataId"].ToList();
            string createdby = null;
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
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
            var addServices = new List<ServicesData>();
            var updateServices = new List<ServicesData>();
            if (model.ServicesType.ToLower() == "ai")
            {
                for (int i = 0; i < breedIds.Count(); i++)
                {
                    if (string.IsNullOrEmpty(quantities[i]))
                        continue;

                    var service = new ServicesData {

                        Breed = await _breedService.GetBreedById(breedIds[i]),
                        Unit = await _unitService.GetUnitById(units[i]),
                        Ward =model.Ward,
                        Tole = toles[i],
                        Date = dates[i],
                        Quantity = quantities[i],
                        FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYear),
                        Species = await _speciesService.GetSpeciesById(model.SpeciesName),
                        Provience = model.Provience,
                        District = model.District,
                        LocalLevel = _workContext.CurrentCustomer.LocalLevel,
                        Quater = model.Quater,
                        ServicesType = model.ServicesType,
                        FarmId = model.FarmId,
                        Farm = await _farmService.GetFarmById(model.FarmId),
                        VaccinationId = model.Vaccination,
                        Vaccination = (!string.IsNullOrEmpty(model.Vaccination)) ? await _vaccinationService.GetVaccinationTypeById(model.Vaccination) : null,
                        CreatedBy = createdby,
                        Month=model.Month

                    };
                    if (!string.IsNullOrEmpty(existingServiceId[i]))
                    {
                        service.Id = existingServiceId[i];
                        updateServices.Add(service);
                    } 
                    else
                    {
                        addServices.Add(service);
                    }

                }
            }
            else
            {
                for (int i = 0; i < speciesids.Count(); i++)
                {
                    if (string.IsNullOrEmpty(quantities[i]))
                        continue;

                    var service = new ServicesData {


                        Unit = await _unitService.GetUnitById(units[i]),
                        Ward = model.Ward,
                        Tole = toles[i],
                        Date = dates[i],
                        Quantity = quantities[i],
                        FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYear),
                        Species = await _speciesService.GetSpeciesById(speciesids[i]),
                        Provience = model.Provience,
                        District = model.District,
                        LocalLevel = _workContext.CurrentCustomer.LocalLevel,
                        Quater = model.Quater,
                        ServicesType = model.ServicesType,
                        FarmId = model.FarmId,
                        Farm = await _farmService.GetFarmById(model.FarmId),
                        VaccinationId = model.Vaccination,
                        Vaccination = (!string.IsNullOrEmpty(model.Vaccination)) ? await _vaccinationService.GetVaccinationTypeById(model.Vaccination) : null,
                        CreatedBy = createdby,
                        AnimalHealthService=model.AnimalHealthService,
                        TreatmentType=model.TreatmentType


                    };
                    if (!string.IsNullOrEmpty(existingServiceId[i]))
                    {
                        service.Id = existingServiceId[i];
                        updateServices.Add(service);
                    }
                    else
                    {
                        addServices.Add(service);
                    }

                }

            }
            if (updateServices.Count > 0)
                await _serviceData.UpdateServiceList(updateServices);
            if (addServices.Count > 0)
                await _serviceData.InsertServiceList(addServices);



            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> GetServiceData( string fiscalyearId, string quater, string serviceType,string district, string locallevel,string ward,string month,string vaccineName="", string species="", string treatMentType="",string animalHealth="")
        {
            string createdby = null;
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
            
            var servicesData = await _serviceData.GetFilteredService(fiscalyearId,month, serviceType, createdby,district, locallevel,vaccineName,treatMentType,animalHealth,"");
            


            
            return Json(servicesData);


        }


        #endregion service
    }
}
