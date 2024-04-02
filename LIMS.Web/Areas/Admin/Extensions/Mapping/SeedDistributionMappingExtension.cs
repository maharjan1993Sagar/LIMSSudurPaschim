using LIMS.Domain.Bali;
using LIMS.Domain.BasicSetup;
using LIMS.Web.Areas.Admin.Models.Bali;
using LIMS.Web.Areas.Admin.Models.BasicSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class SeedDistributionMappingExtension
    {
        public static SeedDistributionModel ToModel(this SeedDistribution entity)
        {
            return entity.MapTo<SeedDistribution, SeedDistributionModel>();
        }

        public static SeedDistribution ToEntity(this SeedDistributionModel model)
        {
            return model.MapTo<SeedDistributionModel, SeedDistribution>();
        }

        public static SeedDistribution ToEntity(this SeedDistributionModel model, SeedDistribution destination)
        {
            return model.MapTo(destination);
        }
    }
}
