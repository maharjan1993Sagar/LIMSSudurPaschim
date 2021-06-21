using LIMS.Core;
using LIMS.Framework.Components;
using LIMS.Services.Authentication.External;
using LIMS.Web.Models.Customer;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace LIMS.Web.ViewComponents
{
    public class ExternalMethodsViewComponent : BaseViewComponent
    {
        private readonly IExternalAuthenticationService _externalAuthenticationService;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;

        public ExternalMethodsViewComponent(
            IExternalAuthenticationService externalAuthenticationService,
            IWorkContext workContext,
            IStoreContext storeContext)
        {
            _externalAuthenticationService = externalAuthenticationService;
            _workContext = workContext;
            _storeContext = storeContext;
        }

        public IViewComponentResult Invoke()
        {
            var models = _externalAuthenticationService
                .LoadActiveExternalAuthenticationMethods(_workContext.CurrentCustomer, _storeContext.CurrentStore.Id)
                .Select(authenticationMethod =>
                {
                    authenticationMethod.GetPublicViewComponent(out string viewComponentName);

                    return new ExternalAuthenticationMethodModel {
                        ViewComponentName = viewComponentName
                    };
                }).ToList();

            return View(models);

        }
    }
}