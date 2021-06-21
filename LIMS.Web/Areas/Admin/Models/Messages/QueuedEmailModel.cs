using LIMS.Core.ModelBinding;
using LIMS.Core.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace LIMS.Web.Areas.Admin.Models.Messages
{
    public partial class QueuedEmailModel : BaseEntityModel
    {
        [LIMSResourceDisplayName("Admin.System.QueuedEmails.Fields.Id")]
        public override string Id { get; set; }

        [LIMSResourceDisplayName("Admin.System.QueuedEmails.Fields.Priority")]
        public string PriorityName { get; set; }

        [LIMSResourceDisplayName("Admin.System.QueuedEmails.Fields.From")]

        public string From { get; set; }

        [LIMSResourceDisplayName("Admin.System.QueuedEmails.Fields.FromName")]

        public string FromName { get; set; }

        [LIMSResourceDisplayName("Admin.System.QueuedEmails.Fields.To")]

        public string To { get; set; }

        [LIMSResourceDisplayName("Admin.System.QueuedEmails.Fields.ToName")]

        public string ToName { get; set; }

        [LIMSResourceDisplayName("Admin.System.QueuedEmails.Fields.ReplyTo")]

        public string ReplyTo { get; set; }

        [LIMSResourceDisplayName("Admin.System.QueuedEmails.Fields.ReplyToName")]

        public string ReplyToName { get; set; }

        [LIMSResourceDisplayName("Admin.System.QueuedEmails.Fields.CC")]

        public string CC { get; set; }

        [LIMSResourceDisplayName("Admin.System.QueuedEmails.Fields.Bcc")]

        public string Bcc { get; set; }

        [LIMSResourceDisplayName("Admin.System.QueuedEmails.Fields.Subject")]

        public string Subject { get; set; }

        [LIMSResourceDisplayName("Admin.System.QueuedEmails.Fields.Body")]

        public string Body { get; set; }

        [LIMSResourceDisplayName("Admin.System.QueuedEmails.Fields.AttachmentFilePath")]

        public string AttachmentFilePath { get; set; }

        [LIMSResourceDisplayName("Admin.System.QueuedEmails.Fields.AttachedDownload")]
        [UIHint("Download")]
        public string AttachedDownloadId { get; set; }

        [LIMSResourceDisplayName("Admin.System.QueuedEmails.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }
        [LIMSResourceDisplayName("Admin.System.QueuedEmails.Fields.SendImmediately")]
        public bool SendImmediately { get; set; }

        [LIMSResourceDisplayName("Admin.System.QueuedEmails.Fields.DontSendBeforeDate")]
        [UIHint("DateTimeNullable")]
        public DateTime? DontSendBeforeDate { get; set; }

        [LIMSResourceDisplayName("Admin.System.QueuedEmails.Fields.SentTries")]
        public int SentTries { get; set; }

        [LIMSResourceDisplayName("Admin.System.QueuedEmails.Fields.SentOn")]
        public DateTime? SentOn { get; set; }

        [LIMSResourceDisplayName("Admin.System.QueuedEmails.Fields.EmailAccountName")]

        public string EmailAccountName { get; set; }
    }
}