using LIMS.Core;
using LIMS.Domain;
using LIMS.Domain.StatisticalData;
using LIMS.Framework.Components;
using LIMS.Services.Basic;
using LIMS.Services.Breed;
using LIMS.Services.Customers;
using LIMS.Services.MoAMAC;
using LIMS.Services.Statisticaldata;
using LIMS.Web.Areas.Admin.Models.Reports;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static LIMS.Web.Areas.Admin.Models.Reports.LivestockReport;

namespace LIMS.Web.Areas.Admin.Components
{
    public class LivestockDolfdReportNepali : BaseViewComponent
    {
        private readonly IWorkContext _workContext;
        private readonly ICustomerService _customerService;
        private readonly ILivestockService _livestockService;
        private readonly ILivestockSpeciesService _speciesService;
        private readonly IFiscalYearService _fiscalYearService;
        private readonly IDolfdService _lssService;
        private readonly IVhlsecService _vhlsecService;
        private readonly IMoAMACService _moAMACService;
        private readonly IAnimalTypeService _animalTypeService;
        public LivestockDolfdReportNepali(IWorkContext workContext, ICustomerService customerService,
            ILivestockService livestockService, ILivestockSpeciesService speciesService,
            IAnimalTypeService animalTypeService, IFiscalYearService fiscalYearService,
            IDolfdService lssService,
            IVhlsecService vhlsecService,
            IMoAMACService moAMACService
            )
        {
            _workContext = workContext;
            _customerService = customerService;
            _livestockService = livestockService;
            _speciesService = speciesService;
            _animalTypeService = animalTypeService;
            _fiscalYearService = fiscalYearService;
            _lssService = lssService;
            _vhlsecService = vhlsecService;
            _moAMACService = moAMACService;
        }


