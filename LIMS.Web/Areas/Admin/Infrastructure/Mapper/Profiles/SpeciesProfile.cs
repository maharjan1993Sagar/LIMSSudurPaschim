﻿using AutoMapper;
using LIMS.Core.Infrastructure.Mapper;
using LIMS.Domain.Breed;
using LIMS.Web.Areas.Admin.Models.Breed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Infrastructure.Mapper.Profiles
{
    public class SpeciesProfile:Profile,IMapperProfile
    {
        public SpeciesProfile()
        {

            CreateMap<Species, SpeciesModel>().ReverseMap();
        }
        public int Order => 0;
    }
}
