using LIMS.Api.DTOs.Customers;
using LIMS.Api.Extensions;
using LIMS.Api.Queries.Models.Customers;
using LIMS.Services.Customers;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace LIMS.Api.Queries.Handlers.Customers
{
    public class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, CustomerDto>
    {
        private readonly ICustomerService _customerService;

        public GetCustomerQueryHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<CustomerDto> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
        {
            return (await _customerService.GetCustomerByEmail(request.Email)).ToModel();
        }
    }
}
