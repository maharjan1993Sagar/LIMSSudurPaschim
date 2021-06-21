using LIMS.Core.ModelBinding;
using LIMS.Core.Models;
using System;

namespace LIMS.Web.Areas.Admin.Models.Logging
{
    public partial class ActivityLogModel : BaseEntityModel
    {
        [LIMSResourceDisplayName("Admin.Configuration.ActivityLog.ActivityLog.Fields.ActivityLogType")]
        public string ActivityLogTypeName { get; set; }
        public string ActivityLogTypeId { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.ActivityLog.ActivityLog.Fields.Customer")]
        public string CustomerId { get; set; }
        [LIMSResourceDisplayName("Admin.Configuration.ActivityLog.ActivityLog.Fields.Customer")]
        public string CustomerEmail { get; set; }
        [LIMSResourceDisplayName("Admin.Configuration.ActivityLog.ActivityLog.Fields.Comment")]
        public string Comment { get; set; }
        [LIMSResourceDisplayName("Admin.Configuration.ActivityLog.ActivityLog.Fields.IpAddress")]
        public string IpAddress { get; set; }
        [LIMSResourceDisplayName("Admin.Configuration.ActivityLog.ActivityLog.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }
    }

    public partial class ActivityStatsModel : BaseEntityModel
    {
        [LIMSResourceDisplayName("Admin.Configuration.ActivityLog.ActivityStats.Fields.ActivityLogType")]
        public string ActivityLogTypeName { get; set; }
        public string ActivityLogTypeId { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.ActivityLog.ActivityStats.Fields.EntityKeyId")]
        public string EntityKeyId { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.ActivityLog.ActivityStats.Fields.Name")]
        public string Name { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.ActivityLog.ActivityStats.Fields.Count")]
        public int Count { get; set; }

    }
}
