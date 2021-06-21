using AutoMapper;
using LIMS.Core.Infrastructure.Mapper;
using LIMS.Domain.Breed;
using LIMS.Web.Areas.Admin.Models.Breed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Infrastructure.Mapper.Profiles
{
    public class BreedProfile: Profile, IMapperProfile
    {
        public BreedProfile()
        {
            CreateMap<BreedReg, BreedModel>().ReverseMap();

        }
        public int Order => 0; 
    }
  
}
