﻿using AutoMapper;
using LIMS.Core.Infrastructure.Mapper;
using LIMS.Domain.Recording;
using LIMS.Web.Areas.Admin.Models.Recording;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Infrastructure.Mapper.Profiles
{
    public class MilkRecordingMappingProfile:Profile,IMapperProfile
    {
        public MilkRecordingMappingProfile()
        {
            CreateMap<MilkRecording, MilkRecordingModel>().ReverseMap();
        }
        public int Order => 0;

    }
}
