using LIMS.Core;
using LIMS.Framework.Components;
using LIMS.Services.Bali;
using LIMS.Services.Localization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Components
{
    public class MarketReport:BaseViewComponent
    {
        private readonly IWorkContext _workContext;
        private readonly ILanguageService _languageService;
        private readonly IStoreContext _storeContext;
        private readonly IBaliRegisterService _baliService;

        public MarketReport(
            IWorkContext workContext,
            ILanguageService languageService,
            IStoreContext storeContext,
             IBaliRegisterService baliService
            )
        {
            _workContext = workContext;
            _languageService = languageService;
            _storeContext = storeContext;
            _baliService = baliService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string fiscalYear)
        {
            var user = _workContext.CurrentCustomer.Id;
            var production = await _baliService.GetbaliRegister(user);
            var productions = production.Where(m => m.FiscalYearId == fiscalYear).ToList();
            return View(productions);

        }

    }
}
