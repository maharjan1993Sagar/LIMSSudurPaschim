using LIMS.Domain.Customers;
using LIMS.Web.Models.Customer;
using MediatR;

namespace LIMS.Web.Features.Models.Customers
{
    public class GetSubAccount : IRequest<SubAccountModel>
    {
        public Customer CurrentCustomer { get; set; }
        public string CustomerId { get; set; }
    }
}
