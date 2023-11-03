using LIMS.Core;
using LIMS.Framework.Components;
using LIMS.Services.Bali;
using LIMS.Services.Basic;
using LIMS.Services.Breed;
using LIMS.Services.Customers;
using LIMS.Services.Localization;
using LIMS.Services.MoAMAC;
using LIMS.Web.Areas.Admin.Models.Bali;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Components
{
    public class SeasonalReportReport : BaseViewComponent
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
      

        public SeasonalReportReport(ILocalizationService localizationService,
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

        public async Task<IViewComponentResult> InvokeAsync(string fiscalYear)
        {
            var createdby = _workContext.CurrentCustomer.Id;
            var progress = await _animalRegistrationService.GetMonthlyPragati(createdby, fiscalyear: fiscalYear);
            var karyakram = await _pujigatKharchaKharakramService.GetPujigatKharchaKharakram(createdby);
            decimal a = 0;
            var TotalBudget = karyakram.ToList().Select(m => (decimal.TryParse(m.BarsikBajet, out a) ? a * 100000 : 0).ToString()).Sum(x=>Convert.ToDecimal(x));
            var firstQuaterBudget= karyakram.ToList().Select(m => (decimal.TryParse(m.PrathamChaumasikBadjet, out a) ? a * 100000 : 0).ToString()).Sum(x => Convert.ToDecimal(x));
            var secondQuaterBudget = karyakram.ToList().Select(m => (decimal.TryParse(m.DosroChaumasikBadjet, out a) ? a * 100000 : 0).ToString()).Sum(x => Convert.ToDecimal(x));
            var thirdQuaterBudget = karyakram.ToList().Select(m => (decimal.TryParse(m.TesroChaumasikBadjet, out a) ? a * 100000 : 0).ToString()).Sum(x => Convert.ToDecimal(x));

            var TotalPugigatBudget = karyakram.ToList().Where(m=>m.Type== "pujigat").Select(m => (decimal.TryParse(m.BarsikBajet, out a) ? a * 100000 : 0).ToString()).Sum(x => Convert.ToDecimal(x));
            var firstPugigatQuaterBudget = karyakram.ToList().Where(m => m.Type == "pujigat").Select(m => (decimal.TryParse(m.PrathamChaumasikBadjet, out a) ? a * 100000 : 0).ToString()).Sum(x => Convert.ToDecimal(x));
            var secondPugigatQuaterBudget = karyakram.ToList().Where(m => m.Type == "pujigat").Select(m => (decimal.TryParse(m.DosroChaumasikBadjet, out a) ? a * 100000 : 0).ToString()).Sum(x => Convert.ToDecimal(x));
            var thirdPugigatQuaterBudget = karyakram.ToList().Where(m => m.Type == "pujigat").Select(m => m.BarsikBajet = ((decimal.TryParse(m.TesroChaumasikBadjet, out a) ? a * 100000 : 0).ToString())).Sum(x => Convert.ToDecimal(x));
            var TotalChaluBudget = karyakram.ToList().Where(m => m.Type == "chalu").Select(m => (decimal.TryParse(m.BarsikBajet, out a) ? a * 100000 : 0).ToString()).Sum(x => Convert.ToDecimal(x));
            var firstChaluQuaterBudget = karyakram.ToList().Where(m => m.Type == "chalu").Select(m => (decimal.TryParse(m.PrathamChaumasikBadjet, out a) ? a * 100000 : 0).ToString()).Sum(x => Convert.ToDecimal(x));
            var secondChaluQuaterBudget = karyakram.ToList().Where(m => m.Type == "chalu").Select(m => (decimal.TryParse(m.DosroChaumasikBadjet, out a) ? a * 100000 : 0).ToString()).Sum(x => Convert.ToDecimal(x));
            var thirdChaluQuaterBudget = karyakram.ToList().Where(m => m.Type == "chalu").Select(m => (decimal.TryParse(m.TesroChaumasikBadjet, out a) ? a * 100000 : 0).ToString()).Sum(x => Convert.ToDecimal(x));


            var firstQuaterKharcha = progress.Where(m => m.Month == "Shrawan" || m.Month == "Bhadra" || m.Month == "Ashwin" || m.Month == "Kartik").Select(m => (decimal.TryParse(m.BitiyaPragati, out a) ? a : 0).ToString()).Sum(x=>Convert.ToDecimal(x));
            var secondKharcha = progress.Where(m => m.Month == "Mangsir" || m.Month == "Poush" || m.Month == "Magh" || m.Month == "Falgun").Select(m => (decimal.TryParse(m.BitiyaPragati, out a) ? a  : 0).ToString()).Sum(x => Convert.ToDecimal(x));
            var thirdQuaterKharcha = progress.Where(m => m.Month == "Chaitra" || m.Month == "Baishakh" || m.Month == "Jestha" || m.Month == "Asar").Select(m => (decimal.TryParse(m.BitiyaPragati, out a) ? a  : 0).ToString()).Sum(x => Convert.ToDecimal(x));
            var yearlyQuaterKharcha = progress.Select(m => (decimal.TryParse(m.BitiyaPragati, out a) ? a * 100000 : 0).ToString()).Sum(x => Convert.ToDecimal(x));

            var firstQuaterChaluKharcha = progress.Where(m => m.pujigatKharchaKharakram.Type == "chalu").Where(m => m.Month == "Shrawan" || m.Month == "Bhadra" || m.Month == "Ashwin" || m.Month == "Kartik").Select(m => (decimal.TryParse(m.BitiyaPragati, out a) ? a  : 0).ToString()).Sum(x => Convert.ToDecimal(x));
            var secondChaluKharcha = progress.Where(m => m.pujigatKharchaKharakram.Type == "chalu").Where(m => m.Month == "Mangsir" || m.Month == "Poush" || m.Month == "Magh" || m.Month == "Falgun").Select(m => (decimal.TryParse(m.BitiyaPragati, out a) ? a  : 0).ToString()).Sum(x => Convert.ToDecimal(x));
            var thirdChaluQuaterKharcha = progress.Where(m => m.pujigatKharchaKharakram.Type == "chalu").Where(m => m.Month == "Chaitra" || m.Month == "Baishakh" || m.Month == "Jestha" || m.Month == "Asar").Select(m => (decimal.TryParse(m.BitiyaPragati, out a) ? a  : 0).ToString()).Sum(x => Convert.ToDecimal(x));
            var yearlyChaluQuaterKharcha = progress.Where(m => m.pujigatKharchaKharakram.Type == "chalu").Select(m => (decimal.TryParse(m.BitiyaPragati, out a) ? a * 100000 : 0)).Sum(x => Convert.ToDecimal(x));
            var firstQuaterPugigatKharcha = progress.Where(m => m.pujigatKharchaKharakram.Type == "pujigat").Where(m => m.Month == "Shrawan" || m.Month == "Bhadra" || m.Month == "Ashwin" || m.Month == "Kartik").Select(m => (decimal.TryParse(m.BitiyaPragati, out a) ? a: 0).ToString()).Sum(x => Convert.ToDecimal(x));
            var secondPugigatChaluKharcha = progress.Where(m => m.pujigatKharchaKharakram.Type == "pujigat").Where(m => m.Month == "Mangsir" || m.Month == "Poush" || m.Month == "Magh" || m.Month == "Falgun").Select(m => (decimal.TryParse(m.BitiyaPragati, out a) ? a : 0).ToString()).Sum(x => Convert.ToDecimal(x));
            var thirdPugigatQuaterKharcha = progress.Where(m => m.pujigatKharchaKharakram.Type == "pujigat").Where(m => m.Month == "Chaitra" || m.Month == "Baishakh" || m.Month == "Jestha" || m.Month == "Asar").Select(m => (decimal.TryParse(m.BitiyaPragati, out a) ? a  : 0).ToString()).Sum(x => Convert.ToDecimal(x));
            var yearlyPugigatQuaterKharcha = progress.Where(m => m.pujigatKharchaKharakram.Type == "pujigat").Select(m => (decimal.TryParse(m.BitiyaPragati, out a) ? a * 100000 : 0).ToString()).Sum(x => Convert.ToDecimal(x));

            var yearlypragatiVar = 0;
            var yearlychaluPragatiVar = 0;
            var yearlyPugigatPragatiVar = 0;

            var firstpragatiVar = 0;
            var firstchaluPragatiVar = 0;
            var firstPugigatPragatiVar = 0;

            var secondpragatiVar = 0;
            var secondchaluPragatiVar = 0;
            var secondPugigatPragatiVar = 0;

            var thirdpragatiVar = 0;
            var thirdchaluPragatiVar = 0;
            var thirdPugigatPragatiVar = 0;



             var yearlypragatiVitiyaVar = 0;
            var yearlychaluPragatiVitiyaVar = 0;
            var yearlyPugigatPragatiVitiyaVar = 0;

            var firstpragatiVitiyaVar = 0;
            var firstchaluPragatiVitiyaVar = 0;
            var firstPugigatPragatiVitiyaVar = 0;

            var secondpragatiVitiyaVar = 0;
            var secondchaluPragatiVitiyaVar = 0;
            var secondPugigatPragatiVitiyaVar = 0;

            var thirdpragatiVitiyaVar = 0;
            var thirdchaluPragatiVitiyaVar = 0;
            var thirdPugigatPragatiVitiyaVar = 0;

            List<MonthlySummerizedReport> monthly = new List<MonthlySummerizedReport>();
            monthly.Add(new MonthlySummerizedReport() {
                Name = _localizationService.GetResource("Admin.Resource.FirstQuater"),
                TotalBudget = firstQuaterBudget,
                PugigatBudget = firstPugigatQuaterBudget,
                ChaluBudget = firstChaluQuaterBudget,
                TotalBudgetKharcha = firstQuaterKharcha,
                ChaluBudgetKharcha = firstQuaterChaluKharcha,
                PugigatBudgetKharcha = firstQuaterPugigatKharcha,
                PragratiVar = firstpragatiVar,
                PugigatPragratiVar = firstPugigatPragatiVar,
                ChaluPragatiVar=firstchaluPragatiVar,
                
                BharitPragati = firstpragatiVitiyaVar,
                PugigatBharitPragati = firstPugigatPragatiVitiyaVar,
                ChaluBharitPragati = firstchaluPragatiVitiyaVar,

               
                




            });
            monthly.Add(new MonthlySummerizedReport() {
                Name = _localizationService.GetResource("Admin.Resource.SecondQuater"),
                TotalBudget = secondQuaterBudget,
                PugigatBudget = secondPugigatQuaterBudget,
                ChaluBudget = secondChaluQuaterBudget,
                TotalBudgetKharcha = secondKharcha,
                ChaluBudgetKharcha = secondChaluKharcha,
                PugigatBudgetKharcha = secondPugigatChaluKharcha,
                PragratiVar = secondpragatiVar,
                PugigatPragratiVar = secondPugigatPragatiVar,
                ChaluPragatiVar = secondchaluPragatiVar,

                BharitPragati = secondpragatiVitiyaVar,
                PugigatBharitPragati = secondPugigatPragatiVitiyaVar,
                ChaluBharitPragati = secondchaluPragatiVitiyaVar,







            });
            monthly.Add(new MonthlySummerizedReport() {
                Name = _localizationService.GetResource("Admin.Resource.thirdQuater"),
                TotalBudget = thirdQuaterBudget,
                PugigatBudget = thirdPugigatQuaterBudget,
                ChaluBudget = thirdChaluQuaterBudget,
                TotalBudgetKharcha = thirdQuaterKharcha,
                ChaluBudgetKharcha = thirdChaluQuaterKharcha,
                PugigatBudgetKharcha = thirdPugigatQuaterKharcha,
                PragratiVar = thirdpragatiVar,
                PugigatPragratiVar = thirdPugigatPragatiVar,
                ChaluPragatiVar = thirdchaluPragatiVar,

                BharitPragati = thirdpragatiVitiyaVar,
                PugigatBharitPragati = thirdPugigatPragatiVitiyaVar,
                ChaluBharitPragati = thirdchaluPragatiVitiyaVar,







            });
           
            monthly.Add(new MonthlySummerizedReport() {
                Name = _localizationService.GetResource("Admin.Resource.Yearly"),
                TotalBudget = TotalBudget,
                PugigatBudget = TotalPugigatBudget,
                ChaluBudget = TotalChaluBudget,
                TotalBudgetKharcha = yearlyQuaterKharcha,
                ChaluBudgetKharcha = yearlyChaluQuaterKharcha,
                PugigatBudgetKharcha = yearlyPugigatQuaterKharcha,
                PragratiVar = yearlypragatiVar,
                PugigatPragratiVar = yearlyPugigatPragatiVar,
                ChaluPragatiVar = yearlychaluPragatiVar,

                BharitPragati = yearlypragatiVitiyaVar,
                PugigatBharitPragati = yearlyPugigatPragatiVitiyaVar,
                ChaluBharitPragati = yearlychaluPragatiVitiyaVar,







            });

            return View(monthly);
        }
    }
}
