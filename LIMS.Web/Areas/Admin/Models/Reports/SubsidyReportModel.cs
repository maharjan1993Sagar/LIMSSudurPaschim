using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Reports
{
    public class SubsidyReportModel
    {
        public string Address { get; set; }
        public string LocalLevel { get; set; }
        public string Level { get; set; }
        public string BudgetId { get; set; }
        public string Ward { get; set; }
        public string FiscalYear { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Xetra{ get; set; }
        public List<SubsidyRowData> Rows { get; set; }
    }
    
    public class SubsidyRowData
    {
        public string SN { get; set; }
        public string BudgetTitle { get; set; }
        public string MainActivity { get; set; }
        public string Remarks { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Male { get; set; }
        public string Female { get; set; }
        public string Dalit { get; set; }
        public string Janajati { get; set; }
        public string Others { get; set; }
        public string Total { get; set; }
        public string Purpose { get; set; }
        public string Upalabdhiharu { get; set; }
    }
       
}
