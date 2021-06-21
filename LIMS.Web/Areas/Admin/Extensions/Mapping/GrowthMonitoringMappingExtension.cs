using LIMS.Domain.Recording;
using LIMS.Web.Areas.Admin.Models.Recording;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class GrowthMonitoringMappingExtension
    {
        public static GrowthMonitoringModel ToModel(this GrowthMonitoring entity)
        {
            return entity.MapTo<GrowthMonitoring, GrowthMonitoringModel>();
        }

        public static GrowthMonitoring ToEntity(this GrowthMonitoringModel model)
        {
            return model.MapTo<GrowthMonitoringModel, GrowthMonitoring>();
        }

        public static GrowthMonitoring ToEntity(this GrowthMonitoringModel model, GrowthMonitoring destination)
        {
            return model.MapTo(destination);
        }
    }
}
