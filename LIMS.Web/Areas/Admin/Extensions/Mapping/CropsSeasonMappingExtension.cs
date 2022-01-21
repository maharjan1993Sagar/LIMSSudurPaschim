using LIMS.Domain.Breed;
using LIMS.Web.Areas.Admin.Models.Breed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class CropsSeasonMappingExtension
    {
        public static CropsSeasonModel ToModel(this CropsSeason entity)
        {
            return entity.MapTo<CropsSeason,CropsSeasonModel>();
        }

        public static CropsSeason ToEntity(this CropsSeasonModel model)
        {
            return model.MapTo<CropsSeasonModel, CropsSeason>();
        }
        public static CropsSeason ToEntity(this CropsSeasonModel model, CropsSeason destination)
        {
            return model.MapTo(destination);
        }
    }
}
