using LIMS.Core;
using LIMS.Domain.Seo;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Mvc.Filters;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Breed;
using LIMS.Services.GeneralCMS;
using LIMS.Services.Localization;
using LIMS.Services.Security;
using LIMS.Services.Stores;
using LIMS.Web.Areas.Admin.Extensions.Mapping;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Web.Areas.Admin.Models.Breed;
using LIMS.Web.Areas.Admin.Models.GeneralCMS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class ImportantLinksController : BaseAdminController
    {
        private readonly IImportantLinksService _importantLinkService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;
        private readonly SeoSettings _seoSettings;

        public ImportantLinksController(
            IImportantLinksService importantLiksService,
            ILanguageService languageService,
            ILocalizationService localizationService,
            IStoreService storeService,
            IWorkContext workContext,
            SeoSettings seoSettings
            )
        {
            _importantLinkService = importantLiksService;
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
            var links = await _importantLinkService.GetImportantLinksByUser(command.Page - 1, command.PageSize);
            
            var gridModel = new DataSourceResult {
                Data = links,
                Total = links.TotalCount
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
        public async Task<IActionResult> Create(ImportantLinksModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var link = model.ToEntity();
                link.UserId = _workContext.CurrentCustomer.Id;
                await _importantLinkService.InsertImportantLinks(link);
                SuccessNotification(_localizationService.GetResource("Admin.ImportantLinks.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = link.Id }) : RedirectToAction("List");
            }
           return View(model);
        }


        [PermissionAuthorizeAction(PermissionActionName.Preview)]
        public async Task<IActionResult> Edit(string id)
        {
            var link = await _importantLinkService.GetImportantLinksById(id);
            if (link == null)
                //No blog post found with the specified id
                return RedirectToAction("List");
            var model = link.ToModel();
          return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(ImportantLinksModel model, bool continueEditing)
        {
            var link = await _importantLinkService.GetImportantLinksById(model.Id);
            if (link == null)
               
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var m = model.ToEntity(link);
                await _importantLinkService.UpdateImportantLinks(m);

                SuccessNotification(_localizationService.GetResource("Admin.ImportantLinks.Updated"));
                if (continueEditing)
                {
                    //selected tab
                    await SaveSelectedTabIndex();
                    return RedirectToAction("Edit", new { id = model.Id });
                }
                return RedirectToAction("List");
            }
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Delete)]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var link = await _importantLinkService.GetImportantLinksById(id);
            if (link == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                await _importantLinkService.DeleteImportantLinks(link);
                SuccessNotification(_localizationService.GetResource("Admin.ImportantLinks.Deleted"));
                return RedirectToAction("List");
            }
            ErrorNotification(ModelState);
            return RedirectToAction("Edit", new { id = id });
        }
      
    }
}
