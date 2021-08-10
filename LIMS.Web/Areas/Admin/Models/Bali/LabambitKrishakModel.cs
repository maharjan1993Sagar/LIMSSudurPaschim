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
        [LIMSResourceDisplayName("LIMS.LabambitKrishak.LabambitKrishakKoNam")]

        public string LabambitKrishakKoNam { get; set; }
        [LIMSResourceDisplayName("LIMS.LabambitKrishak.PhoneNo")]

        public string PhoneNo { get; set; }
        [LIMSResourceDisplayName("LIMS.LabambitKrishak.Sex")]

        public string Sex { get; set; }
        [LIMSResourceDisplayName("LIMS.LabambitKrishak.EthinicGroup")]

        public string EthinicGroup { get; set; }
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
        [LIMSResourceDisplayName("LIMS.Common.Remarks")]

        public string Remarks { get; set; }
       
    }
}
