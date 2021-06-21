using AutoMapper;
using LIMS.Core.Infrastructure.Mapper;
using LIMS.Domain.MedicineInventory;
using LIMS.Services.MedicineInventory;
using LIMS.Web.Areas.Admin.Models.MedicineInventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Infrastructure.Mapper.Profiles
{
    public class MedicineInventoryProfile:Profile,IMapperProfile
    {
        public MedicineInventoryProfile()
        {
            CreateMap<MedicineProgress, MedicineProgressModel>();
            CreateMap<MedicineDemand, MedicineDemandmodel>();
            CreateMap<ReceivedMedicine, ReceivedMedicineModel>();

        }
        public int Order { get; set; } = 0;
    }
}
