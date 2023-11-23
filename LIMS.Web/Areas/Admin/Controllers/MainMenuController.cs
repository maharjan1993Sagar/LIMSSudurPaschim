using LIMS.Core;
using LIMS.Domain.Seo;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Mvc.Filters;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.DynamicMenu;
using LIMS.Services.Localization;
using LIMS.Services.Security;
using LIMS.Services.Stores;
using LIMS.Web.Areas.Admin.Extensions.Mapping;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Web.Areas.Admin.Models.DynamicMenu;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class MainMenuController : BaseAdminController
    {
        private readonly IMainMenuService _mainMenuService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;
        private readonly SeoSettings _seoSettings;

        public MainMenuController(
            IMainMenuService mainMenuService,
            ILanguageService languageService,
            ILocalizationService localizationService,
            IStoreService storeService,
            IWorkContext workContext,
            SeoSettings seoSettings
            )
        {
            _mainMenuService = mainMenuService;
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
            var mainMenu = await _mainMenuService.GetMainMenuByUser(command.Page - 1, command.PageSize);
            
            var gridModel = new DataSourceResult {
                Data = mainMenu,
                Total = mainMenu.TotalCount
            };
            return Json(gridModel);
        }

        [PermissionAuthorizeAction(PermissionActionName.Create)]
        public async Task<IActionResult> Create()
        {
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            ViewBag.MainMenu = MainMenuLink.GetBreedType();
            return View();
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(MainMenuModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var mainMenu = model.ToEntity();
                if (model.IsUrlExternal)
                {
                    mainMenu.Url = model.ExternalUrl;
                }                   
                mainMenu.UserId = _workContext.CurrentCustomer.Id;
                mainMenu.CreatedBy = _workContext.CurrentCustomer.NameEnglish;
                mainMenu.CreatedDate = DateTime.Now;
             
                await _mainMenuService.InsertMainMenu(mainMenu);
               
                SuccessNotification(_localizationService.GetResource("Admin.MainMenu.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = mainMenu.Id }) : RedirectToAction("List");
            }
            ViewBag.MainMenu = MainMenuLink.GetBreedType();

            //If we got this far, something failed, redisplay form
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            return View(model);
        }


        [PermissionAuthorizeAction(PermissionActionName.Preview)]
        public async Task<IActionResult> Edit(string id)
        {
            ViewBag.MainMenu = MainMenuLink.GetBreedType();

            var mainMenu = await _mainMenuService.GetMainMenuById(id);
           
            if (mainMenu == null)
                //No blog post found with the specified id
                return RedirectToAction("List");
            var model = mainMenu.ToModel();
            if (model.IsUrlExternal) 
            {
                model.ExternalUrl = model.Url;
            }
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(MainMenuModel model, bool continueEditing)
        {
            var mainMenu = await _mainMenuService.GetMainMenuById(model.Id);

            if (mainMenu == null)
               
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var m = model.ToEntity(mainMenu);
                m.UserId = _workContext.CurrentCustomer.Id;
                m.EditedBy = _workContext.CurrentCustomer.NameEnglish;
                m.EditedDate = DateTime.Now;
                if (model.IsUrlExternal)
                {
                    m.Url = model.ExternalUrl;
                }
                await _mainMenuService.UpdateMainMenu(m);

                SuccessNotification(_localizationService.GetResource("Admin.MainMenu.Updated"));
                if (continueEditing)
                {
                    //selected tab
                    await SaveSelectedTabIndex();
                    return RedirectToAction("Edit", new { id = model.Id });
                }
                return RedirectToAction("List");
            }
            ViewBag.MainMenu = MainMenuLink.GetBreedType();

            //If we got this far, something failed, redisplay form
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Delete)]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var menu = await _mainMenuService.GetMainMenuById(id);
            if (menu == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                await _mainMenuService.DeleteMainMenu(menu);
                SuccessNotification(_localizationService.GetResource("Admin.MainMenu.Deleted"));
                return RedirectToAction("List");
            }
            ErrorNotification(ModelState);
            return RedirectToAction("Edit", new { id = id });
        }



        public List<SelectListItem> GetSubmenu(string mainmenu) {
           return SubMenuHelper.GetBreedType(mainmenu);
        
        }
    }
}
