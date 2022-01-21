using LIMS.Domain.Breed;
using LIMS.Web.Areas.Admin.Models.Breed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions
{
    public static  class LivestockSpeciesMappingExtension
    {
        public static LivestockSpeciesModel ToModel(this LivestockSpecies entity)
        {
            return entity.MapTo<LivestockSpecies, LivestockSpeciesModel>();
        }

        public static LivestockSpecies ToEntity(this LivestockSpeciesModel model)
        {
            return model.MapTo<LivestockSpeciesModel, LivestockSpecies>();
        }
        public static LivestockSpecies ToEntity(this LivestockSpeciesModel model, LivestockSpecies destination)
        {
            return model.MapTo(destination);
        }

    }
}
