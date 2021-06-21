using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Reports
{
    public class MonthlyProgressReportModel
    {
        public RowDataModel Rows { get; set; }
        public string Topic { get; set; }
        public string FiscalYear { get; set; }
        public string Month { get; set; }
        public string Unit { get; set; }
    }
    
    public class RowDataModel
    {
        public string Title { get; set; }
        public List<int> Data { get; set; }
    }
       
}
