using LIMS.Core.ModelBinding;
using LIMS.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LIMS.Web.Areas.Admin.Models.Messages
{
    public partial class NewsLetterSubscriptionListModel : BaseModel
    {
        public NewsLetterSubscriptionListModel()
        {
            AvailableStores = new List<SelectListItem>();
            ActiveList = new List<SelectListItem>();
            SearchCategoryIds = new List<string>();
            AvailableCategories = new List<SelectListItem>();
        }

        [LIMSResourceDisplayName("Admin.Promotions.NewsLetterSubscriptions.List.SearchEmail")]
        public string SearchEmail { get; set; }

        [LIMSResourceDisplayName("Admin.Promotions.NewsLetterSubscriptions.List.SearchStore")]
        public string StoreId { get; set; }
        public IList<SelectListItem> AvailableStores { get; set; }

        [LIMSResourceDisplayName("Admin.Promotions.NewsLetterSubscriptions.List.SearchActive")]
        public int ActiveId { get; set; }
        [LIMSResourceDisplayName("Admin.Promotions.NewsLetterSubscriptions.List.SearchActive")]
        public IList<SelectListItem> ActiveList { get; set; }

        [LIMSResourceDisplayName("Admin.Promotions.NewsLetterSubscriptions.List.Categories")]
        
        public IList<SelectListItem> AvailableCategories { get; set; }

        [LIMSResourceDisplayName("Admin.Promotions.NewsLetterSubscriptions.List.Category")]
        [UIHint("MultiSelect")]
        public IList<string> SearchCategoryIds { get; set; }

    }
}