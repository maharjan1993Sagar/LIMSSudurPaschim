using AutoMapper;
using LIMS.Core.Infrastructure.Mapper;
using LIMS.Domain.Activities;
using LIMS.Web.Areas.Admin.Models.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Infrastructure.Mapper.Profiles
{
    public class ActivityTargetMappingProfile:Profile,IMapperProfile
    {
        public ActivityTargetMappingProfile()
        {
            CreateMap<TargetRegister, TargetModel>().ReverseMap();
        }
        public int Order { get; set; } = 0;
    }
}
