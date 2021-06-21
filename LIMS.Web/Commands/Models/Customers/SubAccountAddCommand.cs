using LIMS.Domain.Customers;
using LIMS.Domain.Stores;
using LIMS.Services.Customers;
using LIMS.Web.Models.Customer;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace LIMS.Web.Commands.Models.Customers
{
    public class SubAccountAddCommand : IRequest<CustomerRegistrationResult>
    {
        public Customer Customer { get; set; }
        public Store Store { get; set; }
        public SubAccountModel Model { get; set; }
        public IFormCollection Form { get; set; }
    }
}
