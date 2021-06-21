using AutoMapper;
using LIMS.Api.DTOs.AINR;
using LIMS.Core.Infrastructure.Mapper;
using LIMS.Domain.Breed;

namespace LIMS.Api.Infrastructure.Mapper.Profiles
{
    public class SpeciesAndBreedProfile : Profile, IMapperProfile
    {
        public SpeciesAndBreedProfile()
        {
            CreateMap<Species, SpeciesDto>().ReverseMap();
            CreateMap<BreedReg, BreedDto>().ReverseMap();
        }

        public int Order => 0;
    }
}
