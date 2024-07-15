using LIMS.Domain.Bali;
using LIMS.Web.Areas.Admin.Models.Bali;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class SubSectorMappingExtension
    {

        public static SubSectorModel ToModel(this SubSector bali)
        {
            return bali.MapTo<SubSector, SubSectorModel>();
        }
        public static SubSector ToEntity(this SubSectorModel bali)
        {
            return bali.MapTo<SubSectorModel, SubSector>();
        }
        public static SubSector ToEntity(this SubSectorModel source, SubSector destination)
        {
            return source.MapTo(destination);
        }
    }
}
