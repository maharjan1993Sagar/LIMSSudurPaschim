using AutoMapper;
using LIMS.Domain.Directory;
using LIMS.Core.Infrastructure.Mapper;
using LIMS.Web.Areas.Admin.Extensions;
using LIMS.Web.Areas.Admin.Models.Directory;

namespace LIMS.Web.Areas.Admin.Infrastructure.Mapper.Profiles
{
    public class StateProvinceProfile : Profile, IMapperProfile
    {
        public StateProvinceProfile()
        {
            CreateMap<StateProvince, StateProvinceModel>()
                .ForMember(dest => dest.Locales, mo => mo.Ignore());
            CreateMap<StateProvinceModel, StateProvince>()
                .ForMember(dest => dest.Id, mo => mo.Ignore())
                .ForMember(dest => dest.Locales, mo => mo.MapFrom(x => x.Locales.ToLocalizedProperty()));
        }

        public int Order => 0;
    }
}