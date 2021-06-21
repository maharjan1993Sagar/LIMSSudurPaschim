using LIMS.Domain.AInR;
using LIMS.Web.Areas.Admin.Models.AInR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class FarmMappingExtension
    {
        public static FarmModel ToModel(this Farm entity)
        {
            return entity.MapTo<Farm, FarmModel>();
        }

        public static Farm ToEntity(this FarmModel  model)
        {
            return model.MapTo<FarmModel, Farm>();
        }
        public static Farm ToEntity(this FarmModel model, Farm destination)
        {
            return model.MapTo(destination);
        }

    }
}
