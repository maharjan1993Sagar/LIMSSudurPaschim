using Google.Apis.AnalyticsReporting.v4.Data;
using LIMS.Core;
using LIMS.Domain.MoAMAC;
using LIMS.Domain.StatisticalData;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Ainr;
using LIMS.Services.Basic;
using LIMS.Services.Breed;
using LIMS.Services.Customers;
using LIMS.Services.Localization;
using LIMS.Services.MedicineInventory;
using LIMS.Services.MoAMAC;
using LIMS.Services.Security;
using LIMS.Services.Statisticaldata;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Web.Areas.Admin.Models;
using LIMS.Web.Areas.Admin.Models.Reports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static LIMS.Web.Areas.Admin.Models.Reports.LivestockReport;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class ReportController : BaseAdminController
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
        private readonly IReceivedMedicineService _receivedMedicineService;
        private readonly IMedicineProgressService _medicineProgressService;
        public readonly IFiscalYearService _fiscalYearService;
        public readonly ILssService _lssService;
        public readonly IVhlsecService _vhlsecService;

        public ReportController(ILocalizationService localizationService,
            IAnimalRegistrationService animalRegistrationService,
            IFarmService farmService,
            ILanguageService languageService,
            ISpeciesService speciesService,
            IBreedService breedService,
            IAnimalTypeService animalTypeService,
            ILivestockService livestockService,
            ICustomerService customerService,
            IReceivedMedicineService receivedMedicineService,
            IMedicineProgressService medicineProgressService,
            IFiscalYearService fiscalYearService,
            ILssService lssService,
            IWorkContext workContext,
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
            _receivedMedicineService = receivedMedicineService;
            _medicineProgressService = medicineProgressService;
            _fiscalYearService = fiscalYearService;
            _lssService = lssService;
            _vhlsecService = vhlsecService;
        }



        public async Task<IActionResult> LivestockReport()
        {
            var fiscalyear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear").ToList();
            fiscalyear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYear = fiscalyear;
            return View();


        }

        public async Task<IActionResult> LivestockReportDolfd()
        {
            var fiscalyear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear").ToList();
            fiscalyear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYear = fiscalyear;
            string entityId = _workContext.CurrentCustomer.EntityId;
            List<string> vhlsecId = _vhlsecService.GetVhlsecByDolfdId(entityId).Result.Select(m => m.Id).ToList();
            var LssIds = new List<Lss>();
            foreach (var item in vhlsecId)
            {
                LssIds.AddRange(await _lssService.GetLssByVhlsecId(item));
            }
            var lss = new SelectList(LssIds, "Id", "NameEnglish").ToList();
            lss.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.LocalLevel = lss;
            return View();


        }
        [HttpPost]
        public virtual IActionResult LivestockDolfdReportHtml(string FiscalYear,string LocalLevel=null)
        {
            var livestockWardWiseReportHtml = RenderViewComponentToString("LivestockDolfdReport", new { fiscalyear = FiscalYear,lssid=LocalLevel });

            return Json(new
            {
                success = true,
                message = string.Format(_localizationService.GetResource("Admin.Report.LivestockWardWiseReport.Success")),
                livestockWardWiseReportHtml
            });
        }

        public async Task<IActionResult> LivestockReportForFiscalYear(LivestockReport model)
        {
            string FiscalYear = model.FiscalYear;
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
            string createdby = null;
            if (roles.Contains(RoleHelper.VhlsecAdmin))
            {
                createdby = _workContext.CurrentCustomer.Id;
            }
            else
            {
                string adminemail = _workContext.CurrentCustomer.CreatedBy;
                var admin = await _customerService.GetCustomerByEmail(adminemail);
                createdby = admin.Id;
            }


            string vhlsecid = _workContext.CurrentCustomer.EntityId;
            List<string> lssId = _lssService.GetLssByVhlsecId(vhlsecid).Result.Select(m => m.Id).ToList();
            var customers = _customerService.GetCustomerByLssId(lssId, vhlsecid);
            List<string> customerid = customers.Select(x => x.Id).ToList();

            var livestocks = new List<Livestock>();
            foreach (var item in customerid)
            {
                var livestock = await _livestockService.GetLivestock(createdby: item, fiscalyear: FiscalYear);
                var newlivestocks = livestock.ToList();
                livestocks.AddRange(newlivestocks);
            }

            var species = await _speciesService.GetSpecies();
            List<LivestockReport> report = new List<LivestockReport>();

            foreach (var animal in species)
            {
                var livestockReport = new LivestockReport();
                var livestock = livestocks.Where(m => m.Species.EnglishName == animal.EnglishName);
                var Types = livestock.Select(m => m.BreedType).Distinct();
                var headingTypes = livestock.Select(m => m.AnimalType.Name).Distinct();


                livestockReport.Rows = new List<BaseLivestockReportModel>();
                livestockReport.AnimalTypes = headingTypes.ToList();
                livestockReport.AnimalTypes.Add("Total");

                foreach (var breed in Types)
                {
                    BaseLivestockReportModel row = new BaseLivestockReportModel();
                    row.BreedType = breed;
                    var typeNos = new List<int>();
                    foreach (var item in headingTypes)
                    {
                        typeNos.Add(livestock.Where(m => m.AnimalType.Name == item && m.BreedType == breed).Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.NoOfLivestock) ? "0" : m.NoOfLivestock)));
                        if (item == headingTypes.LastOrDefault())
                        {
                            typeNos.Add(typeNos.Sum(m => m));
                        }
                    }
                    row.AnimalTypeNo = typeNos;
                    livestockReport.Rows.Add(row);
                }
                livestockReport.Species = animal.EnglishName;
                var fiscalYear = await _fiscalYearService.GetFiscalYearById(FiscalYear);
                livestockReport.FiscalYear = fiscalYear.NepaliFiscalYear;
                report.Add(livestockReport);

            }

            return View(report);
        }

        public async Task<IActionResult> LivestockReportWard()
        {
            var fiscalyear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear").ToList();
            fiscalyear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYear = fiscalyear;
            return View();
        }

        [HttpPost]
        public virtual IActionResult LivestockWardWiseReportHtml(string FiscalYear)
        {
            var livestockWardWiseReportHtml = RenderViewComponentToString("LiveStockWardWiseReport", new { fiscalyear = FiscalYear });

            return Json(new
            {
                success = true,
                message = string.Format(_localizationService.GetResource("Admin.Report.LivestockWardWiseReport.Success")),
                livestockWardWiseReportHtml
            });
        }

        [HttpPost]
        public virtual IActionResult LivestockFiscalYearReportHtml(string FiscalYear)
        {
            var livestockWardWiseReportHtml = RenderViewComponentToString("LivestockReportByFiscalYear", new { fiscalyear = FiscalYear });

            return Json(new
            {
                success = true,
                message = string.Format(_localizationService.GetResource("Admin.Report.LivestockWardWiseReport.Success")),
                livestockWardWiseReportHtml
            });
        }


        public async Task<IActionResult> LivestockWardWiseReport(string fiscalyear)
        {
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();

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
            var livestocks = await _livestockService.GetLivestock(createdby: createdby, fiscalyear: fiscalyear);

            var species = await _speciesService.GetSpecies();

            List<LivestockwardWiseReport> report = new List<LivestockwardWiseReport>();
            var wards = livestocks.Select(m => m.Ward).Distinct().OrderByDescending(m => m).ToList();
            var specieswithanimal = new List<SpeciesWithAnimalTypeModel>();

            foreach (var animal in species)
            {
                var animalTypes = await _animalTypeService.GetAnimalTypeBySpeciesId(animal.Id);

                specieswithanimal.Add(new SpeciesWithAnimalTypeModel {
                    SpeciesName = animal.EnglishName,
                    AnimalTypes = animalTypes.Select(m => m.Name).ToList()
                });
            }
            foreach (var specie in species)
            {
                foreach (var ward in wards)
                {
                    var livestockReport = new LivestockwardWiseReport();
                    BaseLivestockWardWiseReport row = new BaseLivestockWardWiseReport();
                    row.ward = ward;
                    var typeNos = new List<int>();

                    livestockReport.Rows = new List<BaseLivestockWardWiseReport>();


                    foreach (var item in specieswithanimal.Where(m=>m.SpeciesName==specie.EnglishName))
                    {
                        var livestock = livestocks.Where(m => m.Species.EnglishName == item.SpeciesName);
                        int i = 0;
                        while (i < 3)
                        {
                            if (i == 0)
                            {
                                if (item.AnimalTypes.Count() != 0)
                                {
                                    foreach (var types in item.AnimalTypes)
                                    {
                                        typeNos.Add(livestock.Where(m => m.AnimalType.Name == types && m.BreedType == AnimalType.Native && m.Ward == ward).Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.NoOfLivestock) ? "0" : m.NoOfLivestock)));
                                       
                                    }
                                }
                                else
                                {
                                    typeNos.Add(0);
                                }
                            }
                            if (i == 1)
                            {
                                if (item.AnimalTypes.Count() != 0)
                                {
                                    foreach (var types in item.AnimalTypes)
                                    {
                                        typeNos.Add(livestock.Where(m => m.AnimalType.Name == types && m.BreedType == AnimalType.Crossbred && m.Ward == ward).Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.NoOfLivestock) ? "0" : m.NoOfLivestock)));
                                        //if (item == headingTypes.LastOrDefault())
                                        //{
                                        //    typeNos.Add(typeNos.Sum(m => m));
                                        //}
                                    }
                                }
                                else
                                {
                                    typeNos.Add(0);
                                }

                            }
                            if (i == 2)
                            {
                                if (item.AnimalTypes.Count() != 0)
                                {
                                    foreach (var types in item.AnimalTypes)
                                    {
                                        typeNos.Add(livestock.Where(m => m.AnimalType.Name == types && m.BreedType == AnimalType.Crossbred && m.Ward == ward).Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.NoOfLivestock) ? "0" : m.NoOfLivestock)));
                                        //if (item == headingTypes.LastOrDefault())
                                        //{
                                        //    typeNos.Add(typeNos.Sum(m => m));
                                        //}
                                    }
                                }
                                else
                                {
                                    typeNos.Add(0);
                                }
                            }
                            i++;
                        }
                    }

                    row.AnimalTypeNo = typeNos;
                    livestockReport.Rows.Add(row);
                    report.Add(livestockReport);
                }
            }
            var year = await _fiscalYearService.GetFiscalYearById(fiscalyear);

            var livestockReportByWard = new LivestockReportByWard {
                LivestockwardWiseReports = report,
                Wards = wards,
                Species = specieswithanimal,
                FiscalYear = year.NepaliFiscalYear
            };

            return View(livestockReportByWard);
        }



        public async Task<IActionResult> MedicineReport(string fiscalyear, string month)
        {
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();


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

            var receivedMedicine = await _medicineProgressService.GetMedicineProgress(createdby, month, fiscalyear);
            var deleveredMedicine = await _receivedMedicineService.GetReceivedMedicine(createdby, fiscalyear, month);
            var medicine = new MedicineReportModel();
            medicine.FiscalYear = fiscalyear;
            medicine.Month = month;





            return View();
        }
        public IActionResult Report3()
        {
            return View();
        }
        public IActionResult Report4()
        {
            return View();
        }

    }
}
