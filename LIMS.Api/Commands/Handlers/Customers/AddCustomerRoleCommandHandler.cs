﻿using LIMS.Api.DTOs.Customers;
using LIMS.Api.Extensions;
using LIMS.Services.Customers;
using LIMS.Services.Localization;
using LIMS.Services.Logging;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace LIMS.Api.Commands.Models.Customers
{
    public class AddCustomerRoleCommandHandler : IRequestHandler<AddCustomerRoleCommand, CustomerRoleDto>
    {
        private readonly ICustomerService _customerService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly ILocalizationService _localizationService;

        public AddCustomerRoleCommandHandler(
            ICustomerService customerService,
            ICustomerActivityService customerActivityService,
            ILocalizationService localizationService)
        {
            _customerService = customerService;
            _customerActivityService = customerActivityService;
            _localizationService = localizationService;
        }

        public async Task<CustomerRoleDto> Handle(AddCustomerRoleCommand request, CancellationToken cancellationToken)
        {
            var customerRole = request.Model.ToEntity();
            await _customerService.InsertCustomerRole(customerRole);

            //activity log
            await _customerActivityService.InsertActivity("AddNewCustomerRole", customerRole.Id, _localizationService.GetResource("ActivityLog.AddNewCustomerRole"), customerRole.Name);

            return customerRole.ToModel();
        }
    }
}
