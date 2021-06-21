using LIMS.Core.ModelBinding;
using LIMS.Core.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace LIMS.Web.Areas.Admin.Models.Messages
{
    public partial class QueuedEmailListModel : BaseModel
    {
        [LIMSResourceDisplayName("Admin.System.QueuedEmails.List.StartDate")]
        [UIHint("DateNullable")]
        public DateTime? SearchStartDate { get; set; }

        [LIMSResourceDisplayName("Admin.System.QueuedEmails.List.EndDate")]
        [UIHint("DateNullable")]
        public DateTime? SearchEndDate { get; set; }

        [LIMSResourceDisplayName("Admin.System.QueuedEmails.List.FromEmail")]
        
        public string SearchFromEmail { get; set; }

        [LIMSResourceDisplayName("Admin.System.QueuedEmails.List.ToEmail")]
        
        public string SearchToEmail { get; set; }

        [LIMSResourceDisplayName("Admin.System.QueuedEmails.List.LoadNotSent")]
        public bool SearchLoadNotSent { get; set; }

        [LIMSResourceDisplayName("Admin.System.QueuedEmails.List.MaxSentTries")]
        public int SearchMaxSentTries { get; set; }

        [LIMSResourceDisplayName("Admin.System.QueuedEmails.List.GoDirectlyToNumber")]
        public string GoDirectlyToNumber { get; set; }
    }
}