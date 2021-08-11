using LIMS.Domain.Bali;
using LIMS.Web.Areas.Admin.Models.Bali;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static  class FarmerMappingExtension
    {
        public static FarmerModel ToModel(this Farmer bali)
        {
            return bali.MapTo<Farmer, FarmerModel>();
        }
        public static Farmer ToEntity(this FarmerModel bali)
        {
            return bali.MapTo<FarmerModel, Farmer>();
        }
        public static Farmer ToEntity(this FarmerModel source, Farmer destination)
        {
            return source.MapTo(destination);
        }
    }
}
