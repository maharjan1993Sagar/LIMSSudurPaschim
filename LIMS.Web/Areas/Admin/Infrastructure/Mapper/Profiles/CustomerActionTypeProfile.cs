using AutoMapper;
using LIMS.Domain.Customers;
using LIMS.Core.Infrastructure.Mapper;
using LIMS.Web.Areas.Admin.Models.Customers;

namespace LIMS.Web.Areas.Admin.Infrastructure.Mapper.Profiles
{
    public class CustomerActionTypeProfile : Profile, IMapperProfile
    {
        public CustomerActionTypeProfile()
        {
            CreateMap<CustomerActionTypeModel, CustomerActionType>()
                .ForMember(dest => dest.Id, mo => mo.Ignore())
                .ForMember(dest => dest.SystemKeyword, mo => mo.Ignore());
            CreateMap<CustomerActionType, CustomerActionTypeModel>();
        }

        public int Order => 0;
    }
}