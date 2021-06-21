using LIMS.Core.ModelBinding;
using LIMS.Core.Models;
using System;

namespace LIMS.Web.Areas.Admin.Models.Logging
{
    public partial class LogModel : BaseEntityModel
    {
        [LIMSResourceDisplayName("Admin.System.Log.Fields.LogLevel")]
        public string LogLevel { get; set; }

        [LIMSResourceDisplayName("Admin.System.Log.Fields.ShortMessage")]
        
        public string ShortMessage { get; set; }

        [LIMSResourceDisplayName("Admin.System.Log.Fields.FullMessage")]
        
        public string FullMessage { get; set; }

        [LIMSResourceDisplayName("Admin.System.Log.Fields.IPAddress")]
        
        public string IpAddress { get; set; }

        [LIMSResourceDisplayName("Admin.System.Log.Fields.Customer")]
        public string CustomerId { get; set; }
        [LIMSResourceDisplayName("Admin.System.Log.Fields.Customer")]
        public string CustomerEmail { get; set; }

        [LIMSResourceDisplayName("Admin.System.Log.Fields.PageURL")]
        
        public string PageUrl { get; set; }

        [LIMSResourceDisplayName("Admin.System.Log.Fields.ReferrerURL")]
        
        public string ReferrerUrl { get; set; }

        [LIMSResourceDisplayName("Admin.System.Log.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }
    }
}