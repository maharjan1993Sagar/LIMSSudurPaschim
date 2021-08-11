using LIMS.Core.ModelBinding;
using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Bali
{
    public class FarmerModel:BaseEntity
    {
        [LIMSResourceDisplayName("Lims.Farmer.Name")]
        public string Name { get; set; }
        [LIMSResourceDisplayName("Lims.Common.Province")]

        public string Province { get; set; }
        [LIMSResourceDisplayName("Lims.Common.District")]


        public string District { get; set; }
        [LIMSResourceDisplayName("Lims.Common.LocalLevel")]

        public string LocalLevel { get; set; }
        [LIMSResourceDisplayName("Lims.Common.Address")]

        public string Address { get; set; }
        [LIMSResourceDisplayName("Lims.Common.Phone")]

        public string Phone { get; set; }
        [LIMSResourceDisplayName("Lims.Farmer.TalimName")]

        public string TalimId { get; set; }
        [LIMSResourceDisplayName("Lims.Farmer.IncuvationCenter")]

        public string IncuvationCenterId { get; set; }

        [LIMSResourceDisplayName("Lims.Common.Remarks")]

        public string Remarks { get; set; }
        [LIMSResourceDisplayName("Lims.Farmer.EnglishName")]
        public string NameNepali { get; set; }

    }
}
