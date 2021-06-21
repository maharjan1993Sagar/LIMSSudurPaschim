using LIMS.Domain.Customers;
using LIMS.Domain.Stores;
using MediatR;

namespace LIMS.Web.Commands.Models.Customers
{
    public class DeleteAccountCommand : IRequest<bool>
    {
        public Customer Customer { get; set; }
        public Store Store { get; set; }
    }
}
