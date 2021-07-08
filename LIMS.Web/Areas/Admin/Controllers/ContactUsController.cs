using LIMS.Core;
using LIMS.Domain.Seo;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Mvc.Filters;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Breed;
using LIMS.Services.GeneralCMS;
using LIMS.Services.Localization;
using LIMS.Services.Security;
using LIMS.Services.Stores;
using LIMS.Web.Areas.Admin.Extensions.Mapping;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Web.Areas.Admin.Models.Breed;
using LIMS.Web.Areas.Admin.Models.GeneralCMS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class ContactUsController : BaseAdminController
    {
        private readonly IContactUsService _contactService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;
        private readonly SeoSettings _seoSettings;

        public ContactUsController(
            IContactUsService contactService,
            ILanguageService languageService,
            ILocalizationService localizationService,
            IStoreService storeService,
            IWorkContext workContext,
            SeoSettings seoSettings
            )
        {
            _contactService = contactService;
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
            var contact = await _contactService.GetContactByUser(command.Page - 1, command.PageSize);
            
            var gridModel = new DataSourceResult {
                Data = contact,
                Total = contact.TotalCount
            };
            return Json(gridModel);
        }

        [PermissionAuthorizeAction(PermissionActionName.Create)]
        public async Task<IActionResult> Create()
        {
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
             return View();
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(ContactUsModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var contact = model.ToEntity();
                contact.UserId = _workContext.CurrentCustomer.Id;
                await _contactService.InsertContact(contact);
                SuccessNotification(_localizationService.GetResource("Admin.ContactUs.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = contact.Id }) : RedirectToAction("List");
            }
           return View(model);
        }


        [PermissionAuthorizeAction(PermissionActionName.Preview)]
        public async Task<IActionResult> Edit(string id)
        {
            var contact = await _contactService.GetContactById(id);
            if (contact == null)
                //No blog post found with the specified id
                return RedirectToAction("List");
            var model = contact.ToModel();
          return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(ContactUsModel model, bool continueEditing)
        {
            var contact = await _contactService.GetContactById(model.Id);
            if (contact == null)
               
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var m = model.ToEntity(contact);
                await _contactService.UpdateContact(m);

                SuccessNotification(_localizationService.GetResource("Admin.ContactUs.Updated"));
                if (continueEditing)
                {
                    //selected tab
                    await SaveSelectedTabIndex();
                    return RedirectToAction("Edit", new { id = model.Id });
                }
                return RedirectToAction("List");
            }
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Delete)]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var contact = await _contactService.GetContactById(id);
            if (contact == null)
                //No blog post found with the specified id

                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                await _contactService.DeleteContact(contact);
                SuccessNotification(_localizationService.GetResource("Admin.ContactUs.Deleted"));
                return RedirectToAction("List");
            }
            ErrorNotification(ModelState);
            return RedirectToAction("Edit", new { id = id });
        }
      
    }
}
