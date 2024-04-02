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
    public class PurnaKhopController : BaseAdminController
    {
        private readonly ILocalizationService _localizationService;
        private readonly ISpeciesService _speciesService;
        private readonly IFiscalYearService _fiscalYearService;
        private readonly IPurnaKhopService _vaccinationService;
        private readonly IWorkContext _workContext;
        private readonly IBreedService _breedService;
        private readonly ILssService _lssService;
        private readonly ICustomerService _customerService;
        private readonly IFarmService _farmService;
        private readonly IAnimalRegistrationService _animalRegistrationService;
        private readonly IVaccinationTypeService _vaccinationTypeService;
        private readonly IFarmForVaccinationService _farmForPurnaKhopService;

        public PurnaKhopController(ILocalizationService localizationService,
             ISpeciesService speciesService,
             IFiscalYearService fiscalYearService,
             IPurnaKhopService vaccinationService,
             IWorkContext workContxt,
             IBreedService breedService,
             ILssService lssService,
             ICustomerService customerService,
             IFarmService farmService,
             IAnimalRegistrationService animalRegistrationService,
             IVaccinationTypeService vaccinationTypeService,
             IFarmForVaccinationService farmForPurnaKhopService
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
            _farmForPurnaKhopService = farmForPurnaKhopService;
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
            var entityId = _workContext.CurrentCustomer.EntityId;
            var users = _customerService.GetCustomerByLssId(null, entityId);
            var createdBy = users.Select(m => m.Id).ToList();
            var ai = await _vaccinationService.GetVaccinationByCustomerIds(createdBy, keyword, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult {
                Data = ai,
                Total = ai.Count()
            };
            return Json(gridModel);

        }

        [PermissionAuthorizeAction(PermissionActionName.Create)]

        public async Task<ActionResult> Create()
        {
            var breedType = BreedTypeHelper.GetBreedType();
            breedType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.provience = breedType;
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
            //ViewBag.FiscalYearId = fiscalyear;
            var vaccination = new SelectList(await _vaccinationTypeService.FiletrVaccinationType("Vaccine"), "Id", "MedicalName").ToList();
            vaccination.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.vaccinationId = vaccination;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(PurnaKhopModel model, IFormCollection form)
        {
            var farmFromPurnakhopId = form["FarmFromPurnakhopId"].ToList();
            var vaccinationId = form["Vaccination"].ToList();
            var vaccinationDate = form["VaccinationDate"].ToList();
            var nextVaccinationDate = form["NextVaccinationDate"].ToList();
            var remarks = form["Remarks"].ToList();
            var purnaKhops = new List<PurnaKhop>();
            for (int i = 0; i < farmFromPurnakhopId.Count(); i++)
            {
                purnaKhops.Add(new PurnaKhop() {
                    FarmForPurnaKhopId = farmFromPurnakhopId[i],
                    FarmForPurnaKhop = await _farmForPurnaKhopService.GetVaccinationById(farmFromPurnakhopId[i]),
                    VaccinationTypeId = vaccinationId[i],
                    Vaccination = await _vaccinationTypeService.GetVaccinationTypeById(vaccinationId[i]),
                    VaccinationDate = vaccinationDate[i],
                    NextVaccinationDate = nextVaccinationDate[i],
                    Remarks = remarks[i],
                    FiscalYearId = model.FiscalYearId,
                    FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYearId),
                    CreatedBy = _workContext.CurrentCustomer.Id

                }); ;
            }
            await _vaccinationService.InsertVaccinationList(purnaKhops);

            return RedirectToAction("List");


        }
        public async Task<IActionResult> GetFarmByFiscalYear(string fiscalyear)
        {
            var entityId = _workContext.CurrentCustomer.EntityId;
            var users = _customerService.GetCustomerByLssId(null, entityId);
            var customerids = users.Select(m => m.Id).ToList();
            var farmForPurnaKhop = await _farmForPurnaKhopService.GetFarmByFiscalYear(customerids, fiscalyear);
            var farms = farmForPurnaKhop.Select(m => m.Farm).ToList();
            return Ok(farms);

        }
        [HttpGet]
        public async Task<IActionResult> GetSpeciesByFiscalYear(string fiscalyear, string farmid)
        {
            var entityId = _workContext.CurrentCustomer.EntityId;
            var users = _customerService.GetCustomerByLssId(null, entityId);
            var customerids = users.Select(m => m.Id).ToList();
            var farmForPurnaKhop = await _farmForPurnaKhopService.GetSpeciesbyFarmName(fiscalyear, farmid);
            return Ok(farmForPurnaKhop);

        }
        public async Task<IActionResult> GetVaccination()
        {

            var vaccination = new SelectList(await _vaccinationTypeService.FiletrVaccinationType("Vaccine"), "Id", "MedicalName").ToList();
            vaccination.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            return Ok(vaccination);
        }
    }
}
