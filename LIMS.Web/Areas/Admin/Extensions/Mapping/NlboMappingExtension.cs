using LIMS.Domain.MoAMAC;
using LIMS.Web.Areas.Admin.Models.MoAMAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class NlboMappingExtension
    {
          public static NlboModel ToModel(this Nlbo entity)
        {
            return entity.MapTo<Nlbo, NlboModel>();
        }

        public static Nlbo ToEntity(this NlboModel model)
        {
            return model.MapTo<NlboModel, Nlbo>();
        }
        public static Nlbo ToEntity(this NlboModel model, Nlbo destination)
        {
            return model.MapTo(destination);
        }
    }
}
