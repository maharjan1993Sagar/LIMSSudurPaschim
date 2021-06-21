using LIMS.Core;
using LIMS.Framework.Controllers;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Mvc.Filters;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Ainr;
using LIMS.Services.Breed;
using LIMS.Services.Common;
using LIMS.Services.ExportImport;
using LIMS.Services.Localization;
using LIMS.Services.Security;
using LIMS.Web.Areas.Admin.Extensions.Mapping;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Web.Areas.Admin.Models.AInR;
using LIMS.Web.Areas.Admin.Models.Livestock;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class AnimalRegistrationController : BaseAdminController
    {
        private readonly IAnimalRegistrationService _animalRegistrationService;
        private readonly ISpeciesService _speciesService;
        private readonly IBreedService _breedService;
        private readonly IEarTagService _earTagService;
        private readonly IFarmService _farmService;
        private readonly ILocalizationService _localizationService;
        private readonly ILanguageService _languageService;
        private readonly IWorkContext _workContext;

        public AnimalRegistrationController(ILocalizationService localizationService, IAnimalRegistrationService animalRegistrationService, IFarmService farmService, ILanguageService languageService, ISpeciesService speciesService, IBreedService breedService, IEarTagService earTagService, IWorkContext workContext)
        {
            _localizationService = localizationService;
            _animalRegistrationService = animalRegistrationService;
            _languageService = languageService;
            _farmService = farmService;
            _speciesService = speciesService;
            _breedService = breedService;
            _workContext = workContext;
            _earTagService = earTagService;
        }

        public IActionResult Index() => RedirectToAction("List");

        public IActionResult List() => View();

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command, FarmListModel model)
        {
            var farm = await _farmService.SearchFarm("", command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult {
                Data = farm,
                Total = farm.TotalCount
            };
            return Json(gridModel);
        }

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> AnimalList(string farmid, DataSourceRequest command, AnimalListModel model)
        {
            var animal = await _animalRegistrationService.GetAnimalRegistrationByFarmId(farmid, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult {
                Data = animal,
                Total = animal.TotalCount
            };
            return Json(gridModel);
        }

        public async Task<IActionResult> Create(string farmid)
        {
            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.SpeciesId = species;
            var Provience = GetProvinceList();
            Provience.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Provience = Provience;
            var entryType = GetEntryType();
            entryType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.EntryType = entryType;
            var type = BreedTypeHelper.GetBreedType();
            type.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Type = type;
            var farmEntity = await _farmService.GetFarmById(farmid);
            var model = new AnimalRegistrationModel();
            model.FarmModel = farmEntity.ToModel();
            AnimalListModel animalListModel = new AnimalListModel();
            animalListModel.FarmId = farmid;
            model.AnimalListModel = animalListModel;
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(AnimalRegistrationModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var animalRegistration = model.ToEntity();
                animalRegistration.Farm = await _farmService.GetFarmById(model.FarmId);
                animalRegistration.Species = await _speciesService.GetSpeciesById(model.SpeciesId);
                animalRegistration.Breed = await _breedService.GetBreedById(model.BreedId);
                animalRegistration.CreatedBy = _workContext.CurrentCustomer.Id;
                animalRegistration.Source = _workContext.CurrentCustomer.OrgName;
                await _animalRegistrationService.InsertAnimalRegistration(animalRegistration);

                SuccessNotification(_localizationService.GetResource("Admin.AnimalRegistration.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = animalRegistration.Id }) : RedirectToAction("Create", new { farmid = model.FarmId });
            }
            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.SpeciesId = species;
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            var entryType = GetEntryType();
            entryType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.EntryType = entryType;

            var Provience = GetProvinceList();
            Provience.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            ViewBag.Provience = Provience;
            var type = BreedTypeHelper.GetBreedType();
            type.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Type = type;
            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var animalRegistration = await _animalRegistrationService.GetAnimalRegistrationById(id);
            if (animalRegistration == null)
                return RedirectToAction("List");
            var model = animalRegistration.ToModel();
            var Provience = GetProvinceList();
            Provience.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            var entryType = GetEntryType();
            entryType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.EntryType = entryType;
            ViewBag.Provience = Provience;
            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.SpeciesId = species;
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            var type = BreedTypeHelper.GetBreedType();
            type.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Type = type;
            AnimalListModel animalListModel = new AnimalListModel();
            animalListModel.FarmId = animalRegistration.FarmId;
            model.AnimalListModel = animalListModel;
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(AnimalRegistrationModel model, bool continueEditing)
        {
            var animalRegistration = await _animalRegistrationService.GetAnimalRegistrationById(model.Id);
            if (animalRegistration == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var m = model.ToEntity(animalRegistration);
                animalRegistration.Farm = await _farmService.GetFarmById(model.FarmId);
                animalRegistration.Species = await _speciesService.GetSpeciesById(model.SpeciesId);
                animalRegistration.Breed = await _breedService.GetBreedById(model.BreedId);

                await _animalRegistrationService.UpdateAnimalRegistration(m);

                SuccessNotification(_localizationService.GetResource("Admin.AnimalRegistration.Updated"));
                if (continueEditing)
                {
                    //selected tab
                    await SaveSelectedTabIndex();

                    return RedirectToAction("Edit", new { id = model.Id });
                }
                return RedirectToAction("Create", new { farmid = model.FarmId });
            }
            var entryType = GetEntryType();
            entryType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.EntryType = entryType;
            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.SpeciesId = species;
            var Provience = GetProvinceList();
            Provience.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            ViewBag.Provience = Provience;
            var type = BreedTypeHelper.GetBreedType();
            type.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Type = type;

            //If we got this far, something failed, redisplay form
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);

            return RedirectToAction("Create");
        }

        private List<SelectListItem> GetProvinceList()
        {
            return new List<SelectListItem> {

                new SelectListItem { Text = _localizationService.GetResource("Common.Province.Four"), Value = "Province 4", Selected = true },

            };
        }
        [HttpGet]
        public async Task<IActionResult> GetBreed(string breedType,string species) {
            var breeds= await _breedService.GetBreedByBreedType(breedType);
            breeds = breeds.Where(m => m.Species.Id == species).ToList();
            return Json(breeds.ToList());
        
        
        }
        private List<SelectListItem> GetEntryType()
        {
            return new List<SelectListItem> {
                new SelectListItem {
                    Text="start of program",
                    Value="start of program"
                },
                new SelectListItem {
                    Text="From outside",
                    Value="From outside"

                },
                new SelectListItem {
                    Text="Other",
                    Value="Other"
                 }
            };
        }

        #region Ear Tag
        private string ProcessEarTagNo(string earTag)
        {
            var individualDigits = earTag.ToCharArray().Reverse().Select(x => int.Parse(x.ToString()));
            var checksum = 0;
            for (var i = 0; i < individualDigits.Count(); i++)
            {
                var currentDigit = individualDigits.ElementAt(i);
                checksum += (i + 1) * currentDigit;
            }
            checksum = checksum % 9;
            return earTag + checksum.ToString();
        }

        [HttpPost]
        public async Task<IActionResult> GenerateEarTag(int noOfDigit, int from, int to)
        {
            var existingEarTags = await _earTagService.GetEarTags(from, to);
            if (existingEarTags.Count > 0)
                return Json(new { status = "failure", message = _localizationService.GetResource("Admin.EarTag.AlreadyExists") });
            var tagList = new List<string>();
            while (to >= from)
            {
                var strEarTag = ProcessEarTagNo(from.ToString("D" + noOfDigit));
                await _earTagService.InsertEarTag(new Domain.AInR.EarTag {
                    SerialNo = from,
                    EarTagNo = strEarTag,
                    CreatedBy = _workContext.CurrentCustomer.Id,
                    CreatedAt = DateTime.Now.ToString()
                });
                tagList.Add(strEarTag);
                from++;
            }
            return Json(new { status = "success", data = tagList, message = _localizationService.GetResource("Admin.EarTag.Generated") });
        }

        public IActionResult EarTag() => View();

        [HttpPost]
        public async Task<IActionResult> EarTagList(DataSourceRequest command)
        {
            var earTags = await _earTagService.SearchEarTag(pageIndex: command.Page - 1, pageSize: command.PageSize);
            var gridModel = new DataSourceResult {
                Data = earTags,
                Total = earTags.TotalCount
            };
            return Json(gridModel);
        }

        [PermissionAuthorizeAction(PermissionActionName.Export)]
        [HttpPost, ActionName("EarTag")]
        [FormValueRequired("download-eartag-pdf")]
        public async Task<IActionResult> DownloadEarTagAsPdf(EarTag model, [FromServices] IPdfService pdfService)
        {
            var earTags = await _earTagService.GetEarTags(model.From, model.To);
            var newEarTags = earTags.ToList();
            newEarTags.Reverse();
            try
            {
                byte[] bytes;
                using (var stream = new MemoryStream())
                {
                    await pdfService.PrintEarTagsToPdf(stream, newEarTags);
                    bytes = stream.ToArray();
                }
                return File(bytes, "application/pdf", "eartags.pdf");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("EarTag");
            }
        }
        [PermissionAuthorizeAction(PermissionActionName.Export)]
        [HttpPost, ActionName("EarTag")]
        [FormValueRequired("download-eartag-excel")]
        public async Task<IActionResult> DownloadEarTagAsExcel(EarTag model, [FromServices] IExportManager exportManager)
        {
            var earTags = await _earTagService.GetEarTags(model.From, model.To);
            var newEarTags = earTags.Select(x => new LIMS.Domain.AInR.EarTag { EarTagNo = Convert.ToInt32(x.EarTagNo).ToString() }).ToList();
            newEarTags.Reverse();
            try
            {
                byte[] bytes=exportManager.ExpertEarTagToXlsx(newEarTags);
                  
                return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "eartags.xlsx");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("EarTag");
            }
        }
        #endregion

        #region Animal Autocomplete
        public virtual async Task<IActionResult> SearchAnimalAutoComplete(string term,string farm)
        {
            var result = await _animalRegistrationService.SearchAnimal(farm,term);
            return Json(result);
        }
        public virtual async Task<IActionResult> SearchMaleAnimalAutoComplete(string term)
     {
            string createdBy =  _workContext.CurrentCustomer.Id;
            var result = await _animalRegistrationService.SearchMaleAnimal(createdBy,term);
            return Json(result);
        }
        #endregion
    }
}
