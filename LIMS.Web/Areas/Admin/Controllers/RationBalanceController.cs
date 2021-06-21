using LIMS.Framework.Kendoui;
using LIMS.Services.Localization;
using LIMS.Services.RationBalance;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Web.Areas.Admin.Models.RashanBalance;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using LIMS.Web.Areas.Admin.Extensions.Mapping;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Security;
using LIMS.Framework.Mvc.Filters;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class RationBalanceController :BaseAdminController
    {
        #region Fields
        private readonly IRationBalanceService _rationBalanceService;
        private readonly ILocalizationService _localizationService;
        public RationBalanceController(IRationBalanceService rationBalanceService, ILocalizationService localizationService)
        {
            _rationBalanceService = rationBalanceService;
            _localizationService = localizationService;
        }
        #endregion

        public IActionResult Index() => RedirectToAction("List");
        public IActionResult List() => View();
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command) {
            var rationBalance = await _rationBalanceService.GetFeedLibraries(command.Page-1,command.PageSize);
            var gridModel = new DataSourceResult {
                Data = rationBalance,
                Total = rationBalance.TotalCount
            };
            return Json(gridModel);
        }
        public IActionResult Create()
        {
            var model = new FeedLibraryModel();
            var feedClass = FeedClassHelper.GetFeedLibrary();
            var feedTypeCategory = FeedTypeCategoryHelper.GetFeedTypeCategory();
            var feedType = FeedTypeHelper.GetFeedType();
            var feedFor = FeedForHelper.GetFeedFor();
            feedClass.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            feedTypeCategory.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            feedType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            feedFor.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FeedClass = feedClass;
            ViewBag.FeedTypeCategory = feedTypeCategory;
            ViewBag.FeedType = feedType;
            ViewBag.FeedFor = feedFor;
            return View(model);
        }
        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Create(FeedLibraryModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var feedLibrary = model.ToEntity();
                _rationBalanceService.InsertFeedLibrary(feedLibrary);
                SuccessNotification(_localizationService.GetResource("Admin.farm.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = feedLibrary.Id }) : RedirectToAction("List");
            }
            var feedClass = FeedClassHelper.GetFeedLibrary();
            var feedTypeCategory = FeedTypeCategoryHelper.GetFeedTypeCategory();
            var feedType = FeedTypeHelper.GetFeedType();
            var feedFor = FeedForHelper.GetFeedFor();
            feedClass.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            feedTypeCategory.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            feedType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            feedFor.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FeedClass = feedClass;
            ViewBag.FeedTypeCategory = feedTypeCategory;
            ViewBag.FeedType = feedType;
            ViewBag.FeedFor = feedFor;
            return View(model);
        }

        public async Task<ActionResult> Edit(string id)
        {
            var feedClass = FeedClassHelper.GetFeedLibrary();
            var feedTypeCategory = FeedTypeCategoryHelper.GetFeedTypeCategory();
            var feedType = FeedTypeHelper.GetFeedType();
            var feedFor = FeedForHelper.GetFeedFor();
            feedClass.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            feedTypeCategory.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            feedType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            feedFor.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FeedClass = feedClass;
            ViewBag.FeedTypeCategory = feedTypeCategory;
            ViewBag.FeedType = feedType;
            ViewBag.FeedFor = feedFor;
            var feedLibrary = await _rationBalanceService.GetFeedLibraryById(id);
            if (feedLibrary == null)
                return RedirectToAction("List");
            var model = feedLibrary.ToModel();
            return View(model);
        }
        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(FeedLibraryModel model, bool continueEditing)
        {
            var feedLibrary = await _rationBalanceService.GetFeedLibraryById(model.Id);
            if (feedLibrary == null)
                return RedirectToAction("List");
            if (ModelState.IsValid)
            {
                var entity = model.ToEntity(feedLibrary);
                await _rationBalanceService.UpdateFeedLibrary(entity);
                SuccessNotification(_localizationService.GetResource("Admin.feedLibrary.Updated"));
                if (continueEditing)
                {
                    ////selected tab
                    //await SaveSelectedTabIndex();

                    return RedirectToAction("Edit", new { id = model.Id });
                }
                return RedirectToAction("List");
            }
         
            //If we got this far, something failed, redisplay form

            return View(model);
        }


    }
}
