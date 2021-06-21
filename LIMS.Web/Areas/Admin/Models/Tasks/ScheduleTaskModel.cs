using LIMS.Core.ModelBinding;
using LIMS.Core.Models;
using System;

namespace LIMS.Web.Areas.Admin.Models.Tasks
{
    public partial class ScheduleTaskModel : BaseEntityModel
    {
        [LIMSResourceDisplayName("Admin.System.ScheduleTasks.ScheduleTaskName")]
        public string ScheduleTaskName { get; set; }

        [LIMSResourceDisplayName("Admin.System.ScheduleTasks.LeasedByMachineName")]
        public string LeasedByMachineName { get; set; }

        [LIMSResourceDisplayName("Admin.System.ScheduleTasks.Type")]
        public string Type { get; set; }
        [LIMSResourceDisplayName("Admin.System.ScheduleTasks.Enabled")]
        public bool Enabled { get; set; }
        [LIMSResourceDisplayName("Admin.System.ScheduleTasks.StopOnError")]
        public bool StopOnError { get; set; }
        [LIMSResourceDisplayName("Admin.System.ScheduleTasks.LastStartUtc")]
        public DateTime? LastStartUtc { get; set; }
        [LIMSResourceDisplayName("Admin.System.ScheduleTasks.LastEndUtc")]
        public DateTime? LastEndUtc { get; set; }
        [LIMSResourceDisplayName("Admin.System.ScheduleTasks.LastSuccessUtc")]
        public DateTime? LastSuccessUtc { get; set; }
        [LIMSResourceDisplayName("Admin.System.ScheduleTasks.TimeInterval")]
        public int TimeInterval { get; set; }
    }
}