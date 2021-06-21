using LIMS.Core;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Ainr;
using LIMS.Services.AnimalBreeding;
using LIMS.Services.Breed;
using LIMS.Services.Localization;
using LIMS.Services.Security;
using LIMS.Web.Areas.Admin.Models.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class AnimalBreedingController : BaseAdminController
    {
        private readonly IAnimalRegistrationService _animalRegistrationService;
        private readonly ISpeciesService _speciesService;
        private readonly IBreedService _breedService;
        private readonly IWorkContext _workContext;
        private readonly IFarmService _farmService;
        private readonly ILocalizationService _localizationService;
        private readonly ILanguageService _languageService;
        private readonly IHeatRecordingService _heatRecordingService;
        public AnimalBreedingController(ILocalizationService localizationService, IAnimalRegistrationService animalRegistrationService, IFarmService farmService, ILanguageService languageService, ISpeciesService speciesService, IBreedService breedService, IWorkContext workContext, IHeatRecordingService heatRecordingService)
        {
            _localizationService = localizationService;
            _animalRegistrationService = animalRegistrationService;
            _languageService = languageService;
            _farmService = farmService;
            _speciesService = speciesService;
            _breedService = breedService;
            _workContext = workContext;
            _heatRecordingService = heatRecordingService;
           
        }
        public IActionResult Index() => RedirectToAction("List");

        public IActionResult List() => View();

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command)
        {
            var userid = _workContext.CurrentCustomer.Id;
            var animalRegistration = await _animalRegistrationService.GetAnimalRegistrationByCreatedBy(userid,command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult {
                Data = animalRegistration,
                Total = animalRegistration.TotalCount
            };
            return Json(gridModel);
        }

        






    }
}
