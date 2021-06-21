using LIMS.Domain.Activities;
using LIMS.Web.Areas.Admin.Models.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class ActivityMappingExtension
    {
        public static ActivityModel ToModel(this Activity entity)
        {
            return entity.MapTo<Activity, ActivityModel>();
        }

        public static Activity ToEntity(this ActivityModel model)
        {
            return model.MapTo<ActivityModel, Activity>();
        }
        public static Activity ToEntity(this ActivityModel model, Activity destination)
        {
            return model.MapTo(destination);
        }
    }
}
