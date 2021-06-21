using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.BarGraph
{
    public class BardataModel
    {
       
        public List<string> yaxis { get; set; }

 
        public List<int> Cow { get; set; }
        public List<int> Buffalo { get; set; }

    }
    public class ProductionData
    {
        public List<string> yaxis { get; set; }
        public List<int> xaxis { get; set; }
    }
}
