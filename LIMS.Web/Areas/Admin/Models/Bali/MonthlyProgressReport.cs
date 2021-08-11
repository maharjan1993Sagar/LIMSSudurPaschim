using LIMS.Domain.Bali;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Bali
{
    public class MonthlyProgressReport
    {
        public PujigatKharchaKharakram pujigatKharchaKharakram { get; set; }
        public string PujigatKharchaId { get; set; }
        public string FiscalYearId { get; set; }
        public string Month { get; set; }
        public string VautikPragati { get; set; }
        public string BitiyaPragati { get; set; }
        public string Type { get; set; }
        public string ProgramType { get; set; }
        public string PreviousMonthVautikPragati { get; set; }
        public string PreviousMonthBitiyaPragati { get; set; }
        public string TotalMonthVautikPragati { get; set; }
        public string TotalMonthBitiyaPragati { get; set; }

    }
}
