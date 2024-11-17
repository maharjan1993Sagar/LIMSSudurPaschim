using LIMS.Core;
using LIMS.Domain.Seo;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Mvc.Filters;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Basic;
using LIMS.Services.Localization;
using LIMS.Services.Security;
using LIMS.Services.Stores;
using LIMS.Web.Areas.Admin.Extensions.Mapping;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Web.Areas.Admin.Models.BasicSetup;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class CategoryController : BaseAdminController
    {
        private readonly ICategoryService _CategoryService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;
        private readonly SeoSettings _seoSettings;

        public object CagtegoryHelper { get; private set; }

        public CategoryController(
            ICategoryService CategoryService,
            ILanguageService languageService,
            ILocalizationService localizationService,
            IStoreService storeService,
            IWorkContext workContext,
            SeoSettings seoSettings
            )
        {
            _CategoryService = CategoryService;
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
            var Category = await _CategoryService.GetCategory(command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult {
                Data = Category,
                Total = Category.TotalCount
            };
            return Json(gridModel);
        }

        [PermissionAuthorizeAction(PermissionActionName.Create)]
        public async Task<IActionResult> Create()
        {
            ViewBag.Types = CategoryTypeHelper.GetType();
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            return View();
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(CategoryModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var Category = model.ToEntity();
                await _CategoryService.InsertCategory(Category);
                SuccessNotification(_localizationService.GetResource("Admin.Category.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = Category.Id }) : RedirectToAction("List");
            }
            ViewBag.Types = CategoryTypeHelper.GetType();

            //If we got this far, something failed, redisplay form
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            return View(model);
        }


        [PermissionAuthorizeAction(PermissionActionName.Preview)]
        public async Task<IActionResult> Edit(string id)
        {
            ViewBag.Types = CategoryTypeHelper.GetType();

            var Category = await _CategoryService.GetCategoryById(id);
            if (Category == null)
                return RedirectToAction("List");
            var model = Category.ToModel();

            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);

            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(CategoryModel model, bool continueEditing)
        {
            var Category = await _CategoryService.GetCategoryById(model.Id);
            if (Category == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var m = model.ToEntity();
                await _CategoryService.UpdateCategory(m);

                SuccessNotification(_localizationService.GetResource("Admin.Category.Updated"));
                if (continueEditing)
                {
                    //selected tab
                    await SaveSelectedTabIndex();

                    return RedirectToAction("Edit", new { id = model.Id });
                }
                return RedirectToAction("List");
            }
            ViewBag.Types = CategoryTypeHelper.GetType();

            //If we got this far, something failed, redisplay form
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);

            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Delete)]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var Category = await _CategoryService.GetCategoryById(id);
            if (Category == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                await _CategoryService.DeleteCategory(Category);

                SuccessNotification(_localizationService.GetResource("Admin.Category.Deleted"));
                return RedirectToAction("List");
            }
            ErrorNotification(ModelState);
            return RedirectToAction("Edit", new { id = id });
        }

        public async Task<IActionResult> CategoryList()
        {
            var Categorys = await _CategoryService.GetCategory();
            return Json(Categorys);
        }
    }
}

