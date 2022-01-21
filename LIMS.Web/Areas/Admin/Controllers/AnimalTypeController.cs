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
using LIMS.Web.Areas.Admin.Extensions.Mapping;
using LIMS.Web.Areas.Admin.Models.Breed;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class AnimalTypeController:BaseAdminController
    {
        private readonly IAnimalTypeService _animalTypeService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;
        private readonly SeoSettings _seoSettings;
        private readonly ILivestockSpeciesService _speciesService;
        public AnimalTypeController(
            IAnimalTypeService animalTypeService,
            ILanguageService languageService,
            ILocalizationService localizationService,
            IStoreService storeService,
            IWorkContext workContext,
            SeoSettings seoSettings,
            ILivestockSpeciesService speciesService
            )
        {
            _animalTypeService = animalTypeService;
            _languageService = languageService;
            _localizationService = localizationService;
            _storeService = storeService;
            _workContext = workContext;
            _seoSettings = seoSettings;
            _speciesService = speciesService;
        }

        public IActionResult Index() => RedirectToAction("List");

        public IActionResult List() => View();

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command)
        {
            var animalType = await _animalTypeService.GetAnimalType(command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult {
                Data = animalType,
                Total = animalType.TotalCount
            };
            return Json(gridModel);
        }
        [PermissionAuthorizeAction(PermissionActionName.Create)]
        public async Task<IActionResult> Create()
        {
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            var species = new SelectList(await _speciesService.GetBreed(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.SpeciesId = species;

            return View();
        }
        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(AnimalTypeModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {

                var animalType = model.ToEntity();
                animalType.Species = await _speciesService.GetBreedById(model.SpeciesId);
                await _animalTypeService.InsertAnimalType(animalType);
                SuccessNotification(_localizationService.GetResource("Admin.AnimalType.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = animalType.Id }) : RedirectToAction("List");
            }
            var species = new SelectList(await _speciesService.GetBreed(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.SpeciesId = species;
            //If we got this far, something failed, redisplay form
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            return View(model);
        }


        [PermissionAuthorizeAction(PermissionActionName.Preview)]
        public async Task<IActionResult> Edit(string id)
        {
            var animalType = await _animalTypeService.GetAnimalTypeById(id);
            if (animalType == null)
                return RedirectToAction("List");
            AnimalTypeModel model = animalType.ToModel();

            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            var species = new SelectList(await _speciesService.GetBreed(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.SpeciesId = species;
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(AnimalTypeModel model, bool continueEditing)
        {
            var animalType = await _animalTypeService.GetAnimalTypeById(model.Id);
            if (animalType == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var m = model.ToEntity(animalType);
                animalType.Species = await _speciesService.GetBreedById(model.SpeciesId);
                await _animalTypeService.UpdateAnimalType(m);
              
                SuccessNotification(_localizationService.GetResource("Admin.AnimalType.Updated"));
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
            var species = new SelectList(await _speciesService.GetBreed(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.SpeciesId = species;
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Delete)]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var AnimalType = await _animalTypeService.GetAnimalTypeById(id);
            if (AnimalType == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                await _animalTypeService.DeleteAnimalType(AnimalType);

                SuccessNotification(_localizationService.GetResource("Admin.AnimalType.Deleted"));
                return RedirectToAction("List");
            }
            ErrorNotification(ModelState);
            return RedirectToAction("Edit", new { id = id });
        }


        [AllowAnonymous]
        public async Task<IActionResult> GetAnimalType(string id)
        {
            return Ok(await _animalTypeService.GetAnimalTypeBySpeciesId(id));
        }
    }
}
