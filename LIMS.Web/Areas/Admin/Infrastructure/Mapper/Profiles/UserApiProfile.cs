using AutoMapper;
using LIMS.Domain.Customers;
using LIMS.Core.Infrastructure.Mapper;
using LIMS.Web.Areas.Admin.Models.Customers;

namespace LIMS.Web.Areas.Admin.Infrastructure.Mapper.Profiles
{
    public class UserApiProfile : Profile, IMapperProfile
    {
        public UserApiProfile()
        {
            CreateMap<UserApi, UserApiModel>()
                .ForMember(dest => dest.Password, mo => mo.Ignore());
            CreateMap<UserApiModel, UserApi>()
                .ForMember(dest => dest.Password, mo => mo.Ignore())
                .ForMember(dest => dest.Id, mo => mo.Ignore());
        }

        public int Order => 0;
    }
}