using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Bali
{
    public class MonthlySummerizedReport
    {
        public string Name { get; set; }
        public decimal TotalBudget { get; set; }
        public decimal PugigatBudget { get; set; }
        public decimal ChaluBudget { get; set; }
        public decimal TotalBudgetKharcha { get; set; }
        public decimal PugigatBudgetKharcha { get; set; }
        public decimal ChaluBudgetKharcha { get; set; }
        public decimal PragratiVar { get; set; }
        public decimal PugigatPragratiVar { get; set; }
        public decimal ChaluPragatiVar { get; set; }
        public decimal BharitPragati { get; set; }
        public decimal PugigatBharitPragati { get; set; }
        public decimal ChaluBharitPragati { get; set; }
    }
}
