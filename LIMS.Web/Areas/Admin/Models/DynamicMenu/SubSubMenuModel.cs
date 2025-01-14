﻿using LIMS.Core.ModelBinding;
using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.DynamicMenu
{
    public class SubSubMenuModel:BaseEntity
    {
        [LIMSResourceDisplayName("Admin.SubSubMenu.SubSubMenu")]
        public string SubSubMenuName { get; set; }
        [LIMSResourceDisplayName("Admin.SubSubMenu.SubSubMenuNepali")]
        public string SubSubMenuNameNepali { get; set; }
        [LIMSResourceDisplayName("Admin.SubSubMenu.SubSubMenuUrl")]

        public string Url { get; set; }
        [LIMSResourceDisplayName("Admin.SubSubMenu.ExternalUrl")]

        public string ExternalUrl { get; set; }
        [LIMSResourceDisplayName("Admin.SubSubMenu.IsUrlExternal")]

        public bool IsUrlExternal { get; set; }
        [LIMSResourceDisplayName("Admin.SubSubMenu.IsActiveSubSubMenu")]

        public bool IsActive { get; set; }
        [LIMSResourceDisplayName("Admin.SubSubMenu.SubMenuId")]

        public string SubMenuId { get; set; }
        [LIMSResourceDisplayName("Admin.SubSubMenu.SerialNo")]

        public int SerialNo { get; set; }

    }
}
