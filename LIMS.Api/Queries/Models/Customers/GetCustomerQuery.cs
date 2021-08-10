using LIMS.Api.DTOs.Customers;
using MediatR;

namespace LIMS.Api.Queries.Models.Customers
{
    public class GetCustomerQuery : IRequest<CustomerDto>
    {
        public string Id { get; set; }
    }
}
