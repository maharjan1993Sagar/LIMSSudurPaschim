using AutoMapper;
using LIMS.Core.Infrastructure.Mapper;
using LIMS.Domain.AnimalHealth;
using LIMS.Web.Areas.Admin.Models.AnimalHealth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Infrastructure.Mapper.Profiles
{
    public class FarmForPurnaKhopProfile:Profile,IMapperProfile
    {
        public FarmForPurnaKhopProfile()
        {
            CreateMap<FarmForPurnaKhop, FarmForPurnaKhopModel>().ReverseMap();
        }
        public int Order { get; set; } = 0;

    }
}
