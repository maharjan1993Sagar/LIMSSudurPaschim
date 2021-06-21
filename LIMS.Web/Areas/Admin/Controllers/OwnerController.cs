using LIMS.Framework.Kendoui;
using LIMS.Framework.Mvc.Filters;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Ainr;
using LIMS.Services.Breed;
using LIMS.Services.Localization;
using LIMS.Services.Security;
using LIMS.Web.Areas.Admin.Extensions.Mapping;
using LIMS.Web.Areas.Admin.Models.AInR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class OwnerController : BaseAdminController
    {

        private readonly IOwnerService _ownerService;
        private readonly ISpeciesService _speciesService;
        private readonly IBreedService _breedService;

        private readonly IFarmService _farmService;
        private readonly ILocalizationService _localizationService;
        private readonly ILanguageService _languageService;
        public OwnerController(ILocalizationService localizationService, IOwnerService OwnerService, IFarmService farmService, ILanguageService languageService, ISpeciesService speciesService, IBreedService breedService)
        {
            _localizationService = localizationService;
            _ownerService = OwnerService;
            _languageService = languageService;
            _farmService = farmService;
            _speciesService = speciesService;
            _breedService = breedService;
        }
        public IActionResult Index() => RedirectToAction("List");

        public IActionResult List() => View();

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command)
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
        public async Task<IActionResult> OwnerList(string farmid, DataSourceRequest command)
        {
            var animal = await _ownerService.GetOwner(farmid,command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult {
                Data = animal,
                Total = animal.TotalCount
            };
            return Json(gridModel);
        }
        public async Task<IActionResult> Create(string farmid)
        {
            var Provience = GetProvinceList();
            Provience.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Provience = Provience;
            var ethnicGroup = new List<SelectListItem>() {
                new SelectListItem{
                    Text="Dalit",
                    Value="Dalit"
                },
                new SelectListItem{
                    Text="JanaJati",
                    Value="JanaJati"
                },
                new SelectListItem{
                    Text="Aanya",
                    Value="Aanya"
                },
            };
            ethnicGroup.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            var workerType = new List<SelectListItem>() {
                new SelectListItem{
                    Text="Owner",
                    Value="Owner"
                },
                new SelectListItem{
                    Text="Keeper",
                    Value="Keeper"
                },
            };
            workerType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Type = workerType;
            ViewBag.EthinicGroup = ethnicGroup;
            var farmEntity = await _farmService.GetFarmById(farmid);
            var model = new OwnerModel();
            model.Farm = farmEntity.ToModel();
            List<SelectListItem> Education = GetEducation();
            Education.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Education = Education;
            return View(model);
        }
        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(OwnerModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {

                var Owner = model.ToEntity();
                Owner.Farm = await _farmService.GetFarmById(model.FarmId);
                
                await _ownerService.InsertOwner(Owner);
                SuccessNotification(_localizationService.GetResource("Admin.Owner.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = Owner.Id }) : RedirectToAction("Create", new { farmid = Owner.FarmId });
            }
            List<SelectListItem> Education = GetEducation();
            Education.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Education = Education;
            var ethnicGroup = new List<SelectListItem>() {
                new SelectListItem{
                    Text="Dalit",
                    Value="Dalit"
                },
                new SelectListItem{
                    Text="JanaJati",
                    Value="JanaJati"
                },
                new SelectListItem{
                    Text="Aanya",
                    Value="Aanya"
                },
            };
            ethnicGroup.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            var workerType = new List<SelectListItem>() {
                new SelectListItem{
                    Text="Owner",
                    Value="Owner"
                },
                new SelectListItem{
                    Text="Keeper",
                    Value="Keeper"
                },
            };
            workerType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Type = workerType;
            ViewBag.EthinicGroup = ethnicGroup;
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            var Provience = GetProvinceList();
            Provience.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Provience = Provience;

            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var Owner = await _ownerService.GetOwnerById(id);
            if (Owner == null)
                return RedirectToAction("List");
            var model = Owner.ToModel();
            var Provience = GetProvinceList();
            Provience.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            ViewBag.Provience = Provience;

            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            var ethnicGroup = new List<SelectListItem>() {
                new SelectListItem{
                    Text="Dalit",
                    Value="Dalit"
                },
                new SelectListItem{
                    Text="JanaJati",
                    Value="JanaJati"
                },
                new SelectListItem{
                    Text="Aanya",
                    Value="Aanya"
                },
            };
            ethnicGroup.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            var workerType = new List<SelectListItem>() {
                new SelectListItem{
                    Text="Owner",
                    Value="Owner"
                },
                new SelectListItem{
                    Text="Keeper",
                    Value="Keeper"
                },
            };
            workerType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Type = workerType;
            ViewBag.EthinicGroup = ethnicGroup;
            List<SelectListItem> Education = GetEducation();
            Education.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Education = Education;
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(OwnerModel model, bool continueEditing)
        {
            var Owner = await _ownerService.GetOwnerById(model.Id);
            if (Owner == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var m = model.ToEntity(Owner);
                Owner.Farm = await _farmService.GetFarmById(model.FarmId);
                await _ownerService.UpdateOwner(m);

                SuccessNotification(_localizationService.GetResource("Admin.Owner.Updated"));
                if (continueEditing)
                {
                    //selected tab
                    await SaveSelectedTabIndex();

                    return RedirectToAction("Edit", new { id = model.Id });
                }
                return RedirectToAction("Create", new { farmid = Owner.FarmId });
            }
            var ethnicGroup = new List<SelectListItem>() {
                new SelectListItem{
                    Text="Dalit",
                    Value="Dalit"
                },
                new SelectListItem{
                    Text="JanaJati",
                    Value="JanaJati"
                },
                new SelectListItem{
                    Text="Aanya",
                    Value="Aanya"
                },
            };
            ethnicGroup.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            var workerType = new List<SelectListItem>() {
                new SelectListItem{
                    Text="Owner",
                    Value="Owner"
                },
                new SelectListItem{
                    Text="Keeper",
                    Value="Keeper"
                },
            };
            workerType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Type = workerType;
            ViewBag.EthinicGroup = ethnicGroup;
            var Provience = GetProvinceList();
            Provience.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            ViewBag.Provience = Provience;
            //If we got this far, something failed, redisplay form
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
             List<SelectListItem> Education = GetEducation();
            Education.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Education = Education;
            return View(model);
        }
        private List<SelectListItem> GetProvinceList()
        {
            return new List<SelectListItem> {

                new SelectListItem { Text = _localizationService.GetResource("Common.Province.Four"), Value = "Province 4", Selected = true },

            };
        }
        private List<SelectListItem> GetEducation()
        {
            return new List<SelectListItem>() {
                new SelectListItem{Text="Bachelor",Value="Bachelor"},
                new SelectListItem{Text="+2", Value="+2" },
                 new SelectListItem{Text="Secondary level", Value="Secondary level" },
                  new SelectListItem{Text="Below class 8", Value="Below class 8" }
            };

        }


    }

}

