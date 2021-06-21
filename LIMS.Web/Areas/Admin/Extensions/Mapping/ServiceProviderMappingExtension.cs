using LIMS.Domain.Services;
using LIMS.Web.Areas.Admin.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class ServiceProviderMappingExtension
    {
        public static ServiceProviderModel ToModel(this ServiceProvider entity)
        {
            return entity.MapTo<ServiceProvider, ServiceProviderModel>();
        }
        public static ServiceProvider ToEntity(this ServiceProviderModel model)
        {
            return model.MapTo<ServiceProviderModel, ServiceProvider>();
        }
        public static ServiceProvider ToEntity(this ServiceProviderModel source,  ServiceProvider destination)
        {
            return source.MapTo(destination);
        }
    }
}
