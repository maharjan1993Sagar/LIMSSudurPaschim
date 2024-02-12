using LIMS.Core;
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
using LIMS.Services.MoAMAC;
using LIMS.Services.Security;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Web.Areas.Admin.Models.Bali;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Framework.Mvc;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class MonthlyProgressController : BaseAdminController
    {
        private readonly IMonthlyPragatiService _animalRegistrationService;
        private readonly IBudgetService _budgetService;
        private readonly ISpeciesService _speciesService;
        private readonly IBreedService _breedService;
        private readonly ILocalizationService _localizationService;
        private readonly ILanguageService _languageService;
        private readonly IWorkContext _workContext;
        private readonly IFiscalYearService _fiscalYearService;
        private readonly IMoAMACService _moamacService;
        private readonly IDolfdService _dolfdService;
        private readonly INlboService _nlboService;
        private readonly IVhlsecService _vhlsecService;
        private readonly ICustomerService _customerService;

        public MonthlyProgressController(ILocalizationService localizationService,
            IMonthlyPragatiService animalRegistrationService,
            ILanguageService languageService,
            ISpeciesService speciesService,
            IBreedService breedService,
            IWorkContext workContext,
            IFiscalYearService fiscalYearService,
            IMoAMACService moamacService,
            IDolfdService dolfdService,
            INlboService nlboService,
            IVhlsecService vhlsecService,
            IBudgetService BudgetService,
            ICustomerService customerService

            )
        {
            _localizationService = localizationService;
            _animalRegistrationService = animalRegistrationService;
            _languageService = languageService;
            _speciesService = speciesService;
            _breedService = breedService;
            _workContext = workContext;
            _fiscalYearService = fiscalYearService;
            _budgetService = BudgetService;
            _vhlsecService = vhlsecService;
            _moamacService = moamacService;
            _nlboService = nlboService;
            _dolfdService = dolfdService;
            _customerService = customerService;
        }

        public IActionResult Index() => RedirectToAction("List");

        public IActionResult List() => View();

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command)
        {
            var id = _workContext.CurrentCustomer.Id;
            var MonthlyPragati = await _animalRegistrationService.GetMonthlyPragati(id, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult {
                Data = MonthlyPragati,
                Total = MonthlyPragati.TotalCount
            };
            return Json(gridModel);
        }

        public async Task<IActionResult> Create()
        {
            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.SpeciesId = species;

            var id = _workContext.CurrentCustomer.Id;
            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();
            var CurrentFiscalYear = fiscalyear.Id;

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", CurrentFiscalYear).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;

            var type = ExecutionHelper.GetPrakar();
            // type.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Type = type;

            var programType = ExecutionHelper.Swrot();
            // programType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.ProgramType = programType;

            ViewBag.ExpensesCategory = new SelectList(ExecutionHelper.GetExecTypes(), "Value", "Text");

            var month = new MonthHelper();
            var months = month.GetMonths();

            months.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Month = months;

            var Budget = await _budgetService.GetBudget("", CurrentFiscalYear, "", "");

            var progress = await _animalRegistrationService.GetFilteredMonthlyPragati("",CurrentFiscalYear,"","","");

            MonthlyProgressModel model = new MonthlyProgressModel();

            foreach (var item in Budget)
            {
                var objPragati = progress.FirstOrDefault(m => m.BudgetId == item.Id);

                if (!progress.Any(m => m.BudgetId == item.Id))
                {
                    var pragati = new MonthlyPragati {
                        BudgetId = item.Id,
                        Budget = item,
                        FiscalYearId = item.FiscalYearId,
                        //Month = "",
                        Id = ""
                    };
                    progress.Add(pragati);
                }
                
            }
            model.Pragatis = progress.ToList();


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MonthlyProgressModel model, IFormCollection form)
        {
            var bhautikpragati = form["BhautikPragati"].ToList();
            var bitiyaPragati = form["BitiyaPragati"].ToList();
            var budgetId = form["BudgetId"].ToList();
            var progressDataId = form["ProgressDataId"].ToList();
            var upalabdhiHaru = form["UpalbdiHaru"].ToList();
            var remarks = form["Remarks"].ToList();

            var updateLivestocks = new List<MonthlyPragati>();
            var addLivestocks = new List<MonthlyPragati>();
            string createdby = null;

            createdby = _workContext.CurrentCustomer.Id;

            for (int i = 0; i < budgetId.Count(); i++)
            {
                if (string.IsNullOrEmpty(bitiyaPragati[i]))
                    continue;

                var livestock = new MonthlyPragati {
                    Budget = await _budgetService.GetBudgetById(budgetId[i]),
                    BitiyaPragati = bitiyaPragati[i],
                    VautikPragati = bhautikpragati[i],
                    BudgetId = budgetId[i],
                    UpalbdiHaru = upalabdhiHaru[i],
                    Remarks = remarks[i],
                    FiscalYearId = model.FiscalYearId,
                    FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYearId),
                    CreatedBy = createdby,
                    Month = model.Month
                };



                if (!string.IsNullOrEmpty(progressDataId[i]))
                {
                    livestock.Id = progressDataId[i];
                    updateLivestocks.Add(livestock);
                }
                else
                {
                    addLivestocks.Add(livestock);
                }

            }

            if (updateLivestocks.Count > 0)
                await _animalRegistrationService.UpdateMonthlyPragatiList(updateLivestocks);
            if (addLivestocks.Count > 0)
                await _animalRegistrationService.InsertMonthlyPragatiList(addLivestocks);

            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.SpeciesId = species;
            
            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", model.FiscalYearId).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;

            var type = ExecutionHelper.GetPrakar();
            type.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Type = type;

            var programType = ExecutionHelper.Swrot();
            programType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.ProgramType = programType;
            
            var month = new MonthHelper();
            var months = month.GetMonths();

            months.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Month = months;

            var budget = await _budgetService.GetBudget(createdby, model.FiscalYearId, "", "");

            
            MonthlyProgressModel models = new MonthlyProgressModel();
            models.Pragatis = model.Pragatis;

            return View(models);
        }
        //public async Task<IActionResult> CreateNitigatKaryakram()
        //{
        //    var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
        //    species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
        //    ViewBag.SpeciesId = species;
        //    var id = _workContext.CurrentCustomer.Id;
        //    var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();
        //    var CurrentFiscalYear = fiscalyear.Id;

        //    var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", CurrentFiscalYear).ToList();
        //    fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
        //    ViewBag.FiscalYearId = fiscalYear;
        //    var type = PujigatType();
        //    type.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
        //    ViewBag.Type = type;
        //    var programType = ProgramType();
        //    programType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
        //    ViewBag.ProgramType = programType;
        //    var month = new MonthHelper();
        //    var months = month.GetMonths();

        //    months.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
        //    ViewBag.Month = months;

        //    var budget = await _budgetService.GetBudget(id, CurrentFiscalYear, "", "");

        //    MonthlyProgressModel model = new MonthlyProgressModel();
        //    model.Budget = budget.ToList();
        //    return View(model);
        //}

        //[HttpPost]
        //public async Task<IActionResult> CreateNitigatKaryakram(MonthlyProgressModel model, IFormCollection form)
        //{
        //    var bhautikpragati = form["BhautikPragati"].ToList();
        //    var bitiyaPragati = form["BitiyaPragati"].ToList();
        //    var budgetId = form["BudgetId"].ToList();
        //    var progressDataId = form["ProgressDataId"].ToList();

        //    var SuchanaPrakashan = form["SuchanaPrakashan"].ToList();
        //    var FieldVerification = form["FieldVerification"].ToList();
        //    var Samzauta = form["Samzauta"].ToList();
        //    var Anugaman = form["Anugaman"].ToList();
        //    var UpalbdiHaru = form["UpalbdiHaru"].ToList();
        //    // var KharchaKoSwrot = form["KharchaKoSwrot"].ToList();
        //    var Remarks = form["Remarks"].ToList();


        //    var updateLivestocks = new List<MonthlyPragati>();
        //    var addLivestocks = new List<MonthlyPragati>();
        //    string createdby = null;

        //    createdby = _workContext.CurrentCustomer.Id;

        //    for (int i = 0; i < budgetId.Count(); i++)
        //    {
        //        if (string.IsNullOrEmpty(bitiyaPragati[i]))
        //            continue;

        //        var livestock = new MonthlyPragati {
        //            Budget = await _budgetService.GetBudgetById(budgetId[i]),
        //            BitiyaPragati = bitiyaPragati[i],
        //            //   VautikPragati = bhautikpragati[i],
        //            BudgetId = budgetId[i],
        //            SuchanaPrakashan = SuchanaPrakashan[i],
        //            FieldVerification = FieldVerification[i],
        //            // KharchaKoSwrot= KharchaKoSwrot[i],
        //            Samzauta = Samzauta[i],
        //            Anugaman = Anugaman[i],
        //            UpalbdiHaru = UpalbdiHaru[i],
        //            FiscalYearId = model.FiscalYearId,
        //            Remarks = Remarks[i],
        //            FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYearId),

        //            CreatedBy = createdby,
        //            Month = model.Month

        //        };
        //        if (!string.IsNullOrEmpty(progressDataId[i]))
        //        {
        //            livestock.Id = progressDataId[i];
        //            updateLivestocks.Add(livestock);
        //        }
        //        else
        //        {
        //            addLivestocks.Add(livestock);
        //        }

        //    }
        //    if (updateLivestocks.Count > 0)
        //        await _animalRegistrationService.UpdateMonthlyPragatiList(updateLivestocks);
        //    if (addLivestocks.Count > 0)
        //        await _animalRegistrationService.InsertMonthlyPragatiList(addLivestocks);

        //    var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
        //    species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
        //    ViewBag.SpeciesId = species;
        //    var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", model.FiscalYearId).ToList();
        //    fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
        //    ViewBag.FiscalYearId = fiscalYear;
        //    var type = PujigatType();
        //    type.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
        //    ViewBag.Type = type;
        //    var programType = ProgramType();
        //    programType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
        //    ViewBag.ProgramType = programType;
        //    var month = new MonthHelper();
        //    var months = month.GetMonths();

        //    months.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
        //    ViewBag.Month = months;

        //    var budget = await _budgetService.GetBudget(createdby, model.FiscalYearId, "", "");

        //    MonthlyProgressModel models = new MonthlyProgressModel();
        //    models.Budget = budget.ToList();

        //    return View(models);
        //}
        //public async Task<IActionResult> CreateMainKaryakram()
        //{
        //    var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
        //    species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
        //    ViewBag.SpeciesId = species;
        //    var id = _workContext.CurrentCustomer.Id;
        //    var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();
        //    var CurrentFiscalYear = fiscalyear.Id;

        //    var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", CurrentFiscalYear).ToList();
        //    fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
        //    ViewBag.FiscalYearId = fiscalYear;
        //    var type = PujigatType();
        //    type.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
        //    ViewBag.Type = type;
        //    var programType = ProgramType();
        //    programType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
        //    ViewBag.ProgramType = programType;
        //    var month = new MonthHelper();
        //    var months = month.GetMonths();

        //    months.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
        //    ViewBag.Month = months;
        //    decimal a = 0;
        //    var budget = await _budgetService.GetBudget(id, CurrentFiscalYear, "", "");
        //    MonthlyProgressModel model = new MonthlyProgressModel();
        //    model.Budget = budget.ToList();
        //    model.Budget.ForEach(m => m.Yearly = ((decimal.TryParse(m.Yearly, out a) ? a * 100000 : 0).ToString()));

        //    return View(model);
        //}

        //[HttpPost]
        //public async Task<IActionResult> CreateMainKaryakram(MonthlyProgressModel model, IFormCollection form)
        //{
        //    var bhautikpragati = form["BhautikPragati"].ToList();
        //    var bitiyaPragati = form["BitiyaPragati"].ToList();
        //    var BudgetId = form["BudgetId"].ToList();
        //    var progressDataId = form["ProgressDataId"].ToList();

        //    var VuktaniPauneKoNam = form["VuktaniPauneKoNam"].ToList();
        //    var Remarks = form["Remarks"].ToList();



        //    var updateLivestocks = new List<MonthlyPragati>();
        //    var addLivestocks = new List<MonthlyPragati>();
        //    string createdby = null;

        //    createdby = _workContext.CurrentCustomer.Id;

        //    for (int i = 0; i < BudgetId.Count(); i++)
        //    {
        //        if (string.IsNullOrEmpty(bitiyaPragati[i]))
        //            continue;

        //        var livestock = new MonthlyPragati {
        //            Budget = await _budgetService.GetBudgetById(BudgetId[i]),
        //            BitiyaPragati = bitiyaPragati[i],
        //            //   VautikPragati = bhautikpragati[i],
        //            BudgetId = BudgetId[i],
        //            VuktaniPauneKoNam = VuktaniPauneKoNam[i],
        //            Remarks = Remarks[i],

        //            FiscalYearId = model.FiscalYearId,
        //            FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYearId),

        //            CreatedBy = createdby,
        //            Month = model.Month

        //        };
        //        if (!string.IsNullOrEmpty(progressDataId[i]))
        //        {
        //            livestock.Id = progressDataId[i];
        //            updateLivestocks.Add(livestock);
        //        }
        //        else
        //        {
        //            addLivestocks.Add(livestock);
        //        }

        //    }
        //    if (updateLivestocks.Count > 0)
        //        await _animalRegistrationService.UpdateMonthlyPragatiList(updateLivestocks);
        //    if (addLivestocks.Count > 0)
        //        await _animalRegistrationService.InsertMonthlyPragatiList(addLivestocks);

        //    var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
        //    species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
        //    ViewBag.SpeciesId = species;
        //    var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", model.FiscalYearId).ToList();
        //    fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
        //    ViewBag.FiscalYearId = fiscalYear;
        //    var type = PujigatType();
        //    type.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
        //    ViewBag.Type = type;
        //    var programType = ProgramType();
        //    programType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
        //    ViewBag.ProgramType = programType;
        //    var month = new MonthHelper();
        //    var months = month.GetMonths();

        //    months.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
        //    ViewBag.Month = months;

        //    var budget = await _budgetService.GetBudget(createdby, model.FiscalYearId, "", "");

        //    MonthlyProgressModel models = new MonthlyProgressModel();
        //    models.Budget = budget.ToList();

        //    return View(models);
        //}



        public async Task<IActionResult> Summery()
        {

            var id = _workContext.CurrentCustomer.Id;
            var role = _workContext.CurrentCustomer.CustomerRoles.Select(m => m.Name).ToList();
            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.SpeciesId = species;
            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();
            var CurrentFiscalYear = fiscalyear.Id;
            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", CurrentFiscalYear).ToList();
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

            if (role.Contains("MolmacAdmin") || role.Contains("MolmacUser"))
            {
                string entityId = _workContext.CurrentCustomer.EntityId;
                List<Dolfd> dolfdid = _dolfdService.GetDolfdByMolmacId(entityId).Result.ToList();

                var lss = new SelectList(dolfdid, "Id", "NameEnglish").ToList();
                lss.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
                ViewBag.dolfd = lss;
            }
            else if (role.Contains("DolfdAdmin") || role.Contains("DolfdUser") || role.Contains("AddAdmin") || role.Contains("AddUser"))
            {
                string entityId = _workContext.CurrentCustomer.EntityId;
                List<Vhlsec> dolfdid = _vhlsecService.GetVhlsecByDolfdId(entityId).Result.ToList();

                var lss = new SelectList(dolfdid, "Id", "NameEnglish").ToList();
                lss.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
                ViewBag.vhlsec = lss;
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Summery(DataSourceRequest command, string type, string programType, string fiscalYear, string month, string vhlsecid, string dolfdid)
        {
            if (!string.IsNullOrEmpty(dolfdid) && string.IsNullOrEmpty(vhlsecid))
            {
                if (fiscalYear != null)
                {
                    var id = _workContext.CurrentCustomer.Id;

                    string entity = _workContext.CurrentCustomer.EntityId;
                    List<string> entities = _vhlsecService.GetVhlsecByDolfdId(dolfdid).Result.Select(m => m.Id).ToList();
                    var customers = _customerService.GetCustomerByLssId(entities, dolfdid);
                    List<string> customerid = customers.Select(x => x.Id).ToList();





                    var budget = await _budgetService.GetBudget(customerid, fiscalYear, programType, type, "",command.Page - 1, command.PageSize);


                    var gridModel = new DataSourceResult {
                        Data = budget,
                        Total = budget.TotalCount
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
            else if (!string.IsNullOrEmpty(vhlsecid))
            {
                if (fiscalYear != null)
                {
                    var id = _workContext.CurrentCustomer.Id;

                    string entity = _workContext.CurrentCustomer.EntityId;
                    var customers = _customerService.GetCustomerByLssId(null, vhlsecid);
                    List<string> customerid = customers.Select(x => x.Id).ToList();


                    var budget = await _budgetService.GetBudget(customerid, fiscalYear, programType, type, "", command.Page - 1, command.PageSize);


                    var gridModel = new DataSourceResult {
                        Data = budget,
                        Total = budget.TotalCount
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
            else
            {
                var id = _workContext.CurrentCustomer.Id;

                var role = _workContext.CurrentCustomer.CustomerRoles.Select(m => m.Name).ToList();

                if (fiscalYear != null)
                {
                    var budget = (dynamic)null;
                    if (role.Contains("MolmacAdmin") || role.Contains("MolmacAdmin"))
                    {
                        string entity = _workContext.CurrentCustomer.EntityId;
                        List<string> entities = _dolfdService.GetDolfdByMolmacId(entity).Result.Select(m => m.Id).ToList();
                        List<string> lss = new List<string>();
                        foreach (var item in entities)
                        {
                            lss.AddRange(_vhlsecService.GetVhlsecByDolfdId(item).Result.Select(m => m.Id).ToList());
                        }
                        var customers = _customerService.GetCustomerByLssId(lss, entities, entity);
                        List<string> customerid = customers.Select(x => x.Id).ToList();
                        budget = await _budgetService.GetBudget(customerid, fiscalYear, programType, type, "", command.Page - 1, command.PageSize);

                    }
                    else if (role.Contains("DolfdAdmin") || role.Contains("DolfdUser") || role.Contains("AddAdmin") || role.Contains("AddUser"))
                    {
                        string entity = _workContext.CurrentCustomer.EntityId;
                        List<string> entities = _vhlsecService.GetVhlsecByDolfdId(entity).Result.Select(m => m.Id).ToList();
                        var customers = _customerService.GetCustomerByLssId(entities, entity);
                        List<string> customerid = customers.Select(x => x.Id).ToList();

                        budget = await _budgetService.GetBudget(customerid, fiscalYear, programType, type, "", command.Page - 1, command.PageSize);

                    }
                    else
                    {
                        budget = await _budgetService.GetBudget(id, fiscalYear, programType, type,"","", command.Page - 1, command.PageSize);

                    }
                    //var id = _workContext.CurrentCustomer.Id;



                    var gridModel = new DataSourceResult {
                        Data = budget,
                        Total = budget.TotalCount
                    };
                    return Json(gridModel);
                }
                else
                {


                    var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();
                    var CurrentFiscalYear = fiscalyear.Id;

                    var budget = (dynamic)null;
                    if (role.Contains("MolmacAdmin") || role.Contains("MolmacAdmin"))
                    {
                        string entity = _workContext.CurrentCustomer.EntityId;
                        List<string> entities = _dolfdService.GetDolfdByMolmacId(entity).Result.Select(m => m.Id).ToList();
                        List<string> lss = new List<string>();
                        foreach (var item in entities)
                        {
                            lss.AddRange(_vhlsecService.GetVhlsecByDolfdId(item).Result.Select(m => m.Id).ToList());
                        }
                        var customers = _customerService.GetCustomerByLssId(lss, entities, entity);
                        List<string> customerid = customers.Select(x => x.Id).ToList();
                        budget = await _budgetService.GetBudget(customerid, CurrentFiscalYear, programType, type, "", command.Page - 1, command.PageSize);

                    }
                    else if (role.Contains("DolfdAdmin") || role.Contains("DolfdUser") || role.Contains("AddAdmin") || role.Contains("AddUser"))
                    {
                        string entity = _workContext.CurrentCustomer.EntityId;
                        List<string> entities = _vhlsecService.GetVhlsecByDolfdId(entity).Result.Select(m => m.Id).ToList();
                        var customers = _customerService.GetCustomerByLssId(entities, entity);
                        List<string> customerid = customers.Select(x => x.Id).ToList();

                        budget = await _budgetService.GetBudget(customerid, CurrentFiscalYear, programType, type, "",command.Page - 1, command.PageSize);

                    }
                    else
                    {
                        budget = await _budgetService.GetBudget(id, CurrentFiscalYear, programType, type,"","" ,command.Page - 1, command.PageSize);

                    }
                    //var id = _workContext.CurrentCustomer.Id;



                    var gridModel = new DataSourceResult {
                        Data = budget,
                        Total = budget.TotalCount
                    };
                    return Json(gridModel);


                }
            }
        }
        public async Task<IActionResult> Report()
        {
            var id = _workContext.CurrentCustomer.Id;

            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.SpeciesId = species;
            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();
            var CurrentFiscalYear = fiscalyear.Id;
            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", CurrentFiscalYear).ToList();
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
            var role = _workContext.CurrentCustomer.CustomerRoles.Select(m => m.Name).ToList();
            if (role.Contains("MolmacAdmin") || role.Contains("MolmacUser"))
            {
                string entityId = _workContext.CurrentCustomer.EntityId;
                List<Dolfd> dolfdid = _dolfdService.GetDolfdByMolmacId(entityId).Result.ToList();

                var lss = new SelectList(dolfdid, "Id", "NameEnglish").ToList();
                lss.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
                ViewBag.dolfd = lss;
            }
            else if (role.Contains("DolfdAdmin") || role.Contains("DolfdUser") || role.Contains("AddAdmin") || role.Contains("AddUser"))
            {
                string entityId = _workContext.CurrentCustomer.EntityId;
                List<Vhlsec> dolfdid = _vhlsecService.GetVhlsecByDolfdId(entityId).Result.ToList();

                var lss = new SelectList(dolfdid, "Id", "NameEnglish").ToList();
                lss.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
                ViewBag.vhlsec = lss;
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Report(DataSourceRequest command, string type, string programType, string fiscalYear, string month, string vhlsecid, string dolfdid, string ToMonth)
        {
            var role = _workContext.CurrentCustomer.CustomerRoles.Select(m => m.Name).ToList();

            if (!string.IsNullOrEmpty(dolfdid) && string.IsNullOrEmpty(vhlsecid))
            {
                if (fiscalYear != null && month != null)
                {
                    var id = _workContext.CurrentCustomer.Id;
                    string previousMonth = null;
                    if (!string.IsNullOrEmpty(month))
                    {
                        var monthHelper = new MonthHelper();
                        var months = monthHelper.GetMonths();
                        SelectListItem listindex = months.Where(m => m.Value == month).Single<SelectListItem>();
                        int index = months.IndexOf(listindex);
                        index = index + 1;

                        if (index != 1)
                        {
                            previousMonth = months.ElementAt(index - 2).Value.ToString();
                        }
                        else
                        {
                            previousMonth = "no";
                        }
                    }

                    string entity = _workContext.CurrentCustomer.EntityId;
                    List<string> entities = _vhlsecService.GetVhlsecByDolfdId(dolfdid).Result.Select(m => m.Id).ToList();
                    var customers = _customerService.GetCustomerByLssId(entities, dolfdid);
                    List<string> customerid = customers.Select(x => x.Id).ToList();




                    var MonthlyPragati = await _animalRegistrationService.GetFilteredMonthlyPragati(customerid, fiscalYear, programType, type, month);
                    var PreviousMonthPragati = await _animalRegistrationService.GetFilteredMonthlyPragati(customerid, fiscalYear, programType, type, previousMonth);
                    var FiscalYearPragati = await _animalRegistrationService.GetFilteredYearlyPragati(customerid, fiscalYear, programType, type);

                    var budget = await _budgetService.GetBudget(customerid, fiscalYear, programType, type);


                    decimal a = 0;
                    // budget.ToList().ForEach(m => m.Yearly = ((decimal.TryParse(m.Yearly, out a) ? a * 100000 : 0).ToString()));


                    List<MonthlyProgressReport> report = new List<MonthlyProgressReport>();
                    foreach (var item in budget)
                    {
                        var progress = new MonthlyProgressReport();
                        //progress.BalanceBudget = item;
                        try
                        {
                            progress.BitiyaPragati = (MonthlyPragati.Where(m => m.Budget.Id == item.Id) != null) ? MonthlyPragati.Where(m => m.Budget.Id == item.Id).FirstOrDefault().BitiyaPragati : "";
                        }
                        catch
                        {
                            progress.BitiyaPragati = "0";

                        }
                        try
                        {
                            progress.VautikPragati = Convert.ToString(Math.Round((Convert.ToDecimal(progress.BitiyaPragati) / Convert.ToDecimal(item.Yearly)), 2));
                        }
                        catch
                        {
                            progress.VautikPragati = "0";
                        }
                        try
                        {
                            progress.PreviousMonthBitiyaPragati = (PreviousMonthPragati.Where(m => m.Budget.Id == item.Id) != null) ? PreviousMonthPragati.Where(m => m.Budget.Id == item.Id).FirstOrDefault().BitiyaPragati : "";
                        }
                        catch
                        {
                            progress.PreviousMonthBitiyaPragati = "0";

                        }
                        try
                        {
                            progress.PreviousMonthVautikPragati = Convert.ToString(Math.Round((Convert.ToDecimal(progress.PreviousMonthBitiyaPragati) / Convert.ToDecimal(item.Yearly)), 2));

                        }
                        catch
                        {
                            progress.PreviousMonthVautikPragati = "0";
                        }
                        try
                        {
                            progress.TotalMonthBitiyaPragati = (FiscalYearPragati.Where(m => m.Budget.Id == item.Id) != null) ? FiscalYearPragati.Where(m => m.Budget.Id == item.Id).Sum(m => Convert.ToDecimal(String.IsNullOrEmpty(m.BitiyaPragati) ? "0" : m.BitiyaPragati)).ToString() : "";
                        }
                        catch
                        {
                            progress.TotalMonthBitiyaPragati = "0";
                        }
                        try
                        {
                            progress.TotalMonthVautikPragati = Convert.ToString(Math.Round((Convert.ToDecimal(progress.TotalMonthBitiyaPragati) / Convert.ToDecimal(item.Yearly)), 2));

                        }
                        catch
                        {
                            progress.TotalMonthVautikPragati = "0";
                        }
                        try
                        {
                            progress.BalanceBudget = Convert.ToString(Convert.ToDecimal(item.Yearly) - Convert.ToDecimal(progress.TotalMonthBitiyaPragati));
                        }
                        catch
                        {
                            progress.BalanceBudget = item.Yearly;
                        }
                        report.Add(progress);
                    }
                    var gridModel = new DataSourceResult {
                        Data = report,
                        Total = MonthlyPragati.TotalCount
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
            else if (!string.IsNullOrEmpty(vhlsecid))
            {
                if (fiscalYear != null && month != null)
                {
                    var id = _workContext.CurrentCustomer.Id;
                    string previousMonth = null;
                    if (!string.IsNullOrEmpty(month))
                    {
                        var monthHelper = new MonthHelper();
                        var months = monthHelper.GetMonths();
                        SelectListItem listindex = months.Where(m => m.Value == month).Single<SelectListItem>();
                        int index = months.IndexOf(listindex);
                        index = index + 1;

                        if (index != 1)
                        {
                            previousMonth = months.ElementAt(index - 2).Value.ToString();
                        }
                        else
                        {
                            previousMonth = "no";
                        }
                    }
                    string entity = vhlsecid;
                    var customers = _customerService.GetCustomerByLssId(null, entity);
                    List<string> customerid = customers.Select(x => x.Id).ToList();


                    var MonthlyPragati = await _animalRegistrationService.GetFilteredMonthlyPragati(customerid, fiscalYear, programType, type, month);
                    var PreviousMonthPragati = await _animalRegistrationService.GetFilteredMonthlyPragati(customerid, fiscalYear, programType, type, previousMonth);
                    var FiscalYearPragati = await _animalRegistrationService.GetFilteredYearlyPragati(customerid, fiscalYear, programType, type);

                    var budget = await _budgetService.GetBudget(customerid, fiscalYear, programType, type);
                    decimal a = 0;
                    budget.ToList().ForEach(m => m.Yearly = ((decimal.TryParse(m.Yearly, out a) ? a * 100000 : 0).ToString()));

                    List<MonthlyProgressReport> report = new List<MonthlyProgressReport>();
                    foreach (var item in budget)
                    {
                        var progress = new MonthlyProgressReport();
                        progress.budget = item;
                        try
                        {
                            progress.BitiyaPragati = (MonthlyPragati.Where(m => m.Budget.Id == item.Id) != null) ? MonthlyPragati.Where(m => m.Budget.Id == item.Id).FirstOrDefault().BitiyaPragati : "";
                        }
                        catch
                        {
                            progress.BitiyaPragati = "0";

                        }
                        try
                        {
                            progress.VautikPragati = Convert.ToString(Math.Round((Convert.ToDecimal(progress.BitiyaPragati) / Convert.ToDecimal(item.Yearly)), 2));
                        }
                        catch
                        {
                            progress.VautikPragati = "0";
                        }
                        try
                        {
                            progress.PreviousMonthBitiyaPragati = (PreviousMonthPragati.Where(m => m.Budget.Id == item.Id) != null) ? PreviousMonthPragati.Where(m => m.Budget.Id == item.Id).FirstOrDefault().BitiyaPragati : "";
                        }
                        catch
                        {
                            progress.PreviousMonthBitiyaPragati = "0";

                        }
                        try
                        {
                            progress.PreviousMonthVautikPragati = Convert.ToString(Math.Round((Convert.ToDecimal(progress.PreviousMonthBitiyaPragati) / Convert.ToDecimal(item.Yearly)), 2));

                        }
                        catch
                        {
                            progress.PreviousMonthVautikPragati = "0";
                        }
                        try
                        {
                            progress.TotalMonthBitiyaPragati = (FiscalYearPragati.Where(m => m.Budget.Id == item.Id) != null) ? FiscalYearPragati.Where(m => m.Budget.Id == item.Id).Sum(m => Convert.ToDecimal(String.IsNullOrEmpty(m.BitiyaPragati) ? "0" : m.BitiyaPragati)).ToString() : "";
                        }
                        catch
                        {
                            progress.TotalMonthBitiyaPragati = "0";
                        }
                        try
                        {
                            progress.TotalMonthVautikPragati = Convert.ToString(Math.Round((Convert.ToDecimal(progress.TotalMonthBitiyaPragati) / Convert.ToDecimal(item.Yearly)), 2));

                        }
                        catch
                        {
                            progress.TotalMonthVautikPragati = "0";
                        }
                        try
                        {
                            progress.BalanceBudget = Convert.ToString(Convert.ToDecimal(item.Yearly) - Convert.ToDecimal(progress.TotalMonthBitiyaPragati));
                        }
                        catch
                        {
                            progress.BalanceBudget = item.Yearly;
                        }
                        report.Add(progress);
                    }
                    var gridModel = new DataSourceResult {
                        Data = report,
                        Total = MonthlyPragati.TotalCount
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
            else
            {
                if (fiscalYear != null && month != null)
                {
                    var id = _workContext.CurrentCustomer.Id;
                    string previousMonth = null;
                    if (!string.IsNullOrEmpty(month))
                    {
                        var monthHelper = new MonthHelper();
                        var months = monthHelper.GetMonths();
                        SelectListItem listindex = months.Where(m => m.Value == month).Single<SelectListItem>();
                        int index = months.IndexOf(listindex);
                        index = index + 1;

                        if (index != 1)
                        {
                            previousMonth = months.ElementAt(index - 2).Value.ToString();
                        }
                        else
                        {
                            previousMonth = "no";
                        }
                    }

                    List<string> customerid = (dynamic)null;
                    if (role.Contains("MolmacAdmin") || role.Contains("MolmacAdmin"))
                    {
                        string entity = _workContext.CurrentCustomer.EntityId;
                        List<string> entities = _dolfdService.GetDolfdByMolmacId(entity).Result.Select(m => m.Id).ToList();
                        List<string> lss = new List<string>();
                        foreach (var item in entities)
                        {
                            lss.AddRange(_vhlsecService.GetVhlsecByDolfdId(item).Result.Select(m => m.Id).ToList());
                        }
                        var customers = _customerService.GetCustomerByLssId(lss, entities, entity);
                        customerid = customers.Select(x => x.Id).ToList();

                    }
                    else if (role.Contains("DolfdAdmin") || role.Contains("DolfdUser") || role.Contains("AddAdmin") || role.Contains("AddUser"))
                    {
                        string entity = _workContext.CurrentCustomer.EntityId;
                        List<string> entities = _vhlsecService.GetVhlsecByDolfdId(entity).Result.Select(m => m.Id).ToList();
                        var customers = _customerService.GetCustomerByLssId(entities, entity);
                        customerid = customers.Select(x => x.Id).ToList();


                    }
                    else
                    {
                        customerid = new List<string>();
                        customerid.Add(_workContext.CurrentCustomer.Id);

                    }
                    var MonthlyPragati = await _animalRegistrationService.GetFilteredMonthlyPragati(customerid, fiscalYear, programType, type, month);
                    var PreviousMonthPragati = await _animalRegistrationService.GetFilteredMonthlyPragati(customerid, fiscalYear, programType, type, previousMonth);
                    var FiscalYearPragati = await _animalRegistrationService.GetFilteredYearlyPragati(customerid, fiscalYear, programType, type);

                    var budget = await _budgetService.GetBudget(customerid, fiscalYear, programType, type);
                    decimal a = 0;
                    budget.ToList().ForEach(m => m.Yearly = ((decimal.TryParse(m.Yearly, out a) ? a * 100000 : 0).ToString()));

                    List<MonthlyProgressReport> report = new List<MonthlyProgressReport>();
                    foreach (var item in budget)
                    {
                        var progress = new MonthlyProgressReport();
                        progress.budget = item;
                        try
                        {
                            progress.BitiyaPragati = (MonthlyPragati.Where(m => m.Budget.Id == item.Id) != null) ? MonthlyPragati.Where(m => m.Budget.Id == item.Id).FirstOrDefault().BitiyaPragati : "";
                        }
                        catch
                        {
                            progress.BitiyaPragati = "0";

                        }
                        try
                        {
                            progress.VautikPragati = Convert.ToString(Math.Round((Convert.ToDecimal(progress.BitiyaPragati) / Convert.ToDecimal(item.Yearly)), 2));
                        }
                        catch
                        {
                            progress.VautikPragati = "0";
                        }
                        try
                        {
                            progress.PreviousMonthBitiyaPragati = (PreviousMonthPragati.Where(m => m.Budget.Id == item.Id) != null) ? PreviousMonthPragati.Where(m => m.Budget.Id == item.Id).FirstOrDefault().BitiyaPragati : "";
                        }
                        catch
                        {
                            progress.PreviousMonthBitiyaPragati = "0";

                        }
                        try
                        {
                            progress.PreviousMonthVautikPragati = Convert.ToString(Math.Round((Convert.ToDecimal(progress.PreviousMonthBitiyaPragati) / Convert.ToDecimal(item.Yearly)), 2));

                        }
                        catch
                        {
                            progress.PreviousMonthVautikPragati = "0";
                        }
                        try
                        {
                            progress.TotalMonthBitiyaPragati = (FiscalYearPragati.Where(m => m.Budget.Id == item.Id) != null) ? FiscalYearPragati.Where(m => m.Budget.Id == item.Id).Sum(m => Convert.ToDecimal(String.IsNullOrEmpty(m.BitiyaPragati) ? "0" : m.BitiyaPragati)).ToString() : "";
                        }
                        catch
                        {
                            progress.TotalMonthBitiyaPragati = "0";
                        }
                        try
                        {
                            progress.TotalMonthVautikPragati = Convert.ToString(Math.Round((Convert.ToDecimal(progress.TotalMonthBitiyaPragati) / Convert.ToDecimal(item.Yearly)), 2));

                        }
                        catch
                        {
                            progress.TotalMonthVautikPragati = "0";
                        }
                        try
                        {
                            progress.BalanceBudget = Convert.ToString(Convert.ToDecimal(item.Yearly) - Convert.ToDecimal(progress.TotalMonthBitiyaPragati));
                        }
                        catch
                        {
                            progress.BalanceBudget = item.Yearly;
                        }
                        report.Add(progress);
                    }
                    var gridModel = new DataSourceResult {
                        Data = report,
                        Total = MonthlyPragati.TotalCount
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
        }





        public async Task<IActionResult> SummerizedReport()
        {
            var id = _workContext.CurrentCustomer.Id;
            var role = _workContext.CurrentCustomer.CustomerRoles.Select(m => m.Name).ToList();
            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.SpeciesId = species;
            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();
            var CurrentFiscalYear = fiscalyear.Id;
            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", CurrentFiscalYear).ToList();
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

            if (role.Contains("MolmacAdmin") || role.Contains("MolmacUser"))
            {
                string entityId = _workContext.CurrentCustomer.EntityId;
                List<Dolfd> dolfdid = _dolfdService.GetDolfdByMolmacId(entityId).Result.ToList();

                var lss = new SelectList(dolfdid, "Id", "NameEnglish").ToList();
                lss.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
                ViewBag.dolfd = lss;
            }
            else if (role.Contains("DolfdAdmin") || role.Contains("DolfdUser") || role.Contains("AddAdmin") || role.Contains("AddUser"))
            {
                string entityId = _workContext.CurrentCustomer.EntityId;
                List<Vhlsec> dolfdid = _vhlsecService.GetVhlsecByDolfdId(entityId).Result.ToList();

                var lss = new SelectList(dolfdid, "Id", "NameEnglish").ToList();
                lss.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
                ViewBag.vhlsec = lss;
            }

            return View();
        }

        public async Task<IActionResult> SummerizedReportHtml(string FiscalYear)
        {
            var livestockWardWiseReportHtml = RenderViewComponentToString("SeasonalReportReport", new { fiscalyear = FiscalYear });

            return Json(new
            {
                success = true,
                message = string.Format(_localizationService.GetResource("Admin.Report.LivestockWardWiseReport.Success")),
                livestockWardWiseReportHtml
            });
        }


        public async Task<ActionResult> GetBudget(string type, string programType, string fiscalYear,string month,string expensesCategory)
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
            //Get Budget
            var budget = await _budgetService.GetBudget("", fiscalYear, programType, type,expensesCategory,xetra);

            //Get Progress
            var progress = await _animalRegistrationService.GetFilteredMonthlyPragati("", fiscalYear, programType, type, month, expensesCategory,xetra);

            var lstProgress = new List<MonthlyPragati>();

            foreach (var item in budget)
            {
                var objPragati = progress.FirstOrDefault(m => m.BudgetId == item.Id);

                if (!progress.Any(m => m.BudgetId == item.Id))
                {
                    var pragati = new MonthlyPragati {
                        BudgetId = item.Id,
                        Budget = item,
                        FiscalYearId = item.FiscalYearId,
                        Month = month,
                        VautikPragati= "0",
                        BitiyaPragati = "0",
                        Id = ""
                    };
                    lstProgress.Add(pragati);
                }
                else{
                    lstProgress.Add(objPragati);
                }
                
            }

            //decimal a = 0;
            //progress.ToList().ForEach(m => m. = ((decimal.TryParse(m.Yearly, out a) ? a * 100000 : 0).ToStrin~g()));

            return Json(lstProgress);
        }


        public async Task<ActionResult> UpdateProgress(string budgetId, string progressId, string month, string bitiya, string vautik, string upalabdhiharu, string remarks,string fiscalYear)
        {
            try
            {
                var createdby = _workContext.CurrentCustomer.Id;


                if (!String.IsNullOrEmpty(progressId))
                {
                    var objMonthlyPragati = await _animalRegistrationService.GetMonthlyPragatiById(progressId);
                    if (objMonthlyPragati != null)
                    {
                        objMonthlyPragati.Budget = await _budgetService.GetBudgetById(budgetId);
                        objMonthlyPragati.BitiyaPragati = bitiya;
                        objMonthlyPragati.VautikPragati = vautik;
                        objMonthlyPragati.UpalbdiHaru = upalabdhiharu;
                        objMonthlyPragati.Remarks = remarks;
                        objMonthlyPragati.Month = month;
                        objMonthlyPragati.CreatedAt = DateTime.Now;
                        objMonthlyPragati.CreatedBy = createdby;

                        await _animalRegistrationService.UpdateMonthlyPragati(objMonthlyPragati);
                        return Ok(new { Message = _localizationService.GetResource("Admin.Common.Success"), id = objMonthlyPragati.Id });
                    }
                    else
                    {
                        objMonthlyPragati = new MonthlyPragati();

                        objMonthlyPragati.BudgetId = budgetId;
                        objMonthlyPragati.Budget = await _budgetService.GetBudgetById(budgetId);
                        objMonthlyPragati.BitiyaPragati = bitiya;
                        objMonthlyPragati.VautikPragati = vautik;
                        objMonthlyPragati.UpalbdiHaru = upalabdhiharu;
                        objMonthlyPragati.Remarks = remarks;
                        objMonthlyPragati.Month = month;
                        objMonthlyPragati.CreatedAt = DateTime.Now;
                        objMonthlyPragati.CreatedBy = createdby;
                        objMonthlyPragati.FiscalYearId = fiscalYear;
                        objMonthlyPragati.FiscalYear = await _fiscalYearService.GetFiscalYearById(fiscalYear);


                       
                        await _animalRegistrationService.InsertMonthlyPragati(objMonthlyPragati);

                        return Ok(new { Message = _localizationService.GetResource("Admin.Common.Success"), id = objMonthlyPragati.Id });
                    }
                }
                else if (!String.IsNullOrEmpty(budgetId))
                {
                    var objMonthlyPragati = new MonthlyPragati();

                    objMonthlyPragati.BudgetId = budgetId;
                    objMonthlyPragati.Budget = await _budgetService.GetBudgetById(budgetId);
                    objMonthlyPragati.BitiyaPragati = bitiya;
                    objMonthlyPragati.VautikPragati = vautik;
                    objMonthlyPragati.UpalbdiHaru = upalabdhiharu;
                    objMonthlyPragati.Remarks = remarks;
                    objMonthlyPragati.Month = month;
                    objMonthlyPragati.CreatedAt = DateTime.Now;
                    objMonthlyPragati.CreatedBy = createdby;
                    objMonthlyPragati.FiscalYearId = fiscalYear;
                    objMonthlyPragati.FiscalYear = await _fiscalYearService.GetFiscalYearById(fiscalYear);


                    await _animalRegistrationService.InsertMonthlyPragati(objMonthlyPragati);
                    return Ok(new { Message = _localizationService.GetResource("Admin.Common.Success"), id = objMonthlyPragati.Id });

                }
                
                return Ok(new { Message = _localizationService.GetResource("Admin.Common.Fail"), id=""});

               

            }
            catch(Exception ex)
            {

                return BadRequest(new { Message = _localizationService.GetResource("Admin.Common.Fail")+"   "+ ex.Message, id="" });

            }
        }


        //public async Task<ActionResult> GetNitigatPujigatKharcha(string type, string programType, string fiscalYear)
        //{
        //    var createdby = _workContext.CurrentCustomer.Id;
        //    decimal a = 0;
        //    var budget = await _budgetService.GetNitigatKharakram(createdby, fiscalYear, programType, type);
        //    budget.ToList().ForEach(m => m.Yearly = ((decimal.TryParse(m.Yearly, out a) ? a * 100000 : 0).ToString()));
        //    budget.ToList().ForEach(m => m.ProgramType =_localizationService.GetResource(m.ProgramType));

        //    return Json(budget);
        //}

        //public async Task<ActionResult> GetMainPujigatKharcha(string type, string programType, string fiscalYear)
        //{
        //    var createdby = _workContext.CurrentCustomer.Id;
        //    var budget = await _budgetService.GetMainKharakram(createdby, fiscalYear, programType, type);
        //    decimal a = 0;
        //    budget.ToList().ForEach(m => m.Yearly = ((decimal.TryParse(m.Yearly, out a) ? a * 100000 : 0).ToString()));

        //    return Json(budget);
        //}
        public async Task<ActionResult> GetProgress(string type, string programType, string fiscalYear, string month)
        {
            var createdby = _workContext.CurrentCustomer.Id;
            var budget = await _animalRegistrationService.GetFilteredMonthlyPragati("", fiscalYear, programType, type, month);

            return Json(budget);
        }
        //public async Task<ActionResult> GetNitigatProgress(string type, string programType, string fiscalYear, string month)
        //{
        //    var createdby = _workContext.CurrentCustomer.Id;
        //    var budget = await _animalRegistrationService.GetFilteredNitiMonthlyPragati(createdby, fiscalYear, programType, type, month);
        //    return Json(budget);
        //}
        //public async Task<ActionResult> GetMainProgress(string type, string programType, string fiscalYear, string month)
        //{
        //    var createdby = _workContext.CurrentCustomer.Id;
        //    var budget = await _animalRegistrationService.GetFilteredMainMonthlyPragati(createdby, fiscalYear, programType, type, month);
        //    return Json(budget);
        //}

        public async Task<ActionResult> GetBreed(string species)
        {
            var breed = await _breedService.GetBreedBySpeciesId(species);

            return Json(breed.ToList());
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

        public List<SelectListItem> GetVhlsecByDolfdId(string dolfdId)
        {


            List<Vhlsec> dolfdid = _vhlsecService.GetVhlsecByDolfdId(dolfdId).Result.ToList();

            var lss = new SelectList(dolfdid, "Id", "NameEnglish").ToList();
            lss.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            return lss;
        }
    }
}
