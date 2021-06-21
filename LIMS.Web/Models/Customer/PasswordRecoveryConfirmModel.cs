using LIMS.Core.ModelBinding;
using LIMS.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace LIMS.Web.Models.Customer
{
    public partial class PasswordRecoveryConfirmModel : BaseModel
    {
        [DataType(DataType.Password)]
        [LIMSResourceDisplayName("Account.PasswordRecovery.NewPassword")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [LIMSResourceDisplayName("Account.PasswordRecovery.ConfirmNewPassword")]
        public string ConfirmNewPassword { get; set; }

        public bool DisablePasswordChanging { get; set; }
        public string Result { get; set; }
    }
}