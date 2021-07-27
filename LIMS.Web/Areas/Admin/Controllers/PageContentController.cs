using LIMS.Core;
using LIMS.Domain.Seo;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Mvc.Filters;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Localization;
using LIMS.Services.Security;
using LIMS.Services.Stores;
using LIMS.Web.Areas.Admin.Extensions.Mapping;
using LIMS.Web.Areas.Admin.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LIMS.Services.GeneralCMS;
using LIMS.Web.Areas.Admin.Models.GeneralCMS;
using LIMS.Services.Media;
using LIMS.Domain.NewsEvent;
using LIMS.Web.Areas.Admin.Models.NewsEvent;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class PageContentController : BaseAdminController
    {
        private readonly IPageContentService _pageContentService;
        private readonly IPictureService _pictureService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;
        private readonly SeoSettings _seoSettings;

        public PageContentController(
            IPageContentService pageContentService,
            IPictureService pictureService,
            ILanguageService languageService,
            ILocalizationService localizationService,
            IStoreService storeService,
            IWorkContext workContext,
            SeoSettings seoSettings
            )
        {
            _pageContentService = pageContentService;
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
            var pageContent = await _pageContentService.GetPageContentByUser(command.Page - 1, command.PageSize);
            
            var gridModel = new DataSourceResult {
                Data = pageContent,
                Total = pageContent.TotalCount
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
        public async Task<IActionResult> Create(PageContentModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var PageContent = model.ToEntity();

                var picture = await _pictureService.GetPictureById(model.ImageModel.PictureId);
                await _pictureService.UpdatePicture(picture.Id,
                await _pictureService.LoadPictureBinary(picture),
                picture.MimeType,
                picture.SeoFilename,
                model.ImageModel.OverrideAltAttribute,
                model.ImageModel.OverrideTitleAttribute);

                var contentFile = new NewsEventFile {
                    PictureId = picture.Id,
                    CMSEntityId=PageContent.PageContentId.ToString(),
                    DisplayOrder = 1,
                    AltAttribute = model.ImageModel.OverrideAltAttribute,
                    MimeType = picture.MimeType,
                    SeoFilename = picture.SeoFilename,
                    TitleAttribute = picture.TitleAttribute,
                    PictureUrl=(await _pictureService.GetPictureUrl(picture.Id))
                };

                await _pictureService.SetSeoFilename(picture.Id, _pictureService.GetPictureSeName(PageContent.PageName));

                PageContent.UserId = _workContext.CurrentCustomer.Id;
                PageContent.PageContentFile = contentFile;
                await _pageContentService.InsertPageContent(PageContent);
                SuccessNotification(_localizationService.GetResource("Admin.PageContent.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = PageContent.Id }) : RedirectToAction("List");
            }
           ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            return View(model);
        }


        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        public async Task<IActionResult> Edit(string id)
        {
            var pageContent = await _pageContentService.GetPageContentById(id);            
            if (pageContent == null)
                //No blog post found with the specified id
                return RedirectToAction("List");
            var model = pageContent.ToModel();
            model.ImageModel = new NewsEventFileModel {
                PictureId=pageContent.PageContentFile.PictureId,
                FileName=pageContent.PageContentFile.FileName,
                OverrideTitleAttribute=pageContent.PageContentFile.OverrideTitleAttribute
            };
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(PageContentModel model, bool continueEditing)
        {
            var pageContent = await _pageContentService.GetPageContentById(model.Id);
            if (pageContent == null)
               
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var m = model.ToEntity(pageContent);

                if (pageContent.PageContentFile.PictureId != model.ImageModel.PictureId)
                {
                    var pic = await _pictureService.GetPictureById(pageContent.PageContentFile.PictureId);

                    if (pic != null)
                    {
                        await _pictureService.DeletePicture(pic);
                    }

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
                    if (pageContent.PageContentFile != null)
                    {
                        pageContent.PageContentFile.PictureId = picture.Id;
                        pageContent.PageContentFile.CMSEntityId = pageContent.Id;
                        pageContent.PageContentFile.DisplayOrder = 0;
                        pageContent.PageContentFile.AltAttribute = model.ImageModel.OverrideAltAttribute;
                        pageContent.PageContentFile.MimeType = picture.MimeType;
                        pageContent.PageContentFile.SeoFilename = picture.SeoFilename;
                        pageContent.PageContentFile.TitleAttribute = model.ImageModel.OverrideTitleAttribute;
                        pageContent.PageContentFile.PictureUrl = pictureUrl;
                    }
                    else
                    {
                        pageContent.PageContentFile = new NewsEventFile {
                            PictureId = picture.Id,
                            CMSEntityId = pageContent.Id,
                            DisplayOrder = 0,
                            AltAttribute = model.ImageModel.OverrideAltAttribute,
                            MimeType = picture.MimeType,
                            SeoFilename = picture.SeoFilename,
                            TitleAttribute = model.ImageModel.OverrideTitleAttribute,
                            PictureUrl = pictureUrl
                        };
                    }
                }

               // string pictureUrl = await _pictureService.GetPictureUrl(picture.Id);

               
              await _pageContentService.UpdatePageContent(m);

                SuccessNotification(_localizationService.GetResource("Admin.PageContent.Updated"));
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
            var pageContent = await _pageContentService.GetPageContentById(id);
            if (pageContent == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                await _pageContentService.DeletePageContent(pageContent);
                SuccessNotification(_localizationService.GetResource("Admin.PageContent.Deleted"));
                return RedirectToAction("List");
            }
            ErrorNotification(ModelState);
            return RedirectToAction("Edit", new { id = id });
        }
        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpGet]
        public async Task<IActionResult> Preview(string id)
        {
            var pageContent = await _pageContentService.GetPageContentById(id);
            if (pageContent == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var model = pageContent.ToModel();
                model.ImageUrl = await _pictureService.GetPictureUrl(pageContent.PageContentFile.PictureId);
                SuccessNotification(_localizationService.GetResource("Admin.PageContent.Preview"));
                return View(model);
            }
            ErrorNotification(ModelState);
            return RedirectToAction("Preview", new { id = id });
        }
    }
}
