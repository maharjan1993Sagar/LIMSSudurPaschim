using LIMS.Core.Models;
using LIMS.Web.Areas.Admin.Models.Common;

namespace LIMS.Web.Areas.Admin.Models.Customers
{
    public partial class CustomerAddressModel : BaseModel
    {
        public string CustomerId { get; set; }

        public AddressModel Address { get; set; }
    }
}