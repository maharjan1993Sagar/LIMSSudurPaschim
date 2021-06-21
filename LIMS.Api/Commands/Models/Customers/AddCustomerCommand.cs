using LIMS.Api.DTOs.Customers;
using MediatR;

namespace LIMS.Api.Commands.Models.Customers
{
    public class AddCustomerCommand : IRequest<CustomerDto>
    {
        public CustomerDto Model { get; set; }
    }
}
