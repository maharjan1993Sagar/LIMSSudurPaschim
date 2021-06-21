using LIMS.Core.ModelBinding;
using LIMS.Core.Models;

namespace LIMS.Web.Areas.Admin.Models.ExternalAuthentication
{
    public partial class AuthenticationMethodModel : BaseModel
    {
        [LIMSResourceDisplayName("Admin.Configuration.ExternalAuthenticationMethods.Fields.FriendlyName")]
        
        public string FriendlyName { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.ExternalAuthenticationMethods.Fields.SystemName")]
        
        public string SystemName { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.ExternalAuthenticationMethods.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.ExternalAuthenticationMethods.Fields.IsActive")]
        public bool IsActive { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.ExternalAuthenticationMethods.Fields.Configure")]
        public string ConfigurationUrl { get; set; }
    }
}