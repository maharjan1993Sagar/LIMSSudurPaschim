using LIMS.Api.DTOs.Customers;
using MediatR;

namespace LIMS.Api.Commands.Models.Customers
{
    public class DeleteCustomerRoleCommand : IRequest<bool>
    {
        public CustomerRoleDto Model { get; set; }
    }
}
