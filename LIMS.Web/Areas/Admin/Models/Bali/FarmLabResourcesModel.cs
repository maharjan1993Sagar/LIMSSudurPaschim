using LIMS.Core.ModelBinding;
using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Bali
{
    public class FarmLabResourcesModel:BaseEntity
    {
        [LIMSResourceDisplayName("LIMS.FarmLab.Type")]
        public string Type { get; set; }
        [LIMSResourceDisplayName("LIMS.FarmLab.PlantType")]
        public string PlantType { get; set; }
        [LIMSResourceDisplayName("LIMS.FarmLab.SubType")]

        public string SubType { get; set; }
        [LIMSResourceDisplayName("LIMS.FarmLab.Sector")]

        public string Sector { get; set; }

        [LIMSResourceDisplayName("LIMS.FarmLab.ItemName")]

        public string ItemName { get; set; }
        [LIMSResourceDisplayName("LIMS.FarmLab.Species")]

        public string Species { get; set; }

        [LIMSResourceDisplayName("LIMS.FarmLab.Unit")]

        public string UnitId { get; set; }
        [LIMSResourceDisplayName("LIMS.FarmLab.AvailableQuantity")]

        public string AvailableQuantity { get; set; }
        [LIMSResourceDisplayName("LIMS.FarmLab.UnitPrice")]

        public string UnitPrice { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.Remarks")]

        public string Remarks { get; set; }

    }
}
