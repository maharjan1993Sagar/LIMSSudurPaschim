using LIMS.Domain.BesicSetup;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Bali
{
   public class AanudanKokaryakram:BaseEntity
    {
        public PujigatKharchaKharakram PujigatKharchaKharakram { get; set; }
        public string PujigatKharchaKaryakramId { get; set; }
        public FiscalYear FiscalYear { get; set; }
        public string FiscalyearId { get; set; }
        public string KrishakKoName { get; set; }
        public string PhoneNo { get; set; }
        public string Sex { get; set; }
        public string EthinicGroup { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string LocalLevel { get; set; }
        public string Ward { get; set; }
        public string Tole { get; set; }
        public string AanudanKokisim { get; set; }
        public string AanudanRakam { get; set; }
        public string Remaks { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedAt { get; set; }
    }
}
