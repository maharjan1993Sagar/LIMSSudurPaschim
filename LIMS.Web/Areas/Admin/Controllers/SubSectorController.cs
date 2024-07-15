using LIMS.Core;
using LIMS.Domain.Seo;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Mvc.Filters;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Bali;
using LIMS.Services.Localization;
using LIMS.Services.Security;
using LIMS.Services.Stores;
using LIMS.Web.Areas.Admin.Extensions.Mapping;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Web.Areas.Admin.Models.Bali;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class SubSectorController : BaseAdminController
    {
        private readonly ISubSectorService _SubSectorService;
        private readonly IBudgetSourceService _BudgetSourceService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;
        private readonly SeoSettings _seoSettings;

        public SubSectorController(
            ISubSectorService SubSectorService,
            IBudgetSourceService BudgetSourceService,
            ILanguageService languageService,
            ILocalizationService localizationService,
            IStoreService storeService,
            IWorkContext workContext,
            SeoSettings seoSettings
            )
        {
            _SubSectorService = SubSectorService;
            _BudgetSourceService = BudgetSourceService;
            _languageService = languageService;
            _localizationService = localizationService;
            _storeService = storeService;
            _workContext = workContext;
            _seoSettings = seoSettings;
        }

        public IActionResult Index() => RedirectToAction("List");

        public IActionResult List() => View();

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command)
        {
            var SubSector = await _SubSectorService.GetSubSector(command.Page - 1, command.PageSize);
            
            var gridModel = new DataSourceResult {
                Data = SubSector,
                Total = SubSector.TotalCount
            };
            return Json(gridModel);
        }

        [PermissionAuthorizeAction(PermissionActionName.Create)]
        public async Task<IActionResult> Create()
        {
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            var species = new SelectList(await _BudgetSourceService.GetBudgetSource(), "Id", "NameNepali").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.BudgetSourceId = species;
            //var type = SubSectorTypeHelper.GetSubSectorType();
            //type.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            //ViewBag.Type =type;
            return View();
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(SubSectorModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var SubSector = model.ToEntity();
                SubSector.BudgetSource = await _BudgetSourceService.GetBudgetSourceById(model.BudgetSourceId);
                await _SubSectorService.InsertSubSector(SubSector);
                SuccessNotification(_localizationService.GetResource("Admin.SubSector.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = SubSector.Id }) : RedirectToAction("List");
            }
            var species = new SelectList(await _BudgetSourceService.GetBudgetSource(), "Id", "NameNepali").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.BudgetSourceId = species;
            
            //If we got this far, something failed, redisplay form
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            return View(model);
        }


        [PermissionAuthorizeAction(PermissionActionName.Preview)]
        public async Task<IActionResult> Edit(string id)
        {
            var SubSector = await _SubSectorService.GetSubSectorById(id);
            if (SubSector == null)
                //No blog post found with the specified id
                return RedirectToAction("List");
            var model = SubSector.ToModel();
            var species = new SelectList(await _BudgetSourceService.GetBudgetSource(), "Id", "NameNepali").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.BudgetSourceId = species;
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
          
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(SubSectorModel model, bool continueEditing)
        {
            var SubSector = await _SubSectorService.GetSubSectorById(model.Id);
            if (SubSector == null)
               
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var m = model.ToEntity(SubSector);
                m.BudgetSource = await _BudgetSourceService.GetBudgetSourceById(model.BudgetSourceId);
                await _SubSectorService.UpdateSubSector(m);

                SuccessNotification(_localizationService.GetResource("Admin.SubSector.Updated"));
                if (continueEditing)
                {
                    //selected tab
                    await SaveSelectedTabIndex();
                    return RedirectToAction("Edit", new { id = model.Id });
                }
                return RedirectToAction("List");
            }
            var species = new SelectList(await _BudgetSourceService.GetBudgetSource(), "Id", "Name").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.BudgetSourceId = species;
           
            //If we got this far, something failed, redisplay form
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Delete)]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var SubSector = await _SubSectorService.GetSubSectorById(id);
            if (SubSector == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                await _SubSectorService.DeleteSubSector(SubSector);
                SuccessNotification(_localizationService.GetResource("Admin.SubSector.Deleted"));
                return RedirectToAction("List");
            }
            ErrorNotification(ModelState);
            return RedirectToAction("Edit", new { id = id });
        }
      
    }
}
