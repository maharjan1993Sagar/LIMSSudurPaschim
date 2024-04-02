using LIMS.Domain.Bali;
using LIMS.Domain.BasicSetup;
using LIMS.Web.Areas.Admin.Models.Bali;
using LIMS.Web.Areas.Admin.Models.BasicSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class CropDiseasesMappingExtension
    {
        public static CropDiseasesModel ToModel(this CropDiseases entity)
        {
            return entity.MapTo<CropDiseases, CropDiseasesModel>();
        }

        public static CropDiseases ToEntity(this CropDiseasesModel model)
        {
            return model.MapTo<CropDiseasesModel, CropDiseases>();
        }

        public static CropDiseases ToEntity(this CropDiseasesModel model, CropDiseases destination)
        {
            return model.MapTo(destination);
        }
    }
}
