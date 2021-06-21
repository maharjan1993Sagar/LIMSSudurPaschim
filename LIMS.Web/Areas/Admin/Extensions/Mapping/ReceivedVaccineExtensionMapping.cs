using LIMS.Domain.VaccinationInventory;
using LIMS.Web.Areas.Admin.Models.VaccinationInventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class ReceivedVaccineExtensionMapping
    {
        public static ReceivedVaccineModel ToModel(this ReceivedVaccine entity)
        {
            return entity.MapTo<ReceivedVaccine, ReceivedVaccineModel>();
        }

        public static ReceivedVaccine ToEntity(this ReceivedVaccineModel model)
        {
            return model.MapTo<ReceivedVaccineModel, ReceivedVaccine>();
        }

        public static ReceivedVaccine ToEntity(this ReceivedVaccineModel model, ReceivedVaccine destination)
        {
            return model.MapTo(destination);
        }
    }
}
