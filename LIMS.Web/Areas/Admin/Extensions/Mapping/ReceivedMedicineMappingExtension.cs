using LIMS.Domain.MedicineInventory;
using LIMS.Web.Areas.Admin.Models.MedicineInventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class ReceivedMedicineMappingExtension
    {
        public static ReceivedMedicineModel ToModel(this ReceivedMedicine entity)
        {
            return entity.MapTo<ReceivedMedicine, ReceivedMedicineModel>();
        }

        public static ReceivedMedicine ToEntity(this ReceivedMedicineModel model)
        {
            return model.MapTo<ReceivedMedicineModel, ReceivedMedicine>();
        }

        public static ReceivedMedicine ToEntity(this ReceivedMedicineModel model, ReceivedMedicine destination)
        {
            return model.MapTo(destination);
        }
    }
}
