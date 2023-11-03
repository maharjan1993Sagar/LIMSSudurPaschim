using AutoMapper;
using LIMS.Core.Infrastructure.Mapper;
using LIMS.Domain.Bali;
using LIMS.Web.Areas.Admin.Models.Bali;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Infrastructure.Mapper.Profiles
{
    public class PlantSoilProfile: Profile,IMapperProfile
    {
        public PlantSoilProfile()
        {
            CreateMap<PlantSoilManagement, PlantSoilManagementModel>().ReverseMap();

        }
        public int Order => 0;
    }
}
