using LIMS.Domain.Bali;
using LIMS.Web.Areas.Admin.Models.Bali;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class BaliMappingExtension
    {

        public static BaliModel ToModel(this BaliRegister bali)
        {
            return bali.MapTo<BaliRegister, BaliModel>();
        }
        public static BaliRegister ToEntity(this BaliModel bali)
        {
            return bali.MapTo<BaliModel, BaliRegister>();
        }
        public static BaliRegister ToEntity(this BaliModel source, BaliRegister destination)
        {
            return source.MapTo(destination);
        }
    }
}
