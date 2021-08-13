using LIMS.Core;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Mvc.Filters;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Bali;
using LIMS.Services.Basic;
using LIMS.Services.Breed;
using LIMS.Services.Localization;
using LIMS.Services.Security;
using LIMS.Web.Areas.Admin.Extensions.Mapping;
using LIMS.Web.Areas.Admin.Models.Bali;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class IncubationCenterController:BaseAdminController
    {
        private readonly IIncuvationCenterService _animalRegistrationService;
        private readonly ISpeciesService _speciesService;
        private readonly IBreedService _breedService;
        private readonly ILocalizationService _localizationService;
        private readonly ILanguageService _languageService;
        private readonly IWorkContext _workContext;
        private readonly IFiscalYearService _fiscalYearService;
        private readonly ITalimService _talimService;

        public IncubationCenterController(ILocalizationService localizationService,
            IIncuvationCenterService animalRegistrationService,
            ILanguageService languageService,
            ISpeciesService speciesService,
            IBreedService breedService,
            IWorkContext workContext,
            IFiscalYearService fiscalYearService,
            ITalimService talimService

            )
        {
            _localizationService = localizationService;
            _animalRegistrationService = animalRegistrationService;
            _languageService = languageService;
            _speciesService = speciesService;
            _breedService = breedService;
            _workContext = workContext;
            _fiscalYearService = fiscalYearService;
            _talimService = talimService;
        }

        public IActionResult Index() => RedirectToAction("List");

        public IActionResult List() => View();

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command)
        {
            var id = _workContext.CurrentCustomer.Id;
            var bali = await _animalRegistrationService.GetincuvationCenter(id, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult {
                Data = bali,
                Total = bali.TotalCount
            };
            return Json(gridModel);
        }


        public async Task<IActionResult> Create()
        {
            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.SpeciesId = species;
            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear").ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;
            var ValueChain = new List<SelectListItem>() {
                new SelectListItem {
                    Text="Diary",
                    Value="Diary"
                },
                  new SelectListItem {
                    Text="Meat",
                    Value="Meat"
                },
                  new SelectListItem {
                    Text="Milk",
                    Value="Milk"
                },
                   new SelectListItem {
                    Text="Egg",
                    Value="Egg"
                },
                    new SelectListItem {
                    Text="Vegitable",
                    Value="Vegitable"
                },
                      new SelectListItem {
                    Text="Production",
                    Value="Production"
                },
                       new SelectListItem {
                    Text="Others",
                    Value="Others"
                },
            };
            ValueChain.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.ValueChain = ValueChain;
            var NatureOfWork = new List<SelectListItem>() {
                new SelectListItem {
                    Text="Livestock farm",
                    Value="Livestock farm"
                },
                  new SelectListItem {
                    Text="Poultry farm",
                    Value="Poultry farm"
                },
                  new SelectListItem {
                    Text="Fish farm",
                    Value="Fish farm"
                },
                  
                    new SelectListItem {
                    Text="Vegitable production",
                    Value="Vegitable production"
                },
                      new SelectListItem {
                    Text="Others",
                    Value="Others"
                },
                      
            };
            NatureOfWork.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
              ViewBag.NatureOfWork =NatureOfWork;

            IncubationCenterModel model = new IncubationCenterModel();

            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(IncubationCenterModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var animalRegistration = model.ToEntity();

                animalRegistration.CreatedBy = _workContext.CurrentCustomer.Id;
                await _animalRegistrationService.InsertincuvationCenter(animalRegistration);

                SuccessNotification(_localizationService.GetResource("Admin.Create.successful"));
                return continueEditing ? RedirectToAction("Edit", new { id = animalRegistration.Id }) : RedirectToAction("Index");
            }
            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.SpeciesId = species;
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear").ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;
            var ValueChain = new List<SelectListItem>() {
                new SelectListItem {
                    Text="Diary",
                    Value="Diary"
                },
                  new SelectListItem {
                    Text="Meat",
                    Value="Meat"
                },
                  new SelectListItem {
                    Text="Milk",
                    Value="Milk"
                },
                   new SelectListItem {
                    Text="Egg",
                    Value="Egg"
                },
                    new SelectListItem {
                    Text="Vegitable",
                    Value="Vegitable"
                },
                      new SelectListItem {
                    Text="Production",
                    Value="Production"
                },
                       new SelectListItem {
                    Text="Others",
                    Value="Others"
                },
            };
            ValueChain.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.ValueChain = ValueChain;
            var NatureOfWork = new List<SelectListItem>() {
                new SelectListItem {
                    Text="Livestock farm",
                    Value="Livestock farm"
                },
                  new SelectListItem {
                    Text="Poultry farm",
                    Value="Poultry farm"
                },
                  new SelectListItem {
                    Text="Fish farm",
                    Value="Fish farm"
                },

                    new SelectListItem {
                    Text="Vegitable production",
                    Value="Vegitable production"
                },
                      new SelectListItem {
                    Text="Others",
                    Value="Others"
                },

            };
            NatureOfWork.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
              ViewBag.NatureOfWork =NatureOfWork;

            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var animalRegistration = await _animalRegistrationService.GetincuvationCenterById(id);
            if (animalRegistration == null)
                return RedirectToAction("List");
            var model = animalRegistration.ToModel();
            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.SpeciesId = species;
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear").ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;
            var ValueChain = new List<SelectListItem>() {
                new SelectListItem {
                    Text="Diary",
                    Value="Diary"
                },
                  new SelectListItem {
                    Text="Meat",
                    Value="Meat"
                },
                  new SelectListItem {
                    Text="Milk",
                    Value="Milk"
                },
                   new SelectListItem {
                    Text="Egg",
                    Value="Egg"
                },
                    new SelectListItem {
                    Text="Vegitable",
                    Value="Vegitable"
                },
                      new SelectListItem {
                    Text="Production",
                    Value="Production"
                },
                       new SelectListItem {
                    Text="Others",
                    Value="Others"
                },
            };
            ValueChain.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.ValueChain = ValueChain;
            var NatureOfWork = new List<SelectListItem>() {
                new SelectListItem {
                    Text="Livestock farm",
                    Value="Livestock farm"
                },
                  new SelectListItem {
                    Text="Poultry farm",
                    Value="Poultry farm"
                },
                  new SelectListItem {
                    Text="Fish farm",
                    Value="Fish farm"
                },

                    new SelectListItem {
                    Text="Vegitable production",
                    Value="Vegitable production"
                },
                      new SelectListItem {
                    Text="Others",
                    Value="Others"
                },

            };
            NatureOfWork.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
              ViewBag.NatureOfWork =NatureOfWork;

            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(IncubationCenterModel model, bool continueEditing)
        {
            var animalRegistration = await _animalRegistrationService.GetincuvationCenterById(model.Id);
            if (animalRegistration == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var m = model.ToEntity(animalRegistration);

                await _animalRegistrationService.UpdateincuvationCenter(m);

                SuccessNotification(_localizationService.GetResource("Admin.Update.Successful"));
                if (continueEditing)
                {
                    //selected tab
                    await SaveSelectedTabIndex();

                    return RedirectToAction("Edit", new { id = model.Id });
                }
                return RedirectToAction("Index");
            }
            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear").ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;
            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.SpeciesId = species;
            var ValueChain = new List<SelectListItem>() {
                new SelectListItem {
                    Text="Diary",
                    Value="Diary"
                },
                  new SelectListItem {
                    Text="Meat",
                    Value="Meat"
                },
                  new SelectListItem {
                    Text="Milk",
                    Value="Milk"
                },
                   new SelectListItem {
                    Text="Egg",
                    Value="Egg"
                },
                    new SelectListItem {
                    Text="Vegitable",
                    Value="Vegitable"
                },
                      new SelectListItem {
                    Text="Production",
                    Value="Production"
                },
                       new SelectListItem {
                    Text="Others",
                    Value="Others"
                },
            };
            ValueChain.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.ValueChain = ValueChain;
            var NatureOfWork = new List<SelectListItem>() {
                new SelectListItem {
                    Text="Livestock farm",
                    Value="Livestock farm"
                },
                  new SelectListItem {
                    Text="Poultry farm",
                    Value="Poultry farm"
                },
                  new SelectListItem {
                    Text="Fish farm",
                    Value="Fish farm"
                },

                    new SelectListItem {
                    Text="Vegitable production",
                    Value="Vegitable production"
                },
                      new SelectListItem {
                    Text="Others",
                    Value="Others"
                },

            };
            NatureOfWork.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
              ViewBag.NatureOfWork =NatureOfWork;



            //If we got this far, something failed, redisplay form
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);

            return RedirectToAction("Create");
        }


        public async Task<ActionResult> GetBreed(string species)
        {
            var breed = await _breedService.GetBreedBySpeciesId(species);

            return Json(breed.ToList());
        }
    }
}
