using LIMS.Domain.Customers;
using MediatR;
namespace LIMS.Services.Queries.Models.Customers
{
    public class GetCustomerByIdQuery : IRequest<Customer>
    {
        public string Id { get; set; }
    }
}
