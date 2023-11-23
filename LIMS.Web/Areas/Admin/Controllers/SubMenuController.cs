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
using LIMS.Web.Areas.Admin.Models.Breed;
using LIMS.Web.Areas.Admin.Models.DynamicMenu;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class SubMenuController : BaseAdminController
    {
        private readonly ISubMenuService _subMenuService;
        private readonly IMainMenuService _mainMenuService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;
        private readonly SeoSettings _seoSettings;

        public SubMenuController(
            ISubMenuService subMenuService,
            IMainMenuService mainMenuService,
            ILanguageService languageService,
            ILocalizationService localizationService,
            IStoreService storeService,
            IWorkContext workContext,
            SeoSettings seoSettings
            )
        {
            _subMenuService = subMenuService;
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
            var subMenu = await _subMenuService.GetSubMenuByUser(command.Page - 1, command.PageSize);
            
            var gridModel = new DataSourceResult {
                Data = subMenu,
                Total = subMenu.TotalCount
            };
            return Json(gridModel);
        }

        [PermissionAuthorizeAction(PermissionActionName.Create)]
        public async Task<IActionResult> Create()
        {
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            var mainMenus = new SelectList(await _mainMenuService.GetMainMenuByUser(), "Id", "MainMenuName").ToList();
            mainMenus.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.MainMenuId = mainMenus;
            SubMenuModel model=new SubMenuModel();
            ViewBag.URL = MainMenuLink.GetBreedType();
            return View();
        }   

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(SubMenuModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            { 
                var subMenu = model.ToEntity();
                subMenu.UserId = _workContext.CurrentCustomer.Id;
                subMenu.MainMenu = await _mainMenuService.GetMainMenuById(model.MainMenuId);
                await _subMenuService.InsertSubMenu(subMenu);
                if (subMenu.HasSubSubMenu)
                {
                    subMenu.Url = "";
                }
                else
                {
                    if (subMenu.Name.ToLower().Contains("employee") || (subMenu.Name.ToLower().Contains("staff")))
                        {
                        subMenu.Url = "/Employee/Index";
                    }
                    else
                    {
                        if(model.IsUrlExternal)
                        {
                            subMenu.Url = model.ExternalUrl;
                        }
                        //else

                       // subMenu.Url = "/NewsEvent/Index?subMenu=" + subMenu.Id;
                    }
                    await _subMenuService.UpdateSubMenu(subMenu);
                }
                SuccessNotification(_localizationService.GetResource("Admin.SubMenu.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = subMenu.Id }) : RedirectToAction("List");
            }

            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            var mainMenus = new SelectList(await _mainMenuService.GetMainMenuByUser(), "Id", "MainMenuName").ToList();
            mainMenus.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.MainMenuId = mainMenus;
            ViewBag.URL = MainMenuLink.GetBreedType();
            //If we got this far, something failed, redisplay form
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            return View(model);
        }


        [PermissionAuthorizeAction(PermissionActionName.Preview)]
        public async Task<IActionResult> Edit(string id)
        {
            var subMenu = await _subMenuService.GetSubMenuById(id);
            if (subMenu == null)
                //No blog post found with the specified id
                return RedirectToAction("List");
            var model = subMenu.ToModel();
            if (subMenu.IsUrlExternal)
            {
                model.ExternalUrl = subMenu.Url;
            }
            var mainMenus = new SelectList(await _mainMenuService.GetMainMenuByUser(), "Id", "MainMenuName").ToList();
            mainMenus.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.MainMenuId = mainMenus;
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            ViewBag.URL = MainMenuLink.GetBreedType();
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(SubMenuModel model, bool continueEditing)
        {
            var subMenu = await _subMenuService.GetSubMenuById(model.Id);
            if (subMenu == null)
               
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var m = model.ToEntity(subMenu);
                m.MainMenu = await _mainMenuService.GetMainMenuById(model.MainMenuId);
                if (m.HasSubSubMenu)
                {
                    m.Url = "";
                }
                else
                {
                    if (m.Name.ToLower().Contains("employee")|| (m.Name.ToLower().Contains("staff"))|| (m.Name.ToLower().Contains("organ"))  )
                        {
                        m.Url = "/Employee/Index";
                    }
                    else
                    {
                        if (model.IsUrlExternal)
                        {
                            m.Url = model.ExternalUrl;
                        }
                        //m.Url = "/NewsEvent/Index?subMenu=" + subMenu.Id;
                    }
                    
                }
                await _subMenuService.UpdateSubMenu(m);

                SuccessNotification(_localizationService.GetResource("Admin.SubMenu.Updated"));
                if (continueEditing)
                {
                    //selected tab
                    await SaveSelectedTabIndex();
                    return RedirectToAction("Edit", new { id = model.Id });
                }
                return RedirectToAction("List");
            }
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            var mainMenus = new SelectList(await _mainMenuService.GetMainMenuByUser(), "Id", "MainMenuName").ToList();
            mainMenus.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.MainMenuId = mainMenus;
            ViewBag.URL = MainMenuLink.GetBreedType();
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Delete)]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var subMenu = await _subMenuService.GetSubMenuById(id);
            if (subMenu == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                await _subMenuService.DeleteSubMenu(subMenu);
                SuccessNotification(_localizationService.GetResource("Admin.SubMenu.Deleted"));
                return RedirectToAction("List");
            }
            ErrorNotification(ModelState);
            return RedirectToAction("Edit", new { id = id });
        }
       
    }
}
