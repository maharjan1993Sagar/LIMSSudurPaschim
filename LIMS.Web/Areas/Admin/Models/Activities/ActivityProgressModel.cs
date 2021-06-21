using LIMS.Core.ModelBinding;
using LIMS.Domain;
using LIMS.Domain.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Activities
{
    public class ActivityProgressModel:BaseEntity
    {
        [LIMSResourceDisplayName("Admin.ActivityProgress.Activity")]
        public string ActivityId { get; set; }
        [LIMSResourceDisplayName("Admin.Common.FiscalYear")]
        public string FiscalYearId { get; set; }
        [LIMSResourceDisplayName("Admin.Common.UnitName")]
        public string UnitId { get; set; }
        [LIMSResourceDisplayName("Admin.ActivityProgress.Month")]
        public string Month { get; set; }
        [LIMSResourceDisplayName("Admin.ActivityProgress.Quater")]
        public string Quater { get; set; }
        [LIMSResourceDisplayName("Admin.ActivityProgress.Progress")]
        public string Progress { get; set; }
        public List<Activity> Activities { get; set; }
    }
}
