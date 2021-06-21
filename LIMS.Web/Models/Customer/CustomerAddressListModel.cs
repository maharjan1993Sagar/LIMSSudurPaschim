using LIMS.Core.Models;
using LIMS.Web.Models.Common;
using System.Collections.Generic;

namespace LIMS.Web.Models.Customer
{
    public partial class CustomerAddressListModel : BaseModel
    {
        public CustomerAddressListModel()
        {
            Addresses = new List<AddressModel>();
        }

        public IList<AddressModel> Addresses { get; set; }
    }
}