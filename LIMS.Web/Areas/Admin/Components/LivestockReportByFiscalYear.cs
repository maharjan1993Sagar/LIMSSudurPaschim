using LIMS.Core;
using LIMS.Domain.StatisticalData;
using LIMS.Framework.Components;
using LIMS.Services.Basic;
using LIMS.Services.Breed;
using LIMS.Services.Customers;
using LIMS.Services.MoAMAC;
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
    public class LivestockReportByFiscalYearViewComponent:BaseViewComponent
    {
        private readonly IWorkContext _workContext;
        private readonly ICustomerService _customerService;
        private readonly ILivestockService _livestockService;
        private readonly ISpeciesService _speciesService;
        private readonly IAnimalTypeService _animalTypeService;
        private readonly IFiscalYearService _fiscalYearService;
        private readonly ILssService _lssService;

        public LivestockReportByFiscalYearViewComponent(IWorkContext workContext, ICustomerService customerService,
            ILivestockService livestockService, ISpeciesService speciesService,
            IAnimalTypeService animalTypeService, IFiscalYearService fiscalYearService,
            ILssService lssService
            )
        {
            _workContext = workContext;
            _customerService = customerService;
            _livestockService = livestockService;
            _speciesService = speciesService;
            _animalTypeService = animalTypeService;
            _fiscalYearService = fiscalYearService;
            _lssService = lssService;
        }

        public IAnimalTypeService AnimalTypeService => _animalTypeService;

        public async Task<IViewComponentResult> InvokeAsync(string fiscalYear)
        {
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
                var livestock = await _livestockService.GetLivestock(createdby: item, fiscalyear: fiscalYear);
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
                var fiscal = await _fiscalYearService.GetFiscalYearById(fiscalYear);
                livestockReport.FiscalYear = fiscal.NepaliFiscalYear;
                report.Add(livestockReport);

            }

            return View(report);

        }
    }
}
