using LIMS.Core;
using LIMS.Domain.AInR;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Ainr;
using LIMS.Services.AnimalHealth;
using LIMS.Services.Basic;
using LIMS.Services.Breed;
using LIMS.Services.Customers;
using LIMS.Services.Localization;
using LIMS.Services.MoAMAC;
using LIMS.Services.Security;
using LIMS.Services.Vaccination;
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
    public class VaccinationController : BaseAdminController
    {

        private readonly ILocalizationService _localizationService;
        private readonly ISpeciesService _speciesService;
        private readonly IUnitService _unitService;
        private readonly IFiscalYearService _fiscalYearService;
        private readonly IVaccinationService _vaccinationService;
        private readonly IWorkContext _workContext;
        private readonly IBreedService _breedService;
        private readonly ILssService _lssService;
        private readonly ICustomerService _customerService;
        private readonly IFarmService _farmService;
        private readonly IAnimalRegistrationService _animalRegistrationService;
        private readonly IVaccinationTypeService _vaccinationTypeService;
        private readonly IDiseaseService _diseaseService;
        public VaccinationController(ILocalizationService localizationService,
            ISpeciesService speciesService,
            IUnitService unitService,
            IFiscalYearService fiscalYearService,
            IVaccinationService vaccinationService,
            IWorkContext workContxt,
           IBreedService breedService,
             ILssService lssService,
             ICustomerService customerService,
             IFarmService farmService,
             IAnimalRegistrationService animalRegistrationService,
             IVaccinationTypeService vaccinationTypeService,
             IDiseaseService diseaseService
            )
        {
            _localizationService = localizationService;
            _speciesService = speciesService;
            _unitService = unitService;
            _fiscalYearService = fiscalYearService;
            _vaccinationService = vaccinationService;
            _workContext = workContxt;
            _breedService = breedService;
            _lssService = lssService;
            _customerService = customerService;
            _farmService = farmService;
            _animalRegistrationService = animalRegistrationService;
            _vaccinationTypeService = vaccinationTypeService;
            _diseaseService = diseaseService;
        }
        public IActionResult Index() => RedirectToAction("List");

        public IActionResult List() => View();

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command, string keyword = "")
        {
            string user = _workContext.CurrentCustomer.Id;
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();

            if (roles.Contains(RoleHelper.VhlsecAdmin) || roles.Contains(RoleHelper.VhlsecUser))
            {
                string vhlsecid = _workContext.CurrentCustomer.EntityId;
                List<string> lssId = _lssService.GetLssByVhlsecId(vhlsecid).Result.Select(m => m.Id).ToList();
                var customers = _customerService.GetCustomerByLssId(lssId, vhlsecid);
                List<string> customerid = customers.Select(x => x.Id).ToList();
                var CreatedBy = _workContext.CurrentCustomer.Id;
                var ai = await _vaccinationService.GetVaccination(command.Page-1, command.PageSize);
                var gridModel = new DataSourceResult {
                    Data = ai.Where(m => m.CreatedBy == CreatedBy),
                    Total = ai.Where(m => m.CreatedBy == CreatedBy).Count()
                };
                return Json(gridModel);
            }
            else
            {
                var ai = await _vaccinationService.GetVaccination(command.Page - 1, command.PageSize);
                var CreatedBy = _workContext.CurrentCustomer.Id;
                var gridModel = new DataSourceResult {
                    Data = ai.Where(m => m.CreatedBy == CreatedBy),
                    Total = ai.Where(m => m.CreatedBy == CreatedBy).Count()
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
            var fiscalyear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear").ToList();
            fiscalyear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            var quater = QuaterHelper.GetQuater();
            quater.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.QuaterId = quater;

            ViewBag.SpeciesId = species;
            ViewBag.FiscalYearId = fiscalyear;

            ViewBag.UnitId = unit;
            var vaccination = new SelectList(await _vaccinationTypeService.FiletrVaccinationType("Vaccine"), "Id", "MedicalName").ToList();
            vaccination.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.vaccinationId = vaccination;
            var disease = new SelectList(await _diseaseService.GetDisease(), "Id", "DiseaseNameEnglish").ToList();
            disease.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.DiseaseId = disease;
            VaccinationServiceModel model = new VaccinationServiceModel();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(VaccinationServiceModel model)
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

            var vaccination = model.ToEntity();
            vaccination.Farm = await _farmService.GetFarmById(model.FarmId);
            vaccination.AnimalRegistration = await _animalRegistrationService.GetAnimalRegistrationById(model.AnimalId);
            vaccination.CreatedBy = _workContext.CurrentCustomer.Id;
            vaccination.Disease = await _diseaseService.GetDiseaseById(vaccination.VaccinationForDisease);
            vaccination.VaccinationType = await _vaccinationTypeService.GetVaccinationTypeById(vaccination.VaccinationTypeId);
            vaccination.AnimalRegistrationId = model.AnimalId;
            await _vaccinationService.InsertVaccination(vaccination);
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

