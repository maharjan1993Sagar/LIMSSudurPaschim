using LIMS.Core.ModelBinding;
using LIMS.Core.Models;

namespace LIMS.Web.Areas.Admin.Models.Customers
{
    public partial class CustomerActionTypeModel : BaseEntityModel
    {
        [LIMSResourceDisplayName("Admin.Customer.ActionType.Fields.Name")]
        public string Name { get; set; }
        [LIMSResourceDisplayName("Admin.Customer.ActionType.Fields.Enabled")]
        public bool Enabled { get; set; }
    }
}