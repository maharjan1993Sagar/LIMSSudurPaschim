﻿using LIMS.Api.Commands.Models.Customers;
using LIMS.Services.Customers;
using LIMS.Services.Localization;
using LIMS.Services.Logging;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace LIMS.Api.Commands.Handlers.Customers
{
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, bool>
    {
        private readonly ICustomerService _customerService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly ILocalizationService _localizationService;

        public DeleteCustomerCommandHandler(
            ICustomerService customerService,
            ICustomerActivityService customerActivityService,
            ILocalizationService localizationService)
        {
            _customerService = customerService;
            _customerActivityService = customerActivityService;
            _localizationService = localizationService;
        }

        public async Task<bool> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerService.GetCustomerByEmail(request.Email);
            if (customer != null)
            {
                await _customerService.DeleteCustomer(customer);
                //activity log
                await _customerActivityService.InsertActivity("DeleteCustomer", customer.Id, _localizationService.GetResource("ActivityLog.DeleteCustomer"), customer.Id);
            }

            return true;
        }

    }
}
