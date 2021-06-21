using LIMS.Core;
using LIMS.Domain.Stores;
using LIMS.Framework.Components;
using LIMS.Services.Localization;
using LIMS.Web.Models.Common;
using Microsoft.AspNetCore.Mvc;

namespace LIMS.Web.Components
{
    public class FooterCleanViewComponent : BaseViewComponent
    {
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;

        public FooterCleanViewComponent(
            IWorkContext workContext,
            IStoreContext storeContext)
        {
            _workContext = workContext;
            _storeContext = storeContext;
        }

        public IViewComponentResult Invoke()
        {
            var model = PrepareFooter();
            return View(model);
        }

        private FooterCleanModel PrepareFooter()
        {
            var currentstore = _storeContext.CurrentStore;

            var model = new FooterCleanModel {
                StoreName = currentstore.GetLocalized(x => x.Name, _workContext.WorkingLanguage.Id),
                CompanyName = currentstore.CompanyName,
                CompanyEmail = currentstore.CompanyEmail,
                CompanyAddress = currentstore.CompanyAddress,
                CompanyPhone = currentstore.CompanyPhoneNumber,
                CompanyHours = currentstore.CompanyHours,
            };

            return model;
        }
    }
}
