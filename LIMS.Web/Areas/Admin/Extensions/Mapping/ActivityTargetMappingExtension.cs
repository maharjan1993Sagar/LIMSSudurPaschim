using LIMS.Domain.Activities;
using LIMS.Web.Areas.Admin.Models.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class ActivityTargetMappingExtension
    {
        public static TargetModel ToModel(this TargetRegister entity)
        {
            return entity.MapTo<TargetRegister, TargetModel>();
        }

        public static TargetRegister ToEntity(this TargetModel model)
        {
            return model.MapTo<TargetModel, TargetRegister>();
        }
        public static TargetRegister ToEntity(this TargetModel model, TargetRegister destination)
        {
            return model.MapTo(destination);
        }
    }
}
