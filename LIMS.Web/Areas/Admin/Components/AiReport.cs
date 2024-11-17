using LIMS.Core;
using LIMS.Framework.Components;
using LIMS.Services.Ainr;
using LIMS.Services.Basic;
using LIMS.Services.Breed;
using LIMS.Services.Customers;
using LIMS.Services.Localization;
using LIMS.Services.MoAMAC;
using LIMS.Services.Statisticaldata;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Web.Areas.Admin.Models.Reports;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Components
{
    public class AiReportViewComponent : BaseViewComponent
    {

        private readonly ILivestockSpeciesService _speciesService;
        private readonly ILivestockBreedService _breedService;
        private readonly IAnimalTypeService _animalTypeService;
        private readonly ILivestockService _livestockService;
        private readonly IFarmService _farmService;
        private readonly ILocalizationService _localizationService;
        private readonly ILanguageService _languageService;
        private readonly IWorkContext _workContext;
        public readonly ICustomerService _customerService;
        public readonly IFiscalYearService _fiscalYearService;
        public readonly IProductionionDataService _productionionDataService;
        public readonly ILssService _lssService;
        public readonly IServiceData _serviceData;
        public readonly IVhlsecService _vhlsecService;


        public AiReportViewComponent(ILocalizationService localizationService,
            IAnimalRegistrationService animalRegistrationService,
            IFarmService farmService,
            ILanguageService languageService,
            ILivestockSpeciesService speciesService,
            ILivestockBreedService breedService,
            IAnimalTypeService animalTypeService,
            ILivestockService livestockService,
            ICustomerService customerService,
            IFiscalYearService fiscalYearService,
            IProductionionDataService productionionDataService,
            ILssService lssService,
            IWorkContext workContext,
            IServiceData serviceData,
            IVhlsecService vhlsecService

            )
        {
            _localizationService = localizationService;

            _languageService = languageService;
            _farmService = farmService;
            _speciesService = speciesService;
            _breedService = breedService;
            _workContext = workContext;
            _animalTypeService = animalTypeService;
            _livestockService = livestockService;
            _customerService = customerService;
            _fiscalYearService = fiscalYearService;
            _productionionDataService = productionionDataService;
            _lssService = lssService;
            _serviceData = serviceData;
            _vhlsecService = vhlsecService;
        }
        public async Task<IViewComponentResult> InvokeAsync(string fiscalYear)
        {
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
            //if (roles.Contains(RoleHelper.VhlsecAdmin) || roles.Contains(RoleHelper.VhlsecUser))

            //{
                var months = new MonthHelper();
                var month = months.GetMonths();
                var allmonth = month.Select(m => m.Value).ToList();
                //string vhlsecid = _workContext.CurrentCustomer.EntityId;
                //List<string> lssId = _lssService.GetLssByVhlsecId(vhlsecid).Result.Select(m => m.Id).ToList();
                //var customers = _customerService.GetCustomerByLssId(lssId, vhlsecid);
                //List<string> customerid = customers.Select(x => x.Id).ToList();
                var aidata = await _serviceData.GetService("", "ai", fiscalYear);
                List<string> municipility = aidata.Where(m=>m.LocalLevel!=null).Select(m => m.LocalLevel).Distinct().ToList();
                municipility = municipility.Where(m => m == ExecutionHelper.LocalLevel).ToList();
                var report = new List<AiReportMonth>();
                foreach (var item in municipility)
                {
                    var cow = new List<int>();
                    var goat = new List<int>();

                    var pig = new List<int>();

                    var buffalo = new List<int>();
                    var total = new List<int>();

                    foreach (var items in allmonth)
                    {
                        cow.Add(aidata.Where(m => m.LocalLevel == item && m.Month == items && m.Species.EnglishName.ToLower() == "cow").Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Native) ? "0" : m.Native) + Convert.ToInt32(String.IsNullOrEmpty(m.CrossBreed) ? "0" : m.CrossBreed) + Convert.ToInt32(String.IsNullOrEmpty(m.PureExotic) ? "0" : m.PureExotic)));

                        goat.Add(aidata.Where(m => m.Species.EnglishName.ToLower() == "goat" && m.LocalLevel == item && m.Month == items).Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Native) ? "0" : m.Native) + Convert.ToInt32(String.IsNullOrEmpty(m.CrossBreed) ? "0" : m.CrossBreed) + Convert.ToInt32(String.IsNullOrEmpty(m.PureExotic) ? "0" : m.PureExotic)));
                        pig.Add(aidata.Where(m => m.Species.EnglishName.ToLower() == "pig" && m.LocalLevel == item && m.Month == items).Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Native) ? "0" : m.Native) + Convert.ToInt32(String.IsNullOrEmpty(m.CrossBreed) ? "0" : m.CrossBreed) + Convert.ToInt32(String.IsNullOrEmpty(m.PureExotic) ? "0" : m.PureExotic)));
                        buffalo.Add(aidata.Where(m => m.Species.EnglishName.ToLower() == "buffalo" && m.LocalLevel == item && m.Month == items).Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Native) ? "0" : m.Native) + Convert.ToInt32(String.IsNullOrEmpty(m.CrossBreed) ? "0" : m.CrossBreed) + Convert.ToInt32(String.IsNullOrEmpty(m.PureExotic) ? "0" : m.PureExotic)));
                        total.Add(aidata.Where(m => m.Month == items && m.LocalLevel == item && (m.Species.EnglishName.ToLower() == "buffalo" || m.Species.EnglishName.ToLower() == "cow" || m.Species.EnglishName.ToLower() == "goat")).Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Native) ? "0" : m.Native) + Convert.ToInt32(String.IsNullOrEmpty(m.CrossBreed) ? "0" : m.CrossBreed) + Convert.ToInt32(String.IsNullOrEmpty(m.PureExotic) ? "0" : m.PureExotic)));

                    }
                    cow.Add(cow.Sum());
                    goat.Add(goat.Sum());
                    buffalo.Add(buffalo.Sum());
                    total.Add(total.Sum());
                    report.Add(new AiReportMonth {
                        Cow = cow,

                        Goat = goat,
                        Buffalo = buffalo,
                        Pig = pig,
                        Total = total,
                        Month = allmonth,
                        Municipility = item,
                        FiscalYear = _fiscalYearService.GetFiscalYearById(fiscalYear).Result.NepaliFiscalYear,
                    });

                }

                return View(report);


            //}
            //if (roles.Contains(RoleHelper.DolfdAdmin) || roles.Contains(RoleHelper.DolfdUser))

            //{
            //    var months = new MonthHelper();
            //    var month = months.GetMonths();
            //    var allmonth = month.Select(m => m.Value).ToList();
            //    string entityId = _workContext.CurrentCustomer.EntityId;
            //    List<string> vhlsecId = _vhlsecService.GetVhlsecByDolfdId(entityId).Result.Select(m => m.Id).ToList();
            //    var LssIds = new List<string>();
            //    foreach (var item in vhlsecId)
            //    {
            //        LssIds.AddRange(_lssService.GetLssByVhlsecId(item).Result.Select(m => m.Id).ToList());
            //    }
            //    var customer = _customerService.GetCustomerByLssId(LssIds, vhlsecId, entityId);
            //    var customers = customer.Select(m => m.Id).ToList();
            //    var aidata = await _serviceData.GetService(customers, "ai", fiscalYear);
            //    List<string> municipility = aidata.Where(m => m.LocalLevel != null).Select(m => m.LocalLevel).Distinct().ToList();
            //    var report = new List<AiReportMonth>();
            //    foreach (var item in municipility)
            //    {
            //        var cow = new List<int>();
            //        var goat = new List<int>();

            //        var pig = new List<int>();

            //        var buffalo = new List<int>();
            //        var total = new List<int>();

            //        foreach (var items in allmonth)
            //        {
            //            cow.Add(aidata.Where(m => m.LocalLevel == item && m.Month == items && m.Species.EnglishName.ToLower() == "cow").Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Native) ? "0" : m.Native) + Convert.ToInt32(String.IsNullOrEmpty(m.CrossBreed) ? "0" : m.CrossBreed) + Convert.ToInt32(String.IsNullOrEmpty(m.CrossBreed) ? "0" : m.CrossBreed)));

            //            goat.Add(aidata.Where(m => m.Species.EnglishName.ToLower() == "goat" && m.LocalLevel == item && m.Month == items).Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Native) ? "0" : m.Native) + Convert.ToInt32(String.IsNullOrEmpty(m.CrossBreed) ? "0" : m.CrossBreed) + Convert.ToInt32(String.IsNullOrEmpty(m.CrossBreed) ? "0" : m.PureExotic)));
            //            pig.Add(aidata.Where(m => m.Species.EnglishName.ToLower() == "pig" && m.LocalLevel == item && m.Month == items).Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Native) ? "0" : m.Native) + Convert.ToInt32(String.IsNullOrEmpty(m.CrossBreed) ? "0" : m.CrossBreed) + Convert.ToInt32(String.IsNullOrEmpty(m.CrossBreed) ? "0" : m.PureExotic)));
            //            buffalo.Add(aidata.Where(m => m.Species.EnglishName.ToLower() == "buffalo" && m.LocalLevel == item && m.Month == items).Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Native) ? "0" : m.Native) + Convert.ToInt32(String.IsNullOrEmpty(m.CrossBreed) ? "0" : m.CrossBreed) + Convert.ToInt32(String.IsNullOrEmpty(m.CrossBreed) ? "0" : m.PureExotic)));
            //            total.Add(aidata.Where(m => m.Month == items && m.LocalLevel == item && (m.Species.EnglishName.ToLower() == "buffalo" || m.Species.EnglishName.ToLower() == "cow" || m.Species.EnglishName.ToLower() == "goat")).Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Native) ? "0" : m.Native) + Convert.ToInt32(String.IsNullOrEmpty(m.CrossBreed) ? "0" : m.CrossBreed) + Convert.ToInt32(String.IsNullOrEmpty(m.PureExotic) ? "0" : m.PureExotic)));

            //        }
            //        cow.Add(cow.Sum());
            //        goat.Add(goat.Sum());
            //        buffalo.Add(buffalo.Sum());
            //        total.Add(total.Sum());
            //        report.Add(new AiReportMonth {
            //            Cow = cow,

            //            Goat = goat,
            //            Buffalo = buffalo,
            //            Pig = pig,
            //            Total = total,
            //            Month = allmonth,
            //            Municipility = item,
            //            FiscalYear = _fiscalYearService.GetFiscalYearById(fiscalYear).Result.NepaliFiscalYear,
            //        });

            //    }

            //    return View(report);


            //}
            //if (roles.Contains(RoleHelper.LssAdmin) || roles.Contains(RoleHelper.LssUser))

            //{
            //    var months = new MonthHelper();
            //    var month = months.GetMonths();
            //    var allmonth = month.Select(m => m.Value).ToList();
            //    string vhlsecid = _workContext.CurrentCustomer.EntityId;
            // //   List<string> lssId = _lssService.GetLssByVhlsecId(vhlsecid).Result.Select(m => m.Id).ToList();
            //    var customers = _customerService.GetCustomerByLssId(null, vhlsecid);
            //    List<string> customerid = customers.Select(x => x.Id).ToList();
            //    var aidata = await _serviceData.GetService(customerid, "ai", fiscalYear);
            //    List<string> municipility = aidata.Where(m => m.LocalLevel != null).Select(m => m.LocalLevel).Distinct().ToList();
            //    var report = new List<AiReportMonth>();
            //    foreach (var item in municipility)
            //    {
            //        var cow = new List<int>();
            //        var goat = new List<int>();

            //        var pig = new List<int>();

            //        var buffalo = new List<int>();
            //        var total = new List<int>();

            //        foreach (var items in allmonth)
            //        {
            //            cow.Add(aidata.Where(m => m.LocalLevel == item && m.Month == items && m.Species.EnglishName.ToLower() == "cow").Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Native) ? "0" : m.Native) + Convert.ToInt32(String.IsNullOrEmpty(m.CrossBreed) ? "0" : m.CrossBreed) + Convert.ToInt32(String.IsNullOrEmpty(m.PureExotic) ? "0" : m.PureExotic)));

            //            goat.Add(aidata.Where(m => m.Species.EnglishName.ToLower() == "goat" && m.LocalLevel == item && m.Month == items).Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Native) ? "0" : m.Native) + Convert.ToInt32(String.IsNullOrEmpty(m.CrossBreed) ? "0" : m.CrossBreed) + Convert.ToInt32(String.IsNullOrEmpty(m.PureExotic) ? "0" : m.PureExotic)));
            //            pig.Add(aidata.Where(m => m.Species.EnglishName.ToLower() == "pig" && m.LocalLevel == item && m.Month == items).Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Native) ? "0" : m.Native) + Convert.ToInt32(String.IsNullOrEmpty(m.CrossBreed) ? "0" : m.CrossBreed) + Convert.ToInt32(String.IsNullOrEmpty(m.PureExotic) ? "0" : m.PureExotic)));
            //            buffalo.Add(aidata.Where(m => m.Species.EnglishName.ToLower() == "buffalo" && m.LocalLevel == item && m.Month == items).Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Native) ? "0" : m.Native) + Convert.ToInt32(String.IsNullOrEmpty(m.CrossBreed) ? "0" : m.CrossBreed) + Convert.ToInt32(String.IsNullOrEmpty(m.PureExotic) ? "0" : m.PureExotic)));
            //            total.Add(aidata.Where(m => m.Month == items && m.LocalLevel == item && (m.Species.EnglishName.ToLower() == "buffalo" || m.Species.EnglishName.ToLower() == "cow" || m.Species.EnglishName.ToLower() == "goat")).Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Native) ? "0" : m.Native) + Convert.ToInt32(String.IsNullOrEmpty(m.PureExotic) ? "0" : m.PureExotic) + Convert.ToInt32(String.IsNullOrEmpty(m.CrossBreed) ? "0" : m.CrossBreed)));

            //        }
            //        cow.Add(cow.Sum());
            //        goat.Add(goat.Sum());
            //        buffalo.Add(buffalo.Sum());
            //        total.Add(total.Sum()); 
            //        report.Add(new AiReportMonth {
            //            Cow = cow,

            //            Goat = goat,
            //            Buffalo = buffalo,
            //            Pig = pig,
            //            Total=total,
            //            Month = allmonth,
            //            Municipility = item,
            //            FiscalYear = _fiscalYearService.GetFiscalYearById(fiscalYear).Result.NepaliFiscalYear,
            //        });

            //    }

            //    return View(report);


            //}

            return View();

        }

    }
}
