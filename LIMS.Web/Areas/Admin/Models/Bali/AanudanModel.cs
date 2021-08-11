using LIMS.Core.ModelBinding;
using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Bali.Aanudan
{
    public class AanudanModel:BaseEntity
    {
        [LIMSResourceDisplayName("Lims.Aanudan.PujigatKharchaKaryakramId")]

        public string PujigatKharchaKaryakramId { get; set; }
        [LIMSResourceDisplayName("Lims.Aanudan.FiscalyearId")]

        public string FiscalyearId { get; set; }
        [LIMSResourceDisplayName("Lims.Aanudan.KrishakKoName")]

        public string KrishakKoName { get; set; }
        [LIMSResourceDisplayName("Lims.Aanudan.PhoneNo")]

        public string PhoneNo { get; set; }
        [LIMSResourceDisplayName("Lims.Aanudan.Sex")]

        public string Sex { get; set; }
        [LIMSResourceDisplayName("Lims.Aanudan.EthinicGroup")]
        public string EthinicGroup { get; set; }
        [LIMSResourceDisplayName("Lims.common.Province")]

        public string Province { get; set; }
        [LIMSResourceDisplayName("Lims.common.District")]

        public string District { get; set; }
        [LIMSResourceDisplayName("Lims.common.LocalLevel")]

        public string LocalLevel { get; set; }
        [LIMSResourceDisplayName("Lims.common.Ward")]

        public string Ward { get; set; }
        [LIMSResourceDisplayName("Lims.Aanudan.Tole")]

        public string Tole { get; set; }
        [LIMSResourceDisplayName("Lims.Aanudan.AanudanKokisim")]

        public string AanudanKokisim { get; set; }
        [LIMSResourceDisplayName("Lims.Aanudan.AanudanRakam")]

        public string AanudanRakam { get; set; }
        [LIMSResourceDisplayName("Lims.Aanudan.Remarks")]

        public string Remarks { get; set; }

    }
}
