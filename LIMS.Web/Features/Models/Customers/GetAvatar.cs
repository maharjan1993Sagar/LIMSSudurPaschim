using LIMS.Domain.Customers;
using LIMS.Web.Models.Customer;
using MediatR;

namespace LIMS.Web.Features.Models.Customers
{
    public class GetAvatar : IRequest<CustomerAvatarModel>
    {
        public Customer Customer { get; set; }
    }
}
