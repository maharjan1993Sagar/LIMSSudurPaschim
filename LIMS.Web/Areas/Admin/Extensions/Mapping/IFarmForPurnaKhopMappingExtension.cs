using LIMS.Domain.AnimalHealth;
using LIMS.Web.Areas.Admin.Models.AnimalHealth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class IFarmForPurnaKhopMappingExtension
    {
        public static  FarmForPurnaKhop ToEntity(this FarmForPurnaKhopModel model)
        {
            return model.MapTo<FarmForPurnaKhopModel, FarmForPurnaKhop>();
        }
        public static FarmForPurnaKhopModel ToModel(this FarmForPurnaKhop entity)
        {
            return entity.MapTo<FarmForPurnaKhop,FarmForPurnaKhopModel>();
        }
        public static FarmForPurnaKhop ToEntity(this FarmForPurnaKhopModel source,FarmForPurnaKhop destination)
        {
            return source.MapTo(destination);
        }
    }
}
