using LIMS.Framework.Localization;
using LIMS.Framework.Mapping;
using LIMS.Core.ModelBinding;
using LIMS.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using LIMS.Framework.Mvc.Models;

namespace LIMS.Web.Areas.Admin.Models.Knowledgebase
{
    public class KnowledgebaseCategoryModel : BaseEntityModel, ILocalizedModel<KnowledgebaseCategoryLocalizedModel>, IAclMappingModel, IStoreMappingModel
    {
        public KnowledgebaseCategoryModel()
        {
            Categories = new List<SelectListItem>();
            Locales = new List<KnowledgebaseCategoryLocalizedModel>();
            AvailableCustomerRoles = new List<CustomerRoleModel>();
            AvailableStores = new List<StoreModel>();
        }

        [LIMSResourceDisplayName("Admin.ContentManagement.Knowledgebase.KnowledgebaseCategory.Fields.Name")]
        public string Name { get; set; }

        [LIMSResourceDisplayName("Admin.ContentManagement.Knowledgebase.KnowledgebaseCategory.Fields.ParentCategoryId")]
        public string ParentCategoryId { get; set; }

        [LIMSResourceDisplayName("Admin.ContentManagement.Knowledgebase.KnowledgebaseCategory.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [LIMSResourceDisplayName("Admin.ContentManagement.Knowledgebase.KnowledgebaseCategory.Fields.Description")]
        public string Description { get; set; }

        [LIMSResourceDisplayName("Admin.ContentManagement.Knowledgebase.KnowledgebaseCategory.Fields.Published")]
        public bool Published { get; set; }

        public List<SelectListItem> Categories { get; set; }

        public IList<KnowledgebaseCategoryLocalizedModel> Locales { get; set; }

        [LIMSResourceDisplayName("Admin.ContentManagement.Knowledgebase.KnowledgebaseCategory.Fields.SubjectToAcl")]
        public bool SubjectToAcl { get; set; }

        [LIMSResourceDisplayName("Admin.ContentManagement.Knowledgebase.KnowledgebaseCategory.Fields.MetaKeywords")]
        public string MetaKeywords { get; set; }

        [LIMSResourceDisplayName("Admin.ContentManagement.Knowledgebase.KnowledgebaseCategory.Fields.MetaDescription")]
        public string MetaDescription { get; set; }

        [LIMSResourceDisplayName("Admin.ContentManagement.Knowledgebase.KnowledgebaseCategory.Fields.MetaTitle")]
        public string MetaTitle { get; set; }

        [LIMSResourceDisplayName("Admin.ContentManagement.Knowledgebase.KnowledgebaseCategory.Fields.SeName")]
        public string SeName { get; set; }

        [LIMSResourceDisplayName("Admin.ContentManagement.Knowledgebase.KnowledgebaseCategory.Fields.AclCustomerRoles")]
        public List<CustomerRoleModel> AvailableCustomerRoles { get; set; }

        public string[] SelectedCustomerRoleIds { get; set; }

        //Store mapping
        [LIMSResourceDisplayName("Admin.ContentManagement.Knowledgebase.KnowledgebaseCategory.Fields.LimitedToStores")]
        public bool LimitedToStores { get; set; }
        [LIMSResourceDisplayName("Admin.ContentManagement.Knowledgebase.KnowledgebaseCategory.Fields.AvailableStores")]
        public List<StoreModel> AvailableStores { get; set; }
        public string[] SelectedStoreIds { get; set; }

        public partial class ActivityLogModel : BaseEntityModel
        {
            [LIMSResourceDisplayName("Admin.ContentManagement.Knowledgebase.ActivityLogType")]
            public string ActivityLogTypeName { get; set; }
            [LIMSResourceDisplayName("Admin.ContentManagement.Knowledgebase.ActivityLog.Comment")]
            public string Comment { get; set; }
            [LIMSResourceDisplayName("Admin.ContentManagement.Knowledgebase.ActivityLog.CreatedOn")]
            public DateTime CreatedOn { get; set; }
            [LIMSResourceDisplayName("Admin.ContentManagement.Knowledgebase.ActivityLog.Customer")]
            public string CustomerId { get; set; }
            public string CustomerEmail { get; set; }
        }
    }

    public class KnowledgebaseCategoryLocalizedModel : ILocalizedModelLocal, ISlugModelLocal
    {
        public string LanguageId { get; set; }

        [LIMSResourceDisplayName("Admin.ContentManagement.Knowledgebase.KnowledgebaseCategory.Fields.Name")]
        public string Name { get; set; }

        [LIMSResourceDisplayName("Admin.ContentManagement.Knowledgebase.KnowledgebaseCategory.Fields.Description")]
        public string Description { get; set; }

        [LIMSResourceDisplayName("Admin.ContentManagement.Knowledgebase.KnowledgebaseCategory.Fields.MetaKeywords")]
        public string MetaKeywords { get; set; }

        [LIMSResourceDisplayName("Admin.ContentManagement.Knowledgebase.KnowledgebaseCategory.Fields.MetaDescription")]
        public string MetaDescription { get; set; }

        [LIMSResourceDisplayName("Admin.ContentManagement.Knowledgebase.KnowledgebaseCategory.Fields.MetaTitle")]
        public string MetaTitle { get; set; }

        [LIMSResourceDisplayName("Admin.ContentManagement.Knowledgebase.KnowledgebaseCategory.Fields.SeName")]
        public string SeName { get; set; }
    }
}
