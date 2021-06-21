using LIMS.Domain.MoAMAC;
using LIMS.Web.Areas.Admin.Models.MoAMAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class LssMappingExtension
    {
        public static LssModel ToModel(this Lss entity)
        {
            return entity.MapTo<Lss, LssModel>();
        }

        public static Lss ToEntity(this LssModel model)
        {
            return model.MapTo<LssModel, Lss>();
        }

        public static Lss ToEntity(this LssModel model, Lss destination)
        {
            return model.MapTo(destination);
        }
    }
}
