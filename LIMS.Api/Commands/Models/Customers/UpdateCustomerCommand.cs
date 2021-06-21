﻿using LIMS.Api.DTOs.Customers;
using MediatR;

namespace LIMS.Api.Commands.Models.Customers
{
    public class UpdateCustomerCommand : IRequest<CustomerDto>
    {
        public CustomerDto Model { get; set; }
    }
}
