using LIMS.Core.ModelBinding;
using LIMS.Core.Models;
using LIMS.Domain;
using LIMS.Domain.AInR;
using LIMS.Domain.Breed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.StatisticalData
{
    public class ProductionModel : BaseEntity
    {
        public ProductionModel()
        {
            Statistics = new List<Statistic>();
        }

        [LIMSResourceDisplayName("Admin.StatisticalData.Production.SpeciesName")]
        public string SpeciesName { get; set; }
        [LIMSResourceDisplayName("Admin.StatisticalData.Production.BreedName")]

        public string BreedName { get; set; }
        [LIMSResourceDisplayName("Admin.StatisticalData.Production.ProductionType")]

        public string ProductionType { get; set; }
        [LIMSResourceDisplayName("Admin.StatisticalData.Production.Quantity")]

        public string Quantity { get; set; }
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

        [LIMSResourceDisplayName("Admin.StatisticalData.Production.Date")]
        public string Date { get; set; }
        [LIMSResourceDisplayName("Admin.StatisticalData.Production.Quater")]

        public string Quater { get; set; }
        [LIMSResourceDisplayName("Admin.StatisticalData.Farm.FarmId")]
        public string   FarmId { get; set; }
        public Farm Farm { get; set; }
        public IList<Statistic> Statistics { get; set; }

        public IList<LivestockSpecies> Species { get; set; }
        #region nested class
        public partial class Statistic : BaseModel
        {
            public string BreedId { get; set; }
            public string Quantity { get; set; }
            public string Unit { get; set; }
            public string Ward { get; set; }
            public string Tole { get; set; }
            public DateTime? ProductionDate { get; set; }
        }
        #endregion
    }
}
