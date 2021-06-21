using LIMS.Framework.Controllers;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Mvc;
using LIMS.Framework.Mvc.Filters;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Customers;
using LIMS.Services.Localization;
using LIMS.Services.Security;
using LIMS.Web.Areas.Admin.Extensions;
using LIMS.Web.Areas.Admin.Models.Customers;
using LIMS.Web.Areas.Admin.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace LIMS.Web.Areas.Admin.Controllers
{
    [PermissionAuthorize(PermissionSystemName.CustomerTags)]
    public partial class CustomerTagController : BaseAdminController
    {
        #region Fields
        private readonly ICustomerTagViewModelService _customerTagViewModelService;
        private readonly ILocalizationService _localizationService;
        private readonly ICustomerTagService _customerTagService;
        #endregion

        #region Constructors

        public CustomerTagController(
            ICustomerTagViewModelService customerTagViewModelService,
            ILocalizationService localizationService,
            ICustomerTagService customerTagService)
        {
            _customerTagViewModelService = customerTagViewModelService;
            _localizationService = localizationService;
            _customerTagService = customerTagService;
        }

        #endregion

        #region Customer Tags

        public IActionResult Index() => RedirectToAction("List");

        public IActionResult List() => View();

        [HttpPost]
        [PermissionAuthorizeAction(PermissionActionName.List)]
        public async Task<IActionResult> List(DataSourceRequest command)
        {
            var customertags = await _customerTagService.GetAllCustomerTags();
            var items = new List<(string Id, string Name, int Count)>();
            foreach (var item in customertags)
            {
                items.Add((Id : item.Id, Name : item.Name, Count : await _customerTagService.GetCustomerCount(item.Id)));
            }
            var gridModel = new DataSourceResult
            {
                Data = items.Select(x => new { Id = x.Id, Name = x.Name, Count = x.Count }),
                Total = customertags.Count()
            };
            return Json(gridModel);
        }

        [HttpGet]
        [PermissionAuthorizeAction(PermissionActionName.List)]
        public async Task<IActionResult> Search(string term)
        {
            var customertags = (await _customerTagService.GetCustomerTagsByName(term)).Select(x => x.Name);
            return Json(customertags);
        }

        [HttpPost]
        [PermissionAuthorizeAction(PermissionActionName.List)]
        public async Task<IActionResult> Customers(string customerTagId, DataSourceRequest command)
        {
            var customers = await _customerTagService.GetCustomersByTag(customerTagId, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = customers.Select(x => _customerTagViewModelService.PrepareCustomerModelForList(x)),
                Total = customers.TotalCount
            };
            return Json(gridModel);
        }
        [PermissionAuthorizeAction(PermissionActionName.Create)]
        public IActionResult Create()
        {
            var model = _customerTagViewModelService.PrepareCustomerTagModel();
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [PermissionAuthorizeAction(PermissionActionName.Create)]
        public async Task<IActionResult> Create(CustomerTagModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var customertag = await _customerTagViewModelService.InsertCustomerTagModel(model);
                SuccessNotification(_localizationService.GetResource("Admin.Customers.CustomerTags.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = customertag.Id }) : RedirectToAction("List");
            }

            //If we got this far, something failed, redisplay form
            return View(model);
        }

        
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        public async Task<IActionResult> Edit(CustomerTagModel model, bool continueEditing)
        {
            var customertag = await _customerTagService.GetCustomerTagById(model.Id);
            if (customertag == null)
                //No customer role found with the specified id
                return RedirectToAction("List");

            try
            {
                if (ModelState.IsValid)
                {
                    customertag = await _customerTagViewModelService.UpdateCustomerTagModel(customertag, model);
                    SuccessNotification(_localizationService.GetResource("Admin.Customers.CustomerTags.Updated"));
                    return continueEditing ? RedirectToAction("Edit", new { id = customertag.Id }) : RedirectToAction("List");
                }

                //If we got this far, something failed, redisplay form
                return View(model);
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("Edit", new { id = customertag.Id });
            }
        }

        [HttpPost]
        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        public async Task<IActionResult> CustomerDelete(string Id, string customerTagId)
        {
            var customertag = await _customerTagService.GetCustomerTagById(customerTagId);
            if (customertag == null)
                throw new ArgumentException("No customertag found with the specified id");
            if (ModelState.IsValid)
            {
                await _customerTagService.DeleteTagFromCustomer(customerTagId, Id);
                return new NullJsonResult();
            }
            return ErrorForKendoGridJson(ModelState);
        }

        [HttpPost]
        [PermissionAuthorizeAction(PermissionActionName.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            var customerTag = await _customerTagService.GetCustomerTagById(id);
            if (customerTag == null)
                //No customer role found with the specified id
                return RedirectToAction("List");

            try
            {
                if (ModelState.IsValid)
                {
                    await _customerTagViewModelService.DeleteCustomerTag(customerTag);
                    SuccessNotification(_localizationService.GetResource("Admin.Customers.CustomerTags.Deleted"));
                    return RedirectToAction("List");
                }
                ErrorNotification(ModelState);
                return RedirectToAction("Edit", new { id = customerTag.Id });
            }
            catch (Exception exc)
            {
                ErrorNotification(exc.Message);
                return RedirectToAction("Edit", new { id = customerTag.Id });
            }
        }
        #endregion

        
    }
}
