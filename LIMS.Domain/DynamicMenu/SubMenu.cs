using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.DynamicMenu
{
   public class SubMenu:BaseEntity
    {
        public SubMenu()
        {
            this.SubMenuId = Guid.NewGuid();
        }
        public Guid SubMenuId { get; set; }
        public int SerialNo { get; set; }
        public string Name { get; set; }
        public string NameNepali { get; set; }
        public string Url { get; set; }
        public bool IsUrlExternal { get; set; }
        public string MainMenuId { get; set; }
        public MainMenu MainMenu { get; set; }
        public bool IsActive { get; set; }
        public bool HasSubSubMenu { get; set; }
        public List<SubSubMenu> SubSubMenus { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime EditedDate { get; set; }
        public string EditedBy { get; set; }
        public DateTime DeletedDate { get; set; }
        public string DeletedBy { get; set; }
    }
}
