using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Report
{
    public class SubsidyReportModel
    {
        public string Address { get; set; }
        public string LocalLevel { get; set; }
        public string Level { get; set; }

        public string Ward { get; set; }
        public string FiscalYear { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public List<SubsidyRowData> Rows { get; set; }
    }

    public class SubsidyRowData
    {
        public string SN { get; set; }
        public string BudgetTitle { get; set; }
        public string MainActivity { get; set; }
        public string Remarks { get; set; }
    }
}
