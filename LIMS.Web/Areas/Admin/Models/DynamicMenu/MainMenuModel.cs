using LIMS.Core.ModelBinding;
using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.DynamicMenu
{
    public class MainMenuModel:BaseEntity
    {
        [LIMSResourceDisplayName("Admin.MainMenu.MainMenuName")]
        public string MainMenuName { get; set; } 
        [LIMSResourceDisplayName("Admin.MainMenu.MainMenuNameNepali")]
        public string MainMenuNameNepali { get; set; }
        [LIMSResourceDisplayName("Admin.MainMenu.Url")]
        public string Url { get; set; }
        [LIMSResourceDisplayName("Admin.MainMenu.ExternalUrl")]
        public string ExternalUrl { get; set; }
        [LIMSResourceDisplayName("Admin.MainMenu.IsUrlExternal")]
        public bool IsUrlExternal { get; set; }
        [LIMSResourceDisplayName("Admin.MainMenu.HasSubMenu")]
        public bool HasSubMenu { get; set; }
        [LIMSResourceDisplayName("Admin.MainMenu.IsActive")]
        public bool IsActive { get; set; }
        [LIMSResourceDisplayName("Admin.MainMenu.SerialNo")]
        public int SerialNo { get; set; }
    }
}
