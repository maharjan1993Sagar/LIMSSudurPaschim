﻿using LIMS.Domain.Customers;
using LIMS.Domain.Localization;
using LIMS.Domain.Stores;
using LIMS.Web.Models.Customer;
using MediatR;

namespace LIMS.Web.Features.Models.Customers
{
    public class GetInfo : IRequest<CustomerInfoModel>
    {
        public CustomerInfoModel Model { get; set; }
        public Customer Customer { get; set; }
        public Store Store { get; set; }
        public Language Language { get; set; }
        public bool ExcludeProperties { get; set; }
        public string OverrideCustomCustomerAttributesXml { get; set; } = "";
    }
}
