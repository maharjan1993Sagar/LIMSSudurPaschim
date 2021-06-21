using LIMS.Api.Extensions;
using LIMS.Domain.MoAMAC;
using LIMS.Web.Areas.Admin.Models.MoAMAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class MoALACAttributeMapping
    {
        
            public static MoAMACModel ToModel(this MoAMAC entity)
            {
                return entity.MapTo<MoAMAC, MoAMACModel>();
            }

            public static MoAMAC ToEntity(this MoAMACModel model)
            {
                return model.MapTo<MoAMACModel, MoAMAC>();
            }

            public static MoAMAC ToEntity(this MoAMACModel model, MoAMAC destination)
            {
                return model.MapTo(destination);
            }
        }
    
}
