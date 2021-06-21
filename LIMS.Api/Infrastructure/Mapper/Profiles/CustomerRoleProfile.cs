using AutoMapper;
using LIMS.Api.DTOs.Customers;
using LIMS.Domain.Customers;
using LIMS.Core.Infrastructure.Mapper;

namespace LIMS.Api.Infrastructure.Mapper
{
    public class CustomerRoleProfile : Profile, IMapperProfile
    {
        public CustomerRoleProfile()
        {

            CreateMap<CustomerRoleDto, CustomerRole>()
                .ForMember(dest => dest.GenericAttributes, mo => mo.Ignore());

            CreateMap<CustomerRole, CustomerRoleDto>();

        }

        public int Order => 1;
    }
}
