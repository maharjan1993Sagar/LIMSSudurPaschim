using LIMS.Domain.Services;
using LIMS.Web.Areas.Admin.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class PregnencyDiagnosisMappingExtension
    {
        public static PregnencyDiagnosisModel ToModel(this PregnencyDiagnosis entity)
        {
            return entity.MapTo<PregnencyDiagnosis, PregnencyDiagnosisModel>();
        }

        public static PregnencyDiagnosis ToEntity(this PregnencyDiagnosisModel model)
        {
            return model.MapTo<PregnencyDiagnosisModel, PregnencyDiagnosis>();
        }

        public static PregnencyDiagnosis ToEntity(this PregnencyDiagnosisModel model, PregnencyDiagnosis destination)
        {
            return model.MapTo(destination);
        }
    }
}
