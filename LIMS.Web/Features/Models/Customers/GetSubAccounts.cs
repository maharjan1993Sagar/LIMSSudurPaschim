using LIMS.Domain.Customers;
using LIMS.Web.Models.Customer;
using MediatR;
using System.Collections.Generic;

namespace LIMS.Web.Features.Models.Customers
{
    public class GetSubAccounts : IRequest<IList<SubAccountSimpleModel>>
    {
        public Customer Customer { get; set; }
    }
}
