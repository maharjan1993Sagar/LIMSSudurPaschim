﻿using AutoMapper;
using LIMS.Core.Infrastructure.Mapper;
using LIMS.Domain.RationBalance;
using LIMS.Web.Areas.Admin.Models.RashanBalance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Infrastructure.Mapper.Profiles
{
    public class RashanBalanceMappingProdile:Profile,IMapperProfile
    {
        public RashanBalanceMappingProdile()
        {
            CreateMap<FeedLibrary, FeedLibraryModel>().ReverseMap();
        }
        public int Order { get; set; } = 0;
    }
}
