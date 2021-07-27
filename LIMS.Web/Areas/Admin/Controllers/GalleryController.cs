using LIMS.Core;
using LIMS.Domain.NewsEvent;
using LIMS.Domain.Seo;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Mvc;
using LIMS.Framework.Mvc.Filters;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Breed;
using LIMS.Services.GeneralCMS;
using LIMS.Services.Localization;
using LIMS.Services.Media;
using LIMS.Services.Security;
using LIMS.Services.Stores;
using LIMS.Web.Areas.Admin.Extensions.Mapping;
using LIMS.Web.Areas.Admin.Models.GeneralCMS;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Web.Areas.Admin.Models.Breed;
using LIMS.Web.Areas.Admin.Models.NewsEvent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class GalleryController : BaseAdminController
    {
        private readonly IGalleryService _galleryService;
        private readonly IPictureService _pictureService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;
        private readonly SeoSettings _seoSettings;

        public GalleryController(
            IGalleryService galleryService,
            ILanguageService languageService,
            IPictureService pictureService,
            ILocalizationService localizationService,
            IStoreService storeService,
            IWorkContext workContext,
            SeoSettings seoSettings
            )
        {
            _galleryService = galleryService;
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
            var gallery = await _galleryService.GetGalleryByUser(command.Page - 1, command.PageSize);
            
            var gridModel = new DataSourceResult {
                Data = gallery,
                Total = gallery.TotalCount
            };
            return Json(gridModel);
        }

        [PermissionAuthorizeAction(PermissionActionName.Create)]
        public async Task<IActionResult> Create()
        {
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            var type = GetTypes();
           ViewBag.Type =new SelectList(type,"Value","Text");
            return View();
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(GalleryModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var gallery = model.ToEntity();
                gallery.UserId = _workContext.CurrentCustomer.Id;
                await _galleryService.InsertGallery(gallery);
                SuccessNotification(_localizationService.GetResource("Admin.Gallery.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = gallery.Id }) : RedirectToAction("List");
            }
            var type = GetTypes();
            ViewBag.Type = new SelectList(type,"Value","Text",model.Type);
            return View(model);
        }


        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        public async Task<IActionResult> Edit(string id)
        {
            var gallery = await _galleryService.GetGalleryById(id);
            if (gallery == null)
                //No blog post found with the specified id
                return RedirectToAction("List");
            var model = gallery.ToModel();
            var type = GetTypes();
            ViewBag.Type =new SelectList(type,"Value","Text",model.Type);
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(GalleryModel model, bool continueEditing)
        {
            var gallery = await _galleryService.GetGalleryById(model.Id);
            if (gallery == null)
               
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var m = model.ToEntity(gallery);
                await _galleryService.UpdateGallery(m);

                SuccessNotification(_localizationService.GetResource("Admin.Gallery.Updated"));
                if (continueEditing)
                {
                    //selected tab
                    await SaveSelectedTabIndex();
                    return RedirectToAction("Edit", new { id = model.Id });
                }
                return RedirectToAction("List");
            }
            var type = GetTypes();
            ViewBag.Type =new SelectList(type,"Value","Text",model.Type);
            //If we got this far, something failed, redisplay form
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Delete)]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var gallery = await _galleryService.GetGalleryById(id);
            if (gallery == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                await _galleryService.DeleteGallery(gallery);
                SuccessNotification(_localizationService.GetResource("Admin.Gallery.Deleted"));
                return RedirectToAction("List");
            }
            ErrorNotification(ModelState);
            return RedirectToAction("Edit", new { id = id });
        }
        public List<SelectListItem> GetTypes() {
           var types= new List<SelectListItem>() {
                    
                     new SelectListItem {
                        Text="Photo",
                        Value="Photo",
                    },
                      new SelectListItem {
                        Text="Video",
                        Value="Video",
                    }
            };
            types.Insert(0, new SelectListItem { Text = _localizationService.GetResource("Admin.Common.Select"), Value = "" });
            return types;
        }

        #region Gallery Picture
        public async Task<IActionResult> PictureAdd(string pictureId, int displayOrder,
            string overrideAltAttribute, string overrideTitleAttribute,
            string galleryId)
        {
            if (string.IsNullOrEmpty(pictureId))
                throw new ArgumentException();

            var gallery = await _galleryService.GetGalleryById(galleryId);

            var picture = await _pictureService.GetPictureById(pictureId);
            if (picture == null)
                throw new ArgumentException("No picture found with the specified id");

            await _pictureService.UpdatePicture(picture.Id,
                await _pictureService.LoadPictureBinary(picture),
                picture.MimeType,
                picture.SeoFilename,
                overrideAltAttribute,
                overrideTitleAttribute);
            await _pictureService.SetSeoFilename(pictureId, _pictureService.GetPictureSeName(picture.TitleAttribute));

            string url = await _pictureService.GetPictureUrl(pictureId);

            await _galleryService.InsertPicture(new NewsEventFile {
                PictureId = pictureId,
                CMSEntityId = gallery.Id,
                DisplayOrder = displayOrder,
                AltAttribute = overrideAltAttribute,
                MimeType = picture.MimeType,
                SeoFilename = picture.SeoFilename,
                TitleAttribute = overrideTitleAttribute,
                PictureUrl=url
            });


            return Json(new { Result = true });
        }

        [PermissionAuthorizeAction(PermissionActionName.Preview)]
        [HttpPost]
        public async Task<IActionResult> PictureList(DataSourceRequest command, string galleryId)
        {
            var gallery = await _galleryService.GetGalleryById(galleryId);

            var picturesModel = new List<NewsEventFileModel>();
            if (gallery.Images!=null)
            {
                foreach (var x in gallery.Images.OrderBy(x => x.DisplayOrder))
                {
                    var picture = await _pictureService.GetPictureById(x.PictureId);
                    var m = new NewsEventFileModel {
                        Id = x.Id,
                        CMSEntityId = gallery.Id,
                        PictureId = x.PictureId,
                        PictureUrl = picture != null ? await _pictureService.GetPictureUrl(x.PictureId) : null,
                        OverrideAltAttribute = picture?.AltAttribute,
                        OverrideTitleAttribute = picture?.TitleAttribute,
                        DisplayOrder = x.DisplayOrder
                    };
                    picturesModel.Add(m);
                }
            }

            var gridModel = new DataSourceResult {
                Data = picturesModel,
                Total = picturesModel.Count
            };

            return Json(gridModel);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost]
        public async Task<IActionResult> PictureUpdate(NewsEventFileModel model)
        {
            if (ModelState.IsValid)
            {
                var gallery = await _galleryService.GetGalleryById(model.CMSEntityId);

                var picture = gallery.Images.Where(x => x.Id == model.Id).FirstOrDefault();
                if (picture == null)
                    throw new ArgumentException("No picture found with the specified id");
                picture.CMSEntityId = gallery.Id;

                var pic = await _pictureService.GetPictureById(picture.PictureId);
                if (pic == null)
                    throw new ArgumentException("No picture found with the specified id");

                picture.DisplayOrder = model.DisplayOrder;
                picture.MimeType = pic.MimeType;
                picture.SeoFilename = pic.SeoFilename;
                picture.AltAttribute = model.OverrideAltAttribute;
                picture.TitleAttribute = model.OverrideTitleAttribute;
                picture.PictureUrl = await _pictureService.GetPictureUrl(picture.PictureId);

                await _galleryService.UpdatePicture(picture);

                await _pictureService.UpdatePicture(pic.Id,
                    await _pictureService.LoadPictureBinary(pic),
                    picture.MimeType,
                    picture.SeoFilename,
                    model.OverrideAltAttribute,
                    model.OverrideTitleAttribute);

                return new NullJsonResult();
            }
            return ErrorForKendoGridJson(ModelState);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost]
        public async Task<IActionResult> PictureDelete(NewsEventFileModel model)
        {
            if (ModelState.IsValid)
            {
                var gallery = await _galleryService.GetGalleryById(model.CMSEntityId);

                var picture = gallery.Images.Where(x => x.Id == model.Id).FirstOrDefault();
                if (picture == null)
                    throw new ArgumentException("No farm picture found with the specified id");
                picture.CMSEntityId = gallery.Id;

                var pictureId = picture.PictureId;
                await _galleryService.DeletePicture(picture);

                var pic = await _pictureService.GetPictureById(pictureId);
                if (pic != null)
                    await _pictureService.DeletePicture(pic);

                return new NullJsonResult();
            }
            return ErrorForKendoGridJson(ModelState);
        }
        #endregion


    }
}
