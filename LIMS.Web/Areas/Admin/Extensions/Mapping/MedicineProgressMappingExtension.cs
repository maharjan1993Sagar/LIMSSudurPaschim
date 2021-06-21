using LIMS.Domain.MedicineInventory;
using LIMS.Web.Areas.Admin.Models.MedicineInventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class MedicineProgressMappingExtension
    {
        public static MedicineProgressModel ToModel(this MedicineProgress entity)
        {
            return entity.MapTo<MedicineProgress, MedicineProgressModel>();
        }

        public static MedicineProgress ToEntity(this MedicineProgressModel model)
        {
            return model.MapTo<MedicineProgressModel, MedicineProgress>();
        }

        public static MedicineProgress ToEntity(this MedicineProgressModel model, MedicineProgress destination)
        {
            return model.MapTo(destination);
        }
    }
}
