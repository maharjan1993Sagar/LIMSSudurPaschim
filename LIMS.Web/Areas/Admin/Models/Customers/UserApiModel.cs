using LIMS.Core.ModelBinding;
using LIMS.Core.Models;

namespace LIMS.Web.Areas.Admin.Models.Customers
{
    public partial class UserApiModel : BaseEntityModel
    {
        [LIMSResourceDisplayName("Admin.System.UserApi.Email")]
        public string Email { get; set; }

        [LIMSResourceDisplayName("Admin.System.UserApi.Password")]
        public string Password { get; set; }

        [LIMSResourceDisplayName("Admin.System.UserApi.IsActive")]
        public bool IsActive { get; set; }

    }
}
