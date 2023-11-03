using LIMS.Core;
using LIMS.Domain.MoAMAC;
using LIMS.Services.Bali;
using LIMS.Services.Basic;
using LIMS.Services.Customers;
using LIMS.Services.Localization;
using LIMS.Services.MoAMAC;
using LIMS.Web.Areas.Admin.Models.Dashboard;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{

    public class MolmacDashboardController : BaseAdminController
    {
        private readonly ILabambitKrishakService _animalRegistrationService;
        private readonly IPujigatKharchaKharakramService _pujigatKharchaKharakramService;

        private readonly IFarmerService _farmerService;

        private readonly ILocalizationService _localizationService;
        private readonly ILanguageService _languageService;
        private readonly IWorkContext _workContext;
        private readonly IFiscalYearService _fiscalYearService;
        private readonly ITalimService _talimService;
        private readonly IMonthlyPragatiService _monthlyPragatiService;

        private readonly IIncuvationCenterService _incuvationCenterService;
        private readonly IAnudanService _anudanService;
        public readonly IDolfdService _dolfdService;
        public readonly IVhlsecService _vhlsecService;
        public readonly ICustomerService _customerService;

        public MolmacDashboardController(ILocalizationService localizationService,
            ILabambitKrishakService animalRegistrationService,
            ILanguageService languageService,
          IAnudanService anudanService,
            IWorkContext workContext,
            IFiscalYearService fiscalYearService,
            ITalimService talimService,
            IMonthlyPragatiService monthlyPragatiService,
            IIncuvationCenterService incuvationCenterService,
            IPujigatKharchaKharakramService pujigatKharchaKharakramService,
            IFarmerService farmerService,
              IDolfdService dolfdService,
            IVhlsecService vhlsecService,
            ICustomerService customerService

            )
        {
            _localizationService = localizationService;
            _animalRegistrationService = animalRegistrationService;
            _languageService = languageService;
            _monthlyPragatiService = monthlyPragatiService;
            _workContext = workContext;
            _fiscalYearService = fiscalYearService;
            _talimService = talimService;
            _anudanService = anudanService;
            _incuvationCenterService = incuvationCenterService;
            _pujigatKharchaKharakramService = pujigatKharchaKharakramService;
            _farmerService = farmerService;
            _dolfdService = dolfdService;
            _vhlsecService = vhlsecService;
            _customerService = customerService;
        }
        public async Task<IActionResult> Index()
        {
            var id =  _workContext.CurrentCustomer.Id;
            var currentFisaclYears = await _fiscalYearService.GetCurrentFiscalYear();
            string currentFisaclYear = null;
            if (currentFisaclYears != null)
            {
                currentFisaclYear = currentFisaclYears.Id;
            }
            else
            {
                currentFisaclYear = null;
            }
            DashboardOffice df = new DashboardOffice();
            df.Fy = currentFisaclYears.NepaliFiscalYear;
            var kharcha =await _pujigatKharchaKharakramService.GetPujigatKharchaKharakram(id);
            var monthlyrep = await _monthlyPragatiService.GetMonthlyPragati(id);
            var fiscalyearpragati = monthlyrep.Where(m => m.FiscalYear.Id == currentFisaclYear);
            decimal a = 0;
            var totalbuget = kharcha.Select(m => (decimal.TryParse(m.BarsikBajet, out a) ? a * 100000 : 0)).Sum(i=>i);
            var totalbugets = kharcha.Select(m => (decimal.TryParse(m.BarsikBajet, out a) ? a * 100000 : 0)).Sum(i => i);
            var totalprogress = monthlyrep.Select(m => Convert.ToDecimal(m.BitiyaPragati)).Sum(i => i);
            int z = 0;
            var benifiries = await _animalRegistrationService.GetLabambitKrishakHaru(id);
            var malebeni = benifiries.Where(m => m.FiscalYear.Id == currentFisaclYear).Select(m =>  int.TryParse(m.LabambitKrishakKoNam, out z) ? z : 0).Sum();
            var femalebeni = benifiries.Where(m => m.FiscalYear.Id == currentFisaclYear).Select(m => int.TryParse(m.PhoneNo, out z) ? z : 0).Sum();
            var subsidy = await _anudanService.GetbaliRegister(id);
            var fiscalsub = subsidy.Where(m => m.FiscalYear.Id == currentFisaclYear).Select(m => m.MaleMember).Sum();
            var farmertraining = await _farmerService.Getfarmer(id);
          
            var farmermale = farmertraining.Where(m=>m.FiscalYear!=null&&m.FiscalYear.Id==currentFisaclYear).Sum(m=> int.TryParse(m.Male,out z)?z:0);
            var farmerfemale = farmertraining.Where(m => m.FiscalYear != null && m.FiscalYear.Id == currentFisaclYear).Sum(m => int.TryParse(m.FeMale, out z) ? z : 0);

            var talim = await _talimService.Gettalim(id);
            // var talimfiscal = talim.Where(m => m.FiscalYear.Id == currentFisaclYear).Select(m => m.no).Sum();
            df.Budget = totalbuget;
            df.Progress = totalprogress;
            if (totalbuget == 0)
            {
                df.FinencialPercent = 0;
            }
            else
            {
                df.FinencialPercent = Math.Round((totalprogress / totalbuget * 100), 2);
            }
                df.NoOfMaleBenifiries =malebeni.ToString();
            df.NoOfFeMaleBenifiries = femalebeni.ToString();
            df.TotalSubsidiesAmount = fiscalsub.ToString();
            df.MaleTraining = farmermale.ToString();
            df.FemaleTraining = farmerfemale.ToString();
            var role = _workContext.CurrentCustomer.CustomerRoles.Select(m => m.Name).ToList();
            List<MolmacData> mol = new List<MolmacData>();
            if (role.Contains("MolmacAdmin") || role.Contains("MolmacUser"))
            {
                string entityId = _workContext.CurrentCustomer.EntityId;
                List<Dolfd> dolfdid = _dolfdService.GetDolfdByMolmacId(entityId).Result.ToList();
                List<Vhlsec> vhlsecid = _vhlsecService.GetVhlsec().Result.ToList();
                 foreach(var item in dolfdid)
                {
                    var ids= await _customerService.GetCustomerByEmail(item.UserEmail);
                    var kharchas = await _pujigatKharchaKharakramService.GetPujigatKharchaKharakram(ids.Id);
                    var monthlyreps = await _monthlyPragatiService.GetMonthlyPragati(ids.Id);

                    var fiscalyearpragatis = monthlyreps.Where(m => m.FiscalYear.Id == currentFisaclYear);
                    var totalbugetes = kharchas.Select(m => (decimal.TryParse(m.BarsikBajet, out a) ? a * 100000 : 0)).Sum(i => i);
                    var totalprogresss = monthlyreps.Select(m => Convert.ToDecimal(m.BitiyaPragati)).Sum(i => i);
                    MolmacData molmac = new MolmacData();
                    molmac.ExpancesTillDate = totalprogresss.ToString();
                    molmac.YearlyBudget = totalbugetes.ToString();
                    try
                    {
                        molmac.FinencialProgress = Math.Round((totalprogresss / totalbugetes * 100), 2).ToString();
                    }
                    catch
                    {
                        molmac.FinencialProgress = "0";
                    }
                        molmac.Name = item.NameEnglish.ToString();
                    mol.Add(molmac);
                }
                foreach (var item in vhlsecid)
                {
                    var ids = await _customerService.GetCustomerByEmail(item.UserEmail);
                    if (ids != null)
                    {
                        var kharchas = await _pujigatKharchaKharakramService.GetPujigatKharchaKharakram(ids.Id);
                        var monthlyreps = await _monthlyPragatiService.GetMonthlyPragati(ids.Id);

                        var fiscalyearpragatis = monthlyreps.Where(m => m.FiscalYear.Id == currentFisaclYear);
                        var totalbugetes = kharchas.Select(m => (decimal.TryParse(m.BarsikBajet, out a) ? a * 100000 : 0)).Sum(i => i);
                        var totalprogresss = monthlyreps.Select(m => Convert.ToDecimal(m.BitiyaPragati)).Sum(i => i);
                        MolmacData molmac = new MolmacData();
                        molmac.ExpancesTillDate = totalprogresss.ToString();
                        molmac.YearlyBudget = totalbugetes.ToString();
                        try
                        {
                            molmac.FinencialProgress = Math.Round((totalprogresss / totalbugetes * 100), 2).ToString();
                        }
                        catch
                        {
                            molmac.FinencialProgress = "0";
                        }
                        molmac.Name = item.NameEnglish.ToString();
                        mol.Add(molmac);
                    }
                }

            }
            else if (role.Contains("DolfdAdmin") || role.Contains("DolfdUser") || role.Contains("AddAdmin") || role.Contains("AddUser"))
            {
               
                    string entityId = _workContext.CurrentCustomer.EntityId;
                List<Vhlsec> dolfdid = _vhlsecService.GetVhlsecByDolfdId(entityId).Result.ToList();
                foreach (var item in dolfdid)
                {
                    var ids = await _customerService.GetCustomerByEmail(item.UserEmail);
                    if (ids != null)
                    {
                        var kharchas = await _pujigatKharchaKharakramService.GetPujigatKharchaKharakram(ids.Id);
                        var monthlyreps = await _monthlyPragatiService.GetMonthlyPragati(ids.Id);

                        var fiscalyearpragatis = monthlyreps.Where(m => m.FiscalYear.Id == currentFisaclYear);
                        var totalbugetes = kharchas.Select(m => (decimal.TryParse(m.BarsikBajet, out a) ? a * 100000 : 0)).Sum(i => i);
                        var totalprogresss = monthlyreps.Select(m => Convert.ToDecimal(m.BitiyaPragati)).Sum(i => i);
                        MolmacData molmac = new MolmacData();
                        molmac.ExpancesTillDate = totalprogresss.ToString();
                        molmac.YearlyBudget = totalbugetes.ToString();
                        try
                        {
                            molmac.FinencialProgress = Math.Round((totalprogresss / totalbugetes * 100), 2).ToString();
                        }
                        catch
                        {
                            molmac.FinencialProgress = "0";
                        }
                        molmac.Name = item.NameEnglish.ToString();
                        mol.Add(molmac);
                    }
                }

            }
            else
            {
                df.Aanudans =  _anudanService.GetbaliRegister(id).Result.ToList();
            }
            df.MolmacDatas = mol;
            var fis = await _fiscalYearService.GetFiscalYear();
            var fiscalYear = new SelectList(fis, "Id", "NepaliFiscalYear", currentFisaclYear).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            df.Fiscalyear = currentFisaclYear;
            ViewBag.FiscalYearId = fiscalYear;
           
            return View(df);
        }
        [HttpPost]
        public async Task<IActionResult> Index(string Fiscalyear)
        {
            var id = _workContext.CurrentCustomer.Id;
            var currentFisaclYears = await _fiscalYearService.GetCurrentFiscalYear();
            string currentFisaclYear = null;
            if (!string.IsNullOrEmpty(Fiscalyear))
            {
                currentFisaclYear = Fiscalyear;
                currentFisaclYears = await _fiscalYearService.GetFiscalYearById(Fiscalyear);
            }
            else
            {
                if (currentFisaclYears != null)
                {
                    currentFisaclYear = currentFisaclYears.Id;
                }
                else
                {
                    currentFisaclYear = null;
                }
            }
            DashboardOffice df = new DashboardOffice();
            df.Fy = currentFisaclYears.NepaliFiscalYear;
            var kharcha = await _pujigatKharchaKharakramService.GetPujigatKharchaKharakram(id, currentFisaclYear, "", "");      
            var monthlyrep = await _monthlyPragatiService.GetMonthlyPragati(id);
            var fiscalyearpragati = monthlyrep.Where(m => m.FiscalYear.Id == currentFisaclYear);
            var totalbuget = kharcha.Select(m => Convert.ToDecimal(m.BarsikBajet)).Sum(i => i);
            var totalbugets = kharcha.Select(m => Convert.ToDecimal(m.BarsikBajet)).Sum(i => i);
            var totalprogress = monthlyrep.Where(m=>m.FiscalYear.Id==currentFisaclYear).Select(m => Convert.ToDecimal(m.BitiyaPragati)).Sum(i => i);
            var benifiries = await _animalRegistrationService.GetLabambitKrishakHaru(id);
            var malebeni = benifiries.Where(m => m.FiscalYear.Id == currentFisaclYear).Select(m => Convert.ToInt32(m.LabambitKrishakKoNam)).Sum();
            var femalebeni = benifiries.Where(m => m.FiscalYear.Id == currentFisaclYear).Select(m => Convert.ToInt32(m.PhoneNo)).Sum();
            var subsidy = await _anudanService.GetbaliRegister(id);
            var fiscalsub = subsidy.Where(m => m.FiscalYear.Id == currentFisaclYear).Select(m => m.AanudanRakam).Sum();
            var farmertraining = await _farmerService.Getfarmer(id);
            var farmermale = farmertraining.Where(m => m.Talim.FiscalYearId == currentFisaclYear).Where(m => m.Male != null && m.Male.ToLower() == "male").Count();
            var farmerfemale = farmertraining.Where(m => m.Talim.FiscalYearId == currentFisaclYear).Where(m => m.Male != null && m.Male.ToLower() == "female").Count();

            var talim = await _talimService.Gettalim(id);
            // var talimfiscal = talim.Where(m => m.FiscalYear.Id == currentFisaclYear).Select(m => m.no).Sum();
            df.Budget = totalbuget;
            df.Progress = totalprogress;
            if (totalbuget == 0)
            {
                df.FinencialPercent = 0;
            }
            else
            {
                df.FinencialPercent = Math.Round((totalprogress / totalbuget * 100), 2);
            }
            df.NoOfMaleBenifiries = malebeni.ToString();
            df.NoOfFeMaleBenifiries = femalebeni.ToString();
            df.TotalSubsidiesAmount = fiscalsub.ToString();
            df.MaleTraining = farmermale.ToString();
            df.FemaleTraining = farmerfemale.ToString();
            var role = _workContext.CurrentCustomer.CustomerRoles.Select(m => m.Name).ToList();
            List<MolmacData> mol = new List<MolmacData>();
            if (role.Contains("MolmacAdmin") || role.Contains("MolmacUser"))
            {
                string entityId = _workContext.CurrentCustomer.EntityId;
                List<Dolfd> dolfdid = _dolfdService.GetDolfdByMolmacId(entityId).Result.ToList();
                List<Vhlsec> vhlsecid = _vhlsecService.GetVhlsec().Result.ToList();
                foreach (var item in dolfdid)
                {
                    var ids = await _customerService.GetCustomerByEmail(item.UserEmail);
                    var kharchas = await _pujigatKharchaKharakramService.GetPujigatKharchaKharakram(ids.Id);
                    var monthlyreps = await _monthlyPragatiService.GetMonthlyPragati(ids.Id);

                    var fiscalyearpragatis = monthlyreps.Where(m => m.FiscalYear.Id == currentFisaclYear);
                    var totalbugetes = kharchas.Where(m=>m.FiscalYear.Id==currentFisaclYear).Select(m => Convert.ToDecimal(m.BarsikBajet)).Sum(i => i);
                    var totalprogresss = monthlyreps.Where(m => m.FiscalYear.Id == currentFisaclYear).Select(m => Convert.ToDecimal(m.BitiyaPragati)).Sum(i => i);
                    MolmacData molmac = new MolmacData();
                    molmac.ExpancesTillDate = totalprogresss.ToString();
                    molmac.YearlyBudget = totalbugetes.ToString();
                    try
                    {
                        molmac.FinencialProgress = Math.Round((totalprogresss / totalbugetes * 100), 2).ToString();
                    }
                    catch
                    {
                        molmac.FinencialProgress = "0";
                    }
                    molmac.Name = item.NameEnglish.ToString();
                    mol.Add(molmac);
                }
                foreach (var item in vhlsecid)
                {
                    var ids = await _customerService.GetCustomerByEmail(item.UserEmail);
                    if (ids != null)
                    {
                        var kharchas = await _pujigatKharchaKharakramService.GetPujigatKharchaKharakram(ids.Id);
                        var monthlyreps = await _monthlyPragatiService.GetMonthlyPragati(ids.Id);

                        var fiscalyearpragatis = monthlyreps.Where(m => m.FiscalYear.Id == currentFisaclYear);
                        var totalbugetes = kharchas.Where(m => m.FiscalYear.Id == currentFisaclYear).Select(m => Convert.ToDecimal(m.BarsikBajet)).Sum(i => i);
                        var totalprogresss = monthlyreps.Where(m => m.FiscalYear.Id == currentFisaclYear).Select(m => Convert.ToDecimal(m.BitiyaPragati)).Sum(i => i);
                        MolmacData molmac = new MolmacData();
                        molmac.ExpancesTillDate = totalprogresss.ToString();
                        molmac.YearlyBudget = totalbugetes.ToString();
                        try
                        {
                            molmac.FinencialProgress = Math.Round((totalprogresss / totalbugetes * 100), 2).ToString();
                        }
                        catch
                        {
                            molmac.FinencialProgress = "0";
                        }
                        molmac.Name = item.NameEnglish.ToString();
                        mol.Add(molmac);
                    }
                }

            }
            else if (role.Contains("DolfdAdmin") || role.Contains("DolfdUser") || role.Contains("AddAdmin") || role.Contains("AddUser"))
            {

                string entityId = _workContext.CurrentCustomer.EntityId;
                List<Vhlsec> dolfdid = _vhlsecService.GetVhlsecByDolfdId(entityId).Result.ToList();
                foreach (var item in dolfdid)
                {
                    var ids = await _customerService.GetCustomerByEmail(item.UserEmail);
                    if (ids != null)
                    {
                        var kharchas = await _pujigatKharchaKharakramService.GetPujigatKharchaKharakram(ids.Id);
                        var monthlyreps = await _monthlyPragatiService.GetMonthlyPragati(ids.Id);

                        var fiscalyearpragatis = monthlyreps.Where(m => m.FiscalYear.Id == currentFisaclYear);
                        var totalbugetes = kharchas.Where(m => m.FiscalYear.Id == currentFisaclYear).Select(m => Convert.ToDecimal(m.BarsikBajet)).Sum(i => i);
                        var totalprogresss = fiscalyearpragatis.Select(m => Convert.ToDecimal(m.BitiyaPragati)).Sum(i => i);
                        MolmacData molmac = new MolmacData();
                        molmac.ExpancesTillDate = totalprogresss.ToString();
                        molmac.YearlyBudget = totalbugetes.ToString();
                        try
                        {
                            molmac.FinencialProgress = Math.Round((totalprogresss / totalbugetes * 100), 2).ToString();
                        }
                        catch
                        {
                            molmac.FinencialProgress = "0";
                        }
                        molmac.Name = item.NameEnglish.ToString();
                        mol.Add(molmac);
                    }
                }

            }
            else
            {
                df.Aanudans = _anudanService.GetbaliRegister(id).Result.ToList();
            }
            df.MolmacDatas = mol;
            var fis = await _fiscalYearService.GetFiscalYear();
            var fiscalYear = new SelectList(fis, "Id", "NepaliFiscalYear", currentFisaclYear).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            df.Fiscalyear = currentFisaclYear;
            ViewBag.FiscalYearId = fiscalYear;

            return View(df);
        }
        //[HttpPost]
        //public async Task<IActionResult> Index(string Fiscalyear)
        //{
        //    var id = _workContext.CurrentCustomer.Id;
        //    var currentFisaclYears = await _fiscalYearService.GetCurrentFiscalYear();
        //    string currentFisaclYear = null;
        //    if (string.IsNullOrEmpty(Fiscalyear))
        //    {
        //        if (currentFisaclYears != null)
        //        {
        //            currentFisaclYear = currentFisaclYears.Id;
        //        }
        //        else
        //        {
        //            currentFisaclYear = null;
        //        }
        //    }
        //    else
        //    {
        //        currentFisaclYear = Fiscalyear;
        //        currentFisaclYears = await _fiscalYearService.GetFiscalYearById(Fiscalyear);
        //    }
        //    DashboardOffice df = new DashboardOffice();
        //    df.Fy = currentFisaclYears.NepaliFiscalYear;
        //    var kharcha = await _pujigatKharchaKharakramService.GetPujigatKharchaKharakram(id,currentFisaclYear,"","");
        //    var monthlyrep =  await _monthlyPragatiService.GetMonthlyPragati(id);
        //    var fiscalyearpragati = monthlyrep.Where(m => m.FiscalYear.Id == currentFisaclYear);
        //    var totalbuget = kharcha.Select(m => Convert.ToDecimal(m.BarsikBajet)).Sum(i => i);
        //    var totalbugets = kharcha.Select(m => Convert.ToDecimal(m.BarsikBajet)).Sum(i => i);
        //    var totalprogress = fiscalyearpragati.Select(m => Convert.ToDecimal(m.BitiyaPragati)).Sum(i => i);
        //    var benifiries = await _animalRegistrationService.GetLabambitKrishakHaru(id);
        //    var malebeni = benifiries.Where(m => m.FiscalYear.Id == currentFisaclYear).Select(m => m.LabambitKrishakKoNam).Sum();
        //    var femalebeni = benifiries.Where(m => m.FiscalYear.Id == currentFisaclYear).Select(m => m.PhoneNo).Sum();
        //    var subsidy = await _anudanService.GetbaliRegister(id);
        //    var fiscalsub = subsidy.Where(m => m.FiscalYear.Id == currentFisaclYear).Select(m => m.AanudanRakam).Sum();
        //    var farmertraining = await _farmerService.Getfarmer(id);
        //    var farmermale = farmertraining.Where(m => m.Talim.FiscalYearId == currentFisaclYear).Where(m => m.Male != null && m.Male.ToLower() == "male").Count();
        //    var farmerfemale = farmertraining.Where(m => m.Talim.FiscalYearId == currentFisaclYear).Where(m => m.Male != null && m.Male.ToLower() == "female").Count();

        //    var talim = await _talimService.Gettalim(id);
        //    // var talimfiscal = talim.Where(m => m.FiscalYear.Id == currentFisaclYear).Select(m => m.no).Sum();
        //    if (totalbuget == 0)
        //    {
        //        df.FinencialPercent = 0;
        //    }
        //    else
        //    {
        //        df.FinencialPercent = Math.Round((totalprogress / totalbuget * 100), 2);
        //    }
        //    df.NoOfMaleBenifiries = malebeni.ToString();
        //    df.NoOfFeMaleBenifiries = femalebeni.ToString();
        //    df.TotalSubsidiesAmount = fiscalsub.ToString();
        //    df.MaleTraining = farmermale.ToString();
        //    df.FemaleTraining = farmerfemale.ToString();
        //    var role = _workContext.CurrentCustomer.CustomerRoles.Select(m => m.Name).ToList();
        //    List<MolmacData> mol = new List<MolmacData>();
        //    if (role.Contains("MolmacAdmin") || role.Contains("MolmacUser"))
        //    {
        //        string entityId = _workContext.CurrentCustomer.EntityId;
        //        List<Dolfd> dolfdid = _dolfdService.GetDolfdByMolmacId(entityId).Result.ToList();
        //        List<Vhlsec> vhlsecid = _vhlsecService.GetVhlsec().Result.ToList();
        //        foreach (var item in dolfdid)
        //        {
        //            var ids = await _customerService.GetCustomerByEmail(item.UserEmail);
        //            var kharchas = await _pujigatKharchaKharakramService.GetPujigatKharchaKharakram(ids.Id,currentFisaclYear,"","");
        //            var monthlyreps = await _monthlyPragatiService.GetMonthlyPragati(ids.Id);

        //            var fiscalyearpragatis = monthlyreps.Where(m => m.FiscalYear.Id == currentFisaclYear);
        //            var totalbugetes = kharchas.Select(m => Convert.ToDecimal(m.BarsikBajet)).Sum(i => i);
        //            var totalprogresss = fiscalyearpragatis.Select(m => Convert.ToDecimal(m.BitiyaPragati)).Sum(i => i);
        //            MolmacData molmac = new MolmacData();
        //            molmac.ExpancesTillDate = totalprogresss.ToString();
        //            molmac.YearlyBudget = totalbugetes.ToString();
        //            try
        //            {
        //                molmac.FinencialProgress = Math.Round((totalprogresss / totalbugetes * 100), 2).ToString();
        //            }
        //            catch
        //            {
        //                molmac.FinencialProgress = "0";
        //            }
        //            molmac.Name = item.NameEnglish.ToString();
        //            mol.Add(molmac);
        //        }
        //        foreach (var item in vhlsecid)
        //        {
        //            var ids = await _customerService.GetCustomerByEmail(item.UserEmail);
        //            if (ids != null)
        //            {
        //                var kharchas = await _pujigatKharchaKharakramService.GetPujigatKharchaKharakram(ids.Id,currentFisaclYear,"","");
        //                var monthlyreps = await _monthlyPragatiService.GetMonthlyPragati(ids.Id);

        //                var fiscalyearpragatis = monthlyreps.Where(m => m.FiscalYear.Id == currentFisaclYear);
        //                var totalbugetes = kharchas.Select(m => Convert.ToDecimal(m.BarsikBajet)).Sum(i => i);
        //                var totalprogresss = fiscalyearpragatis.Select(m => Convert.ToDecimal(m.BitiyaPragati)).Sum(i => i);
        //                MolmacData molmac = new MolmacData();
        //                molmac.ExpancesTillDate = totalprogresss.ToString();
        //                molmac.YearlyBudget = totalbugetes.ToString();
        //                try
        //                {
        //                    molmac.FinencialProgress = Math.Round((totalprogresss / totalbugetes * 100), 2).ToString();
        //                }
        //                catch
        //                {
        //                    molmac.FinencialProgress = "0";
        //                }
        //                molmac.Name = item.NameEnglish.ToString();
        //                mol.Add(molmac);
        //            }
        //        }

        //    }
        //    else if (role.Contains("DolfdAdmin") || role.Contains("DolfdUser") || role.Contains("AddAdmin") || role.Contains("AddUser"))
        //    {

        //        string entityId = _workContext.CurrentCustomer.EntityId;
        //        List<Vhlsec> dolfdid = _vhlsecService.GetVhlsecByDolfdId(entityId).Result.ToList();
        //        foreach (var item in dolfdid)
        //        {
        //            var ids = await _customerService.GetCustomerByEmail(item.UserEmail);
        //            if (ids != null)
        //            {
        //                var kharchas = await _pujigatKharchaKharakramService.GetPujigatKharchaKharakram(ids.Id,currentFisaclYear,"","");
        //                var monthlyreps = await _monthlyPragatiService.GetMonthlyPragati(ids.Id);

        //                var fiscalyearpragatis = monthlyreps.Where(m => m.FiscalYear.Id == currentFisaclYear);
        //                var totalbugetes = kharchas.Select(m => Convert.ToDecimal(m.BarsikBajet)).Sum(i => i);
        //                var totalprogresss = fiscalyearpragatis.Select(m => Convert.ToDecimal(m.BitiyaPragati)).Sum(i => i);
        //                MolmacData molmac = new MolmacData();
        //                molmac.ExpancesTillDate = totalprogresss.ToString();
        //                molmac.YearlyBudget = totalbugetes.ToString();
        //                try
        //                {
        //                    molmac.FinencialProgress = Math.Round((totalprogresss / totalbugetes * 100), 2).ToString();
        //                }
        //                catch
        //                {
        //                    molmac.FinencialProgress = "0";
        //                }
        //                molmac.Name = item.NameEnglish.ToString();
        //                mol.Add(molmac);
        //            }
        //        }

        //    }
        //    else
        //    {
        //        df.Aanudans = _anudanService.GetbaliRegister(id).Result.ToList();
        //    }
        //    df.MolmacDatas = mol;

        //    var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", currentFisaclYear).ToList();
        //    fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
        //    ViewBag.FiscalYearId = fiscalYear;
        //    return View(df);
        //}
    }
}
