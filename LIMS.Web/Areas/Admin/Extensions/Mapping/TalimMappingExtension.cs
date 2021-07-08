using LIMS.Domain.Bali;
using LIMS.Web.Areas.Admin.Models.Bali;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class TalimMappingExtension
    {
        public static TalimModel ToModel(this Talim bali)
        {
            return bali.MapTo<Talim, TalimModel>();
        }
        public static Talim ToEntity(this TalimModel bali)
        {
            return bali.MapTo<TalimModel, Talim>();
        }
        public static Talim ToEntity(this TalimModel source, Talim destination)
        {
            return source.MapTo(destination);
        }
    }
}
