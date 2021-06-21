using LIMS.Framework.Localization;
using LIMS.Core.ModelBinding;
using LIMS.Core.Models;
using System.Collections.Generic;

namespace LIMS.Web.Areas.Admin.Models.Customers
{
    public partial class CustomerAttributeModel : BaseEntityModel, ILocalizedModel<CustomerAttributeLocalizedModel>
    {
        public CustomerAttributeModel()
        {
            Locales = new List<CustomerAttributeLocalizedModel>();
        }

        [LIMSResourceDisplayName("Admin.Customers.CustomerAttributes.Fields.Name")]

        public string Name { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.CustomerAttributes.Fields.IsRequired")]
        public bool IsRequired { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.CustomerAttributes.Fields.AttributeControlType")]
        public int AttributeControlTypeId { get; set; }
        [LIMSResourceDisplayName("Admin.Customers.CustomerAttributes.Fields.AttributeControlType")]

        public string AttributeControlTypeName { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.CustomerAttributes.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }


        public IList<CustomerAttributeLocalizedModel> Locales { get; set; }

    }

    public partial class CustomerAttributeLocalizedModel : ILocalizedModelLocal
    {
        public string LanguageId { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.CustomerAttributes.Fields.Name")]

        public string Name { get; set; }

    }
}