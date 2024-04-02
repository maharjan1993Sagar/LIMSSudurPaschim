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
using LIMS.Web.Areas.Admin.Models.BasicSetup;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class FiscalYearController : BaseAdminController
    {
        private readonly IFiscalYearService _fiscalYearService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;
        private readonly SeoSettings _seoSettings;
        private readonly IFiscalYearForGraphService _fiscalYearForGraphService;

        public FiscalYearController(
            IFiscalYearService fiscalYearService,
            ILanguageService languageService,
            ILocalizationService localizationService,
            IStoreService storeService,
            IWorkContext workContext,
            SeoSettings seoSettings,
            IFiscalYearForGraphService fiscalYearForGraphService
            )
        {
            _fiscalYearService = fiscalYearService;
            _languageService = languageService;
            _localizationService = localizationService;
            _storeService = storeService;
            _workContext = workContext;
            _seoSettings = seoSettings;
            _fiscalYearForGraphService = fiscalYearForGraphService;
        }

        public IActionResult Index() => RedirectToAction("List");

        public IActionResult List() => View();

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command)
        {
            var fiscalYear = await _fiscalYearService.GetFiscalYear(command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult {
                Data = fiscalYear,
                Total = fiscalYear.TotalCount
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
        public async Task<IActionResult> Create(FiscalyearModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var fiscalYear = model.ToEntity();
                if(model.CurrentFiscalYear==true)
                {
                   
                    var fiscalYears = await _fiscalYearService.GetCurrentFiscalYear();
                    if (fiscalYears != null)
                    {
                        fiscalYears.CurrentFiscalYear = false;
                        await _fiscalYearService.UpdateFiscalYear(fiscalYears);
                    }

                }
                await _fiscalYearService.InsertFiscalYear(fiscalYear);
                SuccessNotification(_localizationService.GetResource("Admin.FiscalYear.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = fiscalYear.Id }) : RedirectToAction("List");
            }

            //If we got this far, something failed, redisplay form
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            return View(model);
        }


        [PermissionAuthorizeAction(PermissionActionName.Preview)]
        public async Task<IActionResult> Edit(string id)
        {
            var fiscalYear = await _fiscalYearService.GetFiscalYearById(id);
            if (fiscalYear == null)
                return RedirectToAction("List");
            var model = fiscalYear.ToModel();

            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);

            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(FiscalyearModel model, bool continueEditing)
        {
            var fiscalYear = await _fiscalYearService.GetFiscalYearById(model.Id);
            if (fiscalYear == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var m = model.ToEntity(fiscalYear);
                if (model.CurrentFiscalYear == true)
                {
                    var fiscalYears = await _fiscalYearService.GetCurrentFiscalYear();
                    if (fiscalYears != null)
                    {
                        fiscalYears.CurrentFiscalYear = false;
                        await _fiscalYearService.UpdateFiscalYear(fiscalYears);
                    }

                }
                await _fiscalYearService.UpdateFiscalYear(m);

                SuccessNotification(_localizationService.GetResource("Admin.FiscalYear.Updated"));
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
            var fiscalYear = await _fiscalYearService.GetFiscalYearById(id);
            if (fiscalYear == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                await _fiscalYearService.DeleteFiscalYear(fiscalYear);

                SuccessNotification(_localizationService.GetResource("Admin.FiscalYear.Deleted"));
                return RedirectToAction("List");
            }
            ErrorNotification(ModelState);
            return RedirectToAction("Edit", new { id = id });
        }

        public async Task<IActionResult> FiscalYearList()
        {
            var fiscalYears = await _fiscalYearService.GetFiscalYear();
            return Json(fiscalYears);
        }

        public async Task<IActionResult> FiscalYearGraph() {
            ViewBag.FiscalYearId = await _fiscalYearService.GetFiscalYear();
            var q = await _fiscalYearForGraphService.GetFiscalYear();
            if (q != null)
            {
                var r =q.ToModel();
                return View(r);
            }
            else
            {
                var fiscalYearForGraphModel = new FiscalYearForGraphModel();
                return View(fiscalYearForGraphModel);
            }
        
        }
        [HttpPost]
        public async Task<IActionResult> FiscalYearGraph(FiscalYearForGraphModel model,IFormCollection form)
        {
            var fiscalyearforgraph = await _fiscalYearForGraphService.GetFiscalYearById(model.Id);
            if(fiscalyearforgraph!=null)
            {
                var a = model.ToEntity();
                var fiscalyear = form["fiscalyear"].ToList();
                a.FiscalYear = fiscalyear;
                await _fiscalYearForGraphService.UpdateFiscalYear(a);
            }
            else
            {
                var a = model.ToEntity();
                var fiscalyear = form["fiscalyear"].ToList();
                a.FiscalYear = fiscalyear;
                await _fiscalYearForGraphService.InsertFiscalYear(a);
            }
            ViewBag.FiscalYearId = await _fiscalYearService.GetFiscalYear();
            var q =await _fiscalYearForGraphService.GetFiscalYear();
            var r = q.ToModel();
            return View(r);

        }
    }

}

