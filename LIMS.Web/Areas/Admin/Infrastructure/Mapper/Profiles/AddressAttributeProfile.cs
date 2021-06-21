using AutoMapper;
using LIMS.Domain.Common;
using LIMS.Core.Infrastructure.Mapper;
using LIMS.Web.Areas.Admin.Extensions;
using LIMS.Web.Areas.Admin.Models.Common;

namespace LIMS.Web.Areas.Admin.Infrastructure.Mapper.Profiles
{
    public class AddressAttributeProfile : Profile, IMapperProfile
    {
        public AddressAttributeProfile()
        {
            CreateMap<AddressAttribute, AddressAttributeModel>()
                .ForMember(dest => dest.AttributeControlTypeName, mo => mo.Ignore())
                .ForMember(dest => dest.Locales, mo => mo.Ignore());
            CreateMap<AddressAttributeModel, AddressAttribute>()
                .ForMember(dest => dest.Id, mo => mo.Ignore())
                .ForMember(dest => dest.Locales, mo => mo.MapFrom(x => x.Locales.ToLocalizedProperty()))
                .ForMember(dest => dest.AddressAttributeValues, mo => mo.Ignore());
        }

        public int Order => 0;
    }
}