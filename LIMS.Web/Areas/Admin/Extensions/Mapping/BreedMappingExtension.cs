using LIMS.Domain.Breed;
using LIMS.Web.Areas.Admin.Models.Breed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class BreedMappingExtension
    {
        public static BreedModel ToModel(this BreedReg entity)
        {
            return entity.MapTo<BreedReg, BreedModel>();
        }

        public static BreedReg ToEntity(this BreedModel model)
        {
            return model.MapTo<BreedModel, BreedReg>();
        }

        public static BreedReg ToEntity(this BreedModel model, BreedReg destination)
        {
            return model.MapTo(destination);
        }
    }
}
