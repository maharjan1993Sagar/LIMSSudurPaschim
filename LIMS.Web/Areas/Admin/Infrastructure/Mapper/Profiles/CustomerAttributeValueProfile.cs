using AutoMapper;
using LIMS.Domain.Customers;
using LIMS.Core.Infrastructure.Mapper;
using LIMS.Web.Areas.Admin.Extensions;
using LIMS.Web.Areas.Admin.Models.Customers;

namespace LIMS.Web.Areas.Admin.Infrastructure.Mapper.Profiles
{
    public class CustomerAttributeValueProfile : Profile, IMapperProfile
    {
        public CustomerAttributeValueProfile()
        {
            CreateMap<CustomerAttributeValue, CustomerAttributeValueModel>()
                .ForMember(dest => dest.Locales, mo => mo.Ignore())
                .ForMember(dest => dest.Locales, mo => mo.Ignore());
            CreateMap<CustomerAttributeValueModel, CustomerAttributeValue>()
                .ForMember(dest => dest.Id, mo => mo.Ignore())
                .ForMember(dest => dest.Locales, mo => mo.MapFrom(x => x.Locales.ToLocalizedProperty()));
        }

        public int Order => 0;
    }
}