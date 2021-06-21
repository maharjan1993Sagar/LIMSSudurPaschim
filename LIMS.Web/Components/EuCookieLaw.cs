using LIMS.Core;
using LIMS.Domain.Customers;
using LIMS.Domain.Stores;
using LIMS.Framework.Components;
using LIMS.Services.Common;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LIMS.Web.ViewComponents
{
    public class EuCookieLawViewComponent : BaseViewComponent
    {
        private readonly StoreInformationSettings _storeInformationSettings;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        public EuCookieLawViewComponent(StoreInformationSettings storeInformationSettings,
            IWorkContext workContext, IStoreContext storeContext)
        {
            _storeInformationSettings = storeInformationSettings;
            _workContext = workContext;
            _storeContext = storeContext;
        }

        public IViewComponentResult Invoke()
        {
            if (!_storeInformationSettings.DisplayEuCookieLawWarning)
                //disabled
                return Content("");

            var customer = _workContext.CurrentCustomer;
            //ignore search engines because some pages could be indexed with the EU cookie as description
            if (customer.IsSearchEngineAccount())
                return Content("");

            if (customer.GetAttributeFromEntity<bool>(SystemCustomerAttributeNames.EuCookieLawAccepted, _storeContext.CurrentStore.Id))
                //already accepted
                return Content("");

            //ignore notification?
            //right now it's used during logout so popup window is not displayed twice
            if (TempData["LIMS.IgnoreEuCookieLawWarning"] != null && Convert.ToBoolean(TempData["LIMS.IgnoreEuCookieLawWarning"]))
                return Content("");

            return View();

        }
    }
}