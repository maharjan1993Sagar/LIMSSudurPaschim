using LIMS.Domain.StatisticalData;
using LIMS.Web.Areas.Admin.Models.StatisticalData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class ProductionMappingExtension
    {
        public static ProductionModel ToModel(this Production entity)
        {
            return entity.MapTo<Production, ProductionModel>();
        }

        public static Production ToEntity(this ProductionModel model)
        {
            return model.MapTo<ProductionModel, Production>();
        }

        public static Production ToEntity(this ProductionModel model, Production destination)
        {
            return model.MapTo(destination);
        }
    }
}
