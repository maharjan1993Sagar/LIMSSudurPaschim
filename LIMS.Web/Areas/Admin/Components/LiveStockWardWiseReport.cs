using LIMS.Core;
using LIMS.Framework.Components;
using LIMS.Services.Basic;
using LIMS.Services.Breed;
using LIMS.Services.Customers;
using LIMS.Services.Statisticaldata;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Web.Areas.Admin.Models.Reports;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static LIMS.Web.Areas.Admin.Models.Reports.LivestockReport;

namespace LIMS.Web.Areas.Admin.Components
{
    public class LiveStockWardWiseReportViewComponent : BaseAdminViewComponent
    {
        private readonly IWorkContext _workContext;
        private readonly ICustomerService _customerService;
        private readonly ILivestockService _livestockService;
        private readonly ISpeciesService _speciesService;
        private readonly IAnimalTypeService _animalTypeService;
        private readonly IFiscalYearService _fiscalYearService;

        public LiveStockWardWiseReportViewComponent(IWorkContext workContext, ICustomerService customerService, 
            ILivestockService livestockService, ISpeciesService speciesService,
            IAnimalTypeService animalTypeService, IFiscalYearService fiscalYearService)
        {
            _workContext = workContext;
            _customerService = customerService;
            _livestockService = livestockService;
            _speciesService = speciesService;
            _animalTypeService = animalTypeService;
            _fiscalYearService = fiscalYearService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string fiscalYear)
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
            var livestocks = await _livestockService.GetLivestock(createdby: createdby, fiscalyear: fiscalYear);

            var species = await _speciesService.GetSpecies();

            List<LivestockwardWiseReport> report = new List<LivestockwardWiseReport>();
            var wards = livestocks.Select(m => m.Ward).Distinct().OrderBy(m => m).ToList();
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


                    foreach (var item in specieswithanimal.Where(m => m.SpeciesName == specie.EnglishName))
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
            var year = await _fiscalYearService.GetFiscalYearById(fiscalYear);

            var livestockReportByWard = new LivestockReportByWard {
                LivestockwardWiseReports = report,
                Wards = wards,
                Species = specieswithanimal,
                FiscalYear = year.NepaliFiscalYear
            };

            return View(livestockReportByWard);
        }
    }
}
