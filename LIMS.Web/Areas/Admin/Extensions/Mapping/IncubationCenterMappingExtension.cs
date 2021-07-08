using LIMS.Domain.Bali;
using LIMS.Web.Areas.Admin.Models.Bali;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class IncubationCenterMappingExtension
    {
        public static IncubationCenterModel ToModel(this IncubationCenter bali)
        {
            return bali.MapTo<IncubationCenter, IncubationCenterModel>();
        }
        public static IncubationCenter ToEntity(this IncubationCenterModel bali)
        {
            return bali.MapTo<IncubationCenterModel, IncubationCenter>();
        }
        public static IncubationCenter ToEntity(this IncubationCenterModel source, IncubationCenter destination)
        {
            return source.MapTo(destination);
        }
    }
}
