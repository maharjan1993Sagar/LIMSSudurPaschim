using LIMS.Api.DTOs.Customers;
using MediatR;

namespace LIMS.Api.Commands.Models.Customers
{
    public class DeleteCustomerAddressCommand : IRequest<bool>
    {
        public CustomerDto Customer { get; set; }
        public AddressDto Address { get; set; }
    }
}
