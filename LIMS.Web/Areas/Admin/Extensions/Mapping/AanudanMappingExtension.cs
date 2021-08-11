using LIMS.Domain.Bali;
using LIMS.Web.Areas.Admin.Models.Bali.Aanudan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class AanudanMappingExtension
    {
        public static AanudanModel ToModel(this AanudanKokaryakram entity)
        {
            return entity.MapTo<AanudanKokaryakram, AanudanModel>();
        }

        public static AanudanKokaryakram ToEntity(this AanudanModel model)
        {
            return model.MapTo<AanudanModel, AanudanKokaryakram>();
        }
        public static AanudanKokaryakram ToEntity(this AanudanModel model, AanudanKokaryakram destination)
        {
            return model.MapTo(destination);
        }
    }
}
