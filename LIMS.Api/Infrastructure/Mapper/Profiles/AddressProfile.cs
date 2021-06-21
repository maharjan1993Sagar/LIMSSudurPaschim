using AutoMapper;
using LIMS.Api.DTOs.Customers;
using LIMS.Domain.Common;
using LIMS.Core.Infrastructure.Mapper;

namespace LIMS.Api.Infrastructure.Mapper
{
    public class AddressProfile : Profile, IMapperProfile
    {
        public AddressProfile()
        {
            CreateMap<AddressDto, Address>()
                .ForMember(dest => dest.CustomAttributes, mo => mo.Ignore())
                .ForMember(dest => dest.GenericAttributes, mo => mo.Ignore());

            CreateMap<Address, AddressDto>();
        }

        public int Order => 1;
    }
}
