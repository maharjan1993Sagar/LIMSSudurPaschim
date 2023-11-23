using LIMS.Core.ModelBinding;
using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.DynamicMenu
{
    public class SubMenuModel:BaseEntity
    {
        [LIMSResourceDisplayName("Admin.SubMenu.Name")]
        public string Name { get; set; }
        [LIMSResourceDisplayName("Admin.SubMenu.NameNepali")]
        public string NameNepali { get; set; }
        [LIMSResourceDisplayName("Admin.SubMenu.Url")]
        public string Url { get; set; }
        [LIMSResourceDisplayName("Admin.SubMenu.ExternalUrl")]
        public string ExternalUrl { get; set; }
        [LIMSResourceDisplayName("Admin.SubMenu.IsUrlExternal")]
        public bool IsUrlExternal { get; set; }
        [LIMSResourceDisplayName("Admin.SubMenu.MainMenu")]
        public string MainMenuId { get; set; }
        [LIMSResourceDisplayName("Admin.SubMenu.IsActive")]
        public bool IsActive { get; set; }
        [LIMSResourceDisplayName("Admin.SubMenu.HasSubSubMenu")]

        public bool HasSubSubMenu { get; set; }
        [LIMSResourceDisplayName("Admin.SubMenu.SerialNo")]

        public int SerialNo { get; set; }
      
    }
}
