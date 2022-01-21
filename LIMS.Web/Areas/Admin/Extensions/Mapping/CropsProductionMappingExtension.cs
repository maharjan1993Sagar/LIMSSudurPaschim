using LIMS.Domain.Breed;
using LIMS.Web.Areas.Admin.Models.Breed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class CropsProductionMappingExtension
    {
        public static CropsProductionModel ToModel(this CropsProduction entity)
        {
            return entity.MapTo<CropsProduction, CropsProductionModel>();
        }

        public static CropsProduction ToEntity(this CropsProductionModel model)
        {
            return model.MapTo<CropsProductionModel, CropsProduction>();
        }
        public static CropsProduction ToEntity(this CropsProductionModel model, CropsProduction destination)
        {
            return model.MapTo(destination);
        }
    }
}
