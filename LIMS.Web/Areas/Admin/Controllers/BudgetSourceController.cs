using LIMS.Core;
using LIMS.Domain.Seo;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Mvc.Filters;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Bali;
using LIMS.Services.Localization;
using LIMS.Services.Security;
using LIMS.Services.Stores;
using LIMS.Web.Areas.Admin.Extensions.Mapping;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Web.Areas.Admin.Models.Bali;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class BudgetSourceController : BaseAdminController
    {
        private readonly IBudgetSourceService _BudgetSourceService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;
        private readonly SeoSettings _seoSettings;

        public BudgetSourceController(
            IBudgetSourceService BudgetSourceService,
            ILanguageService languageService,
            ILocalizationService localizationService,
            IStoreService storeService,
            IWorkContext workContext,
            SeoSettings seoSettings
            )
        {
            _BudgetSourceService = BudgetSourceService;
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
            var BudgetSource = await _BudgetSourceService.GetBudgetSource(command.Page - 1, command.PageSize);

            var gridModel = new DataSourceResult {
                Data = BudgetSource,
                Total = BudgetSource.TotalCount
            };
            return Json(gridModel);
        }

        [PermissionAuthorizeAction(PermissionActionName.Create)]
        public async Task<IActionResult> Create()
        {
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            return View();
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(BudgetSourceModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var BudgetSource = model.ToEntity();
                await _BudgetSourceService.InsertBudgetSource(BudgetSource);
                SuccessNotification(_localizationService.GetResource("Admin.BudgetSource.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = BudgetSource.Id }) : RedirectToAction("List");
            }
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            return View(model);
        }


        [PermissionAuthorizeAction(PermissionActionName.Preview)]
        public async Task<IActionResult> Edit(string id)
        {
            var BudgetSource = await _BudgetSourceService.GetBudgetSourceById(id);
            if (BudgetSource == null)
                //No blog post found with the specified id
                return RedirectToAction("List");
            var model = BudgetSource.ToModel();
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(BudgetSourceModel model, bool continueEditing)
        {
            var BudgetSource = await _BudgetSourceService.GetBudgetSourceById(model.Id);
            if (BudgetSource == null)

                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var m = model.ToEntity(BudgetSource);
                await _BudgetSourceService.UpdateBudgetSource(m);

                SuccessNotification(_localizationService.GetResource("Admin.BudgetSource.Updated"));
                if (continueEditing)
                {
                    //selected tab
                    await SaveSelectedTabIndex();
                    return RedirectToAction("Edit", new { id = model.Id });
                }
                return RedirectToAction("List");
            }
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Delete)]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var BudgetSource = await _BudgetSourceService.GetBudgetSourceById(id);
            if (BudgetSource == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                await _BudgetSourceService.DeleteBudgetSource(BudgetSource);
                SuccessNotification(_localizationService.GetResource("Admin.BudgetSource.Deleted"));
                return RedirectToAction("List");
            }
            ErrorNotification(ModelState);
            return RedirectToAction("Edit", new { id = id });
        }

    }
}