        public async Task<IViewComponentResult> InvokeAsync(string fiscalYear, string lssid = null)
        {
            var role = _workContext.CurrentCustomer.CustomerRoles.Select(m=>m.Name);

            if (role.Contains("VhlsecAdmin"))
            {
                string entityId = _workContext.CurrentCustomer.EntityId;
              
                var userids = _customerService.GetCustomerByLssId(null, entityId).Select(m => m.Id).ToList();
               


                //var livestocks = new List<Livestock>();
                var livestocks = await _livestockService.GetLivestock(userids, fiscalyear: fiscalYear);

                //foreach (var item in userids)
                //{
                //    var livestock = await _livestockService.GetLivestock(createdby: item, fiscalyear: fiscalYear);
                //    if (livestock.TotalCount > 0)
                //    {
                //        livestocks.AddRange(livestock);
                //    }

                //}

                var species = await _speciesService.GetBreed();
                List<Models.Reports.LivestockReport> report = new List<Models.Reports.LivestockReport>();

                foreach (var animal in species)
                {
                    var livestockReport = new Models.Reports.LivestockReport();
                    var livestock = livestocks.Where(m => m.Species.EnglishName == animal.EnglishName);

                    var headingTypes = livestock.Select(m => m.AnimalType.NepaliName).Distinct();
                    var headingTypes1 = livestock.Select(m => m.AnimalType.Name).Distinct();
                    List<string> heading = headingTypes1.ToList();


                    livestockReport.Rows = new List<BaseLivestockReportModel>();
                    livestockReport.AnimalTypes = headingTypes.ToList();
                    livestockReport.AnimalTypes.Add("जम्मा");
                    var Types = new List<string> { "Native", "Improved" };
                    if (heading.Count() > 0)
                    {
                        foreach (var breed in Types)
                        {
                            BaseLivestockReportModel row = new BaseLivestockReportModel();
                            if (breed == "Native")
                            {
                                row.BreedType = "स्थानिय";
                            }
                            if (breed == "Crossbred")
                            {
                                row.BreedType = "बर्णशंकर";
                            }
                            if (breed == "Improved")
                            {
                                row.BreedType = "उन्नत";
                            }
                            if (breed == "Pure exotic breed")
                            {
                                row.BreedType = "उन्नत";
                            }
                            if (breed == "Broiler commercial")
                            {
                                row.BreedType = "कमर्र्सिएल व्रोईलर्सब";
                            }
                            if (breed == "Layer commercial")
                            {
                                row.BreedType = "कर्मसियल लेयर्स";
                            }
                            var typeNos = new List<int>();
                            foreach (var item in heading)
                            {
                                if (breed == "Native")
                                {
                                    typeNos.Add(livestock.Where(m => m.AnimalType.Name == item).Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Local) ? "0" : m.Local)));
                                }
                                else
                                {
                                    typeNos.Add(livestock.Where(m => m.AnimalType.Name == item).Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Improved) ? "0" : m.Improved)));


                                }
                                if (item == heading.LastOrDefault())
                                {
                                    typeNos.Add(typeNos.Sum(m => m));
                                }
                            }
                            row.AnimalTypeNo = typeNos;
                            livestockReport.Rows.Add(row);
                        }
                    }
                    livestockReport.Species = animal.NepaliName;
                    var fiscal = await _fiscalYearService.GetFiscalYearById(fiscalYear);

                    livestockReport.FiscalYear = fiscal.NepaliFiscalYear;
                    livestockReport.Month = _vhlsecService.GetVhlsecById(entityId).Result.NameNepali;

                    report.Add(livestockReport);

                }

                return View(report);

            }
            else
            {
                string entityId = _workContext.CurrentCustomer.EntityId;
               
                List<string> vhlsecId = _vhlsecService.GetVhlsecByDolfdId(entityId).Result.Select(m => m.Id).ToList();

                var userids = new List<string>();
                if (!string.IsNullOrEmpty(lssid))
                {
                    userids = _customerService.GetCustomerByLssId(null, lssid).Select(m => m.Id).ToList();
                }
                else
                {
                    userids = _customerService.GetCustomerByLssId(vhlsecId, entityId).Select(m => m.Id).ToList();

                }
                var livestocks = new List<Livestock>();

                //var livestocks = new List<Livestock>();
                if (role.Contains("MolmacAdmin"))
                {
                  var l= await _livestockService.GetFilteredLivestock("molmac","", fiscalYearId: fiscalYear,"","");
                    livestocks = l.ToList();
                }
                else
                {
                     var s = await _livestockService.GetLivestock(userids, fiscalyear: fiscalYear);
                    livestocks = s.ToList();
                    
                }

                //foreach (var item in userids)
                //{
                //    var livestock = await _livestockService.GetLivestock(createdby: item, fiscalyear: fiscalYear);
                //    if (livestock.TotalCount > 0)
                //    {
                //        livestocks.AddRange(livestock);
                //    }

                //}

                var species = await _speciesService.GetBreed();
                List<Models.Reports.LivestockReport> report = new List<Models.Reports.LivestockReport>();

                foreach (var animal in species)
                {
                    var livestockReport = new Models.Reports.LivestockReport();
                    var livestock = livestocks.Where(m => m.Species.EnglishName == animal.EnglishName);

                    var headingTypes = livestock.Select(m => m.AnimalType.NepaliName).Distinct();
                    var headingTypes1 = livestock.Select(m => m.AnimalType.Name).Distinct();
                    List<string> heading = headingTypes1.ToList();


                    livestockReport.Rows = new List<BaseLivestockReportModel>();
                    livestockReport.AnimalTypes = headingTypes.ToList();
                    livestockReport.AnimalTypes.Add("जम्मा");
                    var Types = new List<string> { "Native", "Improved" };
                    if (heading.Count() > 0)
                    {
                        foreach (var breed in Types)
                        {
                            BaseLivestockReportModel row = new BaseLivestockReportModel();
                            if (breed == "Native")
                            {
                                row.BreedType = "स्थानिय";
                            }
                            if (breed == "Crossbred")
                            {
                                row.BreedType = "बर्णशंकर";
                            }
                            if (breed == "Improved")
                            {
                                row.BreedType = "उन्नत";
                            }
                            if (breed == "Pure exotic breed")
                            {
                                row.BreedType = "उन्नत";
                            }
                            if (breed == "Broiler commercial")
                            {
                                row.BreedType = "कमर्र्सिएल व्रोईलर्सब";
                            }
                            if (breed == "Layer commercial")
                            {
                                row.BreedType = "कर्मसियल लेयर्स";
                            }
                            var typeNos = new List<int>();
                            foreach (var item in heading)
                            {
                                if (breed == "Native")
                                {
                                    typeNos.Add(livestock.Where(m => m.AnimalType.Name == item).Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Local) ? "0" : m.Local)));
                                }
                                else
                                {
                                    typeNos.Add(livestock.Where(m => m.AnimalType.Name == item).Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Improved) ? "0" : m.Improved)));


                                }
                                if (item == heading.LastOrDefault())
                                {
                                    typeNos.Add(typeNos.Sum(m => m));
                                }
                            }
                            row.AnimalTypeNo = typeNos;
                            livestockReport.Rows.Add(row);
                        }
                    }
                    livestockReport.Species = animal.NepaliName;
                    var fiscal = await _fiscalYearService.GetFiscalYearById(fiscalYear);
                    livestockReport.FiscalYear = fiscal.NepaliFiscalYear;
                    if (role.Contains("DolfdAdmin"))
                    {
                        livestockReport.Month = _lssService.GetDolfdById(entityId).Result.NameNepali;

                    }
                    else
                    {
                        livestockReport.Month = _moAMACService.GetMoAMACById(entityId).Result.NameNepali;

                    }
                    report.Add(livestockReport);

                }
               
                return View(report);
            }

        }
    }

}
