using LIMS.Core.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.StatisticalData
{
    public class FishProductionModel
    {
        [LIMSResourceDisplayName("Admin.StatisticalData.FishProduction.FarmName")]

        public string FarmId { get; set; }
        [LIMSResourceDisplayName("Admin.StatisticalData.FishProduction.NatureOfProduction")]

        public string NatureOfProduction { get; set; }
        [LIMSResourceDisplayName("Admin.common.province")]

        public string Province { get; set; }
        [LIMSResourceDisplayName("Admin.common.district")]

        public string District { get; set; }
        [LIMSResourceDisplayName("Admin.common.locallevel")]

        public string LocalLevel { get; set; }
        [LIMSResourceDisplayName("Admin.common.ward")]

        public string Ward { get; set; }
        [LIMSResourceDisplayName("Admin.common.fiscalyear")]

        public string FiscalYearId { get; set; }
        [LIMSResourceDisplayName("Admin.common.Trimister")]

        public string Trimister { get; set; }
        [LIMSResourceDisplayName("Admin.common.Date")]

        public string Date { get; set; }
        public string SpeciesId { get; set; }
        [LIMSResourceDisplayName("Admin.StatisticalData.FishProduction.Breed")]

        public string BreedId { get; set; }
        [LIMSResourceDisplayName("Admin.StatisticalData.FishProduction.NoOFFish")]

        public string NumberOfFish { get; set; }
        [LIMSResourceDisplayName("Admin.StatisticalData.FishProduction.Area")]

        public string Area { get; set; }
        [LIMSResourceDisplayName("Admin.StatisticalData.FishProduction.Quantity")]
        public string Quantity { get; set; }
        [LIMSResourceDisplayName("Admin.StatisticalData.FishProduction.Remarks")]
        public string Remarks { get; set; }
       

    }
}
