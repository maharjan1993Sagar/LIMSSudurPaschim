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
    public class ActivityMappingProfile:Profile,IMapperProfile
    {
        public ActivityMappingProfile()
        {
            CreateMap<Activity, ActivityModel>().ReverseMap();
        }
        public int Order { get; set; } = 0;
    }
}
