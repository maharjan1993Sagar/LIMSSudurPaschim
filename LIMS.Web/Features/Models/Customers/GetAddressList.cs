using LIMS.Domain.Customers;
using LIMS.Domain.Localization;
using LIMS.Domain.Stores;
using LIMS.Web.Models.Customer;
using MediatR;

namespace LIMS.Web.Features.Models.Customers
{
    public class GetAddressList : IRequest<CustomerAddressListModel>
    {
        public Store Store { get; set; }
        public Customer Customer { get; set; }
        public Language Language { get; set; }
    }
}
