using AutoMapper;
using LIMS.Api.Infrastructure.Mapper;
using LIMS.Core.Infrastructure.Mapper;

namespace LIMS.Api.Tests.Helpers
{
    public static class Mapper
    {
        public static void Run()
        {
            var addressmapper = new AddressProfile();
            var customermapper = new CustomerProfile();
            var customerRolemapper = new CustomerRoleProfile();
            var picturemapper = new PictureProfile();

            var config = new MapperConfiguration(cfg => {                
                cfg.AddProfile(addressmapper.GetType());
                cfg.AddProfile(customermapper.GetType());
                cfg.AddProfile(customerRolemapper.GetType());
                cfg.AddProfile(picturemapper.GetType());
            });
            AutoMapperConfiguration.Init(config);
        }
    }
}
