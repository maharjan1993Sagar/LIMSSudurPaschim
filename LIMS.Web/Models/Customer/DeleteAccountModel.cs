using LIMS.Core.ModelBinding;
using LIMS.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace LIMS.Web.Models.Customer
{
    public partial class DeleteAccountModel : BaseModel
    {
        [DataType(DataType.Password)]
        [LIMSResourceDisplayName("Account.DeleteAccount.Fields.Password")]
        public string Password { get; set; }

    }
}