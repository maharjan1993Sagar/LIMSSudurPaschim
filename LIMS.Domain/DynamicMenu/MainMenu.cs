using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.DynamicMenu
{
    public class MainMenu:BaseEntity
    {
        public MainMenu()
        {
            //this.MainMenuId = Guid.NewGuid();
        }
     //   public Guid MainMenuId { get; set; }
        public int SerialNo { get; set; }
        public string  MainMenuName { get; set; }
        public string  MainMenuNameNepali { get; set; }
        public string  Url { get; set; }
        public bool HasSubMenu { get; set; }
        public bool IsActive { get; set; }
        public bool IsUrlExternal { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime EditedDate { get; set; }
        public string EditedBy { get; set; }
        public DateTime DeletedDate { get; set; }
        public string DeletedBy { get; set; }

    }
}
