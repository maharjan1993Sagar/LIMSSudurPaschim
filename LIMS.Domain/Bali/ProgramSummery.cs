using LIMS.Domain.BesicSetup;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Bali
{
    public class ProgramSummery:BaseEntity
    {
        public PujigatKharchaKharakram PujigatKharchaKharakram { get; set; }
        public string PujigatKharchaKaryaKramId { get; set; }
        public FiscalYear FiscalYear { get; set; }
        public string FiscalYearId { get; set; }
        public string Budget { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
