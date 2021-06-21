using LIMS.Core;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Mvc;
using LIMS.Framework.Mvc.Filters;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Customers;
using LIMS.Services.Localization;
using LIMS.Services.Security;
using LIMS.Web.Areas.Admin.Extensions;
using LIMS.Web.Areas.Admin.Interfaces;
using LIMS.Web.Areas.Admin.Models.Customers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    [PermissionAuthorize(PermissionSystemName.CustomerRoles)]
    public partial class CustomerRoleController : BaseAdminController
    {
        #region Fields

        private readonly ICustomerRoleViewModelService _customerRoleViewModelService;
        private readonly ICustomerService _customerService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly IWorkContext _workContext;

        #endregion

        #region Constructors

        public CustomerRoleController(
            ICustomerRoleViewModelService customerRoleViewModelService,
            ICustomerService customerService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            IWorkContext workContext)
        {
            _customerRoleViewModelService = customerRoleViewModelService;
            _customerService = customerService;
            _localizationService = localizationService;
            _permissionService = permissionService;
            _workContext = workContext;
        }

        #endregion

        #region Customer roles

        public IActionResult Index() => RedirectToAction("List");

        public IActionResult List() => View();

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command)
        {
            var customerRoles = await _customerService.GetAllCustomerRoles(command.Page - 1, command.PageSize, true);
            var gridModel = new DataSourceResult {
                Data = customerRoles.Select(x =>
                {
                    var rolesModel = x.ToModel();
                    return rolesModel;
                }),
                Total = customerRoles.TotalCount,
            };
            return Json(gridModel);
        }

        [PermissionAuthorizeAction(PermissionActionName.Create)]
        public IActionResult Create()
        {
            var model = _customerRoleViewModelService.PrepareCustomerRoleModel();
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(CustomerRoleModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var customerRole = await _customerRoleViewModelService.InsertCustomerRoleModel(model);
                SuccessNotification(_localizationService.GetResource("Admin.Customers.CustomerRoles.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = customerRole.Id }) : RedirectToAction("List");
            }
            //If we got this far, something failed, redisplay form
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Preview)]
        public async Task<IActionResult> Edit(string id)
        {
            var customerRole = await _customerService.GetCustomerRoleById(id);
            if (customerRole == null)
                //No customer role found with the specified id
                return RedirectToAction("List");

            var model = _customerRoleViewModelService.PrepareCustomerRoleModel(customerRole);
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(CustomerRoleModel model, bool continueEditing)
        {
            var customerRole = await _customerService.GetCustomerRoleById(model.Id);
            if (customerRole == null)
                //No customer role found with the specified id
                return RedirectToAction("List");

            try
            {
                if (ModelState.IsValid)
                {
                    if (customerRole.IsSystemRole && !model.Active)
                        throw new LIMSException(_localizationService.GetResource("Admin.Customers.CustomerRoles.Fields.Active.CantEditSystem"));

                    if (customerRole.IsSystemRole && !customerRole.SystemName.Equals(model.SystemName, StringComparison.OrdinalIgnoreCase))
                        throw new LIMSException(_localizationService.GetResource("Admin.Customers.CustomerRoles.Fields.SystemName.CantEditSystem"));

                    customerRole = await _customerRoleViewModelService.UpdateCustomerRoleModel(customerRole, model);
                    SuccessNotification(_localizationService.GetResource("Admin.Customers.CustomerRoles.Updated"));
                    return continueEditing ? RedirectToAction("Edit", new { id = customerRole.Id }) : RedirectToAction("List");
                }

                //If we got this far, something failed, redisplay form
                return View(model);
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("Edit", new { id = customerRole.Id });
            }
        }

        [PermissionAuthorizeAction(PermissionActionName.Delete)]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var customerRole = await _customerService.GetCustomerRoleById(id);
            if (customerRole == null)
                //No customer role found with the specified id
                return RedirectToAction("List");
            if (customerRole.IsSystemRole)
                ModelState.AddModelError("", "You can't delete system role");
            try
            {
                if (ModelState.IsValid)
                {
                    await _customerRoleViewModelService.DeleteCustomerRole(customerRole);
                    SuccessNotification(_localizationService.GetResource("Admin.Customers.CustomerRoles.Deleted"));
                    return RedirectToAction("List");
                }
                ErrorNotification(ModelState);
                return RedirectToAction("Edit", new { id = customerRole.Id });
            }
            catch (Exception exc)
            {
                ErrorNotification(exc.Message);
                return RedirectToAction("Edit", new { id = customerRole.Id });
            }
        }
        #endregion

        

        #region Acl

        [PermissionAuthorizeAction(PermissionActionName.Preview)]
        [HttpPost]
        public async Task<IActionResult> Acl(string customerRoleId)
        {
            var permissionRecords = await _permissionService.GetAllPermissionRecords();
            var model = new List<CustomerRolePermissionModel>();

            foreach (var pr in permissionRecords)
            {
                model.Add(new CustomerRolePermissionModel {
                    Id = pr.Id,
                    Name = pr.GetLocalizedPermissionName(_localizationService, _workContext),
                    SystemName = pr.SystemName,
                    Actions = pr.Actions.ToList(),
                    Access = pr.CustomerRoles.Contains(customerRoleId)
                });
            }

            var gridModel = new DataSourceResult {
                Data = model,
                Total = model.Count()
            };
            return Json(gridModel);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost]
        public async Task<IActionResult> AclUpdate(string customerRoleId, string id, bool access)
        {
            if (!await _permissionService.Authorize(StandardPermissionProvider.ManageAcl))
                ModelState.AddModelError("", "You don't have permission to the update");

            var cr = await _customerService.GetCustomerRoleById(customerRoleId);
            if (cr == null)
                throw new ArgumentException("No customer role found with the specified id");

            var permissionRecord = await _permissionService.GetPermissionRecordById(id);
            if (permissionRecord == null)
                throw new ArgumentException("No permission found with the specified id");

            if (ModelState.IsValid)
            {
                if (access)
                {
                    if (!permissionRecord.CustomerRoles.Contains(customerRoleId))
                        permissionRecord.CustomerRoles.Add(customerRoleId);
                }
                else
                    if (permissionRecord.CustomerRoles.Contains(customerRoleId))
                    permissionRecord.CustomerRoles.Remove(customerRoleId);

                await _permissionService.UpdatePermissionRecord(permissionRecord);

                return new NullJsonResult();
            }
            return ErrorForKendoGridJson(ModelState);
        }


        #endregion
    }
}
