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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class SubSubMenuController : BaseAdminController
    {
        private readonly ISubSubMenuService _subSubMenuService;
        private readonly ISubMenuService _subMenuService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;
        private readonly SeoSettings _seoSettings;

        public SubSubMenuController(
            ISubSubMenuService subSubMenuService,
            ISubMenuService subMenuService,
            ILanguageService languageService,
            ILocalizationService localizationService,
            IStoreService storeService,
            IWorkContext workContext,
            SeoSettings seoSettings
            )
        {
            _subSubMenuService = subSubMenuService;
            _subMenuService = subMenuService;
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
            var subMenu = await _subSubMenuService.GetSubSubMenuByUser(command.Page - 1, command.PageSize);

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
            ViewBag.SubMenuId =await  GetSubMenu();
            ViewBag.MainMenu = MainMenuLink.GetBreedType();
            return View();
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(SubSubMenuModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var subsubMenu = model.ToEntity();
                subsubMenu.SubMenu = await _subMenuService.GetSubMenuById(model.SubMenuId);
                subsubMenu.UserId = _workContext.CurrentCustomer.Id;
                if (model.IsUrlExternal)
                {
                    subsubMenu.Url = model.ExternalUrl;
                }
                await _subSubMenuService.InsertSubSubMenu(subsubMenu);
               
                    subsubMenu.Url = "/NewsEvent/Index?subSubMenu=" + subsubMenu.Id;
                    await _subSubMenuService.UpdateSubSubMenu(subsubMenu);
                
                SuccessNotification(_localizationService.GetResource("Admin.SubSubMenu.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = subsubMenu.Id }) : RedirectToAction("List");
            }

            ViewBag.SubMenuId =await GetSubMenu();
            ViewBag.MainMenu = MainMenuLink.GetBreedType();
            //If we got this far, something failed, redisplay form
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            return View(model);
        }


        [PermissionAuthorizeAction(PermissionActionName.Preview)]
        public async Task<IActionResult> Edit(string id)
        {
            var subMenu = await _subSubMenuService.GetSubSubMenuById(id);
            if (subMenu == null)
                //No blog post found with the specified id
                return RedirectToAction("List");
            var model = subMenu.ToModel();
            if (subMenu.IsUrlExternal)
            {
                model.ExternalUrl = subMenu.Url;
            }
            ViewBag.SubMenuId =await  GetSubMenu();
            ViewBag.MainMenu = MainMenuLink.GetBreedType();
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);

            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(SubSubMenuModel model, bool continueEditing)
        {
            var subMenu = await _subSubMenuService.GetSubSubMenuById(model.Id);
            if (subMenu == null)

                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var m = model.ToEntity(subMenu);
                //m.Url = "/NewsEvent/Index?subSubMenu=" + m.Id;
                if (model.IsUrlExternal)
                {
                    m.Url = model.ExternalUrl;
                }                
                m.SubMenu = await _subMenuService.GetSubMenuById(model.SubMenuId);

                await _subSubMenuService.UpdateSubSubMenu(m);

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
            ViewBag.SubMenuId =await GetSubMenu();
            ViewBag.MainMenu = MainMenuLink.GetBreedType();
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Delete)]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var subMenu = await _subSubMenuService.GetSubSubMenuById(id);
            if (subMenu == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                await _subSubMenuService.DeleteSubSubMenu(subMenu);
                SuccessNotification(_localizationService.GetResource("Admin.SubSubMenu.Deleted"));
                return RedirectToAction("List");
            }
            ErrorNotification(ModelState);
            return RedirectToAction("Edit", new { id = id });
        }

        public async Task<SelectList> GetSubMenu()
        {
            var subMenu = await _subMenuService.GetSubMenuByUser();
            var items = subMenu.Select(m => new SelectListItem {
                Text = m.Name + "(" + m.MainMenu.MainMenuName + ")",
                Value = m.Id.ToString()
            }).ToList();
            items.Insert(0, new SelectListItem { Text = _localizationService.GetResource("Admin.Common.Select"), Value = "" });
            return new SelectList(items,"Value","Text");

        }
    }
}
