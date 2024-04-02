using LIMS.Core;
using LIMS.Domain.AInR;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Ainr;
using LIMS.Services.AnimalBreeding;
using LIMS.Services.Basic;
using LIMS.Services.Breed;
using LIMS.Services.Customers;
using LIMS.Services.Localization;
using LIMS.Services.MoAMAC;
using LIMS.Services.Security;
using LIMS.Web.Areas.Admin.Extensions.Mapping;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Web.Areas.Admin.Models.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class PregnencyDiagnosisController : BaseAdminController
    {

        private readonly ILocalizationService _localizationService;
        private readonly ISpeciesService _speciesService;
        private readonly IUnitService _unitService;
        private readonly IFiscalYearService _fiscalYearService;
        private readonly IPregnencyDiagnosisService _pregnencyDiagnosisService;
        private readonly IWorkContext _workContext;
        private readonly IBreedService _breedService;
        private readonly ILssService _lssService;
        private readonly ICustomerService _customerService;
        private readonly IFarmService _farmService;
        private readonly IAnimalRegistrationService _animalRegistrationService;
        public PregnencyDiagnosisController(ILocalizationService localizationService,
            ISpeciesService speciesService,
            IUnitService unitService,
            IFiscalYearService fiscalYearService,
            IPregnencyDiagnosisService pregnencyDiagnosisService,
            IWorkContext workContxt,
           IBreedService breedService,
             ILssService lssService,
             ICustomerService customerService,
             IFarmService farmService,
             IAnimalRegistrationService animalRegistrationService
            )
        {
            _localizationService = localizationService;
            _speciesService = speciesService;
            _unitService = unitService;
            _fiscalYearService = fiscalYearService;
            _pregnencyDiagnosisService = pregnencyDiagnosisService;
            _workContext = workContxt;
            _breedService = breedService;
            _lssService = lssService;
            _customerService = customerService;
            _farmService = farmService;
            _animalRegistrationService = animalRegistrationService;
        }
        public IActionResult Index() => RedirectToAction("List");

        public IActionResult List() => View();

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command, string keyword = "")
        {
            string user = _workContext.CurrentCustomer.Id;
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();

            if (roles.Contains("VhlsecUser") || roles.Contains("VhlsecAdmin"))
            {
                string vhlsecid = _workContext.CurrentCustomer.EntityId;
                List<string> lssId = _lssService.GetLssByVhlsecId(vhlsecid).Result.Select(m => m.Id).ToList();
                var customers = _customerService.GetCustomerByLssId(lssId, vhlsecid);
                List<string> customerid = customers.Select(x => x.Id).ToList();
                //var CreatedBy = _workContext.CurrentCustomer.Id;
                var pregnencyDiagnoses = await _pregnencyDiagnosisService.GetPregnencyDiagnosis(customerid);
                var gridModel = new DataSourceResult {
                    Data = pregnencyDiagnoses,
                    Total = pregnencyDiagnoses.TotalCount
                };
                return Json(gridModel);
            }
            else
            {
                var CreatedBy = _workContext.CurrentCustomer.Id;
                var ai = await _pregnencyDiagnosisService.GetPregnencyDiagnosis(CreatedBy);
                var gridModel = new DataSourceResult {
                    Data = ai,
                    Total = ai.TotalCount
                };
                return Json(gridModel);
            }
        }


        public async Task<IActionResult> Create()
        {
            var unit = new SelectList(await _unitService.GetUnit(), "Id", "UnitShortName").ToList();
            unit.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            var provience = ProvinceHelper.GetProvince();
            provience.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.provience = provience;
            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;
            var quater = QuaterHelper.GetQuater();
            quater.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.QuaterId = quater;

            ViewBag.SpeciesId = species;
           // ViewBag.FiscalYearId = fiscalyear;

            ViewBag.UnitId = unit;
            var method = new List<SelectListItem>() {
                new SelectListItem{Text="Rector Palpation", Value="Rector Palpation" },
                new SelectListItem{Text="Usg ", Value="Usg" },
            };
            method.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Method = method;
            PregnencyDiagnosisModel model = new PregnencyDiagnosisModel();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(PregnencyDiagnosisModel model)
        {
            if (string.IsNullOrEmpty(model.FarmId))
            {
                var farm = new Farm() {
                    NameEnglish = model.FarmName,
                    MobileNo = model.MobileNo,
                    CreatedBy = _workContext.CurrentCustomer.Id,
                    Source = _workContext.CurrentCustomer.OrgName
                };
                await _farmService.InsertFarm(farm);
                model.FarmId = farm.Id;
            }
            if (string.IsNullOrEmpty(model.AnimalId))
            {
                var animal = new AnimalRegistration() {
                    Name = model.AnimalName,
                    SpeciesId = model.SpeciesId,
                    Species = await _speciesService.GetSpeciesById(model.SpeciesId),
                    Breed = await _breedService.GetBreedById(model.BreedId),
                    BreedId = model.BreedId,
                    EarTagNo = model.Eartag,
                    FarmId = model.FarmId,
                    Farm = await _farmService.GetFarmById(model.FarmId),
                    CreatedBy = _workContext.CurrentCustomer.Id,
                    Source = _workContext.CurrentCustomer.OrgName
                };
                await _animalRegistrationService.InsertAnimalRegistration(animal);

                model.AnimalId = animal.Id;

            }

            var pregnencyDaignosis = model.ToEntity();
            pregnencyDaignosis.Farm = await _farmService.GetFarmById(model.FarmId);
            pregnencyDaignosis.AnimalRegistration = await _animalRegistrationService.GetAnimalRegistrationById(model.AnimalId);
            pregnencyDaignosis.CreatedBy = _workContext.CurrentCustomer.Id;
            pregnencyDaignosis.Source = _workContext.CurrentCustomer.OrgName;

            pregnencyDaignosis.AnimalRegistrationId = model.AnimalId;
            await _pregnencyDiagnosisService.InsertPregnencyDiagnosis(pregnencyDaignosis);
            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<List<Farm>> GetFarm()
        {
            var createdBy = _workContext.CurrentCustomer.Id;
            var farm = await _farmService.GetFarmByCreatedBy(createdBy);
            return farm.ToList();

        }
        [HttpPost]
        public async Task<List<AnimalRegistration>> GetAnimal(string farmId)
        {

            var animal = await _animalRegistrationService.GetAnimalRegistrationByFarmId(farmId);
            return animal.ToList();

        }

        [AllowAnonymous]
        public async Task<List<SelectListItem>> GetUnit()
        {
            var unit = new SelectList(await _unitService.GetUnit(), "Id", "UnitShortName").ToList();
            unit.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            return unit;
        }



    }
}

