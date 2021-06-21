using LIMS.Core.Models;

namespace LIMS.Web.Models.PrivateMessages
{
    public partial class SendPrivateMessageModel : BaseEntityModel
    {
        public string ToCustomerId { get; set; }
        public string CustomerToName { get; set; }
        public bool AllowViewingToProfile { get; set; }

        public string ReplyToMessageId { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }
    }
}