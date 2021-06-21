using LIMS.Domain.Customers;
using LIMS.Domain.Stores;
using LIMS.Web.Models.Customer;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace LIMS.Web.Commands.Models.Customers
{
    public class CustomerRegisteredCommand : IRequest<bool>
    {
        public Customer Customer { get; set; }
        public Store Store { get; set; }
        public RegisterModel Model { get; set; }
        public IFormCollection Form { get; set; }
        public string CustomerAttributesXml { get; set; }
    }
}
