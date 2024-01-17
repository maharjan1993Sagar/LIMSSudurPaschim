using LIMS.Domain.BesicSetup;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Bali
{
    public class MonthlyPragati:BaseEntity
    {
        public Budget Budget { get; set; }
        public string   BudgetId { get; set; }
        public FiscalYear FiscalYear { get; set; }
        public string FiscalYearId { get; set; }
        public string Month { get; set; }
        public string VautikPragati { get; set; }
        public string BitiyaPragati { get; set; }
        public string KharchaKoSwrot { get; set; }
        public string SuchanaPrakashan { get; set; }
        public string FieldVerification { get; set; }
        public string Samzauta { get; set; }
        public string Anugaman { get; set; }
        public string UpalbdiHaru { get; set; }
        public string VuktaniPauneKoNam { get; set; }
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
