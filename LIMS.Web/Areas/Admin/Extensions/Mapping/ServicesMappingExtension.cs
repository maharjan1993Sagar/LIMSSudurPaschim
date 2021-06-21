using LIMS.Domain.StatisticalData;
using LIMS.Web.Areas.Admin.Models.StatisticalData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class ServicesMappingExtension
    {
        public static ServicesModel ToModel(this ServicesData entity)
        {
            return entity.MapTo<ServicesData, ServicesModel>();
        }

        public static ServicesData ToEntity(this ServicesModel model)
        {
            return model.MapTo<ServicesModel, ServicesData>();
        }

        public static ServicesData ToEntity(this ServicesModel model, ServicesData destination)
        {
            return model.MapTo(destination);
        }
    }
}
