using LIMS.Core;
using LIMS.Domain.Seo;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Mvc.Filters;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.NewsEvent;
using LIMS.Services.Localization;
using LIMS.Services.Security;
using LIMS.Services.Stores;
using LIMS.Web.Areas.Admin.Extensions.Mapping;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Web.Areas.Admin.Models.NewsEvent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using LIMS.Domain.NewsEvent;
using MimeKit;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using LIMS.Services.Media;
using LIMS.Services.GeneralCMS;
using LIMS.Web.Areas.Admin.Models.GeneralCMS;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class BannerController : BaseAdminController
    {
        //private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IPictureService _pictureService;
        private readonly IBannerService _bannerService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;
        private readonly SeoSettings _seoSettings;
        private readonly IMediator _mediator;

        public BannerController(
            IPictureService pictureService,
            IBannerService bannerService,
            IMediator mediator,
            ILanguageService languageService,
            ILocalizationService localizationService,
            IStoreService storeService,
            IWorkContext workContext,
            SeoSettings seoSettings
            )
        {
            _pictureService = pictureService;
            _bannerService = bannerService;
            _mediator = mediator;
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
            var banner = await _bannerService.GetBannerByUser(command.Page - 1, command.PageSize);

            var gridModel = new DataSourceResult {
                Data = banner,
                Total = banner.TotalCount
            };
            return Json(gridModel);
        }

        [PermissionAuthorizeAction(PermissionActionName.Create)]
        public async Task<IActionResult> Download(string id)
        {
            var banner = await _bannerService.GetBannerById(id);
            if (banner.Image != null)
            {
                if (banner.Image.PictureId != null)
                {
                    var pic = await _pictureService.GetPictureById(banner.Image.PictureId);
                    var picByte = await _pictureService.LoadPictureBinary(pic);

                    return File(picByte, pic.MimeType);
                }
            }

            return RedirectToAction("Index");
        }
        
        [PermissionAuthorizeAction(PermissionActionName.Create)]
        public async Task<IActionResult> Create()
        {
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            return View();
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(BannerModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var banner = model.ToEntity();


                if ( model.ImageModel.PictureId!=null)
                {                 
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

                    

                    banner.Image = new NewsEventFile {
                            PictureId = picture.Id,
                            CMSEntityId = banner.Id,
                            DisplayOrder = 0,
                            AltAttribute = model.ImageModel.OverrideAltAttribute,
                            MimeType = picture.MimeType,
                            SeoFilename = picture.SeoFilename,
                            TitleAttribute = model.ImageModel.OverrideTitleAttribute,
                            PictureUrl = pictureUrl
                        };
                    
                }
                banner.UserId = _workContext.CurrentCustomer.Id;
                await _bannerService.InsertBanner(banner);

                SuccessNotification(_localizationService.GetResource("Admin.Banner.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = banner.Id }) : RedirectToAction("List");
            }
            return View(model);
        }


        [PermissionAuthorizeAction(PermissionActionName.Preview)]
        public async Task<IActionResult> Edit(string id)
        {
            var banner = await _bannerService.GetBannerById(id);
            if (banner == null)
                //No blog post found with the specified id
                return RedirectToAction("List");
            var model = banner.ToModel();
            model.ImageModel = new NewsEventFileModel {
                PictureId = banner.Image.PictureId,
                OverrideTitleAttribute = banner.Image.OverrideTitleAttribute
            };

            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(BannerModel model, bool continueEditing)
        {
            var banner = await _bannerService.GetBannerById(model.Id);
            if (banner == null)

                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var m = model.ToEntity(banner);

                if (banner.Image.PictureId != model.ImageModel.PictureId)
                {

                    var pic = await _pictureService.GetPictureById(banner.Image.PictureId);

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
                    if (banner.Image != null)
                    {
                        banner.Image.PictureId = picture.Id;
                        banner.Image.CMSEntityId = banner.Id;
                        banner.Image.DisplayOrder = 0;
                        banner.Image.AltAttribute = model.ImageModel.OverrideAltAttribute;
                        banner.Image.MimeType = picture.MimeType;
                        banner.Image.SeoFilename = picture.SeoFilename;
                        banner.Image.TitleAttribute = model.ImageModel.OverrideTitleAttribute;
                        banner.Image.PictureUrl = pictureUrl;
                    }
                    else
                    {


                        banner.Image = new NewsEventFile {
                            PictureId = picture.Id,
                            CMSEntityId = banner.Id,
                            DisplayOrder = 0,
                            AltAttribute = model.ImageModel.OverrideAltAttribute,
                            MimeType = picture.MimeType,
                            SeoFilename = picture.SeoFilename,
                            TitleAttribute = model.ImageModel.OverrideTitleAttribute,
                            PictureUrl = pictureUrl
                        };
                    }
                }

                await _bannerService.UpdateBanner(m);

                SuccessNotification(_localizationService.GetResource("Admin.Banner.Updated"));
                if (continueEditing)
                {
                    //selected tab
                    await SaveSelectedTabIndex();
                    return RedirectToAction("Edit", new { id = model.Id });
                }
                return RedirectToAction("List");
            }
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Delete)]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var banner = await _bannerService.GetBannerById(id);
            if (banner == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                await _bannerService.DeleteBanner(banner);

                if (banner.Image != null)
                {
                    var pic = await _pictureService.GetPictureById(banner.Image.PictureId);
                    await _pictureService.DeletePicture(pic);

                    _pictureService.DeletePictureOnFileSystem(pic);
                }

                SuccessNotification(_localizationService.GetResource("Admin.Banner.Deleted"));
                return RedirectToAction("List");
            }
            ErrorNotification(ModelState);
            return RedirectToAction("Edit", new { id = id });
        }

        [PermissionAuthorizeAction(PermissionActionName.Preview)]
        public async Task<IActionResult> Detail(string id)
        {
            var banner = await _bannerService.GetBannerById(id);
            if (banner == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var model = banner.ToModel();
                if(banner.Image!=null)
                {
                    model.ImageModel = new NewsEventFileModel {
                        Id = banner.Image.Id,
                        PictureId=banner.Image.PictureId,
                        PictureUrl=await _pictureService.GetPictureUrl(banner.Image.PictureId),
                        CMSEntityId=banner.Image.CMSEntityId
                    };

                }

               
                SuccessNotification(_localizationService.GetResource("Admin.Banner.Details"));
                return View(model);
            }
            ErrorNotification(ModelState);
            return RedirectToAction("List");
        }


    }
}
