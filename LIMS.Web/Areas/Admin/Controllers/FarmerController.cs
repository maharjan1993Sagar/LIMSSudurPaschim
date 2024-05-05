using LIMS.Core;
using LIMS.Domain.Bali;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Mvc.Filters;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Bali;
using LIMS.Services.Basic;
using LIMS.Services.Breed;
using LIMS.Services.Localization;
using LIMS.Services.LocalStructure;
using LIMS.Services.Security;
using LIMS.Web.Areas.Admin.Extensions.Mapping;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Web.Areas.Admin.Models.Bali;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class FarmerController : BaseAdminController
    {
        private readonly IFarmerService _animalRegistrationService;
        private readonly ISpeciesService _speciesService;
        private readonly IBreedService _breedService;
        private readonly ILocalizationService _localizationService;
        private readonly ILanguageService _languageService;
        private readonly IWorkContext _workContext;
        private readonly IFiscalYearService _fiscalYearService;
        private readonly ITalimService _talimService;
        private readonly IIncuvationCenterService _incuvationCenterService;
        private readonly IPujigatKharchaKharakramService _pujigatKharchaKharakramService;
        private readonly ILocalLevelService _localLevelService;
        private readonly IBudgetService _budgetService;

        public FarmerController(ILocalizationService localizationService,
            IFarmerService animalRegistrationService,
            ILanguageService languageService,
            ISpeciesService speciesService,
            IBreedService breedService,
            IWorkContext workContext,
            IFiscalYearService fiscalYearService,
            ITalimService talimService,
            IIncuvationCenterService incuvationCenterService,
            IPujigatKharchaKharakramService pujigatKharchaKharakramService,
            ILocalLevelService localLevelService,
            IBudgetService budgetService
            )
        {
            _localizationService = localizationService;
            _animalRegistrationService = animalRegistrationService;
            _languageService = languageService;
            _speciesService = speciesService;
            _breedService = breedService;
            _workContext = workContext;
            _fiscalYearService = fiscalYearService;
            _talimService = talimService;
            _incuvationCenterService = incuvationCenterService;
            _pujigatKharchaKharakramService = pujigatKharchaKharakramService;
            _localLevelService = localLevelService;
            _budgetService = budgetService;
        }

        public IActionResult Index() => RedirectToAction("List");

        public IActionResult List()
           => View();

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command)
        {
            var id = _workContext.CurrentCustomer.Id;
            var bali = await _animalRegistrationService.Getfarmer("", command.Page - 1, command.PageSize);
            var filtered = bali.Where(m => !String.IsNullOrEmpty(m.TalimId)).ToList();
            var gridModel = new DataSourceResult {
                Data = filtered,
                Total = filtered.Count(),
            };
            return Json(gridModel);
        }
        public async Task<IActionResult> KirshakReport()
        {
            var createdby = _workContext.CurrentCustomer.Id;
            var incuvationCenter = new SelectList(await _incuvationCenterService.GetincuvationCenter(createdby), "Id", "OrganizationNameEnglish").ToList();
            incuvationCenter.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.IncuvationCenter = incuvationCenter;
           
            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();
            var CurrentFiscalYear = fiscalyear.Id;
            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", CurrentFiscalYear).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;

            var talim = new SelectList(await _talimService.Gettalim(createdby), "Id", "NameEnglish").ToList();
            talim.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Talim = talim;
           
            var localLevels = await _localLevelService.GetLocalLevel("KATHMANDU");
            var localLevelSelect = new SelectList(localLevels).ToList();
            localLevelSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.LocalLevels = new SelectList(localLevelSelect, "Text","Text", ExecutionHelper.LocalLevel);

            return View();
        }
        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> KirshakReport(DataSourceRequest command,string fiscalyear, string budgetId, string talimId, string localLevel)
        {
            var id = _workContext.CurrentCustomer.Id;
           
            var farmers = await _animalRegistrationService.GetfarmerByPugigatType("",localLevel,budgetId,fiscalyear, talimId,"", command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult {
                Data = farmers,
                Total = farmers.TotalCount
            };
            return Json(gridModel);
        }


        public async Task<IActionResult> Create()
        {
            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.SpeciesId = species;
            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;

            string createdby = _workContext.CurrentCustomer.Id;
            
            //var incuvationCenter = new SelectList(await _incuvationCenterService.GetincuvationCenter(createdby), "Id", "OrganizationNameEnglish").ToList();
            //incuvationCenter.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            //ViewBag.IncuvationCenter = incuvationCenter;

            //var talim = new SelectList(await _talimService.Gettalim(createdby), "Id", "NameEnglish").ToList();
            //talim.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            //ViewBag.Talim = talim;

            FarmerModel model = new FarmerModel();

            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(FarmerModel model,IFormCollection col)
        {           
            var animalRegistration = model.ToEntity();
           // animalRegistration.Incubation = await _incuvationCenterService.GetincuvationCenterById(animalRegistration.IncuvationCenterId);
          //  animalRegistration.Talim = await _talimService.GettalimById(animalRegistration.TalimId);
            animalRegistration.FiscalYear = await _fiscalYearService.GetFiscalYearById(animalRegistration.FiscalYearId);

            animalRegistration.CreatedBy = _workContext.CurrentCustomer.Id;

            var Male = col["Male"].ToList();
            var Female = col["Female"].ToList();
            var Dalit = col["Dalit"].ToList();
            var Janajati = col["Janajati"].ToList();

            

            var Others = col["Others"].ToList();
            var Name = col["Name"].ToList();
            var Address = col["Address"].ToList();
            var Phone = col["Phone"].ToList();
            // var WardNo = col["WardNo"].ToList();
            var Ward = col["WardNo"].ToList();
            var Remarks = col["Remarks"].ToList();
            var LivestockDataId = col["LivestockDataId"].ToList();
            var Sex = col["Gender"].ToList();
            var EthinicGroup = col["Caste"].ToList();

            List<Farmer> update = new List<Farmer>();
            List<Farmer> insert = new List<Farmer>();
            for (int i = 0; i < Name.Count(); i++)
            {
                if (string.IsNullOrEmpty(Name[i]))
                    continue;
                Farmer farm = new Farmer();
                farm.IncuvationCenterId = model.IncuvationCenterId;
                farm.Talim = animalRegistration.Talim;
                farm.TalimId = animalRegistration.TalimId;
                farm.Incubation = animalRegistration.Incubation;
                farm.IncuvationCenterId = animalRegistration.IncuvationCenterId;
                farm.FiscalYear = animalRegistration.FiscalYear;
                farm.FiscalYearId = animalRegistration.FiscalYearId;
                farm.CreatedBy = animalRegistration.CreatedBy;
                farm.District = animalRegistration.District;
                farm.Name = Name[i];
                farm.Phone = Phone[i];
                farm.Address = Address[i];
                farm.Ward = Ward[i];
                farm.Male = Sex[i] == "Male" ? "1" : "0";
                farm.FeMale = Sex[i] == "Female" ? "1" : "0";
                farm.Dalit = EthinicGroup[i] == "Dalit" ? "1" : "0";
                farm.Janajati = EthinicGroup[i] == "Janajati" ? "1" : "0";
                farm.Others = EthinicGroup[i] == "Anya" ? "1" : "0";
                farm.Gender = Sex[i];
                farm.Caste = EthinicGroup[i];
                farm.Remarks =Remarks[i];
                if(!string.IsNullOrEmpty(LivestockDataId[i]))
                {
                    farm.Id = LivestockDataId[i];
                    await _animalRegistrationService.Updatefarmer(farm);
                }
                else
                {
                    await _animalRegistrationService.Insertfarmer(farm);
                }
            }
           
                SuccessNotification(_localizationService.GetResource("Admin.Create.successful"));
                return RedirectToAction("List");
          
        }


        public async Task<IActionResult> CreateOne()
        {
            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.SpeciesId = species;
            
            var c =await _fiscalYearService.GetCurrentFiscalYear();
            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear",c.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;
           
            string createdby = _workContext.CurrentCustomer.Id;
            var incuvationCenter = new SelectList(await _incuvationCenterService.GetincuvationCenter(createdby), "Id", "OrganizationNameEnglish").ToList();
            incuvationCenter.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.IncuvationCenter = incuvationCenter;
            
            var talim = new SelectList(await _talimService.Gettalim(createdby), "Id", "NameEnglish").ToList();
            talim.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Talim = talim;
            
            FarmerModel model = new FarmerModel();
            model.District = _workContext.CurrentCustomer.OrgAddress;

            var localLevels = await _localLevelService.GetLocalLevel("KATHMANDU");
            var localLevelSelect = new SelectList(localLevels).ToList();
            localLevelSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.LocalLevels = new SelectList(localLevelSelect, "Text","Text", ExecutionHelper.LocalLevel);

            return View(model);
        }


        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> CreateOne(FarmerModel model, IFormCollection col)
        {
            var animalRegistration = model.ToEntity();
            animalRegistration.Talim = await _talimService.GettalimById(animalRegistration.TalimId);
            animalRegistration.Incubation = await _incuvationCenterService.GetincuvationCenterById(animalRegistration.IncuvationCenterId);
            animalRegistration.pujigatKharchaKharakram = await _pujigatKharchaKharakramService.GetPujigatKharchaKharakramById(animalRegistration.pujigatKharchaKharakramId);
            animalRegistration.Budget = await _budgetService.GetBudgetById(animalRegistration.BudgetId);
            animalRegistration.FiscalYear = await _fiscalYearService.GetFiscalYearById(animalRegistration.FiscalYearId);
            animalRegistration.CreatedBy = _workContext.CurrentCustomer.Id;
            var Male = col["Male"].ToList();
            var localLevel = col["LocalLevel"];
            var Gender = col["Gender"].ToList();
            var Caste = col["Caste"].ToList();
            var Female = col["Female"].ToList();
            var Dalit = col["Dalit"].ToList();
            var Janajati = col["Janajati"].ToList();
            var StartDate = col["StartDate"].ToList();
            var EndDate = col["EndDate"].ToList();
            var Others = col["Others"].ToList();
            var Name = col["Name"].ToList();
            var Address = col["Address"].ToList();
            var Phone = col["Phone"].ToList();
            // var WardNo = col["WardNo"].ToList();
            var Ward = col["WardNo"].ToList();
            var Duration = col["Duration"].ToList();
            var Purpose = col["Purpose"].ToList();
            var Remarks = col["Remarks"].ToList();
            var LivestockDataId = col["LivestockDataId"].ToList();
            List<Farmer> update = new List<Farmer>();
            List<Farmer> insert = new List<Farmer>();
            for (int i = 0; i < Name.Count(); i++)
            {
                if (string.IsNullOrEmpty(Name[i]))
                    continue;
                Farmer farm = new Farmer();
                //farm.Incubation = animalRegistration.Incubation;
                //farm.IncuvationCenterId = animalRegistration.IncuvationCenterId;
                farm.TalimId = animalRegistration.TalimId;
                farm.Talim = animalRegistration.Talim;
                farm.pujigatKharchaKharakram = animalRegistration.pujigatKharchaKharakram;
                farm.pujigatKharchaKharakramId = animalRegistration.pujigatKharchaKharakramId;
                farm.Budget = animalRegistration.Budget;
                farm.BudgetId = animalRegistration.BudgetId;
                farm.FiscalYear = animalRegistration.FiscalYear;
                farm.FiscalYearId = animalRegistration.FiscalYearId;
                farm.CreatedBy = animalRegistration.CreatedBy;
                farm.District = animalRegistration.District;
                farm.LocalLevel = _workContext.CurrentCustomer.LocalLevel;
                farm.TotalExpenses =model.TotalExpense;
                farm.Logistics = model.Logistics;
                farm.Name = Name[i];
                farm.Phone = Phone[i];
                farm.Address = Address[i];
                farm.Ward = Ward[i];
                farm.Male = (Gender[i] == "Male") ? "1" : "0";
                farm.FeMale = (Gender[i] == "Female") ? "1" : "0";
                farm.Dalit = (Caste[i] == "Dalit") ? "1" : "0";
                farm.Janajati = (Caste[i] == "Janajati") ? "1" : "0";
                farm.Others = (Caste[i] == "Anya") ? "1" : "0";
                farm.Remarks = Remarks[0];
                farm.StartDate = Convert.ToDateTime(StartDate[0]);
                farm.EndDate = Convert.ToDateTime(EndDate[0]);
                farm.Duration =  (farm.EndDate - farm.StartDate).TotalDays.ToString();
                farm.Purpose = Purpose[0];
                farm.Gender = Gender[i];
                farm.Caste = Caste[i];  
                if (!string.IsNullOrEmpty(LivestockDataId[i]))
                {
                    farm.Id = LivestockDataId[i];
                    await _animalRegistrationService.Updatefarmer(farm);
                }
                else
                {
                    await _animalRegistrationService.Insertfarmer(farm);
                }
            }
            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;

            var localLevels = await _localLevelService.GetLocalLevel("KATHMANDU");
            var localLevelSelect = new SelectList(localLevels).ToList();
            localLevelSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.LocalLevels = new SelectList(localLevelSelect, "Text","Text", ExecutionHelper.LocalLevel);

            SuccessNotification(_localizationService.GetResource("Admin.Create.successful"));
            return View(model);

        }


        public async Task<IActionResult> TrainingReport()
        {
            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.SpeciesId = species;

            var c = await _fiscalYearService.GetCurrentFiscalYear();
            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", c.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;

            string createdby = _workContext.CurrentCustomer.Id;
            var incuvationCenter = new SelectList(await _incuvationCenterService.GetincuvationCenter(createdby), "Id", "OrganizationNameEnglish").ToList();
            incuvationCenter.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.IncuvationCenter = incuvationCenter;

            var talim = new SelectList(await _talimService.Gettalim(createdby), "Id", "NameEnglish").ToList();
            talim.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Talim = talim;

            FarmerModel model = new FarmerModel();
            model.District = _workContext.CurrentCustomer.OrgAddress;

            var localLevels = await _localLevelService.GetLocalLevel("KATHMANDU");
            var localLevelSelect = new SelectList(localLevels).ToList();
            localLevelSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.LocalLevels = new SelectList(localLevelSelect, "Text","Text", ExecutionHelper.LocalLevel);

            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var animalRegistration = await _animalRegistrationService.GetfarmerById(id);
            if (animalRegistration == null)
                return RedirectToAction("List");
            var model = animalRegistration.ToModel();
            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.SpeciesId = species;
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;
            var createdby = _workContext.CurrentCustomer.Id;
            var incuvationCenter = new SelectList(await _incuvationCenterService.GetincuvationCenter(createdby), "Id", "OrganizationNameEnglish").ToList();
            incuvationCenter.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.IncuvationCenter = incuvationCenter;
            var talim = new SelectList(await _talimService.Gettalim(createdby), "Id", "NameEnglish").ToList();
            talim.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Talim = talim;
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(FarmerModel model, bool continueEditing)
        {
            var animalRegistration = await _animalRegistrationService.GetfarmerById(model.Id);
            if (animalRegistration == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var m = model.ToEntity(animalRegistration);
                m.Incubation = await _incuvationCenterService.GetincuvationCenterById(m.IncuvationCenterId);
                m.Talim = await _talimService.GettalimById(m.TalimId);
                await _animalRegistrationService.Updatefarmer(m);

                SuccessNotification(_localizationService.GetResource("Admin.Update.Successful"));
                if (continueEditing)
                {
                    //selected tab
                    await SaveSelectedTabIndex();

                    return RedirectToAction("Edit", new { id = model.Id });
                }
                return RedirectToAction("TabView", "AanudanKaryakram");
            }
            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;
            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.SpeciesId = species;

            var createdby = _workContext.CurrentCustomer.Id;
            var incuvationCenter = new SelectList(await _incuvationCenterService.GetincuvationCenter(createdby), "Id", "OrganizationNameEnglish").ToList();
            incuvationCenter.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.IncuvationCenter = incuvationCenter;
            
            var talim = new SelectList(await _talimService.Gettalim(createdby), "Id", "NameEnglish").ToList();
            talim.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Talim = talim;


            //If we got this far, something failed, redisplay form
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);

            return RedirectToAction("Create");
        }


        public async Task<ActionResult> GetTalim(string fiscalyear)
        {
            var createdby = _workContext.CurrentCustomer.Id;
            var breed = await _talimService.Gettalim(createdby);
            var talim = breed.Where(m => m.FiscalYear.Id == fiscalyear);

            return Json(talim.ToList());
        }
        [HttpPost]
        public async Task<ActionResult> GetTalimData(string fiscalyear,string district,string talim)
        {
            var createdby = _workContext.CurrentCustomer.Id;
            var t = await _animalRegistrationService.GetfarmerByIncuvationCenter(createdby, district, talim, fiscalyear);

            return Json(t.ToList());
        }
        [HttpPost]
        public async Task<ActionResult> GetTalimDataNew(string fiscalyear, string budgetId, string talimId,string localLevel)
        {
            var createdby = _workContext.CurrentCustomer.Id;
            var t = await _animalRegistrationService.GetfarmerByPugigatType("", localLevel,budgetId, fiscalyear, talimId,"");

            return Json(t.ToList());
        }
    }
}
