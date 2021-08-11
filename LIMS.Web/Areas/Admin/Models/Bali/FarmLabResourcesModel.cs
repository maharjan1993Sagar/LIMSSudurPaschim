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
        [LIMSResourceDisplayName("LIMS.FarmLab.Unit")]

        public string UnitId { get; set; }
        [LIMSResourceDisplayName("LIMS.FarmLab.AvailableQuantity")]

        public string AvailableQuantity { get; set; }
        [LIMSResourceDisplayName("LIMS.FarmLab.UnitPrice")]

        public string UnitPrice { get; set; }
        
    }
}
