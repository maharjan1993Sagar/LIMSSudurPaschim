using LIMS.Domain.Customers;
using LIMS.Domain.Localization;
using LIMS.Web.Models.Customer;
using MediatR;
using System.Collections.Generic;

namespace LIMS.Web.Features.Models.Customers
{
    public class GetCustomAttributes : IRequest<IList<CustomerAttributeModel>>
    {
        public Customer Customer { get; set; }
        public Language Language { get; set; }
        public string OverrideAttributesXml { get; set; } = "";
    }
}
