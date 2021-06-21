using LIMS.Core;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Ainr;
using LIMS.Services.Customers;
using LIMS.Services.Localization;
using LIMS.Services.MoAMAC;
using LIMS.Services.Recording;
using LIMS.Services.Security;
using LIMS.Web.Areas.Admin.Extensions.Mapping;
using LIMS.Web.Areas.Admin.Models.Recording;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class PerformanceRecordingController : BaseAdminController
    {
        private readonly IAnimalRegistrationService _animalRegistrationService;

        private readonly IFarmService _farmService;
        private readonly ILocalizationService _localizationService;
        private readonly ILanguageService _languageService;
        private readonly IWorkContext _workContext;
        private readonly IMilkRecordingService _milkRecordingService;
        private readonly IGrowthMonitoringService _growthMonitoringService;
        private readonly ILssService _lssService;
        private readonly ICustomerService _customerService;
        public PerformanceRecordingController(ILocalizationService localizationService, IAnimalRegistrationService animalRegistrationService, IFarmService farmService, ILanguageService languageService, IWorkContext workContext, IMilkRecordingService milkRecordingService, IGrowthMonitoringService growthMonitoringService, ILssService lssService, ICustomerService customerService)
        {
            _localizationService = localizationService;
            _animalRegistrationService = animalRegistrationService;
            _languageService = languageService;
            _farmService = farmService;
            _workContext = workContext;
            _milkRecordingService = milkRecordingService;
            _growthMonitoringService = growthMonitoringService;
            _lssService = lssService;
            _customerService = customerService;

        }
        public IActionResult Index() => RedirectToAction("List");

        public IActionResult List() => View();

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command,string keyword="")
        {
            string user = _workContext.CurrentCustomer.Id;
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();

            if (roles.Contains("VhlsecUser") || roles.Contains("VhlsecAdmin"))
            {
                string vhlsecid = _workContext.CurrentCustomer.EntityId;
                List<string> lssId = _lssService.GetLssByVhlsecId(vhlsecid).Result.Select(m => m.Id).ToList();
                var customers = _customerService.GetCustomerByLssId(lssId, vhlsecid);
                List<string> customerid = customers.Select(x => x.Id).ToList();
                var animal = await _animalRegistrationService.GetAnimalByLss(customerid, keyword, command.Page - 1, command.PageSize);
                var currentuser = _workContext.CurrentCustomer.Id;
                var gridModel = new DataSourceResult {
                    Data = animal,
                    Total = animal.TotalCount
                };
                return Json(gridModel);
            }
            else
            {
                string vhlsecid = _workContext.CurrentCustomer.EntityId;
                var customers = _customerService.GetCustomerByLssId(null, vhlsecid);
                List<string> customerid = customers.Select(x => x.Id).ToList();
                var animalRegistrations = await _animalRegistrationService.GetAnimalByLss(customerid, keyword, command.Page - 1, command.PageSize);
                var gridModel = new DataSourceResult {
                    Data = animalRegistrations,
                    Total = animalRegistrations.TotalCount
                };
                return Json(gridModel);
            }
        }
        public async Task<IActionResult> PerformanceTab(string animalid)
        {
            var milkAndGrowth = new MilkAndGrowth();
            var animal = await _animalRegistrationService.GetAnimalRegistrationById(animalid);

            var animalRegistrationModel = animal.ToModel();
            milkAndGrowth.MilkRecordingModel = new MilkRecordingModel() { AnimalRegistrationModel = animalRegistrationModel };
            milkAndGrowth.GrowthMonitoringModel = new GrowthMonitoringModel() { AnimalRegistrationModel = animalRegistrationModel };
            var recordingPeriod = new List<SelectListItem> {
                new SelectListItem { Text = "Morning", Value = "Morning" },
                new SelectListItem { Text = "Afternoon", Value = "Afternoon" },
                new SelectListItem { Text = "Evening", Value = "Evening" },
            };
            recordingPeriod.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.RecordingPeriod = recordingPeriod;
            return View(milkAndGrowth);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGrowthMonitoring(GrowthMonitoringModel model, bool continueEditing)
        {

            if (ModelState.IsValid)
            {

                var growthMonitoring = model.ToEntity();
                growthMonitoring.AnimalRegistration = await _animalRegistrationService.GetAnimalRegistrationById(model.AnimalRegistrationId);
                growthMonitoring.CreatedBy = _workContext.CurrentCustomer.Email;
                await _growthMonitoringService.InsertGrowthMonitoring(growthMonitoring);
                SuccessNotification(_localizationService.GetResource("Admin.GrowthMonitoring.Added"));
                return RedirectToAction("PerformanceTab", new { animalid = model.AnimalRegistrationId });
            }
            var milkAndGrowth = new MilkAndGrowth();
            var animal = await _animalRegistrationService.GetAnimalRegistrationById(model.AnimalRegistrationId);

            var animalRegistrationModel = animal.ToModel();
            milkAndGrowth.MilkRecordingModel = new MilkRecordingModel() { AnimalRegistrationModel = animalRegistrationModel };
            milkAndGrowth.GrowthMonitoringModel = new GrowthMonitoringModel() { AnimalRegistrationModel = animalRegistrationModel };
            var recordingPeriod = new List<SelectListItem> {
                new SelectListItem { Text = "Morning", Value = "Morning" },
                new SelectListItem { Text = "Afternoon", Value = "Afternoon" },
                new SelectListItem { Text = "Evening", Value = "Evening" },
            };
            recordingPeriod.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.RecordingPeriod = recordingPeriod;
            return RedirectToAction("PerformanceTab", milkAndGrowth);

        }
        [HttpPost]
        public async Task<IActionResult> CreateMilkRecording(MilkRecordingModel model, bool continueEditing)
        {

            if (ModelState.IsValid)
            {

                var milkRecording = model.ToEntity();
                milkRecording.AnimalRegistration = await _animalRegistrationService.GetAnimalRegistrationById(model.AnimalRegistrationId);
                milkRecording.CreatedBy = _workContext.CurrentCustomer.Email;
                await _milkRecordingService.InsertMilkRecording(milkRecording);

                SuccessNotification(_localizationService.GetResource("Admin.MilkRecording.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = milkRecording.Id }) : RedirectToAction("PerformanceTab", new { animalid = model.AnimalRegistrationId });
            }
            var milkAndGrowth = new MilkAndGrowth();
            var animal = await _animalRegistrationService.GetAnimalRegistrationById(model.AnimalRegistrationId);

            var animalRegistrationModel = animal.ToModel();
            milkAndGrowth.MilkRecordingModel = new MilkRecordingModel() { AnimalRegistrationModel = animalRegistrationModel };
            milkAndGrowth.GrowthMonitoringModel = new GrowthMonitoringModel() { AnimalRegistrationModel = animalRegistrationModel };
            var recordingPeriod = new List<SelectListItem> {
                new SelectListItem { Text = "Morning", Value = "Morning" },
                new SelectListItem { Text = "Afternoon", Value = "Afternoon" },
                new SelectListItem { Text = "Evening", Value = "Evening" },
            };
            recordingPeriod.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.RecordingPeriod = recordingPeriod;
            return RedirectToAction("PerformanceTab", milkAndGrowth);
          
        }


        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> GrowthMonitoringList(string animalid, DataSourceRequest command)
        {
            var growth = await _growthMonitoringService.GetGrowthMonitoring(command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult {
                Data = growth.Where(m => m.AnimalRegistration.Id == animalid),
                Total = growth.TotalCount
            };
            return Json(gridModel);
        }


        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> MilkRecordingList(string animalid, DataSourceRequest command)
        {
            var milkRecordings = await _milkRecordingService.GetMilkRecording(command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult {
                Data = milkRecordings.Where(m => m.AnimalRegistration.Id == animalid),
                Total = milkRecordings.TotalCount
            };
            return Json(gridModel);
        }

        
        //edit
        
        //[HttpPost]
        //public async Task<IActionResult> EditMilkrecording(string id)
        //{
        //    var milkRecording = await _milkRecordingService.GetMilkRecordingById(id);
        //    if (milkRecording == null)
        //        return RedirectToAction("List");
        //    var model = milkRecording.ToModel();


        //    ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);

        //    return PartialView("_CreateOrUpdate.MilkRecordingForm", model);
        //}
       


        /// <summary>
        /// Adding milk recording
        /// </summary>
        /// <param name="model">Milk recording model</param>
        /// <returns></returns>
        [HttpPost]

        public async Task<IActionResult> EditMilkrecording(MilkRecordingModel model)
        {
            var milkRecording = await _milkRecordingService.GetMilkRecordingById(model.Id);
            if (milkRecording == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var m = model.ToEntity(milkRecording);
                m.AnimalRegistration = await _animalRegistrationService.GetAnimalRegistrationById(model.AnimalRegistrationId);

                await _milkRecordingService.UpdateMilkRecording(m);



            }




            //If we got this far, something failed, redisplay form
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);

            return PartialView("_CreateOrUpdate.MilkRecordingForm", model);
        }


    }
}
