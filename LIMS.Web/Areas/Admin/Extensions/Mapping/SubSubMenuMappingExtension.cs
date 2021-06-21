using LIMS.Domain.Breed;
using LIMS.Domain.DynamicMenu;
using LIMS.Web.Areas.Admin.Models.DynamicMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class SubSubMenuMappingExtension
    {
        public static SubSubMenuModel ToModel(this SubSubMenu entity)
        {
            return entity.MapTo<SubSubMenu, SubSubMenuModel>();
        }

        public static SubSubMenu ToEntity(this SubSubMenuModel model)
        {
            return model.MapTo<SubSubMenuModel, SubSubMenu>();
        }

        public static SubSubMenu ToEntity(this SubSubMenuModel model, SubSubMenu destination)
        {
            return model.MapTo(destination);
        }
    }
}
