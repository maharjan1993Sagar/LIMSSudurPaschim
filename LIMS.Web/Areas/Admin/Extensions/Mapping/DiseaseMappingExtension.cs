using LIMS.Domain.BasicSetup;
using LIMS.Web.Areas.Admin.Models.BasicSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public  static class DiseaseMappingExtension
    {
        public static DiseaseModel ToModel(this Disease entity)
        {
            return entity.MapTo<Disease, DiseaseModel>();
        }

        public static Disease ToEntity(this DiseaseModel model)
        {
            return model.MapTo<DiseaseModel, Disease>();
        }

        public static Disease ToEntity(this DiseaseModel model, Disease destination)
        {
            return model.MapTo(destination);
        }
    }
}
