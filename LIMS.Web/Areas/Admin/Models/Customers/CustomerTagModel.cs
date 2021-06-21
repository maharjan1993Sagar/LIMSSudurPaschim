using LIMS.Core.ModelBinding;
using LIMS.Core.Models;

namespace LIMS.Web.Areas.Admin.Models.Customers
{
    public partial class CustomerTagModel : BaseEntityModel
    {
        [LIMSResourceDisplayName("Admin.Customers.CustomerTags.Fields.Name")]

        public string Name { get; set; }
    }
}