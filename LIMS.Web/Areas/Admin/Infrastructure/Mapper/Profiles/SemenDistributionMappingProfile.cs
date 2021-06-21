using AutoMapper;
using LIMS.Core.Infrastructure.Mapper;
using LIMS.Domain.SemenDistribution;
using LIMS.Web.Areas.Admin.Models.Semen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Infrastructure.Mapper.Profiles
{
    public class SemenDistributionMappingProfile:Profile,IMapperProfile
    {
        public SemenDistributionMappingProfile()
        {
            CreateMap<SemenDistribution, SemenDistributionModel>().ReverseMap();
        }
        public int Order { get; set; } = 0;
    }
}
