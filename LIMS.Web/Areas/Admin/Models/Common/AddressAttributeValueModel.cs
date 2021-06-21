﻿using LIMS.Framework.Localization;
using LIMS.Core.ModelBinding;
using LIMS.Core.Models;
using System.Collections.Generic;

namespace LIMS.Web.Areas.Admin.Models.Common
{
    public partial class AddressAttributeValueModel : BaseEntityModel, ILocalizedModel<AddressAttributeValueLocalizedModel>
    {
        public AddressAttributeValueModel()
        {
            Locales = new List<AddressAttributeValueLocalizedModel>();
        }

        public string AddressAttributeId { get; set; }

        [LIMSResourceDisplayName("Admin.Address.AddressAttributes.Values.Fields.Name")]

        public string Name { get; set; }

        [LIMSResourceDisplayName("Admin.Address.AddressAttributes.Values.Fields.IsPreSelected")]
        public bool IsPreSelected { get; set; }

        [LIMSResourceDisplayName("Admin.Address.AddressAttributes.Values.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        public IList<AddressAttributeValueLocalizedModel> Locales { get; set; }

    }

    public partial class AddressAttributeValueLocalizedModel : ILocalizedModelLocal
    {
        public string LanguageId { get; set; }

        [LIMSResourceDisplayName("Admin.Address.AddressAttributes.Values.Fields.Name")]

        public string Name { get; set; }
    }
}