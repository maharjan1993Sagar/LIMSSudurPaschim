using LIMS.Domain.MoAMAC;
using LIMS.Web.Areas.Admin.Models.MoAMAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class DolfdAttributeMapping
    {
        public static DolfdModel ToModel(this Dolfd entity)
        {
            return entity.MapTo<Dolfd, DolfdModel>();
        }

        public static Dolfd ToEntity(this DolfdModel model)
        {
            return model.MapTo<DolfdModel, Dolfd>();
        }

        public static Dolfd ToEntity(this DolfdModel model, Dolfd destination)
        {
            return model.MapTo(destination);
        }
    }
}
