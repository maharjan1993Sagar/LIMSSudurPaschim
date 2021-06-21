using LIMS.Services.Localization;
using LIMS.Services.Logging;
using LIMS.Services.Notifications.Customers;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace LIMS.Services.Events
{
    public class CustomerLoggedInEventHandler : INotificationHandler<CustomerLoggedInEvent>
    {
        private readonly ICustomerActivityService _customerActivityService;
        private readonly ILocalizationService _localizationService;

        public CustomerLoggedInEventHandler(
            ICustomerActivityService customerActivityService,
            ILocalizationService localizationService)
        {
            _customerActivityService = customerActivityService;
            _localizationService = localizationService;
        }

        public async Task Handle(CustomerLoggedInEvent notification, CancellationToken cancellationToken)
        {
            //activity log
            await _customerActivityService.InsertActivity("PublicStore.Login", "", _localizationService.GetResource("ActivityLog.PublicStore.Login"), notification.Customer);
        }
    }
}
