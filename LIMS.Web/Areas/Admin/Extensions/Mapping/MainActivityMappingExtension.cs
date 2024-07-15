using LIMS.Domain.Bali;
using LIMS.Web.Areas.Admin.Models.Bali;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class MainActivityCodeMappingExtension
    {

        public static MainActivityCodeModel ToModel(this MainActivityCode bali)
        {
            return bali.MapTo<MainActivityCode, MainActivityCodeModel>();
        }
        public static MainActivityCode ToEntity(this MainActivityCodeModel bali)
        {
            return bali.MapTo<MainActivityCodeModel, MainActivityCode>();
        }
        public static MainActivityCode ToEntity(this MainActivityCodeModel source, MainActivityCode destination)
        {
            return source.MapTo(destination);
        }
    }
}
