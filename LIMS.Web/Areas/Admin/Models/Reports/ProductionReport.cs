using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Reports
{
    public class ProductionReport
    {
        public string FarmName { get; set; }
        public string Address { get; set; }
        public List<int> Production { get; set; }
        public string Remarks { get; set; }
        public List<string> Species { get; set; }
        public List<string> SpeciesMeat { get; set; }

        public string FiscalYear { get; set; }


    }
}
