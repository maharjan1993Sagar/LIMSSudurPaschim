using LIMS.Core;
using LIMS.Domain.Activities;
using LIMS.Domain.Seo;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Mvc.Filters;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Activities;
using LIMS.Services.Basic;
using LIMS.Services.Localization;
using LIMS.Services.Security;
using LIMS.Services.Stores;
using LIMS.Web.Areas.Admin.Extensions.Mapping;
using LIMS.Web.Areas.Admin.Models.Activities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class ActivityController:BaseAdminController
    {
        
        private readonly IActivityService _activityService;
        public readonly IFiscalYearService _fiscalYearService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;
        private readonly SeoSettings _seoSettings;

        public ActivityController(
            IActivityService ActivityService,
            ILanguageService languageService,
            ILocalizationService localizationService,
            IStoreService storeService,
            IWorkContext workContext,
            SeoSettings seoSettings,
            IFiscalYearService fiscalYearService
           
            )
        {
            _activityService = ActivityService;
          
            _languageService = languageService;
            _localizationService = localizationService;
            _storeService = storeService;
            _workContext = workContext;
            _seoSettings = seoSettings;
            _fiscalYearService = fiscalYearService;
        }

        public IActionResult Index() => RedirectToAction("List");

        public IActionResult List() => View();

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command)
        {
            string createdby = _workContext.CurrentCustomer.Id;
            var activity = await _activityService.GetActivity(createdby,command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult {
                Data = activity,
                Total = activity.TotalCount
            };
            return Json(gridModel);
        }
        [PermissionAuthorizeAction(PermissionActionName.Create)]
        public async Task<IActionResult> Create()
        {
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            var fiscalyear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear").ToList();
            fiscalyear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
           
            
            ViewBag.FiscalYearId = fiscalyear;

            return View();
        }
        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(ActivityModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {

                var activity = model.ToEntity();
                activity.CreatedBy = _workContext.CurrentCustomer.Id;
                activity.FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYearId);
                await _activityService.InsertActivity(activity);
                SuccessNotification(_localizationService.GetResource("Admin.Activity.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = activity.Id }) : RedirectToAction("List");
            }
            var fiscalyear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear").ToList();
            fiscalyear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));


            ViewBag.FiscalYearId = fiscalyear;
            //If we got this far, something failed, redisplay form
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            return View(model);
        }


        [PermissionAuthorizeAction(PermissionActionName.Preview)]
        public async Task<IActionResult> Edit(string id)
        {
            var activity = await _activityService.GetActivityById(id);
            if (activity == null)
                return RedirectToAction("List");
            ActivityModel model = activity.ToModel();
            var fiscalyear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear").ToList();
            fiscalyear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));


            ViewBag.FiscalYearId = fiscalyear;
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);

            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(ActivityModel model, bool continueEditing)
        {
            var activity = await _activityService.GetActivityById(model.Id);
            if (activity == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                Activity m = model.ToEntity(activity);
                activity.FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYearId);
                await _activityService.UpdateActivity(m);
             
                SuccessNotification(_localizationService.GetResource("Admin.Activity.Updated"));
                if (continueEditing)
                {
                    //selected tab
                    await SaveSelectedTabIndex();

                    return RedirectToAction("Edit", new { id = model.Id });
                }
                return RedirectToAction("List");
            }
            var fiscalyear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear").ToList();
            fiscalyear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));


            ViewBag.FiscalYearId = fiscalyear;

            //If we got this far, something failed, redisplay form
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);

            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Delete)]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var Activity = await _activityService.GetActivityById(id);
            if (Activity == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                await _activityService.DeleteActivity(Activity);

                SuccessNotification(_localizationService.GetResource("Admin.Activity.Deleted"));
                return RedirectToAction("List");
            }
            ErrorNotification(ModelState);
            return RedirectToAction("Edit", new { id = id });
        }

    }
}
