using LIMS.Domain.Breed;
using LIMS.Domain.StatisticalData;
using LIMS.Web.Areas.Admin.Models.StatisticalData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public  static class LivestockMappingEtension
    {
        public static LivestockModel ToModel(this Livestock entity)
        {
            return entity.MapTo<Livestock, LivestockModel>();
        }

        public static Livestock ToEntity(this LivestockModel model)
        {
            return model.MapTo<LivestockModel, Livestock>();
        }

        public static Livestock ToEntity(this LivestockModel model, Livestock destination)
        {
            return model.MapTo(destination);
        }
    }
}
