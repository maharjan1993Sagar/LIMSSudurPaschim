using LIMS.Domain.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Activities
{
    public class ActivityProgressListModel
    {

        public string ActivityName { get; set; }
        public string FiscalYear { get; set; }
        public string UnitName { get; set; }
        public string Month { get; set; }
        public string Quater { get; set; }
        public string Progress { get; set; }
    }
}
