using LIMS.Core.ModelBinding;
using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.StatisticalData
{
    public class LivestockModel:BaseEntity
    {
        [LIMSResourceDisplayName("Admin.StatisticalData.Livestock.SpeciesName")]
        public string SpeciesName { get; set; }

        [LIMSResourceDisplayName("Admin.StatisticalData.Livestock.NoOfLiveStock")]

        public string NoOfLivestock { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Unit")]

        public string Unit { get; set; }
        [LIMSResourceDisplayName("Admin.Common.FiscalYear")]

        public string FiscalYear { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Provience")]
        public string Provience { get; set; }

        [LIMSResourceDisplayName("Admin.Common.District")]
        public string District { get; set; }

        [LIMSResourceDisplayName("Admin.Common.LocalLevel")]
        public string LocalLevel { get; set; }

        [LIMSResourceDisplayName("Admin.Common.Ward")]
        public string Ward { get; set; }

        [LIMSResourceDisplayName("Admin.Common.Tole")]
        public string Tole { get; set; }

        [LIMSResourceDisplayName("Admin.StatisticalData.Livestock.Farm")]
        public string FarmId { get; set; }
        [LIMSResourceDisplayName("Admin.StatisticalData.Livestock.Date")]
        public string Date { get; set; }
        [LIMSResourceDisplayName("Admin.StatisticalData.Livestock.Quater")]

        public string Quater { get; set; }
        [LIMSResourceDisplayName("Admin.StatisticalData.Livestock.BreedType")]

        public string BreedType { get; set; }
        [LIMSResourceDisplayName("Admin.StatisticalData.Livestock.Month")]

        public string Month { get; set; }
    }
}
