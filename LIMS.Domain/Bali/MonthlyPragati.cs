using LIMS.Domain.BesicSetup;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Bali
{
    public class MonthlyPragati:BaseEntity
    {
        public PujigatKharchaKharakram pujigatKharchaKharakram { get; set; }
        public string   PujigatKharchaId { get; set; }
        public FiscalYear FiscalYear { get; set; }
        public string FiscalYearId { get; set; }
        public string Month { get; set; }
        public string VautikPragati { get; set; }
        public string BitiyaPragati { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
