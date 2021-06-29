using LIMS.Core;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Basic;
using LIMS.Services.Customers;
using LIMS.Services.ExportImport;
using LIMS.Services.Localization;
using LIMS.Services.Security;
using LIMS.Services.Stores;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class PujigatKharchaKharyakramController : BaseAdminController
    {
        #region fields
        private readonly IPujigatKharchaKharakramService _pujigatKharchaKharakramService;
        private readonly ICustomerService _customerService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly IStoreService _storeService;
        private readonly IExportManager _exportManager;
        private readonly IWorkContext _workContext;
        private readonly IImportManager _importManager;

        #endregion
        #region ctor
        public PujigatKharchaKharyakramController(
            IPujigatKharchaKharakramService pujigatKharchaKharakramService,
            ICustomerService customerService,
            ILanguageService languageService,
             ILocalizationService localizationService,
             IStoreService storeService,
              IExportManager exportManager,
              IWorkContext workContext,
              IImportManager importManager

            )
        {
            _pujigatKharchaKharakramService = pujigatKharchaKharakramService;
            _customerService = customerService;
            _languageService = languageService;
            _localizationService = localizationService;
            _storeService = storeService;
            _exportManager = exportManager;
            _workContext = workContext;
            _importManager = importManager;
        }
        #endregion
        public IActionResult Index() =>RedirectToAction("List");
        public IActionResult List() => View();
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest  command) {
            var categories = await _pujigatKharchaKharakramService.GetPujigatKharchaKharakram(command.Page, command.PageSize);
            var gridModel = new DataSourceResult {
                Data = categories,
                Total = categories.TotalCount
            };
            return Json(gridModel);


        }
//        [PermissionAuthorizeAction(PermissionActionName.Import)]
        [HttpPost]
        public async Task<IActionResult> ImportFromXlsx(IFormFile importexcelfile)
        {
            //a vendor and staff cannot import categories
           
            try
            {
                if (importexcelfile != null && importexcelfile.Length > 0)
                {
                    await _importManager.ImportCategoryFromXlsx(importexcelfile.OpenReadStream());
                }
                else
                {
                    ErrorNotification(_localizationService.GetResource("Admin.Common.UploadFile"));
                    return RedirectToAction("List");
                }
                SuccessNotification(_localizationService.GetResource("Admin.Catalog.Category.Imported"));
                return RedirectToAction("List");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("List");
            }
        }

    }
}
