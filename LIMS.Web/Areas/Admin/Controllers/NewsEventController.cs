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

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class NewsEventController : BaseAdminController
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly INewsEventService _newsEventService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;
        private readonly SeoSettings _seoSettings;
        private readonly IMediator _mediator;

        public NewsEventController(
            IWebHostEnvironment hostEnvironment,
            INewsEventService newsEventService,
            IMediator mediator,
            ILanguageService languageService,
            ILocalizationService localizationService,
            IStoreService storeService,
            IWorkContext workContext,
            SeoSettings seoSettings
            )
        {
            _hostEnvironment = hostEnvironment;
            _newsEventService = newsEventService;
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
            var newsEvent = await _newsEventService.GetNewsEvent(command.Page - 1, command.PageSize);
            
            var gridModel = new DataSourceResult {
                Data = newsEvent,
                Total = newsEvent.TotalCount
            };
            return Json(gridModel);
        }

        [PermissionAuthorizeAction(PermissionActionName.Create)]
        public async Task<IActionResult> Create()
        {
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            var types = new SelectList(GetNewsEventType(), "Value", "Text").ToList();
            ViewBag.Type = types;
            var newsModel = new NewsEventTenderModel();
            newsModel.FileModel = new NewsEventFileModel();
            return View(newsModel);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(NewsEventTenderModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var NewsEvent = model.ToEntity();
                if (model.FileModel != null)
                {
                    if (!Directory.Exists(_hostEnvironment.WebRootPath + "/uploads/newsEvent"))
                    {
                        Directory.CreateDirectory(_hostEnvironment.WebRootPath + "/uploads/newsEvent");
                    }
                    
                    if (!Directory.Exists(_hostEnvironment.WebRootPath + "/uploads/newsEvent/"+ NewsEvent.NewsEventTenderId.ToString()))
                    {
                        Directory.CreateDirectory(_hostEnvironment.WebRootPath + "/uploads/newsEvent/"+ NewsEvent.NewsEventTenderId.ToString());
                    }

                    string uploads = Path.Combine(_hostEnvironment.WebRootPath, "uploads/newsEvent/"+ NewsEvent.NewsEventTenderId.ToString());
                    string filePath = Path.Combine(uploads, model.FileModel.File.FileName);
                    using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.FileModel.File.CopyToAsync(fileStream);
                    }

                    var newseventFile = new NewsEventFile { 
                    FileName=model.FileModel.File.FileName,
                    FileSize=model.FileModel.File.Length,
                    FilePath="~/uploads/newsEvent/"+NewsEvent.NewsEventTenderId.ToString()+"/"+ model.FileModel.File.FileName,
                    MimeType=model.FileModel.File.ContentType,
                    Type= model.FileModel.File.ContentType,
                    AltAttribute= model.FileModel.File.FileName,
                    NewsEventTenderId=NewsEvent.NewsEventTenderId.ToString(),
                    SeoFilename=model.FileModel.File.FileName                    
                    };
                    NewsEvent.NewsEventFile = newseventFile;
                }

                await _newsEventService.InsertNewsEvent(NewsEvent);

                

                SuccessNotification(_localizationService.GetResource("Admin.NewsEvent.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = NewsEvent.Id }) : RedirectToAction("List");
            }
            var types= new SelectList(GetNewsEventType(), "Value", "Text").ToList();
            ViewBag.Type = types;
           ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            return View(model);
        }


        [PermissionAuthorizeAction(PermissionActionName.Preview)]
        public async Task<IActionResult> Edit(string id)
        {
            var newsEvent = await _newsEventService.GetNewsEventById(id);
            if (newsEvent == null)
                //No blog post found with the specified id
                return RedirectToAction("List");
            var model = newsEvent.ToModel();
            ViewBag.Type = new SelectList(GetNewsEventType(),model.Type);
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(NewsEventTenderModel model, bool continueEditing)
        {
            var newsEvent = await _newsEventService.GetNewsEventById(model.Id);
            if (newsEvent == null)
               
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var m = model.ToEntity(newsEvent);
                await _newsEventService.UpdateNewsEvent(m);

                SuccessNotification(_localizationService.GetResource("Admin.NewsEvent.Updated"));
                if (continueEditing)
                {
                    //selected tab
                    await SaveSelectedTabIndex();
                    return RedirectToAction("Edit", new { id = model.Id });
                }
                return RedirectToAction("List");
            }
            ViewBag.Type = new SelectList(GetNewsEventType(), model.Type);
           //If we got this far, something failed, redisplay form
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Delete)]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var newsEvent = await _newsEventService.GetNewsEventById(id);
            if (newsEvent == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                await _newsEventService.DeleteNewsEvent(newsEvent);
                SuccessNotification(_localizationService.GetResource("Admin.NewsEvent.Deleted"));
                return RedirectToAction("List");
            }
            ErrorNotification(ModelState);
            return RedirectToAction("Edit", new { id = id });
        }
        
        public List<SelectListItem> GetNewsEventType() {
            var types = NoticeType.GetTypes();
            var selectTypes = types.Select(m => new SelectListItem {Text=m,Value=m }).ToList();
            selectTypes.Insert(0, new SelectListItem { Text = _localizationService.GetResource("Admin.Common.Select"), Value = "" });
            return selectTypes;
        }
    }
}
