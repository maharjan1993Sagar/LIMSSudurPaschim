using LIMS.Core;
using LIMS.Domain.Seo;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Mvc.Filters;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Localization;
using LIMS.Services.MoAMAC;
using LIMS.Services.Security;
using LIMS.Services.Stores;
using LIMS.Web.Areas.Admin.Extensions.Mapping;
using LIMS.Web.Areas.Admin.Models.MoAMAC;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class MoAMACController : BaseAdminController
    {
        private readonly IMoAMACService _molmacService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;
        private readonly SeoSettings _seoSettings;

        public MoAMACController(
            IMoAMACService molmacService,
            ILanguageService languageService,
            ILocalizationService localizationService,
            IStoreService storeService,
            IWorkContext workContext,
            SeoSettings seoSettings
            )
        {
            _molmacService = molmacService;
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
            var molmac = await _molmacService.GetMoAMAC(command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult {
                Data = molmac,
                Total = molmac.TotalCount
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
        public async Task<IActionResult> Create(MoAMACModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {                
                var molmac = model.ToEntity();
                await _molmacService.InsertMoAMAC(molmac);
                SuccessNotification(_localizationService.GetResource("Admin.Molmac.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = molmac.Id }) : RedirectToAction("List");
            }

            //If we got this far, something failed, redisplay form
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            return View(model);
        }


        [PermissionAuthorizeAction(PermissionActionName.Preview)]
        public async Task<IActionResult> Edit(string id)
        {
            var molmac = await _molmacService.GetMoAMACById(id);
            if (molmac == null)
                return RedirectToAction("List");
            var model = molmac.ToModel();

            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
         
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(MoAMACModel model, bool continueEditing)
        {
            var molmac = await _molmacService.GetMoAMACById(model.Id);
            if (molmac == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var m = model.ToEntity();
                await _molmacService.UpdateMoAMAC(m);

                SuccessNotification(_localizationService.GetResource("Admin.Molmac.Updated"));
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
            var molmac = await _molmacService.GetMoAMACById(id);
            if (molmac == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                await _molmacService.DeleteMoAMAC(molmac);

                SuccessNotification(_localizationService.GetResource("Admin.Molmac.Deleted"));
                return RedirectToAction("List");
            }
            ErrorNotification(ModelState);
            return RedirectToAction("Edit", new { id = id });
        }
    }
}
