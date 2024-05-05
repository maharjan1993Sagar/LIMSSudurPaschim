using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Reports
{
    public class AIReport
    {
        public string LocalLevel { get; set; }
        public string Municipility { get; set; }
        public int Cow { get; set; }
        public int Buffalo { get; set; }
        public int Goat { get; set; }
        public int Pig { get; set; }
        public int total;
        public int Total {

            get {
                return Cow + Buffalo + Goat + Pig;
            }


            set {
                total = Cow + Buffalo + Goat + Pig;

            }
            }
        public string FiscalYear { get; set; }
    }
}
