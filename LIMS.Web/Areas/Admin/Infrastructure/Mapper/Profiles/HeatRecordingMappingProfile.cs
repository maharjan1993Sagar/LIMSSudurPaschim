using AutoMapper;
using LIMS.Core.Infrastructure.Mapper;
using LIMS.Domain.Services;
using LIMS.Services.AnimalBreeding;
using LIMS.Web.Areas.Admin.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Infrastructure.Mapper.Profiles
{
    public class HeatRecordingMappingProfile:Profile,IMapperProfile
    {
        public HeatRecordingMappingProfile()
        {
            CreateMap<HeatRecording, HeatRecordingModel>().ReverseMap();
        }
        public int Order => 0;
    }
}
