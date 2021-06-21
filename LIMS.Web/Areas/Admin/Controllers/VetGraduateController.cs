using LIMS.Core;
using LIMS.Domain.Seo;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Mvc.Filters;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Localization;
using LIMS.Services.Professionals;
using LIMS.Services.Security;
using LIMS.Services.Stores;
using LIMS.Web.Areas.Admin.Models.Professionals;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using LIMS.Domain.Professionals;
using LIMS.Web.Areas.Admin.Extensions.Mapping;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class VetGraduateController : BaseAdminController
    {
        private readonly IVetGraduateService _vetGraduateService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;
        private readonly SeoSettings _seoSettings;

        public VetGraduateController(
            IVetGraduateService vetGraduateService,
            ILanguageService languageService,
            ILocalizationService localizationService,
            IStoreService storeService,
            IWorkContext workContext,
            SeoSettings seoSettings
            )
        {
            _vetGraduateService = vetGraduateService;
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
            var vetGraduate = await _vetGraduateService.GetVetGraduate(command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult {
                Data = vetGraduate,
                Total = vetGraduate.TotalCount
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
        public async Task<IActionResult> Create(VetGraduateModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var VetGraduate = model.ToEntity();
                await _vetGraduateService.InsertVetGraduate(VetGraduate);
                SuccessNotification(_localizationService.GetResource("Admin.VetGraduate.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = VetGraduate.Id }) : RedirectToAction("List");
            }

            //If we got this far, something failed, redisplay form
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Preview)]
        public async Task<IActionResult> Edit(string id)
        {
            var vetGraduate = await _vetGraduateService.GetVetGraduateById(id);
            if (vetGraduate == null)
                //No blog post found with the specified id
                return RedirectToAction("List");
            var model = vetGraduate.ToModel();

            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(VetGraduateModel model, bool continueEditing)
        {
            var vetGraduate = await _vetGraduateService.GetVetGraduateById(model.Id);
            if (vetGraduate == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var m = model.ToEntity();
                await _vetGraduateService.UpdateVetGraduate(m);
                SuccessNotification(_localizationService.GetResource("Admin.VetGraduate.Updated"));

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
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Delete)]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var vetGraduate = await _vetGraduateService.GetVetGraduateById(id);
            if (vetGraduate == null)
                //No blog post found with the specified id
                return RedirectToAction("List");


            if (ModelState.IsValid)
            {
                await _vetGraduateService.DeleteVetGraduate(vetGraduate);
                SuccessNotification(_localizationService.GetResource("Admin.VetGraduate.Deleted"));
                return RedirectToAction("List");
            }
            ErrorNotification(ModelState);
            return RedirectToAction("Edit", new { id = id });
        }
    }
}
