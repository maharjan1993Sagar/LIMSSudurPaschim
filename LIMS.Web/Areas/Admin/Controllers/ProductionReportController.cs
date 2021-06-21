using LIMS.Core;
using LIMS.Domain.StatisticalData;
using LIMS.Services.Ainr;
using LIMS.Services.Basic;
using LIMS.Services.Breed;
using LIMS.Services.Customers;
using LIMS.Services.Localization;
using LIMS.Services.MedicineInventory;
using LIMS.Services.MoAMAC;
using LIMS.Services.Statisticaldata;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Web.Areas.Admin.Models.Reports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class ProductionReportController : BaseAdminController
    {
        private readonly ISpeciesService _speciesService;
        private readonly IBreedService _breedService;
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

        public ProductionReportController(ILocalizationService localizationService,
            IAnimalRegistrationService animalRegistrationService,
            IFarmService farmService,
            ILanguageService languageService,
            ISpeciesService speciesService,
            IBreedService breedService,
            IAnimalTypeService animalTypeService,
            ILivestockService livestockService,
            ICustomerService customerService,
            IFiscalYearService fiscalYearService,
            IProductionionDataService productionionDataService,
            ILssService lssService,
            IWorkContext workContext)
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
        }

        public async Task<IActionResult> Index()
        {
            var fiscalyear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear").ToList();
            fiscalyear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYear = fiscalyear;
            return View();

        }
        public async Task<IActionResult> Report(string fiscalyear)
        {
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
            if (roles.Contains(RoleHelper.LssUser) || roles.Contains(RoleHelper.LssAdmin))
            {

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
                var productions = await _productionionDataService.GetProduction(createdby, fiscalyear);

                var species = await _speciesService.GetSpecies();

                var prod = new ProductionReport();
                var sp = new List<string>();

                foreach (var item in species)
                {
                    sp.Add(item.EnglishName);
                }

                var farms = productions.Where(m => m.Farm != null).Select(m => m.Farm.Id).Distinct().ToList();

                var productionReports = new List<ProductionReport>();
                foreach (var item in farms)
                {
                    var typeNos = new List<int>();
                    var milklist = new List<int>();
                    var meatlist = new List<int>();
                    foreach (var animal in sp)
                    {
                        typeNos.Add(productions.Where(m => m.Farm != null).Where(m => m.Farm.Id == item && m.Species.EnglishName == animal && m.ProductionType.ToLower() == "milk").Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity)));
                        milklist.Add(productions.Where(m => m.Farm != null).Where(m => m.Farm.Id == item && m.Species.EnglishName == animal && m.ProductionType.ToLower() == "milk").Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity)));


                    }
                    typeNos.Add(milklist.Sum());

                    foreach (var animal in sp)
                    {
                        if (animal.ToLower() != "cow")
                        {
                            typeNos.Add(productions.Where(m => m.Farm != null).Where(m => m.Farm.Id == item && m.Species.EnglishName == animal && m.ProductionType.ToLower() == "meat").Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity)));
                            meatlist.Add(productions.Where(m => m.Farm != null).Where(m => m.Farm.Id == item && m.Species.EnglishName == animal && m.ProductionType.ToLower() == "meat").Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity)));
                        }

                    }
                    typeNos.Add(meatlist.Sum());
                    var farm = await _farmService.GetFarmById(item);
                    var year = await _fiscalYearService.GetFiscalYearById(fiscalyear);
                    productionReports.Add(new ProductionReport {
                        FarmName = farm.NameEnglish,
                        Address = farm.District + " " + farm.LocalLevel,
                        Species = sp,
                       
                        Production = typeNos,
                        FiscalYear = year.NepaliFiscalYear

                    }
                        );


                }

                return View(productionReports);
            }
            else
            {
                string vhlsecid = _workContext.CurrentCustomer.EntityId;
                List<string> lssId = _lssService.GetLssByVhlsecId(vhlsecid).Result.Select(m => m.Id).ToList();
                var customers = _customerService.GetCustomerByLssId(lssId, vhlsecid);
                List<string> customerid = customers.Select(x => x.Id).ToList();

                var productions = new List<Production>();
                foreach (var item in customerid)
                {
                    var livestock = await _productionionDataService.GetProduction(createdBy: item, fiscalyear: fiscalyear);
                    var newlivestocks = livestock.ToList();
                    productions.AddRange(newlivestocks);
                }

                var species = await _speciesService.GetSpecies();

                var prod = new ProductionReport();
                var sp = new List<string>();

                foreach (var item in species)
                {
                    sp.Add(item.EnglishName);
                }

                var farms = productions.Where(m => m.Farm != null).Select(m => m.Farm.Id).Distinct().ToList();
                var productionReports = new List<ProductionReport>();
                foreach (var item in farms)
                {
                    var typeNos = new List<int>();
                    var milklist = new List<int>();
                    var meatlist = new List<int>();
                    foreach (var animal in sp)
                    {
                        typeNos.Add(productions.Where(m=>m.Farm!=null).Where(m => m.Farm.Id == item && m.Species.EnglishName == animal && m.ProductionType.ToLower() == "milk").Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity)));
                        milklist.Add(productions.Where(m => m.Farm != null).Where(m => m.Farm.Id == item && m.Species.EnglishName == animal && m.ProductionType.ToLower() == "milk").Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity)));


                    }
                    typeNos.Add(milklist.Sum());

                    foreach (var animal in sp)
                    {
                        if (animal.ToLower() != "cow")
                        {
                            typeNos.Add(productions.Where(m => m.Farm != null).Where(m => m.Farm.Id == item && m.Species.EnglishName == animal && m.ProductionType.ToLower() == "meat").Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity)));
                            meatlist.Add(productions.Where(m => m.Farm != null).Where(m => m.Farm.Id == item && m.Species.EnglishName == animal && m.ProductionType.ToLower() == "meat").Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity)));
                        }

                    }
                    typeNos.Add(meatlist.Sum());
                    var farm = await _farmService.GetFarmById(item);
                    var year = await _fiscalYearService.GetFiscalYearById(fiscalyear);
                    productionReports.Add(new ProductionReport {
                        FarmName = farm.NameEnglish,
                        Address = farm.District + " " + farm.LocalLevel,
                        Species = sp,
                        Production = typeNos,
                        FiscalYear = year.NepaliFiscalYear

                    }
                        );


                }

                return View(productionReports);


            }

        }


        [HttpPost]
        public virtual IActionResult ProductionReportHtml(string FiscalYear)
        {
            var livestockWardWiseReportHtml = RenderViewComponentToString("Production", new { fiscalyear = FiscalYear });

            return Json(new
            {
                success = true,
                message = string.Format(_localizationService.GetResource("Admin.Report.LivestockWardWiseReport.Success")),
                livestockWardWiseReportHtml
            });
        }


    }
}
