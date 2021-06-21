using AutoMapper;
using LIMS.Domain.Customers;
using LIMS.Core.Infrastructure.Mapper;
using LIMS.Web.Areas.Admin.Extensions;
using LIMS.Web.Areas.Admin.Models.Customers;

namespace LIMS.Web.Areas.Admin.Infrastructure.Mapper.Profiles
{
    public class CustomerAttributeProfile : Profile, IMapperProfile
    {
        public CustomerAttributeProfile()
        {
            CreateMap<CustomerAttribute, CustomerAttributeModel>()
                .ForMember(dest => dest.Locales, mo => mo.Ignore())
                .ForMember(dest => dest.AttributeControlTypeName, mo => mo.Ignore())
                .ForMember(dest => dest.Locales, mo => mo.Ignore());
            CreateMap<CustomerAttributeModel, CustomerAttribute>()
                .ForMember(dest => dest.Id, mo => mo.Ignore())
                .ForMember(dest => dest.Locales, mo => mo.MapFrom(x => x.Locales.ToLocalizedProperty()))
                .ForMember(dest => dest.CustomerAttributeValues, mo => mo.Ignore());
        }

        public int Order => 0;
    }
}