using LIMS.Core.ModelBinding;

namespace LIMS.Web.Areas.Admin.Models.Settings
{
    public class AdminSearchSettingsModel
    {
        [LIMSResourceDisplayName("Admin.Configuration.Settings.AdminSearch.Fields.SearchInProducts")]
        public bool SearchInProducts { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Settings.AdminSearch.Fields.ProductsDisplayOrder")]
        public int ProductsDisplayOrder { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Settings.AdminSearch.Fields.SearchInCategories")]
        public bool SearchInCategories { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Settings.AdminSearch.Fields.CategoriesDisplayOrder")]
        public int CategoriesDisplayOrder { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Settings.AdminSearch.Fields.SearchInManufacturers")]
        public bool SearchInManufacturers { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Settings.AdminSearch.Fields.ManufacturersDisplayOrder")]
        public int ManufacturersDisplayOrder { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Settings.AdminSearch.Fields.SearchInTopics")]
        public bool SearchInTopics { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Settings.AdminSearch.Fields.TopicsDisplayOrder")]
        public int TopicsDisplayOrder { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Settings.AdminSearch.Fields.SearchInNews")]
        public bool SearchInNews { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Settings.AdminSearch.Fields.NewsDisplayOrder")]
        public int NewsDisplayOrder { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Settings.AdminSearch.Fields.SearchInBlogs")]
        public bool SearchInBlogs { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Settings.AdminSearch.Fields.BlogsDisplayOrder")]
        public int BlogsDisplayOrder { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Settings.AdminSearch.Fields.SearchInCustomers")]
        public bool SearchInCustomers { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Settings.AdminSearch.Fields.CustomersDisplayOrder")]
        public int CustomersDisplayOrder { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Settings.AdminSearch.Fields.SearchInOrders")]
        public bool SearchInOrders { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Settings.AdminSearch.Fields.OrdersDisplayOrder")]
        public int OrdersDisplayOrder { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Settings.AdminSearch.Fields.MinSearchTermLength")]
        public int MinSearchTermLength { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Settings.AdminSearch.Fields.MaxSearchResultsCount")]
        public int MaxSearchResultsCount { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Settings.AdminSearch.Fields.SearchInMenu")]
        public bool SearchInMenu { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Settings.AdminSearch.Fields.MenuDisplayOrder")]
        public int MenuDisplayOrder { get; set; }
    }
}
