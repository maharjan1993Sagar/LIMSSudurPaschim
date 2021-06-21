using LIMS.Core;
using LIMS.Domain.Customers;
using LIMS.Framework.Components;
using LIMS.Framework.UI;
using LIMS.Services.Security;
using LIMS.Web.Models.Common;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LIMS.Web.ViewComponents
{
    public class AdminHeaderLinksViewComponent : BaseViewComponent
    {
        private readonly IWorkContext _workContext;
        private readonly IPageHeadBuilder _pageHeadBuilder;
        private readonly IPermissionService _permissionService;

        private readonly CustomerSettings _customerSettings;

        public AdminHeaderLinksViewComponent(IPageHeadBuilder pageHeadBuilder,
            IPermissionService permissionService,
            IWorkContext workContext,
            CustomerSettings customerSettings)
        {
            _pageHeadBuilder = pageHeadBuilder;
            _workContext = workContext;
            _customerSettings = customerSettings;
            _permissionService = permissionService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new AdminHeaderLinksModel {
                ImpersonatedCustomerEmailUsername = _workContext.CurrentCustomer.IsRegistered() ? (_customerSettings.UsernamesEnabled ? _workContext.CurrentCustomer.Username : _workContext.CurrentCustomer.Email) : "",
                IsCustomerImpersonated = _workContext.OriginalCustomerIfImpersonated != null,
                DisplayAdminLink = await _permissionService.Authorize(StandardPermissionProvider.AccessAdminPanel),
                EditPageUrl = _pageHeadBuilder.GetEditPageUrl()
            };

            if (!model.DisplayAdminLink && !model.IsCustomerImpersonated)
                return Content("");
            return View(model);
        }
    }
}