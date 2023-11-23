using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Website1.Models
{
    public class MainMenuModel
    {
        public MainMenuModel()
        {
            SubMenus = new List<SubMenuModel>();
        }
        public string MainMenuId { get; set; }
        public int SerialNo { get; set; }
        public string MainMenuName { get; set; }
        public string MainMenuNameNepali { get; set; }
        public string Url { get; set; }
        public bool IsUrlExternal { get; set; }
        public bool HasSubMenu { get; set; }
        public bool IsActive { get; set; }
        public string UserId { get; set; }
        public List<SubMenuModel> SubMenus { get; set; }
    }

    public class SubMenuModel
    {
        public SubMenuModel()
        {
            SubSubMenus = new List<SubSubMenuModel>();
        }
        public string SubMenuId { get; set; }
        public int SerialNo { get; set; }
        public string Name { get; set; }
        public string NameNepali { get; set; }
        public string Url { get; set; }
        public bool IsUrlExternal { get; set; }
        public bool IsActive { get; set; }
        public bool HasSubSubMenu { get; set; }
        public List<SubSubMenuModel> SubSubMenus { get; set; }
    }

    public class SubSubMenuModel
    {
        public string SubSubMenuId { get; set; }
        public int SerialNo { get; set; }
        public string SubSubMenuName { get; set; }
        public string SubSubMenuNameNepali { get; set; }
        public string Url { get; set; }
        public bool IsUrlExternal { get; set; }
        public bool IsActive { get; set; }
        public string UserId { get; set; }
    }
}
