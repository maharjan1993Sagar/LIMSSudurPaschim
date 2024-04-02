using LIMS.Core;
using LIMS.Domain.AInR;
using LIMS.Framework.Kendoui;
using LIMS.Services.Ainr;
using LIMS.Services.Basic;
using LIMS.Services.Breed;
using LIMS.Services.Customers;
using LIMS.Services.Localization;
using LIMS.Services.MoAMAC;
using LIMS.Services.Semen;
using LIMS.Services.User;
using LIMS.Web.Areas.Admin.Extensions.Mapping;
using LIMS.Web.Areas.Admin.Models.Semen;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class SemenDistributionController : BaseAdminController
    {
        #region fields
        private readonly IServiceProviderService _serviceProvider;
        private readonly ISemenDistributionService _semenDistributionService;
        private readonly IWorkContext _workContext;
        private readonly ICustomerService _customerService;
        private readonly ILocalizationService _localizationService;
        private readonly ISpeciesService _speciesService;
        private readonly IBreedService _breedService;
        private readonly IAnimalRegistrationService _animalRegistrationService;
        private readonly IFiscalYearService _fiscalYearService;
        private readonly ILssService _lssService;
        private readonly IVhlsecService _vhlsecService;
        #endregion
        #region semendistribution

        public SemenDistributionController(
            ISemenDistributionService semenDistribution,
            IServiceProviderService serviceProvider,
            ICustomerService customerService,
            IWorkContext workContext,
            ILocalizationService localizationService,
            ISpeciesService speciesService,
            IBreedService breedService,
            IAnimalRegistrationService animalRegistrationService,
            IFiscalYearService fiscalYearService,
            IVhlsecService vhlsecService,
        ILssService lssService
            )
        {
            _serviceProvider = serviceProvider;
            _semenDistributionService = semenDistribution;
            _customerService = customerService;
            _workContext = workContext;
            _localizationService = localizationService;
            _speciesService = speciesService;
            _breedService = breedService;
            _animalRegistrationService = animalRegistrationService;
            _fiscalYearService = fiscalYearService;
            _lssService = lssService;
            _vhlsecService = vhlsecService;
        }
        public IActionResult Index() => RedirectToAction("List");
        public IActionResult List()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command)
        {
            string id = _workContext.CurrentCustomer.EntityId;
            var customerids = _customerService.GetCustomerByLssId(null, id);
            var ids = customerids.Select(m => m.Id).ToList();
            var semenDistribution = await _semenDistributionService.GetSemenDistribution(ids);
            var gridModel = new DataSourceResult {
                Data = semenDistribution,
                Total = semenDistribution.TotalCount
            };
            return Json(gridModel);
        }
        
        public async Task<IActionResult> Create()
        {
            var serviceProvider = await _serviceProvider.GetServiceProvider();
            var technician = new SelectList(serviceProvider, "Id","NameEnglish").ToList();
            technician.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Technician = technician;
            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;
            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            species=species.Where(m => m.Text.ToLower() != "fish").ToList();
            ViewBag.species = species;

            var vhlsec = await _vhlsecService.GetVhlsec();
            var lss = await _lssService.GetLss();
            var lssname = lss.Select(m => m.NameEnglish).ToList();
            var vhlsecname = vhlsec.Select(m => m.NameEnglish).ToList();
            vhlsecname.AddRange(lssname);
            var organizationname=vhlsecname.Select(x => new SelectListItem { Text = x, Value = x })
                 .ToList();
            ViewBag.OrganizationName = organizationname;

            var organizationtype = new List<SelectListItem>(){
                  new SelectListItem {
                    Text="Select type",
                    Value=""
            },
                new SelectListItem {
                    Text="Technician",
                    Value="Technician"
            },
                new SelectListItem {
                    Text="Organization",
                    Value="Organization"
            }
            };
            ViewBag.Type = organizationtype;
            var semenDistribution = new SemenDistributionModel();
            return View(semenDistribution);
        }
        [HttpPost]
        public async Task<IActionResult> Create(SemenDistributionModel model)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.AnimalRegistrationId))
                {
                    var animal = new AnimalRegistration() {
                        Name = model.BullName,
                        SpeciesId = model.SpeciesId,
                        Species = await _speciesService.GetSpeciesById(model.SpeciesId),
                        Breed = await _breedService.GetBreedById(model.BreedId),
                        BreedId = model.BreedId,
                        EarTagNo = model.EarTag,
                       DamId=model.DamId,
                       SireId=model.SireId,
                       Gender="Male",
                        CreatedBy = _workContext.CurrentCustomer.Id,
                        Source = _workContext.CurrentCustomer.OrgName
                    };
                    await _animalRegistrationService.InsertAnimalRegistration(animal);

                    model.AnimalRegistrationId = animal.Id;

                }
               var semenDistribution = model.ToEntity();
                if (model.Type == "Technician")
                {
                    semenDistribution.ServiceProvider = await _serviceProvider.GetServiceProviderById(semenDistribution.ServiceProviderId);
                }

                    semenDistribution.FiscalYear= await _fiscalYearService.GetFiscalYearById(model.FiscalYearId);
                semenDistribution.AnimalRegistration= await _animalRegistrationService.GetAnimalRegistrationById(model.AnimalRegistrationId);
                semenDistribution.CreatedBy = _workContext.CurrentCustomer.Id;
                semenDistribution.CreatedAt = DateTime.Now.ToShortDateString();
                await _semenDistributionService.InsertSemenDIstribution(semenDistribution);

                return RedirectToAction("List");
            }
            var serviceProvider = await _serviceProvider.GetServiceProvider();
            var technician = new SelectList(serviceProvider, "NameEnglish", "Id").ToList();
            technician.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Technician = technician;
            return View(model);

        }
        public async Task<IActionResult> Edit(string id)
        {
                var serviceProvider = await _serviceProvider.GetServiceProvider();
                var technician = new SelectList(serviceProvider, "NameEnglish", "Id").ToList();
                technician.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
                ViewBag.Technician = technician;
               var semenDistribution = _semenDistributionService.GetSemenDistributionById(id);
                return View(semenDistribution);
            
        }
        [HttpPost]
        public async Task<IActionResult> Edit(SemenDistributionModel model)
        {
            var semenDistribution = _semenDistributionService.GetSemenDistributionById(model.Id);
            if (semenDistribution == null)
                //No blog post found with the specified id
                return RedirectToAction("List");
            if (ModelState.IsValid)
            {
                var entity = model.ToEntity();
                entity.ServiceProvider = await _serviceProvider.GetServiceProviderById(entity.ServiceProviderId);
                await _semenDistributionService.UpdateSemenDistribution(entity);
                return RedirectToAction("List");
            }
            return View(model); ;


        }
        #endregion
    }
}
