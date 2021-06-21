using LIMS.Core.ModelBinding;
using LIMS.Domain;
using LIMS.Domain.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Activities
{
    public class TargetModel:BaseEntity
    {
        [LIMSResourceDisplayName("Admin.Target.ActivityName")]
        public string ActivityId { get; set; }
        [LIMSResourceDisplayName("Admin.Common.UnitName")]

        public string UnitId { get; set; }
        [LIMSResourceDisplayName("Admin.Target.QuaterOneTarget")]

        public string QuaterOneTarget { get; set; }
        [LIMSResourceDisplayName("Admin.Target.QuaterTwoTarget")]

        public string QuaterTwoTarget { get; set; }
        [LIMSResourceDisplayName("Admin.Target.QuaterThreeTarget")]

        public string QuaterThreeTarget { get; set; }
        [LIMSResourceDisplayName("Admin.Target.QuaterFourTarget")]

        public string QuaterFourTarget { get; set; }
        [LIMSResourceDisplayName("Admin.Target.AnualTarget")]

        public string AnualTarget { get; set; }
        [LIMSResourceDisplayName("Admin.Common.FiscalYear")]

        public string FiscalYearId { get; set; }
        public List<Activity> Activities { get; set; }
    }
}
