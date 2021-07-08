using LIMS.Core;
using LIMS.Domain.NewsEvent;
using LIMS.Domain.Seo;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Mvc.Filters;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Breed;
using LIMS.Services.GeneralCMS;
using LIMS.Services.Localization;
using LIMS.Services.Media;
using LIMS.Services.Security;
using LIMS.Services.Stores;
using LIMS.Web.Areas.Admin.Extensions.Mapping;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Web.Areas.Admin.Models.Breed;
using LIMS.Web.Areas.Admin.Models.GeneralCMS;
using LIMS.Web.Areas.Admin.Models.NewsEvent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class EmployeeController : BaseAdminController
    {
        private readonly IEmployeeService _employeeService;
        private readonly IPictureService _pictureService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;
        private readonly SeoSettings _seoSettings;

        public EmployeeController(
            IEmployeeService employeeService,
            IPictureService pictureService,
            ILanguageService languageService,
            ILocalizationService localizationService,
            IStoreService storeService,
            IWorkContext workContext,
            SeoSettings seoSettings
            )
        {
            _employeeService = employeeService;
            _pictureService = pictureService;
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
            var employee = await _employeeService.GetEmployeeByUser(command.Page - 1, command.PageSize);

            var gridModel = new DataSourceResult {
                Data = employee,
                Total = employee.TotalCount
            };
            return Json(gridModel);
        }

        [PermissionAuthorizeAction(PermissionActionName.Create)]
        public async Task<IActionResult> Create()
        {
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            ViewBag.Type = new SelectList(GetTypes(), "Value", "Text");
            return View();
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(EmployeeModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var employee = model.ToEntity();
                employee.UserId = _workContext.CurrentCustomer.Id;
                if (model.ImageModel.PictureId != null)
                {
                    var picture = await _pictureService.GetPictureById(model.ImageModel.PictureId);

                    await _pictureService.UpdatePicture(picture.Id,
                    await _pictureService.LoadPictureBinary(picture),
                    picture.MimeType,
                    picture.SeoFilename,
                    model.ImageModel.OverrideAltAttribute,
                    model.ImageModel.OverrideTitleAttribute);

                    await _pictureService.SetSeoFilename(picture.Id, _pictureService.GetPictureSeName(picture.TitleAttribute));

                    employee.Image = new NewsEventFile {
                        PictureId = picture.Id,
                        CMSEntityId = employee.Id,
                        DisplayOrder = 0,
                        AltAttribute = model.ImageModel.OverrideAltAttribute,
                        MimeType = picture.MimeType,
                        SeoFilename = picture.SeoFilename,
                        TitleAttribute = model.ImageModel.OverrideTitleAttribute,
                        PictureUrl=(await _pictureService.GetPictureUrl(model.ImageModel.PictureId))
                    };

                }

                await _employeeService.InsertEmployee(employee);
                SuccessNotification(_localizationService.GetResource("Admin.Employee.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = employee.Id }) : RedirectToAction("List");
            }
            ViewBag.Type = new SelectList(GetTypes(), "Value", "Text");
            //If we got this far, something failed, redisplay form
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            return View(model);
        }


        [PermissionAuthorizeAction(PermissionActionName.Preview)]
        public async Task<IActionResult> Edit(string id)
        {
            var employee = await _employeeService.GetEmployeeById(id);
            if (employee == null)
                //No blog post found with the specified id
                return RedirectToAction("List");
            var model = employee.ToModel();
            if (employee.Image != null)
            {
                model.ImageModel = new NewsEventFileModel {
                    PictureId = employee.Image.PictureId ?? employee.Image.PictureId,
                    OverrideTitleAttribute = employee.Image.OverrideTitleAttribute,
                };
            }
            ViewBag.Type = new SelectList(GetTypes(), "Text", "Value", model.Type);
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(EmployeeModel model, bool continueEditing)
        {
            var employee = await _employeeService.GetEmployeeById(model.Id);
            if (employee == null)

                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var m = model.ToEntity(employee);

                //Delete the existing picture
                if (employee.Image.PictureId != model.ImageModel.PictureId)
                {

                    var pic = await _pictureService.GetPictureById(employee.Image.PictureId);

                    if (pic != null)
                        await _pictureService.DeletePicture(pic);


                    //update the new picture
                    var picture = await _pictureService.GetPictureById(model.ImageModel.PictureId);
                    await _pictureService.UpdatePicture(picture.Id,
                    await _pictureService.LoadPictureBinary(picture),
                    picture.MimeType,
                    picture.SeoFilename,
                    model.ImageModel.OverrideAltAttribute,
                    model.ImageModel.OverrideTitleAttribute);

                    await _pictureService.SetSeoFilename(picture.Id, _pictureService.GetPictureSeName(picture.TitleAttribute));
                    string pictureUrl = await _pictureService.GetPictureUrl(model.ImageModel.PictureId);
                    if (employee.Image != null)
                    {
                        employee.Image.PictureId = picture.Id;
                        employee.Image.CMSEntityId = employee.Id;
                        employee.Image.DisplayOrder = 0;
                        employee.Image.AltAttribute = model.ImageModel.OverrideAltAttribute;
                        employee.Image.MimeType = picture.MimeType;
                        employee.Image.SeoFilename = picture.SeoFilename;
                        employee.Image.TitleAttribute = model.ImageModel.OverrideTitleAttribute;
                        employee.Image.PictureUrl = pictureUrl;
                    }
                    else
                    {
                        employee.Image = new NewsEventFile {
                            PictureId = picture.Id,
                            CMSEntityId = employee.Id,
                            DisplayOrder = 0,
                            AltAttribute = model.ImageModel.OverrideAltAttribute,
                            MimeType = picture.MimeType,
                            SeoFilename = picture.SeoFilename,
                            TitleAttribute = model.ImageModel.OverrideTitleAttribute,
                            PictureUrl=pictureUrl
                        };
                    }
                }
                await _employeeService.UpdateEmployee(m);

                SuccessNotification(_localizationService.GetResource("Admin.Employee.Updated"));
                if (continueEditing)
                {
                    //selected tab
                    await SaveSelectedTabIndex();
                    return RedirectToAction("Edit", new { id = model.Id });
                }
                return RedirectToAction("List");
            }
            ViewBag.Type = new SelectList(GetTypes(), "Text", "Value", model.Type);
            //If we got this far, something failed, redisplay form
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Delete)]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var employee = await _employeeService.GetEmployeeById(id);
            if (employee == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                if (employee.Image != null)
                {
                    var pic = await _pictureService.GetPictureById(employee.Image.PictureId);
                    await _pictureService.DeletePicture(pic);
                }
                await _employeeService.DeleteEmployee(employee);
                SuccessNotification(_localizationService.GetResource("Admin.Employee.Deleted"));
                return RedirectToAction("List");
            }
            ErrorNotification(ModelState);
            return RedirectToAction("Edit", new { id = id });
        }
        public List<SelectListItem> GetTypes()
        {
            var types = new List<SelectListItem>() {

                     new SelectListItem {
                        Text="Hon. Minister",
                        Value="Hon. Minister",
                    }, new SelectListItem {
                        Text="Hon. State Minister",
                        Value="Hon. State Minister",
                    },
                      new SelectListItem {
                        Text="Employee",
                        Value="Employee",
                    }
            };
            types.Insert(0, new SelectListItem { Text = _localizationService.GetResource("Admin.Common.Select"), Value = "" });

            return types;
        }
    }
}
