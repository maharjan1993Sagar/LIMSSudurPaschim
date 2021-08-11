using LIMS.Domain.Bali;
using LIMS.Web.Areas.Admin.Models.Bali;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class FarmLabMappingExtension
    {
        public static FarmLabResourcesModel ToModel(this FarmLabResources bali)
        {
            return bali.MapTo<FarmLabResources, FarmLabResourcesModel>();
        }
        public static FarmLabResources ToEntity(this FarmLabResourcesModel bali)
        {
            return bali.MapTo<FarmLabResourcesModel, FarmLabResources>();
        }
        public static FarmLabResources ToEntity(this FarmLabResourcesModel source, FarmLabResources destination)
        {
            return source.MapTo(destination);
        }
    }
}
