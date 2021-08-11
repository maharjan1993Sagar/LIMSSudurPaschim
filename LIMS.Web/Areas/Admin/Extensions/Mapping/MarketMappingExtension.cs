using LIMS.Domain.Bali;
using LIMS.Web.Areas.Admin.Models.Bali;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class MarketMappingExtension
    {
        public static MarketModel ToModel(this MarketData bali)
        {
            return bali.MapTo<MarketData, MarketModel>();
        }
        public static MarketData ToEntity(this MarketModel bali)
        {
            return bali.MapTo<MarketModel, MarketData>();
        }
        public static MarketData ToEntity(this MarketModel source, MarketData destination)
        {
            return source.MapTo(destination);
        }
    }
}
