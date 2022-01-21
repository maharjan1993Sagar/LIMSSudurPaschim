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
            CreateMap<LivestockBreed, LivestockBreedModel>().ReverseMap();
            CreateMap<LivestockSpecies, LivestockSpeciesModel>().ReverseMap();
            CreateMap<CropsSeason, CropsSeasonModel>().ReverseMap();


        }
        public int Order => 0; 
    }
  
}
