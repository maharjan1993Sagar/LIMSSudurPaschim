using LIMS.Domain.Activities;
using LIMS.Web.Areas.Admin.Models.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class ActivityProgressMappingExtension
    {
          public static ActivityProgressModel ToModel(this ActivityProgress entity)
        {
            return entity.MapTo<ActivityProgress, ActivityProgressModel>();
        }

        public static ActivityProgress ToEntity(this ActivityProgressModel model)
        {
            return model.MapTo<ActivityProgressModel, ActivityProgress>();
        }
        public static ActivityProgress ToEntity(this ActivityProgressModel model, ActivityProgress destination)
        {
            return model.MapTo(destination);
        }

    }
}
