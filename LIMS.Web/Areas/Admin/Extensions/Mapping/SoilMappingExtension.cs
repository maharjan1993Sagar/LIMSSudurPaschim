using LIMS.Domain.Bali;
using LIMS.Web.Areas.Admin.Models.Bali;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class SoilMappingExtension
    {
        public static SoilModel ToModel(this Soil bali)
        {
            return bali.MapTo<Soil, SoilModel>();
        }
        public static Soil ToEntity(this SoilModel bali)
        {
            return bali.MapTo<SoilModel, Soil>();
        }
        public static Soil ToEntity(this SoilModel source, Soil destination)
        {
            return source.MapTo(destination);
        }
    }
}
