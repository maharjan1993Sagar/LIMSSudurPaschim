using LIMS.Domain.Customers;
using MediatR;
using System.Collections.Generic;

namespace LIMS.Services.Commands.Models.Customers
{
    public class CustomerActionEventReactionCommand : IRequest<bool>
    {
        public CustomerActionEventReactionCommand()
        {
            CustomerActionTypes = new List<CustomerActionType>();
        }
        public IList<CustomerActionType> CustomerActionTypes { get; set; }
        public CustomerAction Action { get; set; }
        public string CustomerId { get; set; }
    }
}
