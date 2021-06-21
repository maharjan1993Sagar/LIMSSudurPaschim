using LIMS.Domain.Breed;
using LIMS.Domain.DynamicMenu;
using LIMS.Web.Areas.Admin.Models.DynamicMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class MainMenuMappingExtension
    {
        public static MainMenuModel ToModel(this MainMenu entity)
        {
            return entity.MapTo<MainMenu, MainMenuModel>();
        }

        public static MainMenu ToEntity(this MainMenuModel model)
        {
            return model.MapTo<MainMenuModel, MainMenu>();
        }

        public static MainMenu ToEntity(this MainMenuModel model, MainMenu destination)
        {
            return model.MapTo(destination);
        }
    }
}
