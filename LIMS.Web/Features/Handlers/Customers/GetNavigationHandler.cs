using LIMS.Domain.Customers;
using LIMS.Web.Features.Models.Customers;
using LIMS.Web.Models.Customer;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace LIMS.Web.Features.Handlers.Customers
{
    public class GetNavigationHandler : IRequestHandler<GetNavigation, CustomerNavigationModel>
    {

        private readonly CustomerSettings _customerSettings;

        public GetNavigationHandler(
            CustomerSettings customerSettings)
        {
            _customerSettings = customerSettings;
        }

        public async Task<CustomerNavigationModel> Handle(GetNavigation request, CancellationToken cancellationToken)
        {
            var model = new CustomerNavigationModel();
            model.HideAvatar = !_customerSettings.AllowCustomersToUploadAvatars;
            model.HideDeleteAccount = !_customerSettings.AllowUsersToDeleteAccount;
            model.HideDownloadableProducts = _customerSettings.HideDownloadableProductsTab;
            model.HideBackInStockSubscriptions = _customerSettings.HideBackInStockSubscriptionsTab;
            model.HideAuctions = _customerSettings.HideAuctionsTab;
            model.HideNotes = _customerSettings.HideNotesTab;
            model.HideDocuments = _customerSettings.HideDocumentsTab;
            model.HideReviews = _customerSettings.HideReviewsTab;
            model.HideCourses = _customerSettings.HideCoursesTab;
            model.HideSubAccounts = _customerSettings.HideSubAccountsTab || !request.Customer.IsOwner();
            model.SelectedTab = (CustomerNavigationEnum)request.SelectedTabId;

            return await Task.FromResult(model);
        }
    }
}
