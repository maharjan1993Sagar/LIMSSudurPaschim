using LIMS.Domain.Breed;
using LIMS.Web.Areas.Admin.Models.Breed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class LIvestcokBreedMappingExtension
    {
        public static LivestockBreedModel ToModel(this LivestockBreed entity)
        {
            return entity.MapTo<LivestockBreed, LivestockBreedModel>();
        }

        public static LivestockBreed ToEntity(this LivestockBreedModel model)
        {
            return model.MapTo<LivestockBreedModel, LivestockBreed>();
        }
        public static LivestockBreed ToEntity(this LivestockBreedModel model, LivestockBreed destination)
        {
            return model.MapTo(destination);
        }
    }
}
