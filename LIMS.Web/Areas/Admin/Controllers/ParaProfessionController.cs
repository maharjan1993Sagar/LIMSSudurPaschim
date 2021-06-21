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
    public class ParaProfessionController : BaseAdminController
    {
        private readonly IParaProfessionalService _paraProfessionalsService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;
        private readonly SeoSettings _seoSettings;

        public ParaProfessionController(
            IParaProfessionalService paraProfessionalsService,
            ILanguageService languageService,
            ILocalizationService localizationService,
            IStoreService storeService,
            IWorkContext workContext,
            SeoSettings seoSettings
            )
        {
            _paraProfessionalsService = paraProfessionalsService;
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
            var paraProfessionals = await _paraProfessionalsService.GetParaProfessionals(command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult {
                Data = paraProfessionals,
                Total = paraProfessionals.TotalCount
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
        public async Task<IActionResult> Create(ParaProfessionalsModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var ParaProfessionals = model.ToEntity();
                await _paraProfessionalsService.InsertParaProfessionals(ParaProfessionals);
                SuccessNotification(_localizationService.GetResource("Admin.ParaProfessional.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = ParaProfessionals.Id }) : RedirectToAction("List");
            }

            //If we got this far, something failed, redisplay form
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            return View(model);
        }


        [PermissionAuthorizeAction(PermissionActionName.Preview)]
        public async Task<IActionResult> Edit(string id)
        {
            var paraProfessionals = await _paraProfessionalsService.GetParaProfessionalsById(id);
            if (paraProfessionals == null)
                //No blog post found with the specified id
                return RedirectToAction("List");
            var model = paraProfessionals.ToModel();

            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(ParaProfessionalsModel model, bool continueEditing)
        {
            var paraProfessionals = await _paraProfessionalsService.GetParaProfessionalsById(model.Id);
            if (paraProfessionals == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                ParaProfessionals m = model.ToEntity();
                await _paraProfessionalsService.UpdateParaProfessionals(m);
                SuccessNotification(_localizationService.GetResource("Admin.ParaProfessional.Updated"));
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
            var paraProfessionals = await _paraProfessionalsService.GetParaProfessionalsById(id);
            if (paraProfessionals == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                await _paraProfessionalsService.DeleteParaProfessionals(paraProfessionals);
                SuccessNotification(_localizationService.GetResource("Admin.ParaProfessional.Deleted"));
                return RedirectToAction("List");
            }
            ErrorNotification(ModelState);
            return RedirectToAction("Edit", new { id = id });
        }

    }
}
