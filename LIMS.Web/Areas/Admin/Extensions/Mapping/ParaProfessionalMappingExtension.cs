using LIMS.Domain.Professionals;
using LIMS.Domain.Vaccination;
using LIMS.Web.Areas.Admin.Models.Professionals;
using LIMS.Web.Areas.Admin.Models.Vaccination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class ParaProfessionalMappingExtension
    {
        public static ParaProfessionalsModel ToModel(this ParaProfessionals entity)
        {
            return entity.MapTo<ParaProfessionals, ParaProfessionalsModel>();
        }

        public static ParaProfessionals ToEntity(this ParaProfessionalsModel model)
        {
            return model.MapTo<ParaProfessionalsModel, ParaProfessionals>();
        }

        public static ParaProfessionals ToEntity(this ParaProfessionalsModel model, ParaProfessionals destination)
        {
            return model.MapTo(destination);
        }
    }
}
