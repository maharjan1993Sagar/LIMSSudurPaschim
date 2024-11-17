using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Reports
{
    public class AiReportMonth
    {
        public string Municipility { get; set; }
        public string Designation { get; set; }
        public List<int> Cow { get; set; }
        public List<int> Buffalo { get; set; }
        public List<int> Goat { get; set; }
        public List<int> Pig { get; set; }
        public List<int> Total { get; set; }

        public List<string> Month { get; set; }
        public string FiscalYear { get; set; }

    }
}
