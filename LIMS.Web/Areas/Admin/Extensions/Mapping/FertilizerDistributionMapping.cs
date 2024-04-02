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
    public static class FertilizerDistributionMappingExtension
    {
        public static FertilizerDistributionModel ToModel(this FertilizerDistribution entity)
        {
            return entity.MapTo<FertilizerDistribution, FertilizerDistributionModel>();
        }

        public static FertilizerDistribution ToEntity(this FertilizerDistributionModel model)
        {
            return model.MapTo<FertilizerDistributionModel, FertilizerDistribution>();
        }

        public static FertilizerDistribution ToEntity(this FertilizerDistributionModel model, FertilizerDistribution destination)
        {
            return model.MapTo(destination);
        }
    }
}
