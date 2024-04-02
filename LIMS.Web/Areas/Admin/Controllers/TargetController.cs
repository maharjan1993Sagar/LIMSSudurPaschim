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
    public class TargetController:BaseAdminController
    {
        #region fields
        private readonly ILocalizationService _localizationService;
        private readonly IUnitService _unitService;
        public readonly IFiscalYearService _fiscalYearService;
        public readonly IWorkContext _workContext;
        public readonly ITargetRegisterService _targetService;
        public readonly ILssService _lssService;
        public readonly ICustomerService _customerService;
        public readonly IActivityService _activityService;
        #endregion fields
        #region ctor
        public TargetController(ILocalizationService localizationService,
            
             IUnitService unitService,
              IFiscalYearService fiscalYearService,
             
              IWorkContext workContext,
             
              ILssService lssService,
              ICustomerService customerService,
              ITargetRegisterService targetService,
              IActivityService activityService
             )
        {
            _localizationService = localizationService;
            
            _unitService = unitService;
            _fiscalYearService = fiscalYearService;
            _workContext = workContext;
            _targetService = targetService;
            _lssService = lssService;
            _customerService = customerService;
            _activityService = activityService;
            
        }
        #endregion ctor
        #region Target
        public IActionResult Index() => RedirectToAction("List");

        public IActionResult List() => View();

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command, string keyword = "")
        {
            string user = _workContext.CurrentCustomer.Id;
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
            string createdby = null;
            //if (roles.Contains(RoleHelper.LssAdmin) || roles.Contains(RoleHelper.VhlsecAdmin) || roles.Contains(RoleHelper.DolfdAdmin))
            //{
            //    createdby = _workContext.CurrentCustomer.Id;
            //}
            //else
            //{
            //    string adminemail = _workContext.CurrentCustomer.CreatedBy;
            //    var admin = await _customerService.GetCustomerByEmail(adminemail);
            //    createdby = admin.Id;
            //}
            var production = await _targetService.GetTargetRegister(command.Page - 1, command.PageSize);
               
                var gridModel = new DataSourceResult {
                    Data = production.Where(m => m.CreatedBy == createdby),
                    Total = production.Where(m => m.CreatedBy == createdby).Count()
                };
                return Json(gridModel);
            
        }


        public async Task<ActionResult> Create()
        {

            var unit = new SelectList(await _unitService.GetUnit(), "Id", "UnitShortName").ToList();
            unit.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));


            var provience = ProvinceHelper.GetProvince();
            provience.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.provience = provience;

            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;
            var quater = QuaterHelper.GetQuater();
            quater.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.QuaterId = quater;

            //ViewBag.FiscalYearId = fiscalyear;

            ViewBag.UnitId = unit;
           
            var target = new TargetModel();
            string createdby = null;
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();

            //if (roles.Contains(RoleHelper.LssAdmin) || roles.Contains(RoleHelper.VhlsecAdmin) || roles.Contains(RoleHelper.DolfdAdmin))
            //{
            //    createdby = _workContext.CurrentCustomer.Id;
            //}
            //else
            //{
            //    string adminemail = _workContext.CurrentCustomer.CreatedBy;
            //    var admin = await _customerService.GetCustomerByEmail(adminemail);
            //    createdby = admin.Id;
            //}
            var activities =await _activityService.GetActivity(createdby);
            target.Activities = activities.ToList();
            return View(target);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TargetModel model, IFormCollection form)
        {
            var activityId = form["ActivityId"].ToList();
            var units = form["Unit"].ToList();
            var quaterOneTarget = form["QuaterOneTarget"].ToList();
            var quaterTwoTarget = form["QuaterTwoTarget"].ToList();
            var quaterThreeTarget = form["QuaterThreeTarget"].ToList();
            var anualTarget = form["AnualTarget"].ToList();

            var existingServiceDataIds = form["TargetDataId"].ToList();
            var updateTargets = new List<TargetRegister>();
            var addTargets = new List<TargetRegister>();
            string createdby = null;
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
            //if (roles.Contains(RoleHelper.LssAdmin) || roles.Contains(RoleHelper.VhlsecAdmin) || roles.Contains(RoleHelper.DolfdAdmin))
            //{
            //    createdby = _workContext.CurrentCustomer.Id;
            //}
            //else
            //{
            //    string adminemail = _workContext.CurrentCustomer.CreatedBy;
            //    var admin = await _customerService.GetCustomerByEmail(adminemail);
            //    createdby = admin.Id;
            //}
            for (int i = 0; i < activityId.Count(); i++)
            {

                var target = new TargetRegister {
                    Activity = await _activityService.GetActivityById(activityId[i]),
                    QuaterOneTarget = quaterOneTarget[i],
                    QuaterTwoTarget = quaterTwoTarget[i],
                    QuaterThreeTarget = quaterThreeTarget[i],
                    AnualTarget=anualTarget[i],
                    Unit = await _unitService.GetUnitById(units[i]),
                    FiscalYearId=model.FiscalYearId,
                    FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYearId),
                    CreatedBy = createdby,

                };
                if (!string.IsNullOrEmpty(existingServiceDataIds[i]))
                {
                    target.Id = existingServiceDataIds[i];
                    updateTargets.Add(target);
                }
                else
                {
                    addTargets.Add(target);
                }

            }
            if (updateTargets.Count > 0)
                await _targetService.UpdateTargetRegisterList(updateTargets);
            if (addTargets.Count > 0)
                await _targetService.InsertTargetRegisterList(addTargets);
            return RedirectToAction("Index");
        }



            
        

        [HttpPost]
        public async Task<IActionResult> GetActivityByFiscalYear(string fiscalyear) {
            string createdby = null;

            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();

            //if (roles.Contains(RoleHelper.LssAdmin) || roles.Contains(RoleHelper.VhlsecAdmin) || roles.Contains(RoleHelper.DolfdAdmin))
            //{
            //    createdby = _workContext.CurrentCustomer.Id;
            //}
            //else
            //{
            //    string adminemail = _workContext.CurrentCustomer.CreatedBy;
            //    var admin = await _customerService.GetCustomerByEmail(adminemail);
            //    createdby = admin.Id;
            //}
            var activities = await _activityService.GetActivityByFiscalYear(createdby,fiscalyear);
            return Json(activities);
        }
        [HttpPost]
        public async Task<IActionResult> GetServiceDataByFiscalYear(string fiscalyear)
        {
            string createdby = null;

            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();

            //if (roles.Contains(RoleHelper.LssAdmin) || roles.Contains(RoleHelper.VhlsecAdmin) || roles.Contains(RoleHelper.DolfdAdmin))
            //{
            //    createdby = _workContext.CurrentCustomer.Id;
            //}
            //else
            //{
            //    string adminemail = _workContext.CurrentCustomer.CreatedBy;
            //    var admin = await _customerService.GetCustomerByEmail(adminemail);
            //    createdby = admin.Id;
            //}
            var activities = await _targetService.GetFilteredTarget(createdby, fiscalyear);
            return Json(activities);
        }
        #endregion Target
    }
}
