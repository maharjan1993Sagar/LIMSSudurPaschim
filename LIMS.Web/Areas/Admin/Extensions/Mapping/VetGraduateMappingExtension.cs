using LIMS.Domain.Professionals;
using LIMS.Web.Areas.Admin.Models.Professionals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class VetGraduateMappingExtension
    {
        public static VetGraduateModel ToModel(this VetGraduate entity)
        {
            return entity.MapTo<VetGraduate, VetGraduateModel>();
        }

        public static VetGraduate ToEntity(this VetGraduateModel model)
        {
            return model.MapTo<VetGraduateModel, VetGraduate>();
        }

        public static VetGraduate ToEntity(this VetGraduateModel model, VetGraduate destination)
        {
            return model.MapTo(destination);
        }
    }
}
