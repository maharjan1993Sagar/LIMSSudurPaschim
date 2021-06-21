using LIMS.Domain.MedicineInventory;
using LIMS.Web.Areas.Admin.Models.MedicineInventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class MedicineDemandMappingExtension
    {
        public static MedicineDemandmodel ToModel(this MedicineDemand entity)
        {
            return entity.MapTo<MedicineDemand, MedicineDemandmodel>();
        }

        public static MedicineDemand ToEntity(this MedicineDemandmodel model)
        {
            return model.MapTo<MedicineDemandmodel, MedicineDemand>();
        }

        public static MedicineDemand ToEntity(this MedicineDemandmodel model, MedicineDemand destination)
        {
            return model.MapTo(destination);
        }
    }
}
