﻿using LIMS.Domain.Customers;
using LIMS.Domain.Localization;
using LIMS.Web.Models.Customer;
using MediatR;

namespace LIMS.Web.Features.Models.Customers
{
    public class GetDocuments : IRequest<DocumentsModel>
    {
        public Customer Customer { get; set; }
        public Language Language { get; set; }
    }
}
