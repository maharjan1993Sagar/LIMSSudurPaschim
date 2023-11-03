using LIMS.Domain.Bali;
using LIMS.Web.Areas.Admin.Models.Bali;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class PlantSoilManagementMappingExtension
    {
        public static PlantSoilManagementModel ToModel(this PlantSoilManagement bali)
        {
            return bali.MapTo<PlantSoilManagement, PlantSoilManagementModel>();
        }
        public static PlantSoilManagement ToEntity(this PlantSoilManagementModel bali)
        {
            return bali.MapTo<PlantSoilManagementModel, PlantSoilManagement>();
        }
        public static PlantSoilManagement ToEntity(this PlantSoilManagementModel source, PlantSoilManagement destination)
        {
            return source.MapTo(destination);
        }
    }
}
