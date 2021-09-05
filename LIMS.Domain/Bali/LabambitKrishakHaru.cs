using LIMS.Domain.BesicSetup;
using LIMS.Domain.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Bali
{
    public class LabambitKrishakHaru:BaseEntity
    {
        public FiscalYear FiscalYear { get; set; }
        public string FiscalyearId { get; set; }
        public PujigatKharchaKharakram PujigatKharchaKharakram { get; set; }
        public string PujigatKharchaKaryakramId { get; set; }
        public int? LabambitKrishakKoNam { get; set; }
        public int? PhoneNo { get; set; }
        public int? Sex { get; set; }
        public int? EthinicGroup { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string LocalLevel { get; set; }
        public string WardNo { get; set; }
        public string Tole { get; set; }
        public Picture Picture { get; set; }
        public string PictureId { get; set; }
        public int? Remarks { get; set; }
        public string WorkDone { get; set; }
        public string BeneficiaryType { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
