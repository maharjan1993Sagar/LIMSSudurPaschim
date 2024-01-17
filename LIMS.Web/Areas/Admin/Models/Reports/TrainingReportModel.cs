using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Reports
{
    public class TrainingReportModel
    {
        public string Address { get; set; }
        public string LocalLevel { get; set; }
        public string BudgetId { get; set; }
        public string Level { get; set; }
        public string Ward { get; set; }
        public string FiscalYear { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public List<TrainingRowData> Rows { get; set; }
    }
    
    public class TrainingRowData
    {
        public string SN { get; set; }
        public string BudgetTitle { get; set; }
        public string TrainingTitle { get; set; }
        public string MainActivity { get; set; }
        public string Remarks { get; set; }
    }
       
}
