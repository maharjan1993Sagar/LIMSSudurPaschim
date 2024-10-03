using LIMS.Core.ModelBinding;
using LIMS.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace LIMS.Web.Models.Customer
{
    public class ResetModel : BaseEntityModel
    {
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [LIMSResourceDisplayName("Account.ResetModel.NewPassword")]

        public string NewPassword { get; set; }
       
    }
}
