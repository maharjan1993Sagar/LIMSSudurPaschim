using LIMS.Core;
using LIMS.Domain.Bali;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Mvc;
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
using Microsoft.AspNetCore.Hosting;
using System.IO;
using LIMS.Web.Areas.Admin.Models.Bali;
using LIMS.Web.Areas.Admin.Extensions.Mapping;

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
        private IHostingEnvironment _environment;
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
              IFiscalYearService fiscalYearService,
              IHostingEnvironment environment

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
            _environment = environment;
        }
        #endregion
        public IActionResult Index() =>RedirectToAction("List");
        public IActionResult TabEntry() => View();
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest  command,string keyword) {
            var createdby = _workContext.CurrentCustomer.Id;
            var categories = await _pujigatKharchaKharakramService.GetPujigatKharchaKharakram(createdby,keyword,command.Page-1, command.PageSize);
            var gridModel = new DataSourceResult {
                Data = categories,
                Total = categories.TotalCount
            };
            return Json(gridModel);


        }

        [HttpPost]
        public async Task<IActionResult> ListOther(DataSourceRequest command, string keyword)
        {
            var createdby = _workContext.CurrentCustomer.Id;
            var categories = await _pujigatKharchaKharakramService.GetPujigatKharchaKharakramSelect(createdby, keyword, command.Page - 1, command.PageSize);
            
            var gridModel = new DataSourceResult {
                Data = categories,
                Total = categories.TotalCount
            };
            return Json(gridModel);


        }


        public async Task<IActionResult> Create() {
            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;
            var type = PujigatType();
            type.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Type = type;
            var programType = ProgramType();
            programType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.ProgramType = programType;

            return View();
        
        
        }

        [HttpPost]
        public async Task<IActionResult> Create(PugigatKharchaKaryakramModel pujigatKharchaKharakram)
        {
            var p = pujigatKharchaKharakram.ToEntity();
            p.CreatedAt = DateTime.Now;
            p.CreatedBy = _workContext.CurrentCustomer.Id;
            await _pujigatKharchaKharakramService.InsertPujigatKharchaKharakram(p);
            return RedirectToAction("TabEntry");
        }

            public FileResult Download() {

            string filePath= _environment.WebRootPath +"/Import_format.xlsx";
            byte[] bytes = System.IO.File.ReadAllBytes(filePath);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", "Import_format.xlsx");

        }

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
                    return RedirectToAction("TabEntry");
                }
                SuccessNotification(_localizationService.GetResource("Admin.Catalog.Category.Imported"));
                return RedirectToAction("TabEntry");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("TabEntry");
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


         [HttpPost]
        public async Task<IActionResult> Edit(PujigatKharchaKharakram pujigatKharchaKharakram)
        {
            var userapi = await _pujigatKharchaKharakramService.GetPujigatKharchaKharakramById(pujigatKharchaKharakram.Id);
            if (userapi == null)
                throw new ArgumentException("No program found with the specified id ");
            if (ModelState.IsValid)
            {
                
                await _pujigatKharchaKharakramService.UpdatePujigatKharchaKharakram(pujigatKharchaKharakram);
                return new NullJsonResult();
            }
            return ErrorForKendoGridJson(ModelState);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var userapi = await _pujigatKharchaKharakramService.GetPujigatKharchaKharakramById(id);
            if (userapi == null)
                throw new ArgumentException("No program found with the specified id ");
            if (ModelState.IsValid)
            {
                await _pujigatKharchaKharakramService.DeletePujigatKharchaKharakram(userapi);
                return new NullJsonResult();
            }
            return ErrorForKendoGridJson(ModelState);
        }

    }
}
