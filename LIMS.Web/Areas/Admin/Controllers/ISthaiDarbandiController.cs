using LIMS.Core;
using LIMS.Domain.Bali;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Mvc.Filters;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Bali;
using LIMS.Services.Basic;
using LIMS.Services.Localization;
using LIMS.Services.Security;
using LIMS.Web.Areas.Admin.Models.Bali;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class ISthaiDarbandiController:BaseAdminController
    {
        private readonly IDolfdAstaiPadService _animalRegistrationService;

        private readonly ILocalizationService _localizationService;
        private readonly ILanguageService _languageService;
        private readonly IWorkContext _workContext;
        private readonly IFiscalYearService _fiscalYearService;

        public ISthaiDarbandiController(ILocalizationService localizationService,
            IDolfdAstaiPadService animalRegistrationService,
            ILanguageService languageService,

            IWorkContext workContext,
            IFiscalYearService fiscalYearService,
            IUnitService unitService
            )
        {
            _localizationService = localizationService;
            _animalRegistrationService = animalRegistrationService;
            _languageService = languageService;

            _workContext = workContext;
            _fiscalYearService = fiscalYearService;
        }

        public IActionResult Index() => RedirectToAction("List");

        public IActionResult List() => View();

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command, string keyword = "")
        {
            var id = _workContext.CurrentCustomer.Id;
            var bali = await _animalRegistrationService.GetTahaData(id, keyword);
            var gridModel = new DataSourceResult {
                Data = bali,
                Total = bali.TotalCount
            };
            return Json(gridModel);
        }


        public IActionResult MarketList() => View();

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> MarketList(DataSourceRequest command)
        {
            var id = _workContext.CurrentCustomer.Id;
            var bali = await _animalRegistrationService.GetTahaData(id);
            var gridModel = new DataSourceResult {
                Data = bali,
                Total = bali.TotalCount
            };
            return Json(gridModel);
        }




        public async Task<IActionResult> Create()
        {

            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;


            return View();
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(DolfdShthaiTahaEntryModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var animalRegistration = new DolfdSthaiTahaEntry();
                animalRegistration.FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYearId);

                animalRegistration.CreatedBy = _workContext.CurrentCustomer.Id;
                await _animalRegistrationService.InsertTahaData(animalRegistration);

                SuccessNotification(_localizationService.GetResource("Admin.Create.successful"));
                return continueEditing ? RedirectToAction("Edit", new { id = animalRegistration.Id }) : RedirectToAction("Index");
            }

            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;
            return View(model);
        }


        public async Task<ActionResult> GetDolfdAstaiPadByFiscalyear(string fiscalYear)
        {
            var cratedby = _workContext.CurrentCustomer.Id;
            var breed = await _animalRegistrationService.GetTahaData(cratedby, 0, int.MaxValue, fiscalYear);

            return Json(breed.ToList());
        }

    }
}
