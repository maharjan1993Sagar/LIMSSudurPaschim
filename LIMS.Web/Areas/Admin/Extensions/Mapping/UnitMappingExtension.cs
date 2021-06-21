using LIMS.Domain.BasicSetup;
using LIMS.Web.Areas.Admin.Models.BasicSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class UnitMappingExtension
    {
        public static UnitModel ToModel(this Unit entity)
        {
            return entity.MapTo<Unit, UnitModel>();
        }

        public static Unit ToEntity(this UnitModel model)
        {
            return model.MapTo<UnitModel, Unit>();
        }

        public static Unit ToEntity(this UnitModel model, Unit destination)
        {
            return model.MapTo(destination);
        }
    }
}
