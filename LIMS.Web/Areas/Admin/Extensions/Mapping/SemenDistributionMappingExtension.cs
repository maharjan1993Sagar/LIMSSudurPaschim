using LIMS.Domain.SemenDistribution;
using LIMS.Web.Areas.Admin.Models.Semen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class SemenDistributionMappingExtension
    {
        public static SemenDistribution ToEntity(this SemenDistributionModel model)
        {
            return model.MapTo<SemenDistributionModel, SemenDistribution>();
        }
        public static SemenDistributionModel TOModel(this SemenDistribution entity)
        {
            return entity.MapTo<SemenDistribution, SemenDistributionModel>();
        }
        public static SemenDistribution TOEntity(SemenDistributionModel source, SemenDistribution destination)
        {
            return source.MapTo(destination);
        }

    }
}
