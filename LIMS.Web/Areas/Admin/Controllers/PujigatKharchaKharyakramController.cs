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
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private readonly IFiscalYearService _fiscalYearService;

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
              IImportManager importManager,
              IFiscalYearService fiscalYearService

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
            _fiscalYearService = fiscalYearService;
        }
        #endregion
        public IActionResult Index() =>RedirectToAction("List");
        public IActionResult List() => View();
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest  command) {
            var createdby = _workContext.CurrentCustomer.Id;
            var categories = await _pujigatKharchaKharakramService.GetPujigatKharchaKharakram(createdby,command.Page-1, command.PageSize);
            var gridModel = new DataSourceResult {
                Data = categories,
                Total = categories.TotalCount
            };
            return Json(gridModel);


        }
//        [PermissionAuthorizeAction(PermissionActionName.Import)]
        [HttpPost]
        public async Task<IActionResult> ImportFromXlsx(IFormFile importexcelfile,string Type,string FiscalYear,string ProgramType)
        {
            //a vendor and staff cannot import categories
           
            try
            {
                if (importexcelfile != null && importexcelfile.Length > 0)
                {
                    await _importManager.ImportCategoryFromXlsx(importexcelfile.OpenReadStream(),Type,FiscalYear,ProgramType);
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

        public async Task<IActionResult> GetFiscalYear() {

            return Json(await _fiscalYearService.GetFiscalYear());

        }
        public List<SelectListItem> PujigatType()
        {

            return new List<SelectListItem>() {
                new SelectListItem {
                    Text="chalu",
                    Value="chalu"

                },
                 new SelectListItem {
                    Text="pujigat",
                    Value="pujigat"

                },
                // new SelectListItem {
                //    Text="kha si na.",
                //    Value="kha si na."

                //}
            };

        }

        public List<SelectListItem> ProgramType()
        {

            return new List<SelectListItem>() {
                new SelectListItem {
                    Text=_localizationService.GetResource("Lims.PujigatKharcha.SanghKoSasarthaAnudanAntargat"),
                    Value="Lims.PujigatKharcha.SanghKoSasarthaAnudanAntargat",

                },
                 new SelectListItem {
                    Text=_localizationService.GetResource("Lims.PujigatKharcha.PardeshKoBajetAntargat"),
                    Value="Lims.PujigatKharcha.PardeshKoBajetAntargat",


                },
                
            };

        }


    }
}
