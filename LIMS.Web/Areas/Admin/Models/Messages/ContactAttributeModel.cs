using LIMS.Framework.Localization;
using LIMS.Framework.Mapping;
using LIMS.Core.ModelBinding;
using LIMS.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LIMS.Framework.Mvc.Models;

namespace LIMS.Web.Areas.Admin.Models.Messages
{
    public partial class ContactAttributeModel : BaseEntityModel, ILocalizedModel<ContactAttributeLocalizedModel>, IAclMappingModel, IStoreMappingModel
    {
        public ContactAttributeModel()
        {
            Locales = new List<ContactAttributeLocalizedModel>();
            AvailableCustomerRoles = new List<CustomerRoleModel>();
            AvailableStores = new List<StoreModel>();
        }

        [LIMSResourceDisplayName("Admin.Catalog.Attributes.ContactAttributes.Fields.Name")]

        public string Name { get; set; }

        [LIMSResourceDisplayName("Admin.Catalog.Attributes.ContactAttributes.Fields.TextPrompt")]

        public string TextPrompt { get; set; }

        [LIMSResourceDisplayName("Admin.Catalog.Attributes.ContactAttributes.Fields.IsRequired")]
        public bool IsRequired { get; set; }

        [LIMSResourceDisplayName("Admin.Catalog.Attributes.ContactAttributes.Fields.AttributeControlType")]
        public int AttributeControlTypeId { get; set; }
        [LIMSResourceDisplayName("Admin.Catalog.Attributes.ContactAttributes.Fields.AttributeControlType")]

        public string AttributeControlTypeName { get; set; }

        [LIMSResourceDisplayName("Admin.Catalog.Attributes.ContactAttributes.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }


        [LIMSResourceDisplayName("Admin.Catalog.Attributes.ContactAttributes.Fields.MinLength")]
        [UIHint("Int32Nullable")]
        public int? ValidationMinLength { get; set; }

        [LIMSResourceDisplayName("Admin.Catalog.Attributes.ContactAttributes.Fields.MaxLength")]
        [UIHint("Int32Nullable")]
        public int? ValidationMaxLength { get; set; }

        [LIMSResourceDisplayName("Admin.Catalog.Attributes.ContactAttributes.Fields.FileAllowedExtensions")]
        public string ValidationFileAllowedExtensions { get; set; }

        [LIMSResourceDisplayName("Admin.Catalog.Attributes.ContactAttributes.Fields.FileMaximumSize")]
        [UIHint("Int32Nullable")]
        public int? ValidationFileMaximumSize { get; set; }

        [LIMSResourceDisplayName("Admin.Catalog.Attributes.ContactAttributes.Fields.DefaultValue")]
        public string DefaultValue { get; set; }

        public IList<ContactAttributeLocalizedModel> Locales { get; set; }

        //condition
        public bool ConditionAllowed { get; set; }
        public ConditionModel ConditionModel { get; set; }

        //Store mapping
        [LIMSResourceDisplayName("Admin.Catalog.Attributes.ContactAttributes.Fields.LimitedToStores")]
        public bool LimitedToStores { get; set; }
        [LIMSResourceDisplayName("Admin.Catalog.Attributes.ContactAttributes.Fields.AvailableStores")]
        public List<StoreModel> AvailableStores { get; set; }
        public string[] SelectedStoreIds { get; set; }

        //ACL
        [LIMSResourceDisplayName("Admin.Catalog.Attributes.ContactAttributes.Fields.SubjectToAcl")]
        public bool SubjectToAcl { get; set; }
        [LIMSResourceDisplayName("Admin.Catalog.Attributes.ContactAttributes.Fields.AclCustomerRoles")]
        public List<CustomerRoleModel> AvailableCustomerRoles { get; set; }
        public string[] SelectedCustomerRoleIds { get; set; }
    }

    public partial class ConditionModel : BaseEntityModel
    {
        [LIMSResourceDisplayName("Admin.Catalog.Attributes.ContactAttributes.Condition.EnableCondition")]
        public bool EnableCondition { get; set; }

        [LIMSResourceDisplayName("Admin.Catalog.Attributes.ContactAttributes.Condition.Attributes")]
        public string SelectedAttributeId { get; set; }

        public IList<AttributeConditionModel> ConditionAttributes { get; set; }
    }
    public partial class AttributeConditionModel : BaseEntityModel
    {
        public string Name { get; set; }

        public IList<SelectListItem> Values { get; set; }

        public string SelectedValueId { get; set; }
    }
    public partial class ContactAttributeLocalizedModel : ILocalizedModelLocal
    {
        public string LanguageId { get; set; }

        [LIMSResourceDisplayName("Admin.Catalog.Attributes.ContactAttributes.Fields.Name")]

        public string Name { get; set; }

        [LIMSResourceDisplayName("Admin.Catalog.Attributes.ContactAttributes.Fields.TextPrompt")]

        public string TextPrompt { get; set; }

    }
}