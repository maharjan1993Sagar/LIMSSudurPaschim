using LIMS.Domain.Services;
using LIMS.Web.Areas.Admin.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class AIMappingExtension
    {
        public static AIServiceModel ToModel(this AIService entity)
        {
            return entity.MapTo<AIService, AIServiceModel>();
        }

        public static AIService ToEntity(this AIServiceModel model)
        {
            return model.MapTo<AIServiceModel, AIService>();
        }
        public static AIService ToEntity(this AIServiceModel model, AIService destination)
        {
            return model.MapTo(destination);
        }
    }
}
