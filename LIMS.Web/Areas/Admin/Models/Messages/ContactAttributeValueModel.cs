using LIMS.Framework.Localization;
using LIMS.Core.ModelBinding;
using LIMS.Core.Models;
using System.Collections.Generic;

namespace LIMS.Web.Areas.Admin.Models.Messages
{
    public partial class ContactAttributeValueModel : BaseEntityModel, ILocalizedModel<ContactAttributeValueLocalizedModel>
    {
        public ContactAttributeValueModel()
        {
            Locales = new List<ContactAttributeValueLocalizedModel>();
        }

        public string ContactAttributeId { get; set; }

        [LIMSResourceDisplayName("Admin.Catalog.Attributes.ContactAttributes.Values.Fields.Name")]
        public string Name { get; set; }

        [LIMSResourceDisplayName("Admin.Catalog.Attributes.ContactAttributes.Values.Fields.ColorSquaresRgb")]
        public string ColorSquaresRgb { get; set; }
        public bool DisplayColorSquaresRgb { get; set; }

        [LIMSResourceDisplayName("Admin.Catalog.Attributes.ContactAttributes.Values.Fields.IsPreSelected")]
        public bool IsPreSelected { get; set; }

        [LIMSResourceDisplayName("Admin.Catalog.Attributes.ContactAttributes.Values.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        public IList<ContactAttributeValueLocalizedModel> Locales { get; set; }

    }

    public partial class ContactAttributeValueLocalizedModel : ILocalizedModelLocal
    {
        public string LanguageId { get; set; }

        [LIMSResourceDisplayName("Admin.Catalog.Attributes.ContactAttributes.Values.Fields.Name")]
        public string Name { get; set; }
    }
}