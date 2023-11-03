using LIMS.Core.ModelBinding;
using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [UIHint("Decimal")]
        public decimal AanudanRakam { get; set; }
        [LIMSResourceDisplayName("Lims.Aanudan.Remarks")]

        public string Remarks { get; set; }
        [LIMSResourceDisplayName("Lims.Aanudan.DateOfSubsidy")]
        [UIHint("date")]
        public DateTime DateOfSubsidy { get; set; }
        [LIMSResourceDisplayName("Lims.Aanudan.MaleMember")]
        [UIHint("int32")]
        public int? MaleMember { get; set; }
        [LIMSResourceDisplayName("Lims.Aanudan.FemaleMember")]
        [UIHint("int32")]
        public int? FemaleMember { get; set; }
        [UIHint("int32")]
        [LIMSResourceDisplayName("Lims.Aanudan.DalitMember")]
        public int? DalitMember { get; set; }
        [UIHint("int32")]
        [LIMSResourceDisplayName("Lims.Aanudan.JanajatiMember")]
        public int? JanajatiMember { get; set; }
        [UIHint("int32")]
        [LIMSResourceDisplayName("Lims.Aanudan.Others")]
        public int? Others { get; set; }
        [LIMSResourceDisplayName("Lims.Aanudan.ExpectedOutput")]
        public string ExpectedOutput { get; set; }
        [UIHint("Decimal")]
        [LIMSResourceDisplayName("Lims.Aanudan.FarmerContribution")]
        public decimal FarmerContribution { get; set; }
        [LIMSResourceDisplayName("Lims.Aanudan.SubsidyCategory")]
        public string SubsidyCategory { get; set; }
        [LIMSResourceDisplayName("Lims.Aanudan.NameOfCategory")]

        public string NameOfCategory { get; set; }
        [LIMSResourceDisplayName("Lims.Aanudan.ProposeOfSubsidy")]

        public string ProposeOfSubsidy { get; set; }
        [LIMSResourceDisplayName("Lims.Aanudan.Area")]

        public string Area { get; set; }
        [LIMSResourceDisplayName("Lims.Aanudan.AnudanReceiverType")]
        public string AnudanReceiverType { get; set; }

    }
}
