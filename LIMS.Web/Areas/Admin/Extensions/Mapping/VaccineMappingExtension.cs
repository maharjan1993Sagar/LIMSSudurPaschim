using LIMS.Domain.Services;
using LIMS.Domain.Vaccination;
using LIMS.Services.AnimalHealth;
using LIMS.Web.Areas.Admin.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class VaccineMappingExtension
    {
        public static VaccinationServiceModel ToModel(this AnimalVaccination entity)
        {
            return entity.MapTo<AnimalVaccination, VaccinationServiceModel>();
        }

        public static AnimalVaccination ToEntity(this VaccinationServiceModel model)
        {
            return model.MapTo<VaccinationServiceModel, AnimalVaccination>();
        }

        public static AnimalVaccination ToEntity(this VaccinationServiceModel model, AnimalVaccination destination)
        {
            return model.MapTo(destination);
        }
    }
}
