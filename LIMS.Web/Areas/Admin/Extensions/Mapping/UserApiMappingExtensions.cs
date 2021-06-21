using LIMS.Domain.Customers;
using LIMS.Web.Areas.Admin.Models.Customers;

namespace LIMS.Web.Areas.Admin.Extensions
{
    public static class UserApiMappingExtensions
    {
        public static UserApiModel ToModel(this UserApi entity)
        {
            return entity.MapTo<UserApi, UserApiModel>();
        }

        public static UserApi ToEntity(this UserApiModel model)
        {
            return model.MapTo<UserApiModel, UserApi>();
        }

        public static UserApi ToEntity(this UserApiModel model, UserApi destination)
        {
            return model.MapTo(destination);
        }
    }
}