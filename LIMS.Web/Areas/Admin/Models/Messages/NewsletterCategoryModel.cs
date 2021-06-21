using LIMS.Framework.Localization;
using LIMS.Framework.Mapping;
using LIMS.Core.ModelBinding;
using LIMS.Core.Models;
using System.Collections.Generic;
using LIMS.Framework.Mvc.Models;

namespace LIMS.Web.Areas.Admin.Models.Messages
{
    public partial class NewsletterCategoryModel : BaseEntityModel, ILocalizedModel<NewsletterCategoryLocalizedModel>, IStoreMappingModel
    {
        public NewsletterCategoryModel()
        {
            Locales = new List<NewsletterCategoryLocalizedModel>();
            AvailableStores = new List<StoreModel>();
        }

        [LIMSResourceDisplayName("Admin.Promotions.NewsletterCategory.Fields.Name")]

        public string Name { get; set; }

        [LIMSResourceDisplayName("Admin.Promotions.NewsletterCategory.Fields.Description")]

        public string Description { get; set; }

        [LIMSResourceDisplayName("Admin.Promotions.NewsletterCategory.Fields.Selected")]
        public bool Selected { get; set; }

        [LIMSResourceDisplayName("Admin.Promotions.NewsletterCategory.Fields.AvailableStores")]
        public List<StoreModel> AvailableStores { get; set; }

        [LIMSResourceDisplayName("Admin.Promotions.NewsletterCategory.Fields.LimitedToStores")]
        public bool LimitedToStores { get; set; }
        public string[] SelectedStoreIds { get; set; }

        [LIMSResourceDisplayName("Admin.Promotions.NewsletterCategory.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }
        public IList<NewsletterCategoryLocalizedModel> Locales { get; set; }
    }

    public partial class NewsletterCategoryLocalizedModel : ILocalizedModelLocal
    {
        public string LanguageId { get; set; }

        [LIMSResourceDisplayName("Admin.Promotions.NewsletterCategory.Fields.Name")]

        public string Name { get; set; }

        [LIMSResourceDisplayName("Admin.Promotions.NewsletterCategory.Fields.Description")]

        public string Description { get; set; }

    }
}