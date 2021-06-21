using AutoMapper;
using LIMS.Core.Infrastructure.Mapper;
using LIMS.Domain.VaccinationInventory;
using LIMS.Web.Areas.Admin.Models.VaccinationInventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Infrastructure.Mapper.Profiles
{
    public class ReceivedVaccineProfile:Profile,IMapperProfile
    {
        public ReceivedVaccineProfile()
        {
            CreateMap<ReceivedVaccine, ReceivedVaccineModel>().ReverseMap();

        }
        public int Order { get; set; } = 0;
    }
}
