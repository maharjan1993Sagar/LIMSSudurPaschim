using LIMS.Domain.Vaccination;
using LIMS.Web.Areas.Admin.Models.Vaccination;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class VaccineTypeMappingExtension
    {
        public static VaccinationTypeModel ToModel(this VaccinationType entity)
        {
            return entity.MapTo<VaccinationType, VaccinationTypeModel>();
        }

        public static VaccinationType ToEntity(this VaccinationTypeModel model)
        {
            return model.MapTo<VaccinationTypeModel, VaccinationType>();
        }

        public static VaccinationType ToEntity(this VaccinationTypeModel model, VaccinationType destination)
        {
            return model.MapTo(destination);
        }
    }
}
