using LIMS.Domain.Data;
using LIMS.Domain.Customers;
using LIMS.Services.Queries.Models.Customers;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace LIMS.Services.Queries.Handlers.Customers
{
    public class GetCustomerQueryHandler : IRequestHandler<GetCustomerByIdQuery, Customer>
    {
        private readonly IRepository<Customer> _customerRepository;

        public GetCustomerQueryHandler(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public Task<Customer> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Id))
                return Task.FromResult<Customer>(null);

            return _customerRepository.GetByIdAsync(request.Id);
        }
    }
}
