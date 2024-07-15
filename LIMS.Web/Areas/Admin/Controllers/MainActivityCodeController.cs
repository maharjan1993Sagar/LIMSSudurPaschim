using LIMS.Core;
using LIMS.Domain.Bali;
using LIMS.Domain.Seo;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Mvc.Filters;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Bali;
using LIMS.Services.Basic;
using LIMS.Services.Localization;
using LIMS.Services.Security;
using LIMS.Services.Stores;
using LIMS.Web.Areas.Admin.Extensions.Mapping;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Web.Areas.Admin.Models.Bali;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class MainActivityCodeController : BaseAdminController
    {
        private readonly IMainActivityCodeService _MainActivityCodeService;
        private readonly IPujigatKharchaKharakramService _pujigatService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;
        private readonly SeoSettings _seoSettings;

        public MainActivityCodeController(
            IMainActivityCodeService MainActivityCodeService,
            IPujigatKharchaKharakramService pujigatService,
            ILanguageService languageService,
            ILocalizationService localizationService,
            IStoreService storeService,
            IWorkContext workContext,
            SeoSettings seoSettings
            )
        {
            _MainActivityCodeService = MainActivityCodeService;
            _pujigatService = pujigatService;
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
            var MainActivityCode = await _MainActivityCodeService.GetMainActivityCode(command.Page - 1, command.PageSize);
            
            var gridModel = new DataSourceResult {
                Data = MainActivityCode,
                Total = MainActivityCode.TotalCount
            };
            return Json(gridModel);
        }
        public async Task<IActionResult> AddCode()
        {
            var pujigats = await _pujigatService.GetLimbis_Code();
            var mainActivityCodes = await _MainActivityCodeService.GetMainActivityCode();

            var codes = mainActivityCodes.Select(m => m.Limbis_Code).ToList();

            var RemainCodes = pujigats.Except(codes).ToList();

            return View(RemainCodes);

        }

        [HttpPost]
        public async Task<IActionResult> AddCode(IFormCollection col)
        {
            var codes = col["LimbsCode"].ToList();

            foreach (var item in codes)
            {
                var isExists = await _MainActivityCodeService.IsExistsCode(item.Trim());
                if (!isExists)
                {
                    var mainActivityCode = new MainActivityCode() {
                        Limbis_Code = item.Trim()
                    };
                    await _MainActivityCodeService.InsertMainActivityCode(mainActivityCode);
                   
                }
            }

            return RedirectToAction("Index");           
        
        }


        [PermissionAuthorizeAction(PermissionActionName.Create)]
        public async Task<IActionResult> Create()
        {
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            return View();
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(MainActivityCodeModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var MainActivityCode = model.ToEntity();
                await _MainActivityCodeService.InsertMainActivityCode(MainActivityCode);
                SuccessNotification(_localizationService.GetResource("Admin.MainActivityCode.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = MainActivityCode.Id }) : RedirectToAction("List");
            }
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            return View(model);
        }


        [PermissionAuthorizeAction(PermissionActionName.Preview)]
        public async Task<IActionResult> Edit(string id)
        {
            var MainActivityCode = await _MainActivityCodeService.GetMainActivityCodeById(id);
            if (MainActivityCode == null)
                //No blog post found with the specified id
                return RedirectToAction("List");
            var model = MainActivityCode.ToModel();
           ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
           return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(MainActivityCodeModel model, bool continueEditing)
        {
            var MainActivityCode = await _MainActivityCodeService.GetMainActivityCodeById(model.Id);
            if (MainActivityCode == null)
               
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var m = model.ToEntity(MainActivityCode);
                await _MainActivityCodeService.UpdateMainActivityCode(m);

                SuccessNotification(_localizationService.GetResource("Admin.MainActivityCode.Updated"));
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
            var MainActivityCode = await _MainActivityCodeService.GetMainActivityCodeById(id);
            if (MainActivityCode == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                await _MainActivityCodeService.DeleteMainActivityCode(MainActivityCode);
                SuccessNotification(_localizationService.GetResource("Admin.MainActivityCode.Deleted"));
                return RedirectToAction("List");
            }
            ErrorNotification(ModelState);
            return RedirectToAction("Edit", new { id = id });
        }
       
    }
}
