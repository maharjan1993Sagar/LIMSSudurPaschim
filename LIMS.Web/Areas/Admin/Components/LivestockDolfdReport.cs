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
    public class LivestockDolfdReportViewComponent: BaseViewComponent
    {
        private readonly IWorkContext _workContext;
        private readonly ICustomerService _customerService;
        private readonly ILivestockService _livestockService;
        private readonly ISpeciesService _speciesService;
        private readonly IFiscalYearService _fiscalYearService;
        private readonly ILssService _lssService;
        private readonly IVhlsecService _vhlsecService;
        private readonly IAnimalTypeService _animalTypeService;
        public LivestockDolfdReportViewComponent(IWorkContext workContext, ICustomerService customerService,
            ILivestockService livestockService, ISpeciesService speciesService,
            IAnimalTypeService animalTypeService, IFiscalYearService fiscalYearService,
            ILssService lssService,
            IVhlsecService vhlsecService
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
        }


        public async Task<IViewComponentResult> InvokeAsync(string fiscalYear,string lssid=null)
        {

            string entityId = _workContext.CurrentCustomer.EntityId;
            List<string> vhlsecId = _vhlsecService.GetVhlsecByDolfdId(entityId).Result.Select(m => m.Id).ToList();
            var LssIds = new List<string>();
            foreach (var item in vhlsecId)
            {
                LssIds.AddRange(_lssService.GetLssByVhlsecId(item).Result.Select(m => m.Id).ToList());
            }
            var userids = (dynamic)null;
            if (lssid != null)
            {


                 userids = _customerService.GetCustomerByLssId(null,lssid).Select(m => m.Id).ToList();

            }
            else
            {
                 userids = _customerService.GetCustomerByLssId(LssIds, vhlsecId, entityId).Select(m => m.Id).ToList();

            }


            var livestocks = new List<Livestock>();
           
            foreach (var item in userids)
            {
                var livestock = await _livestockService.GetLivestock(createdby: item, fiscalyear: fiscalYear);
                if (livestock.TotalCount > 0)
                {
                    livestocks.AddRange(livestock);
                }
                
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
