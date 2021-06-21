using LIMS.Domain.Breed;
using LIMS.Domain.DynamicMenu;
using LIMS.Web.Areas.Admin.Models.DynamicMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class SubMenuMappingExtension
    {
        public static SubMenuModel ToModel(this SubMenu entity)
        {
            return entity.MapTo<SubMenu, SubMenuModel>();
        }

        public static SubMenu ToEntity(this SubMenuModel model)
        {
            return model.MapTo<SubMenuModel, SubMenu>();
        }

        public static SubMenu ToEntity(this SubMenuModel model, SubMenu destination)
        {
            return model.MapTo(destination);
        }
    }
}
