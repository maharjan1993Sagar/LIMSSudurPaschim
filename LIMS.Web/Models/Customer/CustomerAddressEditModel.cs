using LIMS.Core.Models;
using LIMS.Web.Models.Common;

namespace LIMS.Web.Models.Customer
{
    public partial class CustomerAddressEditModel : BaseModel
    {
        public CustomerAddressEditModel()
        {
            Address = new AddressModel();
        }
        public AddressModel Address { get; set; }
    }
}