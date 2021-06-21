using LIMS.Domain.MoAMAC;
using LIMS.Web.Areas.Admin.Models.MoAMAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class VhlsecMappingExtension
    {
        public static VhlsecModel ToModel(this Vhlsec entity)
        {
            return entity.MapTo<Vhlsec, VhlsecModel>();
        }

        public static Vhlsec ToEntity(this VhlsecModel model)
        {
            return model.MapTo<VhlsecModel, Vhlsec>();
        }

        public static Vhlsec ToEntity(this VhlsecModel model, Vhlsec destination)
        {
            return model.MapTo(destination);
        }
    }
}
