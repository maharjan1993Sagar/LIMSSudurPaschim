using LIMS.Domain.Breed;
using LIMS.Web.Areas.Admin.Models.Breed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class SpeciesMappingExtension
    {
        public static SpeciesModel ToModel(this Species entity)
        {
            return entity.MapTo<Species, SpeciesModel>();
        }

        public static Species ToEntity(this SpeciesModel model)
        {
            return model.MapTo<SpeciesModel, Species>();
        }

        public static Species ToEntity(this SpeciesModel model, Species destination)
        {
            return model.MapTo(destination);
        }
    }
}
