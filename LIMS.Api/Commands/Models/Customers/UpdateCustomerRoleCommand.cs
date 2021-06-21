using LIMS.Api.DTOs.Customers;
using MediatR;

namespace LIMS.Api.Commands.Models.Customers
{
    public class UpdateCustomerRoleCommand : IRequest<CustomerRoleDto>
    {
        public CustomerRoleDto Model { get; set; }
    }
}
