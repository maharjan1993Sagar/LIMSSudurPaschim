using AutoMapper;
using LIMS.Domain.Customers;
using LIMS.Core.Infrastructure.Mapper;
using LIMS.Web.Areas.Admin.Models.Customers;

namespace LIMS.Web.Areas.Admin.Infrastructure.Mapper.Profiles
{
    public class CustomerTagProfile : Profile, IMapperProfile
    {
        public CustomerTagProfile()
        {
            CreateMap<CustomerTag, CustomerTagModel>();
            CreateMap<CustomerTagModel, CustomerTag>()
                .ForMember(dest => dest.Id, mo => mo.Ignore());
        }

        public int Order => 0;
    }
}