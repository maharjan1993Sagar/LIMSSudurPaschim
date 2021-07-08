using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.DynamicMenu
{
  public class SubSubMenu:BaseEntity
    {
        public SubSubMenu()
        {
            this.SubSubMenuId = Guid.NewGuid();
        }
        public Guid  SubSubMenuId { get; set; }
        public int SerialNo { get; set; }
        public string SubSubMenuName { get; set; }
        public string SubSubMenuNameNepali { get; set; }
        public string  Url { get; set; }
        public bool IsActive { get; set; }
        public string SubMenuId { get; set; }
        public SubMenu SubMenu { get; set; }

        public string UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime EditedDate { get; set; }
        public string EditedBy { get; set; }
        public DateTime DeletedDate { get; set; }
        public string DeletedBy { get; set; }

    }
}
