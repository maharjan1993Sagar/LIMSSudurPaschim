using LIMS.Core;
using LIMS.Domain.Activities;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Activities;
using LIMS.Services.Basic;
using LIMS.Services.Customers;
using LIMS.Services.Localization;
using LIMS.Services.MoAMAC;
using LIMS.Services.Security;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Web.Areas.Admin.Models.Activities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class ActivityProgressController:BaseAdminController
    {
        #region fields
        private readonly ILocalizationService _localizationService;
        private readonly IUnitService _unitService;
        public readonly IFiscalYearService _fiscalYearService;
        public readonly IWorkContext _workContext;
        public readonly IActivityProgressService _activityProgressService;
        public readonly ILssService _lssService;
        public readonly ICustomerService _customerService;
        public readonly IActivityService _activityService;
        #endregion fields
        #region ctor
        public ActivityProgressController(ILocalizationService localizationService,

             IUnitService unitService,
              IFiscalYearService fiscalYearService,

              IWorkContext workContext,

              ILssService lssService,
              ICustomerService customerService,
              IActivityProgressService activityProgressService,
              IActivityService activityService
             )
        {
            _localizationService = localizationService;

            _unitService = unitService;
            _fiscalYearService = fiscalYearService;
            _workContext = workContext;
            _activityProgressService = activityProgressService;
            _lssService = lssService;
            _customerService = customerService;
            _activityService = activityService;

        }
        #endregion ctor
        #region ActivityProgress
        public IActionResult Index() => RedirectToAction("List");

        public IActionResult List() => View();

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command, string keyword = "")
        {
            string user = _workContext.CurrentCustomer.Id;
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
            string createdby = null;
            if (roles.Contains(RoleHelper.LssAdmin) || roles.Contains(RoleHelper.VhlsecAdmin) || roles.Contains(RoleHelper.DolfdAdmin))
            {
                createdby = _workContext.CurrentCustomer.Id;
            }
            else
            {
                string adminemail = _workContext.CurrentCustomer.CreatedBy;
                var admin = await _customerService.GetCustomerByEmail(adminemail);
                createdby = admin.Id;
            }
            var activityProgress = await _activityProgressService.GetActivityProgress(createdby,command.Page - 1, command.PageSize);

            var activityProgressListModel = new List<ActivityProgressListModel>();
            foreach (var item in activityProgress)
            {
                var activityProgressList = new ActivityProgressListModel();
               
                activityProgressList.ActivityName = (item.Activity==null)?" ":item.Activity.ActivityName;
                activityProgressList.FiscalYear= (item.FiscalYear == null) ? " " : item.FiscalYear.EnglishFiscalYear;
                activityProgressList.UnitName = (item.Unit == null) ? " " : item.Unit.UnitNameEnglish;
                activityProgressList.Month = item.Month;
                activityProgressList.Quater = item.Quater;
                activityProgressList.Progress = item.Progress;
                activityProgressListModel.Add(activityProgressList);
            }


            var gridModel = new DataSourceResult {
                Data = activityProgressListModel,
                Total = activityProgress.TotalCount
            };
            return Json(gridModel);

        }


        public async Task<ActionResult> Create()
        {

            var unit = new SelectList(await _unitService.GetUnit(), "Id", "UnitShortName").ToList();
            unit.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            var fiscalyear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear").ToList();
            fiscalyear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            var quater = QuaterHelper.GetQuater();
            quater.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.QuaterId = quater;

            ViewBag.FiscalYearId = fiscalyear;

            ViewBag.UnitId = unit;
            MonthHelper month = new MonthHelper();
            var months = month.GetMonths();
            months.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Months = months;
            var activityProgress = new ActivityProgressModel();
            string createdby = null;
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();

            if (roles.Contains(RoleHelper.LssAdmin) || roles.Contains(RoleHelper.VhlsecAdmin) || roles.Contains(RoleHelper.DolfdAdmin))
            {
                createdby = _workContext.CurrentCustomer.Id;
            }
            else
            {
                string adminemail = _workContext.CurrentCustomer.CreatedBy;
                var admin = await _customerService.GetCustomerByEmail(adminemail);
                createdby = admin.Id;
            }
            var activities = await _activityService.GetActivity(createdby);
            activityProgress.Activities = activities.ToList();
            return View(activityProgress);
        }
        [HttpPost]
        public async Task<IActionResult> Create(ActivityProgressModel model, IFormCollection form)
        {
            var progress = form["Progress"].ToList();
            var units = form["Unit"].ToList();
            var activity = form["ActivityId"].ToList();
            var serviceDataIds = form["ServiceDataId"].ToList();
            var updateProgress = new List<ActivityProgress>();
            var addProgress = new List<ActivityProgress>();
            string createdby = null;
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
            if (roles.Contains(RoleHelper.LssAdmin) || roles.Contains(RoleHelper.VhlsecAdmin) || roles.Contains(RoleHelper.DolfdAdmin))
            {
                createdby = _workContext.CurrentCustomer.Id;
            }
            else
            {
                string adminemail = _workContext.CurrentCustomer.CreatedBy;
                var admin = await _customerService.GetCustomerByEmail(adminemail);
                createdby = admin.Id;
            }
            for (int i = 0; i < progress.Count(); i++)
            {


                var activityprogress = new ActivityProgress {
                    Month = model.Month,
                    FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYearId),
                    FiscalYearId = model.FiscalYearId,
                    Unit = await _unitService.GetUnitById(units[i]),
                    UnitId = units[i],
                    Progress = progress[i],
                    Activity = await _activityService.GetActivityById(activity[i]),
                    ActivityId=activity[i],
                    CreatedBy = createdby,

                };
                if (!string.IsNullOrEmpty(serviceDataIds[i]))
                {
                    activityprogress.Id = serviceDataIds[i];
                    updateProgress.Add(activityprogress);
                }
                else
                {
                    addProgress.Add(activityprogress);
                }

            }
            if (updateProgress.Count > 0)
                await _activityProgressService.UpdateActivityProgressList(updateProgress);
            if (addProgress.Count > 0)
                await _activityProgressService.InsertActivityProgressList(addProgress);




            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> GetActivityByFiscalYear(string fiscalyear)
        {
            string createdby = null;

            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();

            if (roles.Contains(RoleHelper.LssAdmin) || roles.Contains(RoleHelper.VhlsecAdmin) || roles.Contains(RoleHelper.DolfdAdmin))
            {
                createdby = _workContext.CurrentCustomer.Id;
            }
            else
            {
                string adminemail = _workContext.CurrentCustomer.CreatedBy;
                var admin = await _customerService.GetCustomerByEmail(adminemail);
                createdby = admin.Id;
            }
            var activities = await _activityService.GetActivityByFiscalYear(createdby, fiscalyear);
            return Json(activities);
        }
        [HttpPost]
        public async Task<IActionResult> FilterProgressByFiscalYear(string fiscalyear, string month) {

            string createdby = null;

            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();

            if (roles.Contains(RoleHelper.LssAdmin) || roles.Contains(RoleHelper.VhlsecAdmin) || roles.Contains(RoleHelper.DolfdAdmin))
            {
                createdby = _workContext.CurrentCustomer.Id;
            }
            else
            {
                string adminemail = _workContext.CurrentCustomer.CreatedBy;
                var admin = await _customerService.GetCustomerByEmail(adminemail);
                createdby = admin.Id;
            }
            var activities = await _activityProgressService.GetFilteredProgress(createdby, fiscalyear,month);
            return Json(activities);
        }


    

        #endregion ActivityProgress

    }
}
