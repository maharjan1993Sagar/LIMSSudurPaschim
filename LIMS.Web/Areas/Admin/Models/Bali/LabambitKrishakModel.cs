using LIMS.Core.ModelBinding;
using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Bali
{
    public class LabambitKrishakModel:BaseEntity
    {
        [LIMSResourceDisplayName("LIMS.LabambitKrishak.Fiscalyear")]
        public string FiscalyearId { get; set; }
        [LIMSResourceDisplayName("LIMS.LabambitKrishak.PujigatKharchaKaryakramId")]

        public string PujigatKharchaKaryakramId { get; set; }
        [LIMSResourceDisplayName("LIMS.LabambitKrishak.NoOfMale")]
        [UIHint("int32nullable")]

        public int? LabambitKrishakKoNam { get; set; }
        [LIMSResourceDisplayName("LIMS.LabambitKrishak.NoFemale")]
        [UIHint("int32nullable")]
        public int? Sex { get; set; }
        [LIMSResourceDisplayName("LIMS.LabambitKrishak.Dalit")]
        [UIHint("int32nullable")]

        public int? PhoneNo { get; set; }
      
        [LIMSResourceDisplayName("LIMS.LabambitKrishak.Janajati")]
        [UIHint("int32nullable")]

        public int? EthinicGroup { get; set; }
        [LIMSResourceDisplayName("LIMS.LabambitKrishak.YouthMember")]
        [UIHint("int32nullable")]

        public int? Remarks { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.Province")]

        public string Province { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.District")]

        public string District { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.LocalLevel")]

        public string LocalLevel { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.WardNo")]

        public string WardNo { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.Tole")]

        public string Tole { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.Picture")]

        [UIHint("Picture")]
        public string PictureId { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.Workdone")]

        public string WorkDone { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.BeneficiaryType")]

        public string BeneficiaryType { get; set; }



    }
}
