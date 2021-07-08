using LIMS.Core;
using LIMS.Domain.AInR;
using LIMS.Domain.AnimalHealth;
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
using LIMS.Web.Areas.Admin.Models.AnimalHealth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class FarmForPurnaKhopController : BaseAdminController
    {

        private readonly ILocalizationService _localizationService;
        private readonly ISpeciesService _speciesService;
        private readonly IFiscalYearService _fiscalYearService;
        private readonly IFarmForVaccinationService _vaccinationService;
        private readonly IWorkContext _workContext;
        private readonly IBreedService _breedService;
        private readonly ILssService _lssService;
        private readonly ICustomerService _customerService;
        private readonly IFarmService _farmService;
        private readonly IAnimalRegistrationService _animalRegistrationService;
        private readonly IVaccinationTypeService _vaccinationTypeService;
        public FarmForPurnaKhopController(ILocalizationService localizationService,
             ISpeciesService speciesService,
             IFiscalYearService fiscalYearService,
             IFarmForVaccinationService vaccinationService,
             IWorkContext workContxt,
             IBreedService breedService,
             ILssService lssService,
             ICustomerService customerService,
             IFarmService farmService,
             IAnimalRegistrationService animalRegistrationService,
             IVaccinationTypeService vaccinationTypeService
            )
        {
            _localizationService = localizationService;
            _speciesService = speciesService;
            _fiscalYearService = fiscalYearService;
            _vaccinationService = vaccinationService;
            _workContext = workContxt;
            _breedService = breedService;
            _lssService = lssService;
            _customerService = customerService;
            _farmService = farmService;
            _animalRegistrationService = animalRegistrationService;
            _vaccinationTypeService = vaccinationTypeService;
        }

        public ActionResult Index() => RedirectToAction("List");

        public ActionResult List()
        {
            return View();
        }
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
                var Vaccination = await _vaccinationService.GetVaccinationByCustomerIds(customerid, "", command.Page - 1, command.PageSize);
                var gridModel = new DataSourceResult {
                    Data = Vaccination,
                    Total = Vaccination.TotalCount
                };
                return Json(gridModel);
            }
            else
            {
                var CreatedBy = _workContext.CurrentCustomer.EntityId;
                var users = _customerService.GetCustomerByLssId(null, CreatedBy);
                List<string> customerid = users.Select(x => x.Id).ToList();

                var ai = await _vaccinationService.GetVaccinationByCustomerIds(customerid,"", command.Page - 1, command.PageSize);

                var gridModel = new DataSourceResult {
                    Data = ai,
                    Total = ai.TotalCount
                };
                return Json(gridModel);
            }
        }

        [PermissionAuthorizeAction(PermissionActionName.Create)]

        public async Task<ActionResult> Create()
        {
            var breedType = BreedTypeHelper.GetBreedType();
            breedType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.BreedType = breedType;
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
            var vaccination = new SelectList(await _vaccinationTypeService.FiletrVaccinationType("Vaccine"), "Id", "MedicalName").ToList();
            vaccination.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.vaccinationId = vaccination;
            var model = new FarmForPurnaKhopModel();

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(FarmForPurnaKhopModel model, IFormCollection form)
        {
            var speciesId = form["SpeciesId"].ToList();

            if (speciesId.Count() > 0)
            {
                if (string.IsNullOrEmpty(model.FarmId))
                {
                    var farm = new Farm() {
                        NameEnglish = model.FarmName,
                        MobileNo = model.MobileNo,
                        Provience = model.Province,
                        District = model.District,
                        LocalLevel = model.LocalLevel,
                        Ward = model.Ward,
                        Tole = model.Tole,
                        CreatedBy = _workContext.CurrentCustomer.Id,
                        Source = _workContext.CurrentCustomer.OrgName
                    };
                    await _farmService.InsertFarm(farm);
                    model.FarmId = farm.Id;
                }
                var breedId = form["BreedId"].ToList();
                var breedType = form["BreedType"].ToList();
                var age = form["Age"].ToList();
                var eartag = form["EarTag"].ToList();
                var animalName = form["AnimalName"].ToList();
                var vaccinationId = form["VaccinationTypeId"].ToList();
                var disease = form["Disease"].ToList();
                List<FarmForPurnaKhop> purnaKhops = new List<FarmForPurnaKhop>();
                for (int i = 0; i < speciesId.Count; i++)
                {
                    var vaccination = new FarmForPurnaKhop() {
                        FarmId = model.FarmId,
                        Farm = await _farmService.GetFarmById(model.FarmId),
                        SpeciesId = speciesId[i],
                        Species = await _speciesService.GetSpeciesById(speciesId[i]),
                        Breed = await _breedService.GetBreedById(breedId[i]),
                        BreedId = breedId[i],
                        AnimalName = animalName[i],
                        Age = age[i],
                        EarTag = eartag[i],
                        FiscalYearId = model.FiscalYearId,
                        FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYearId),
                        Vaccination = await _vaccinationTypeService.GetVaccinationTypeById(vaccinationId[i]),
                        District = model.District,
                        LocalLevel = model.LocalLevel,
                        Ward = model.Ward,
                        Tole = model.Tole,
                        Province = model.Province,
                        Disease = (disease == null) ? false : true,
                        CreatedBy = _workContext.CurrentCustomer.Id,
                        Date = model.Date

                    };


                    purnaKhops.Add(vaccination);

                }
                await _vaccinationService.InsertVaccinationList(purnaKhops);
                return RedirectToAction("List");

            }

            var breedTypes = BreedTypeHelper.GetBreedType();
            breedTypes.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.BreedType = breedTypes;
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
            var vaccinations = new SelectList(await _vaccinationTypeService.FiletrVaccinationType("Vaccine"), "Id", "MedicalName").ToList();
            vaccinations.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.vaccinationId = vaccinations;

            return View(model);
        }
    }
}
