using LIMS.Core.ModelBinding;
using LIMS.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace LIMS.Web.Models.Customer
{
    public partial class LoginModel : BaseModel
    {
        public bool CheckoutAsGuest { get; set; }

        [DataType(DataType.EmailAddress)]
        [LIMSResourceDisplayName("Account.Login.Fields.Email")]
        public string Email { get; set; }

        public bool UsernamesEnabled { get; set; }
        [LIMSResourceDisplayName("Account.Login.Fields.UserName")]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [LIMSResourceDisplayName("Account.Login.Fields.Password")]
        public string Password { get; set; }

        [LIMSResourceDisplayName("Account.Login.Fields.RememberMe")]
        public bool RememberMe { get; set; }

        public bool DisplayCaptcha { get; set; }

    }
}