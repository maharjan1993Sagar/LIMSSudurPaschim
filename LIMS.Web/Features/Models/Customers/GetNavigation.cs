using LIMS.Domain.Customers;
using LIMS.Domain.Localization;
using LIMS.Domain.Stores;
using LIMS.Web.Models.Customer;
using MediatR;

namespace LIMS.Web.Features.Models.Customers
{
    public class GetNavigation : IRequest<CustomerNavigationModel>
    {
        public int SelectedTabId { get; set; } = 0;
        public Customer Customer { get; set; }
        public Language Language { get; set; }
        public Store Store { get; set; }
    }
}
