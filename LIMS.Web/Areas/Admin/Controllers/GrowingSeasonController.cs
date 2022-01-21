using LIMS.Core;
using LIMS.Domain.Seo;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Mvc.Filters;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Breed;
using LIMS.Services.Localization;
using LIMS.Services.Security;
using LIMS.Services.Stores;
using LIMS.Web.Areas.Admin.Extensions.Mapping;
using LIMS.Web.Areas.Admin.Models.Breed;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class GrowingSeasonController:BaseAdminController
    {
        private readonly ICropsSeason _breedService;
        private readonly IBreedService _SpeciesService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;
        private readonly SeoSettings _seoSettings;

        public GrowingSeasonController(
            ICropsSeason breedService,
            IBreedService SpeciesService,
            ILanguageService languageService,
            ILocalizationService localizationService,
            IStoreService storeService,
            IWorkContext workContext,
            SeoSettings seoSettings
            )
        {
            _breedService = breedService;
            _SpeciesService = SpeciesService;
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
            var breed = await _breedService.GetBreed(command.Page - 1, command.PageSize);

            var gridModel = new DataSourceResult {
                Data = breed,
                Total = breed.TotalCount
            };
            return Json(gridModel);
        }

        [PermissionAuthorizeAction(PermissionActionName.Create)]
        public async Task<IActionResult> Create()
        {
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            var species = new SelectList(await _SpeciesService.GetBreed(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.SpeciesId = species;
           
            return View();
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(CropsSeasonModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var Breed = model.ToEntity();
                Breed.Species = await _SpeciesService.GetBreedById(model.SpeciesId);
                await _breedService.InsertBreed(Breed);
                SuccessNotification(_localizationService.GetResource("Admin.Breed.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = Breed.Id }) : RedirectToAction("List");
            }
            var species = new SelectList(await _SpeciesService.GetBreed(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.SpeciesId = species;
            
            //If we got this far, something failed, redisplay form
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            return View(model);
        }


        [PermissionAuthorizeAction(PermissionActionName.Preview)]
        public async Task<IActionResult> Edit(string id)
        {
            var breed = await _breedService.GetBreedById(id);
            if (breed == null)
                //No blog post found with the specified id
                return RedirectToAction("List");
            var model = breed.ToModel();
            var species = new SelectList(await _SpeciesService.GetBreed(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.SpeciesId = species;
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(CropsSeasonModel model, bool continueEditing)
        {
            var breed = await _breedService.GetBreedById(model.Id);
            if (breed == null)

                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var m = model.ToEntity(breed);
                m.Species = await _SpeciesService.GetBreedById(model.SpeciesId);
                await _breedService.UpdateBreed(m);

                SuccessNotification(_localizationService.GetResource("Admin.Breed.Updated"));
                if (continueEditing)
                {
                    //selected tab
                    await SaveSelectedTabIndex();
                    return RedirectToAction("Edit", new { id = model.Id });
                }
                return RedirectToAction("List");
            }
            var species = new SelectList(await _SpeciesService.GetBreed(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.SpeciesId = species;
            
            //If we got this far, something failed, redisplay form
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Delete)]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var breed = await _breedService.GetBreedById(id);
            if (breed == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                await _breedService.DeleteBreed(breed);
                SuccessNotification(_localizationService.GetResource("Admin.Breed.Deleted"));
                return RedirectToAction("List");
            }
            ErrorNotification(ModelState);
            return RedirectToAction("Edit", new { id = id });
        }
    }
}
