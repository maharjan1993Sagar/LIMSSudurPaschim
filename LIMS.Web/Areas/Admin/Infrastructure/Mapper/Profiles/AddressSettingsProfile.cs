using AutoMapper;
using LIMS.Domain.Common;
using LIMS.Core.Infrastructure.Mapper;
using LIMS.Web.Areas.Admin.Models.Settings;

namespace LIMS.Web.Areas.Admin.Infrastructure.Mapper.Profiles
{
    public class AddressSettingsProfile : Profile, IMapperProfile
    {
        public AddressSettingsProfile()
        {
            CreateMap<AddressSettings, CustomerUserSettingsModel.AddressSettingsModel>()
                .ForMember(dest => dest.GenericAttributes, mo => mo.Ignore());
            CreateMap<CustomerUserSettingsModel.AddressSettingsModel, AddressSettings>();
        }

        public int Order => 0;
    }
}