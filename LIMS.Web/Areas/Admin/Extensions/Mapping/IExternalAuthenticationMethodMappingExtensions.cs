using LIMS.Services.Authentication.External;
using LIMS.Web.Areas.Admin.Models.ExternalAuthentication;

namespace LIMS.Web.Areas.Admin.Extensions
{
    public static class IExternalAuthenticationMethodMappingExtensions
    {
        public static AuthenticationMethodModel ToModel(this IExternalAuthenticationMethod entity)
        {
            return entity.MapTo<IExternalAuthenticationMethod, AuthenticationMethodModel>();
        }
    }
}