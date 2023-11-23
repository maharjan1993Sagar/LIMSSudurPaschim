using LIMS.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Api.DTOs.GeneralCMS
{
    public class MainMenuDto:BaseApiEntityModel
    {
        public MainMenuDto()
        {
            SubMenus = new List<SubMenuDto>();
        }
        public string MainMenuId { get; set; }
        public int SerialNo { get; set; }
        public string MainMenuName { get; set; }
        public string MainMenuNameNepali { get; set; }
        public string Url { get; set; }
        public bool HasSubMenu { get; set; }
        public bool IsActive { get; set; }
        public string UserId { get; set; }
        public List<SubMenuDto> SubMenus { get; set; }
    }

    public class SubMenuDto: BaseApiEntityModel
    {
        public SubMenuDto()
        {
            SubSubMenus = new List<SubSubMenuDto>();
        }
        public string SubMenuId { get; set; }
        public int SerialNo { get; set; }
        public string Name { get; set; }
        public string NameNepali { get; set; }
        public string Url { get; set; }
        public bool IsActive { get; set; }
        public bool HasSubSubMenu { get; set; }
        public List<SubSubMenuDto> SubSubMenus { get; set; }
    }

    public class SubSubMenuDto: BaseApiEntityModel
    {
        public string SubSubMenuId { get; set; }
        public int SerialNo { get; set; }
        public string SubSubMenuName { get; set; }
        public string SubSubMenuNameNepali { get; set; }
        public string Url { get; set; }
        public bool IsActive { get; set; }
        public string UserId { get; set; }
    }
}
