using LIMS.Core;
using LIMS.Domain.Breed;
using LIMS.Domain.Seo;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Mvc.Filters;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Breed;
using LIMS.Services.Localization;
using LIMS.Services.Security;
using LIMS.Services.Stores;
using LIMS.Web.Areas.Admin.Extensions;
using LIMS.Web.Areas.Admin.Extensions.Mapping;
using LIMS.Web.Areas.Admin.Models.Breed;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class LivestockSpeciesController : BaseAdminController
    {
        private readonly ILivestockSpeciesService _SpeciesService;
        private readonly ILivestockBreedService _BreedService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;
        private readonly SeoSettings _seoSettings;

        public LivestockSpeciesController(
            ILivestockSpeciesService SpeciesService,
            ILanguageService languageService,
            ILocalizationService localizationService,
            IStoreService storeService,
            IWorkContext workContext,
            SeoSettings seoSettings,
            ILivestockBreedService BreedService
            )
        {
            _SpeciesService = SpeciesService;
            _BreedService = BreedService;
            _languageService = languageService;
            _localizationService = localizationService;
            _storeService = storeService;
            _workContext = workContext;
            _seoSettings = seoSettings;
        }

        public IActionResult Index() => RedirectToAction("List");

        public IActionResult List() => View();

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command)
        {
            var species = await _SpeciesService.GetBreed(command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult {
                Data = species,
                Total = species.TotalCount
            };
            return Json(gridModel);
        }
        [PermissionAuthorizeAction(PermissionActionName.Create)]
        public async Task<IActionResult> Create()
        {
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            var sp = new LivestockSpeciesModel();

            return View(sp);
        }
        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(LivestockSpeciesModel model, IFormCollection form, bool continueEditing)
        {
            if (ModelState.IsValid)
            {

                LivestockSpecies Species = model.ToEntity();
                await _SpeciesService.InsertBreed(Species);
                SuccessNotification(_localizationService.GetResource("Admin.Species.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = Species.Id }) : RedirectToAction("List");
            }

            //If we got this far, something failed, redisplay form
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            return View(model);
        }


        [PermissionAuthorizeAction(PermissionActionName.Preview)]
        public async Task<IActionResult> Edit(string id)
        {
            var Species = await _SpeciesService.GetBreedById(id);
            if (Species == null)
                return RedirectToAction("List");
            LivestockSpeciesModel model = Species.ToModel();

            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);

            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(LivestockSpeciesModel model, IFormCollection form, bool continueEditing)
        {
            var Species = await _SpeciesService.GetBreedById(model.Id);
            if (Species == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {

                LivestockSpecies m = model.ToEntity();
                await _SpeciesService.UpdateBreed(m);
                List<LivestockBreed> breed = await _BreedService.GetBreedBySpeciesId(m.Id);
                breed.ForEach(c => c.Species = m);
                await _BreedService.UpdateBreed(breed);
                SuccessNotification(_localizationService.GetResource("Admin.Species.Updated"));
                if (continueEditing)
                {
                    //selected tab
                    await SaveSelectedTabIndex();

                    return RedirectToAction("Edit", new { id = model.Id });
                }
                return RedirectToAction("List");
            }

            //If we got this far, something failed, redisplay form
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);

            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Delete)]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var Species = await _SpeciesService.GetBreedById(id);
            if (Species == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                await _SpeciesService.DeleteBreed(Species);

                SuccessNotification(_localizationService.GetResource("Admin.Species.Deleted"));
                return RedirectToAction("List");
            }
            ErrorNotification(ModelState);
            return RedirectToAction("Edit", new { id = id });
        }

    }
}
