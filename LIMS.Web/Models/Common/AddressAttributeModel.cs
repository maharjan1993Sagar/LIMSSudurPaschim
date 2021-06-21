﻿using LIMS.Core.Models;
using System.Collections.Generic;

namespace LIMS.Web.Models.Common
{
    public partial class AddressAttributeModel : BaseEntityModel
    {
        public AddressAttributeModel()
        {
            Values = new List<AddressAttributeValueModel>();
        }

        public string Name { get; set; }

        public bool IsRequired { get; set; }

        /// <summary>
        /// Default value for textboxes
        /// </summary>
        public string DefaultValue { get; set; }

        public IList<AddressAttributeValueModel> Values { get; set; }
    }

    public partial class AddressAttributeValueModel : BaseEntityModel
    {
        public string Name { get; set; }

        public bool IsPreSelected { get; set; }
    }
}