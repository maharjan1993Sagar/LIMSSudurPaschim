using LIMS.Api.Commands.Models.Customers;
using LIMS.Api.DTOs.Customers;
using LIMS.Api.Extensions;
using LIMS.Services.Customers;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LIMS.Api.Commands.Handlers.Customers
{
    public class AddCustomerAddressCommandHandler : IRequestHandler<AddCustomerAddressCommand, AddressDto>
    {
        private readonly ICustomerService _customerService;

        public AddCustomerAddressCommandHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<AddressDto> Handle(AddCustomerAddressCommand request, CancellationToken cancellationToken)
        {
            var address = request.Address.ToEntity();
            address.CreatedOnUtc = DateTime.UtcNow;
            address.Id = "";
            address.CustomerId = request.Customer.Id;
            await _customerService.InsertAddress(address);
            return address.ToModel();
        }
    }
}
