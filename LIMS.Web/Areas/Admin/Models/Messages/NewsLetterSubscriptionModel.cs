using LIMS.Core.ModelBinding;
using LIMS.Core.Models;

namespace LIMS.Web.Areas.Admin.Models.Messages
{
    public partial class NewsLetterSubscriptionModel : BaseEntityModel
    {
        [LIMSResourceDisplayName("Admin.Promotions.NewsLetterSubscriptions.Fields.Email")]
        public string Email { get; set; }

        [LIMSResourceDisplayName("Admin.Promotions.NewsLetterSubscriptions.Fields.Active")]
        public bool Active { get; set; }

        [LIMSResourceDisplayName("Admin.Promotions.NewsLetterSubscriptions.Fields.Store")]
        public string StoreName { get; set; }

        [LIMSResourceDisplayName("Admin.Promotions.NewsLetterSubscriptions.Fields.Categories")]
        public string Categories { get; set; }

        [LIMSResourceDisplayName("Admin.Promotions.NewsLetterSubscriptions.Fields.CreatedOn")]
        public string CreatedOn { get; set; }
    }
}