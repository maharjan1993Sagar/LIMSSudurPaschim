using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.AInR
{
    public class EarTag
    {
        public int SerialNo { get; set; }
        public string EarTagNo { get; set; }
        public int From { get; set; }
        public int To { get; set; }

    }
}
