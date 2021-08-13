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

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class MonthlyProgressController : BaseAdminController
    {
        private readonly IMonthlyPragatiService _animalRegistrationService;
        private readonly IPujigatKharchaKharakramService _pujigatKharchaKharakramService;
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
            IPujigatKharchaKharakramService pujigatKharchaKharakramService,
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
            _pujigatKharchaKharakramService = pujigatKharchaKharakramService;
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

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear",CurrentFiscalYear).ToList();
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
            var pujigatKaryakram = await _pujigatKharchaKharakramService.GetPujigatKharchaKharakram(id, CurrentFiscalYear, "", "");
            
            MonthlyProgressModel model = new MonthlyProgressModel();
            model.pujigatKharchaKharakram = pujigatKaryakram.ToList();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MonthlyProgressModel model,IFormCollection form)
        {
            var bhautikpragati = form["BhautikPragati"].ToList();
            var bitiyaPragati = form["BitiyaPragati"].ToList();
            var pujigatKharchaId = form["PujigatKharchaId"].ToList();
            var progressDataId = form["ProgressDataId"].ToList();
            var updateLivestocks = new List<MonthlyPragati>();
            var addLivestocks = new List<MonthlyPragati>();
            string createdby = null;
           
                createdby = _workContext.CurrentCustomer.Id;
           
            for (int i = 0; i < pujigatKharchaId.Count(); i++)
            {
                if (string.IsNullOrEmpty(bitiyaPragati[i])|| string.IsNullOrEmpty(bhautikpragati[i]))
                    continue;

                var livestock = new MonthlyPragati {
                    pujigatKharchaKharakram = await _pujigatKharchaKharakramService.GetPujigatKharchaKharakramById(pujigatKharchaId[i]),
                    BitiyaPragati = bitiyaPragati[i],
                    VautikPragati = bhautikpragati[i],
                    PujigatKharchaId=pujigatKharchaId[i],
                    FiscalYearId = model.FiscalYearId,
                    FiscalYear=await _fiscalYearService.GetFiscalYearById(model.FiscalYearId),
                  
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
            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear").ToList();
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


            return View(model);
        }


        public async Task<IActionResult> Summery() {
            var id =  _workContext.CurrentCustomer.Id;
            var role = _workContext.CurrentCustomer.CustomerRoles.Select(m => m.Name).ToList();
           
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Summery(DataSourceRequest command)
        {
            var id = _workContext.CurrentCustomer.Id;
            var MonthlyPragati = await _pujigatKharchaKharakramService.GetPujigatKharchaKharakram(id, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult {
                Data = MonthlyPragati,
                Total = MonthlyPragati.TotalCount
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
                List<Dolfd> dolfdid =  _dolfdService.GetDolfdByMolmacId(entityId).Result.ToList();
              
                var lss = new SelectList(dolfdid, "Id", "NameEnglish").ToList();
                lss.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
                ViewBag.dolfd = lss;
            }
            else if(role.Contains("DolfdAdmin") || role.Contains("DolfdUser")|| role.Contains("AddAdmin")|| role.Contains("AddUser"))
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
        public async Task<IActionResult> Report(DataSourceRequest command, string type, string programType, string fiscalYear, string month,string vhlsecid,string dolfdid)
        {
            if (!string.IsNullOrEmpty(dolfdid)&&string.IsNullOrEmpty(vhlsecid))
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
                    var customers = _customerService.GetCustomerByLssId(entities, vhlsecid);
                    List<string> customerid = customers.Select(x => x.Id).ToList();




                    var MonthlyPragati = await _animalRegistrationService.GetFilteredMonthlyPragati(customerid, fiscalYear, programType, type, month, command.Page - 1, command.PageSize);
                    var PreviousMonthPragati = await _animalRegistrationService.GetFilteredMonthlyPragati(customerid, fiscalYear, programType, type, previousMonth, command.Page - 1, command.PageSize);
                    var FiscalYearPragati = await _animalRegistrationService.GetFilteredYearlyPragati(customerid, fiscalYear, programType, type, command.Page - 1, command.PageSize);

                    var pujigatKaryakram = await _pujigatKharchaKharakramService.GetPujigatKharchaKharakram(customerid, fiscalYear, programType, type);

                    List<MonthlyProgressReport> report = new List<MonthlyProgressReport>();
                    foreach (var item in pujigatKaryakram)
                    {
                        var progress = new MonthlyProgressReport();
                        progress.pujigatKharchaKharakram = item;
                        try
                        {
                            progress.BitiyaPragati = (MonthlyPragati.Where(m => m.pujigatKharchaKharakram.Id == item.Id) != null) ? MonthlyPragati.Where(m => m.pujigatKharchaKharakram.Id == item.Id).FirstOrDefault().BitiyaPragati : "";
                        }
                        catch
                        {
                            progress.BitiyaPragati = "0";

                        }
                        try
                        {
                            progress.VautikPragati = (MonthlyPragati.Where(m => m.pujigatKharchaKharakram.Id == item.Id) != null) ? MonthlyPragati.Where(m => m.pujigatKharchaKharakram.Id == item.Id).FirstOrDefault().VautikPragati : "";
                        }
                        catch
                        {
                            progress.VautikPragati = "0";
                        }
                        try
                        {
                            progress.PreviousMonthBitiyaPragati = (PreviousMonthPragati.Where(m => m.pujigatKharchaKharakram.Id == item.Id) != null) ? PreviousMonthPragati.Where(m => m.pujigatKharchaKharakram.Id == item.Id).FirstOrDefault().BitiyaPragati : "";
                        }
                        catch
                        {
                            progress.PreviousMonthBitiyaPragati = "0";

                        }
                        try
                        {
                            progress.PreviousMonthVautikPragati = (PreviousMonthPragati.Where(m => m.pujigatKharchaKharakram.Id == item.Id) != null) ? PreviousMonthPragati.Where(m => m.pujigatKharchaKharakram.Id == item.Id).FirstOrDefault().VautikPragati : "";
                        }
                        catch
                        {
                            progress.PreviousMonthVautikPragati = "0";
                        }
                        try
                        {
                            progress.TotalMonthBitiyaPragati = (FiscalYearPragati.Where(m => m.pujigatKharchaKharakram.Id == item.Id) != null) ? FiscalYearPragati.Where(m => m.pujigatKharchaKharakram.Id == item.Id).Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.BitiyaPragati) ? "0" : m.BitiyaPragati)).ToString() : "";
                        }
                        catch
                        {
                            progress.TotalMonthBitiyaPragati = "0";
                        }
                        try
                        {
                            progress.TotalMonthVautikPragati = (FiscalYearPragati.Where(m => m.pujigatKharchaKharakram.Id == item.Id) != null) ? FiscalYearPragati.Where(m => m.pujigatKharchaKharakram.Id == item.Id).Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.VautikPragati) ? "0" : m.VautikPragati)).ToString() : "";
                        }
                        catch
                        {
                            progress.TotalMonthVautikPragati = "0";
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
            else if(!string.IsNullOrEmpty(vhlsecid))
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
                    var customers = _customerService.GetCustomerByLssId(null, entity);
                    List<string> customerid = customers.Select(x => x.Id).ToList();


                    var MonthlyPragati = await _animalRegistrationService.GetFilteredMonthlyPragati(customerid, fiscalYear, programType, type, month, command.Page - 1, command.PageSize);
                    var PreviousMonthPragati = await _animalRegistrationService.GetFilteredMonthlyPragati(customerid, fiscalYear, programType, type, previousMonth, command.Page - 1, command.PageSize);
                    var FiscalYearPragati = await _animalRegistrationService.GetFilteredYearlyPragati(customerid, fiscalYear,programType, type,  command.Page - 1, command.PageSize);

                    var pujigatKaryakram = await _pujigatKharchaKharakramService.GetPujigatKharchaKharakram(customerid, fiscalYear, programType, type);

                    List<MonthlyProgressReport> report = new List<MonthlyProgressReport>();
                    foreach (var item in pujigatKaryakram)
                    {
                        var progress = new MonthlyProgressReport();
                        progress.pujigatKharchaKharakram = item;
                        try
                        {
                            progress.BitiyaPragati = (MonthlyPragati.Where(m => m.pujigatKharchaKharakram.Id == item.Id) != null) ? MonthlyPragati.Where(m => m.pujigatKharchaKharakram.Id == item.Id).FirstOrDefault().BitiyaPragati : "";
                        }
                        catch
                        {
                            progress.BitiyaPragati = "0";

                        }
                        try
                        {
                            progress.VautikPragati = (MonthlyPragati.Where(m => m.pujigatKharchaKharakram.Id == item.Id) != null) ? MonthlyPragati.Where(m => m.pujigatKharchaKharakram.Id == item.Id).FirstOrDefault().VautikPragati : "";
                        }
                        catch
                        {
                            progress.VautikPragati = "0";
                        }
                        try
                        {
                            progress.PreviousMonthBitiyaPragati = (PreviousMonthPragati.Where(m => m.pujigatKharchaKharakram.Id == item.Id) != null) ? PreviousMonthPragati.Where(m => m.pujigatKharchaKharakram.Id == item.Id).FirstOrDefault().BitiyaPragati : "";
                        }
                        catch
                        {
                            progress.PreviousMonthBitiyaPragati = "0";

                        }
                        try
                        {
                            progress.PreviousMonthVautikPragati = (PreviousMonthPragati.Where(m => m.pujigatKharchaKharakram.Id == item.Id) != null) ? PreviousMonthPragati.Where(m => m.pujigatKharchaKharakram.Id == item.Id).FirstOrDefault().VautikPragati : "";
                        }
                        catch
                        {
                            progress.PreviousMonthVautikPragati = "0";
                        }
                        try
                        {
                            progress.TotalMonthBitiyaPragati = (FiscalYearPragati.Where(m => m.pujigatKharchaKharakram.Id == item.Id) != null) ? FiscalYearPragati.Where(m => m.pujigatKharchaKharakram.Id == item.Id).Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.BitiyaPragati) ? "0" : m.BitiyaPragati)).ToString() : "";
                        }
                        catch
                        {
                            progress.TotalMonthBitiyaPragati = "0";
                        }
                        try
                        {
                            progress.TotalMonthVautikPragati = (FiscalYearPragati.Where(m => m.pujigatKharchaKharakram.Id == item.Id) != null) ? FiscalYearPragati.Where(m => m.pujigatKharchaKharakram.Id == item.Id).Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.VautikPragati) ? "0" : m.VautikPragati)).ToString() : "";
                        }
                        catch
                        {
                            progress.TotalMonthVautikPragati = "0";
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

                    var MonthlyPragati = await _animalRegistrationService.GetFilteredMonthlyPragati(id, fiscalYear, programType, type, month, command.Page - 1, command.PageSize);
                    var PreviousMonthPragati = await _animalRegistrationService.GetFilteredMonthlyPragati(id, fiscalYear, programType, type, previousMonth, command.Page - 1, command.PageSize);
                    var FiscalYearPragati = await _animalRegistrationService.GetFilteredYearlyPragati(id, fiscalYear,  programType, type, command.Page - 1, command.PageSize);

                    var pujigatKaryakram = await _pujigatKharchaKharakramService.GetPujigatKharchaKharakram(id, fiscalYear, programType, type);

                    List<MonthlyProgressReport> report = new List<MonthlyProgressReport>();
                    foreach (var item in pujigatKaryakram)
                    {
                        var progress = new MonthlyProgressReport();
                        progress.pujigatKharchaKharakram = item;
                        try
                        {
                            progress.BitiyaPragati = (MonthlyPragati.Where(m => m.pujigatKharchaKharakram.Id == item.Id) != null) ? MonthlyPragati.Where(m => m.pujigatKharchaKharakram.Id == item.Id).FirstOrDefault().BitiyaPragati : "";
                        }
                        catch
                        {
                            progress.BitiyaPragati = "0";

                        }
                        try
                        {
                            progress.VautikPragati = (MonthlyPragati.Where(m => m.pujigatKharchaKharakram.Id == item.Id) != null) ? MonthlyPragati.Where(m => m.pujigatKharchaKharakram.Id == item.Id).FirstOrDefault().VautikPragati : "";
                        }
                        catch
                        {
                            progress.VautikPragati = "0";
                        }
                        try
                        {
                            progress.PreviousMonthBitiyaPragati = (PreviousMonthPragati.Where(m => m.pujigatKharchaKharakram.Id == item.Id) != null) ? PreviousMonthPragati.Where(m => m.pujigatKharchaKharakram.Id == item.Id).FirstOrDefault().BitiyaPragati : "";
                        }
                        catch
                        {
                            progress.PreviousMonthBitiyaPragati = "0";

                        }
                        try
                        {
                            progress.PreviousMonthVautikPragati = (PreviousMonthPragati.Where(m => m.pujigatKharchaKharakram.Id == item.Id) != null) ? PreviousMonthPragati.Where(m => m.pujigatKharchaKharakram.Id == item.Id).FirstOrDefault().VautikPragati : "";
                        }
                        catch
                        {
                            progress.PreviousMonthVautikPragati = "0";
                        }
                        try
                        {
                            progress.TotalMonthBitiyaPragati = (FiscalYearPragati.Where(m => m.pujigatKharchaKharakram.Id == item.Id) != null) ? FiscalYearPragati.Where(m => m.pujigatKharchaKharakram.Id == item.Id).Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.BitiyaPragati) ? "0" : m.BitiyaPragati)).ToString() : "";
                        }
                        catch
                        {
                            progress.TotalMonthBitiyaPragati = "0";
                        }
                        try
                        {
                            progress.TotalMonthVautikPragati = (FiscalYearPragati.Where(m => m.pujigatKharchaKharakram.Id == item.Id) != null) ? FiscalYearPragati.Where(m => m.pujigatKharchaKharakram.Id == item.Id).Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.VautikPragati) ? "0" : m.VautikPragati)).ToString() : "";
                        }
                        catch
                        {
                            progress.TotalMonthVautikPragati = "0";
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

        public async Task<ActionResult> GetPujigatKharcha(string type, string programType, string fiscalYear)
        {
            var createdby = _workContext.CurrentCustomer.Id;
            var pujigatKaryakram = await _pujigatKharchaKharakramService.GetPujigatKharchaKharakram(createdby, fiscalYear, programType, type );
             return Json(pujigatKaryakram);
        }



        public async Task<ActionResult> GetProgress(string type, string programType, string fiscalYear,string month)
        {
            var createdby = _workContext.CurrentCustomer.Id;
            var pujigatKaryakram = await _animalRegistrationService.GetFilteredMonthlyPragati(createdby, fiscalYear, programType, type,month);
            return Json(pujigatKaryakram);
        }

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
