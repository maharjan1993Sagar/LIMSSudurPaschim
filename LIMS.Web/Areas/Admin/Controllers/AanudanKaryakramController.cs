﻿using LIMS.Core;
using LIMS.Domain.Bali;
using LIMS.Domain.MoAMAC;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Mvc.Filters;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Bali;
using LIMS.Services.Basic;
using LIMS.Services.Breed;
using LIMS.Services.Customers;
using LIMS.Services.Localization;
using LIMS.Services.LocalStructure;
using LIMS.Services.Media;
using LIMS.Services.MoAMAC;
using LIMS.Services.Security;
using LIMS.Web.Areas.Admin.Extensions.Mapping;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Web.Areas.Admin.Models.Bali;
using LIMS.Web.Areas.Admin.Models.Bali.Aanudan;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class AanudanKaryakramController : BaseAdminController
    {
        private readonly ILabambitKrishakService _farmerService;
        private readonly IBudgetService _BudgetService;
        private readonly ISpeciesService _speciesService;
        private readonly IBreedService _breedService;
        private readonly ILocalizationService _localizationService;
        private readonly ILanguageService _languageService;
        private readonly IWorkContext _workContext;
        private readonly IFiscalYearService _fiscalYearService;
        private readonly ITalimService _talimService;
        private readonly IIncuvationCenterService _incuvationCenterService;
        private readonly IPictureService _pictureService;
        private readonly IAnudanService _anudanService;
        public readonly IDolfdService _dolfdService;
        public readonly IVhlsecService _vhlsecService;
        public readonly ICustomerService _customerService;
        public readonly IBudgetService _budgetService;
        public readonly ILocalLevelService _localLevelService;


        public AanudanKaryakramController(ILocalizationService localizationService,
            ILabambitKrishakService farmerService,
            ILanguageService languageService,
            ISpeciesService speciesService,
            IBreedService breedService,
            IWorkContext workContext,
            IFiscalYearService fiscalYearService,
            ITalimService talimService,
            IIncuvationCenterService incuvationCenterService,
            IBudgetService BudgetService,
            IPictureService pictureService,
            IAnudanService anudanService,
            IDolfdService dolfdService,
            IVhlsecService vhlsecService,
             ICustomerService customerService,
            IBudgetService budgetService,
            ILocalLevelService localLevelService

            )
        {
            _localizationService = localizationService;
            _farmerService = farmerService;
            _languageService = languageService;
            _speciesService = speciesService;
            _breedService = breedService;
            _workContext = workContext;
            _fiscalYearService = fiscalYearService;
            _talimService = talimService;
            _incuvationCenterService = incuvationCenterService;
            _BudgetService = BudgetService;
            _pictureService = pictureService;
            _anudanService = anudanService;
            _dolfdService = dolfdService;
            _vhlsecService = vhlsecService;
            _customerService = customerService;
            _budgetService = budgetService;
            _localLevelService = localLevelService;
        }

        public IActionResult Index() => RedirectToAction("List");

        public async Task<IActionResult> TabView(int index=0)
        {
            await SaveSelectedTabIndex(index);
            return View("TabView");
        }

        public IActionResult List() => View();

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command)
        {
            var id = _workContext.CurrentCustomer.Id;
            var bali = await _anudanService.GetbaliRegister(id, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult {
                Data = bali,
                Total = bali.TotalCount
            };
            return Json(gridModel);
        }

        public async Task<IActionResult> Report()
        {
            var id = _workContext.CurrentCustomer.Id;
            var role = _workContext.CurrentCustomer.CustomerRoles.Select(m => m.Name).ToList();
            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.SpeciesId = species;

            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();
            
            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;
            var type = PujigatType();
            type.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Type = type;
            var programType = ProgramType();
            programType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.ProgramType = programType;
            var month = new MonthHelper();
            var months = month.GetMonths();

            months.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Month = months;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Report(DataSourceRequest command, string type, string programType, string fiscalYear)
        {
            if (type != null || fiscalYear != null||programType!=null)
            {
                var id = _workContext.CurrentCustomer.Id;
                var krishak = await _anudanService.GetFilteredLabambitKrishak(id, fiscalYear, programType, type);
                List<AanudanReport> report = new List<AanudanReport>();
                foreach (var item in krishak)
                {
                    var labambit = new AanudanReport();
                    labambit.Budget = item.Budget;
                    labambit.Rakam = Convert.ToString(item.AanudanRakam);
                    labambit.PhoneNo = item.PhoneNo;
                    labambit.Name = item.KrishakKoName;
                    labambit.Address = item.District + " " + item.LocalLevel;
                    labambit.MaleMember = item.MaleMember.ToString();
                    labambit.FemaleMember = item.FemaleMember.ToString();
                    labambit.DalitMember = item.DalitMember.ToString();
                    labambit.JanajatiMember = item.JanajatiMember.ToString();
                    labambit.Id = item.Id;

                    report.Add(labambit);
                    //labambit.Male=.

                }
                var gridModel = new DataSourceResult {
                    Data = report,
                    Total = report.Count()
                };
                return Json(gridModel);
            }

            else
            {
                List<MonthlyProgressReport> report = new List<MonthlyProgressReport>();

                var gridModel = new DataSourceResult {
                    Data = report,
                    Total = report.Count
                };
                return Json(gridModel);

            }
        }

        public async Task<IActionResult> OrgReport()
        {
            var id = _workContext.CurrentCustomer.Id;
            var role = _workContext.CurrentCustomer.CustomerRoles.Select(m => m.Name).ToList();
           
            
            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();
            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;

            var localLevels = await _localLevelService.GetLocalLevel(ExecutionHelper.District);
            var localLevelSelect = new SelectList(localLevels).ToList();
            localLevelSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.LocalLevels = new SelectList(localLevelSelect, "Text","Text", ExecutionHelper.LocalLevel);

            //ViewBag.Expences = new SelectList(ExecutionHelper.GetPrakar(), "Value", "Text"); //Type
            //ViewBag.Swrot = new SelectList(ExecutionHelper.Swrot(), "Value", "Text"); //ProgramType


            //var month = new MonthHelper();
            //var months = month.GetMonths();
            //months.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            //ViewBag.Month = months;



            //var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
            //species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            //ViewBag.SpeciesId = species;

            //ViewBag.TypeOFExecution = new SelectList(ExecutionHelper.GetTypeOfExecution(), "Value", "Text");

            //var type = PujigatType();
            //type.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            //ViewBag.Type = type;

            //var programType = ProgramType();
            //programType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            //ViewBag.ProgramType = programType;


            //if (role.Contains("MolmacAdmin") || role.Contains("MolmacUser"))
            //{
            //string entityId = _workContext.CurrentCustomer.EntityId;
            //List<Dolfd> dolfdid = _dolfdService.GetDolfdByMolmacId(entityId).Result.ToList();

            //var lss = new SelectList(dolfdid, "Id", "NameEnglish").ToList();
            //lss.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            //ViewBag.dolfd = lss;
            //}
            //else if (role.Contains("DolfdAdmin") || role.Contains("DolfdUser") || role.Contains("AddAdmin") || role.Contains("AddUser"))
            //{
            //    string entityId = _workContext.CurrentCustomer.EntityId;
            //    List<Vhlsec> dolfdid = _vhlsecService.GetVhlsecByDolfdId(entityId).Result.ToList();

            //    var lss = new SelectList(dolfdid, "Id", "NameEnglish").ToList();
            //    lss.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            //    ViewBag.vhlsec = lss;
            //}
            var model = new MonthlyProgressModel();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> OrgReport(DataSourceRequest command, string fiscalYear, string budgetId, string localLevel)//string vhlsecid,string type, string sourceOfFund,
        {           
                var id = _workContext.CurrentCustomer.Id;
                var krishak = await _anudanService.GetFilteredSubsidy("", fiscalYear, localLevel,budgetId,"");
                List<AanudanReport> report = new List<AanudanReport>();
                foreach (var item in krishak)
                {
                    var labambit = new AanudanReport();
                    labambit.Id = item.Id;
                    labambit.Budget = item.Budget;
                    labambit.Rakam = Convert.ToString(item.AanudanRakam);
                    labambit.PhoneNo = item.PhoneNo;
                    labambit.Name = item.KrishakKoName;
                    labambit.FiscalYearId   = item.FiscalyearId;
                    labambit.MaleMember  = item.MaleMember.ToString();
                    labambit.FemaleMember = item.FemaleMember.ToString();
                    labambit.DalitMember = item.DalitMember.ToString();
                    labambit.OtherMember = item.Others.ToString();
                    labambit.Address = item.LocalLevel;
                    labambit.OrgName = _customerService.GetCustomerById(item.CreatedBy).Result.OrgName;                    
                    report.Add(labambit);
                }

                var gridModel = new DataSourceResult {
                    Data = report,
                    Total = report.Count()
                };
                return Json(gridModel);


            //if (!string.IsNullOrEmpty(dolfdid))// && string.IsNullOrEmpty(vhlsecid))
            //{
            //    List<string> entities = _vhlsecService.GetVhlsecByDolfdId(dolfdid).Result.Select(m => m.Id).ToList();
            //    var customers = _customerService.GetCustomerByLssId(entities, dolfdid);
            //    List<string> customerid = customers.Select(x => x.Id).ToList();

            // }
            //else if (!string.IsNullOrEmpty(vhlsecid))
            //{
            //    string entity = vhlsecid;
            //    var customers = _customerService.GetCustomerByLssId(null, entity);
            //    List<string> customerid = customers.Select(x => x.Id).ToList();
            //    var krishak = await _anudanService.GetFilteredLabambitKrishak(customerid, fiscalYear, programType, type);
            //    List<AanudanReport> report = new List<AanudanReport>();
            //    foreach (var item in krishak)
            //    {
            //        var labambit = new AanudanReport();
            //        labambit.Id = item.Id;
            //        labambit.Budget = item.Budget;
            //        labambit.Rakam = Convert.ToString(item.AanudanRakam);
            //        labambit.PhoneNo = item.PhoneNo;
            //        labambit.Name = item.KrishakKoName;
            //        labambit.Address = item.District + " " + item.LocalLevel;
            //        labambit.OrgName = _customerService.GetCustomerById(item.CreatedBy).Result.OrgName;

            //        report.Add(labambit);
            //        //labambit.Male=.

            //    }
            //    var gridModel = new DataSourceResult {
            //        Data = report,
            //        Total = report.Count()
            //    };
            //    return Json(gridModel);
            //}
            //else
            //{
            //    var id = _workContext.CurrentCustomer.Id;
            //    var krishak = await _anudanService.GetFilteredLabambitKrishak(id, fiscalYear, programType, type);
            //    List<AanudanReport> report = new List<AanudanReport>();
            //    foreach (var item in krishak)
            //    {
            //        var labambit = new AanudanReport();
            //        labambit.Budget = item.Budget;
            //        labambit.Rakam = Convert.ToString(item.AanudanRakam);
            //        labambit.PhoneNo = item.PhoneNo;
            //        labambit.Name = item.KrishakKoName;
            //        labambit.Address = item.District + " " + item.LocalLevel;
            //        labambit.OrgName = _customerService.GetCustomerById(item.CreatedBy).Result.OrgName;

            //        report.Add(labambit);
            //        //labambit.Male=.

            //    }
            //    var gridModel = new DataSourceResult {
            //        Data = report,
            //        Total = report.Count()
            //    };
            //    return Json(gridModel);

            //}

        }


        public async Task<IActionResult> Create()
        {
            var createdby = _workContext.CurrentCustomer.Id;
            var fiscal = await _fiscalYearService.GetCurrentFiscalYear();
            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear",fiscal.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;

            var budget = await _budgetService.GetBudget("");
            var anudan = budget.Where(m => m.ExpensesCategory == "Subsidy").ToList();

            var budgetSelect = new SelectList(anudan, "Id", "ActivityName").ToList();
            budgetSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Budget = budgetSelect;

            var sex = new List<SelectListItem>() {
                new SelectListItem {
                    Text="Male",
                    Value="Male"
                },
                  new SelectListItem {
                    Text="Female",
                    Value="Female"
                },
            };

            sex.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Sex = sex;
            var EthnicGroup = new List<SelectListItem>() {
                new SelectListItem {
                    Text="Dalit",
                    Value="Dalit"
                },
                  new SelectListItem {
                    Text="Janajati",
                    Value="Janajati"
                },
                  new SelectListItem {
                    Text="Aanya",
                    Value="Aanya"
                },
            };
            EthnicGroup.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.EthnicGroup = EthnicGroup;

            AanudanModel model = new AanudanModel();
            model.District = _workContext.CurrentCustomer.OrgAddress;
            model.LocalLevel = _workContext.CurrentCustomer.LocalLevel;

            var localLevels = await _localLevelService.GetLocalLevel(ExecutionHelper.District);
            var localLevelSelect = new SelectList(localLevels).ToList();
            localLevelSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.LocalLevels = new SelectList(localLevelSelect, "Text","Text", ExecutionHelper.LocalLevel);

            return View(model);

        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(AanudanModel model, IFormCollection col)
        {            
            var farmer = model.ToEntity();
            farmer.CreatedBy = _workContext.CurrentCustomer.Id;
               
            farmer.FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalyearId);
            farmer.Budget = await _budgetService.GetBudgetById(model.BudgetId);
            farmer.CreatedBy = _workContext.CurrentCustomer.Id;

            var District = col["District"].ToList();
            var Add = col["Address"].ToList();
            //var Male = col["MaleMember"].ToList();
            //var Female = col["FemaleMember"].ToList();
            //var Dalit = col["DalitMember"].ToList();
            //var Janajati = col["JanajatiMember"].ToList();
            //var Others = col["Others"].ToList();           
            var Sex = col["Sex"].ToList();
            var EthinicGroup = col["EthinicGroup"].ToList();
            var Name = col["KrishakKoName"].ToList();
            var Address = col["LocalLevel"].ToList();
            var Phone = col["PhoneNo"].ToList();
            // var Remarks = col["Remarks"].ToList();
            var Ward = col["Ward"].ToList();
            var AnudanReceiverType = col["AnudanReceiverType"].ToList();
           // var Area = col["Area"].ToList();
            var category = col["AanudanKokisim"];
            var subsidycategory = col["SubsidyCategory"];
            var AanudanRakam = col["AanudanRakam"];
            var FarmerContribution = col["FarmerContribution"];
            var Remarks = col["Remarks"].ToList();
            var ExpectedOutput = col["ExpectedOutput"].ToList();
            var LivestockDataId = col["LivestockDataId"].ToList();

            List<AanudanKokaryakram> update = new List<AanudanKokaryakram>();
            List<AanudanKokaryakram> insert = new List<AanudanKokaryakram>();
            for (int i = 0; i < Name.Count(); i++)
            {
                if (string.IsNullOrEmpty(Name[i]))
                    continue;
                AanudanKokaryakram farm = new AanudanKokaryakram();

                farm.Budget = farmer.Budget;
                farm.BudgetId = farmer.BudgetId;
                farm.FiscalYear = farmer.FiscalYear;
                farm.FiscalyearId = farmer.FiscalyearId;
                farm.CreatedBy = farmer.CreatedBy;
                farm.District = farmer.District;
                farm.LocalLevel = farmer.LocalLevel;
                farm.Remaks = Remarks[0];
                farm.ExpectedOutput = farmer.ExpectedOutput;
                farm.KrishakKoName = Name[i];
                farm.PhoneNo = Phone[i];
                farm.LocalLevel = _workContext.CurrentCustomer.LocalLevel;
                farm.Ward = Ward[i];
                farm.MaleMember = Sex[i] == "Male" ? 1 : 0;
                farm.FemaleMember = Sex[i] == "Female" ? 1 : 0;
                farm.DalitMember = EthinicGroup[i] == "Dalit" ? 1 : 0;
                farm.JanajatiMember = EthinicGroup[i] == "Janajati" ? 1 : 0;
                farm.Others = EthinicGroup[i] == "Anya" ? 1 : 0;
                farm.SubsidyCategory = subsidycategory[i];               
                farm.AanudanKokisim = category[i];              
                farm.AnudanReceiverType = AnudanReceiverType[i];
                farm.AanudanRakam = Convert.ToDecimal(AanudanRakam[i]);
                farm.FarmerContribution = Convert.ToDecimal(FarmerContribution[i]);
                farm.Address = Add[i];
                farm.Sex = Sex[i];
                farm.EthinicGroup = EthinicGroup[i];


                if (!string.IsNullOrEmpty(LivestockDataId[i]))
                {
                    farm.Id = LivestockDataId[i];
                    await _anudanService.UpdatebaliRegister(farm);
                }
                else
                {                  
                    await _anudanService.InsertbaliRegister(farm);
                }
            }
            

            //  await _anudanService.InsertbaliRegister(farmer);

            SuccessNotification(_localizationService.GetResource("Admin.Create.successful"));
               // return continueEditing ? RedirectToAction("Edit", new { id = farmer.Id }) : RedirectToAction("TabView");
            
            var createdby = _workContext.CurrentCustomer.Id;
            var incuvationCenter = new SelectList(await _incuvationCenterService.GetincuvationCenter(createdby), "Id", "OrganizationNameEnglish").ToList();
            incuvationCenter.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.IncuvationCenter = incuvationCenter;
            
            var talim = new SelectList(await _talimService.Gettalim(createdby), "Id", "NameEnglish").ToList();
            talim.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Talim = talim;

            var budget = await _budgetService.GetBudget(createdby);
            var anudan = budget.Where(m => m.ExpensesCategory == "Subsidy").ToList();
            var budgetSelect = new SelectList(anudan, "Id", "ActivityName").ToList();
            budgetSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Budget = budget;           

            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.SpeciesId = species;
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);

            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;

            var localLevels = await _localLevelService.GetLocalLevel(ExecutionHelper.District);
            var localLevelSelect = new SelectList(localLevels).ToList();
            localLevelSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.LocalLevels = new SelectList(localLevelSelect, "Text","Text", ExecutionHelper.LocalLevel);

            var EthnicGroup = new List<SelectListItem>() {
                new SelectListItem {
                    Text="Dalit",
                    Value="Dalit"
                },
                  new SelectListItem {
                    Text="Janajati",
                    Value="Janajati"
                },
                  new SelectListItem {
                    Text="Aanya",
                    Value="Aanya"
                },
            };
            EthnicGroup.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.EthnicGroup = EthnicGroup;
            
            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var farmer = await _anudanService.GetbaliRegisterById(id);
            if (farmer == null)
                return RedirectToAction("List");
            var model = farmer.ToModel();
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
            var sex = new List<SelectListItem>() {
                new SelectListItem {
                    Text="Male",
                    Value="Male"
                },
                  new SelectListItem {
                    Text="Female",
                    Value="Female"
                },
            };
            sex.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Sex = sex;
            var pujigatKaryakram = new SelectList(await _budgetService.GetBudget(createdby), "Id", "ActivityName").ToList();
            pujigatKaryakram.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            var EthnicGroup = new List<SelectListItem>() {
                new SelectListItem {
                    Text="Dalit",
                    Value="Dalit"
                },
                  new SelectListItem {
                    Text="Janajati",
                    Value="Janajati"
                },
                  new SelectListItem {
                    Text="Aanya",
                    Value="Aanya"
                },
            };
            EthnicGroup.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.EthnicGroup = EthnicGroup;
            ViewBag.pujigatKaryakram = pujigatKaryakram;


            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(AanudanModel model, bool continueEditing)
        {
            var farmer = await _anudanService.GetbaliRegisterById(model.Id);
            if (farmer == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var m = model.ToEntity(farmer);
                farmer.FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalyearId);
                farmer.Budget = await _budgetService.GetBudgetById(model.BudgetId);



                //  var farmPicture =await _pictureService.GetPictureById(farm.PictureId);
                //if (farmPicture == null)
                //    throw new ArgumentException("No farm picture found with the specified id");

                
                await _anudanService.UpdatebaliRegister(m);

                SuccessNotification(_localizationService.GetResource("Admin.Update.Successful"));
                if (continueEditing)
                {
                    //selected tab
                    await SaveSelectedTabIndex();

                    return RedirectToAction("Edit", new { id = model.Id });
                }
                return RedirectToAction("TabView");
            }
            var EthnicGroup = new List<SelectListItem>() {
                new SelectListItem {
                    Text="Dalit",
                    Value="Dalit"
                },
                  new SelectListItem {
                    Text="Janajati",
                    Value="Janajati"
                },
                  new SelectListItem {
                    Text="Aanya",
                    Value="Aanya"
                },
            };
            EthnicGroup.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.EthnicGroup = EthnicGroup;
            var sex = new List<SelectListItem>() {
                new SelectListItem {
                    Text="Male",
                    Value="Male"
                },
                  new SelectListItem {
                    Text="Female",
                    Value="Female"
                },
            };
            sex.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Sex = sex;


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

            var pujigatKaryakram = new SelectList(await _budgetService.GetBudget(createdby), "Id", "ActivityName").ToList();
            pujigatKaryakram.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
                        ViewBag.pujigatKaryakram = pujigatKaryakram;

            //If we got this far, something failed, redisplay form
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);

            return RedirectToAction("Create");
        }


        public async Task<ActionResult> GetBreed(string species)
        {
            var breed = await _breedService.GetBreedBySpeciesId(species);

            return Json(breed.ToList());
        }
        public async Task<ActionResult> GetProgram(string fiscalyear)
        {
            var createdby = _workContext.CurrentCustomer.Id;
            var budget = await _budgetService.GetBudget(createdby);
            var karyakram = budget.Where(m => m.FiscalYearId == fiscalyear);
            return Json(karyakram.ToList());
        }
        public async Task<ActionResult> GetAnudan(string fiscalyear,string program,string localLevel)
        {
            var createdby = _workContext.CurrentCustomer.Id;
            var roles = _workContext.CurrentCustomer.CustomerRoles.Select(m => m.Name).ToList();
            string xetra="";

            if (roles.Contains("Agriculture"))
            {
                xetra = "कृषि विकास";
            }
            if (roles.Contains("Livestock"))
            {
                xetra = "पशु तथा मत्स्य विकास ";
            }
            if (roles.Contains("Administrators"))
            {
                xetra = "";
            }
            var anudan = await _anudanService.GetFilteredSubsidy("",fiscalyear,localLevel,program,xetra);
            //var karyakram = budget.Where(m => m.FiscalYear.Id == fiscalyear);
            return Json(anudan.ToList());
        }
        public List<SelectListItem> PujigatType()
        {

            return new List<SelectListItem>() {
                new SelectListItem {
                    Text="chalu",
                    Value="chalu"

                },
                 new SelectListItem {
                    Text="pujigat",
                    Value="pujigat"

                },

            };

        }

        public List<SelectListItem> ProgramType()
        {

            return new List<SelectListItem>() {
                new SelectListItem {
                    Text=_localizationService.GetResource("Lims.PujigatKharcha.SanghKoSasarthaAnudanAntargat"),
                    Value="Lims.PujigatKharcha.SanghKoSasarthaAnudanAntargat",

                },
                 new SelectListItem {
                    Text=_localizationService.GetResource("Lims.PujigatKharcha.PardeshKoBajetAntargat"),
                    Value="Lims.PujigatKharcha.PardeshKoBajetAntargat",


                },

            };

        }

    }
}