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
using System;
using System.IO;
using LIMS.Domain.DynamicMenu;
using LIMS.Services.DynamicMenu;

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
        private readonly IMainMenuService _mainMenu;
        private readonly ISubMenuService _subMenuService;
        private readonly ISubSubMenuService _subSubMenuService;

        public NewsEventController(
            IWebHostEnvironment hostEnvironment,
            INewsEventService newsEventService,
            IMediator mediator,
            ILanguageService languageService,
            ILocalizationService localizationService,
            IStoreService storeService,
            IWorkContext workContext,
            SeoSettings seoSettings,
            IMainMenuService mainMenu,
            ISubMenuService subMenuService,
            ISubSubMenuService subSubMenuService
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
            _mainMenu = mainMenu;
            _subMenuService = subMenuService;
            _subSubMenuService = subSubMenuService;
        }

        public IActionResult Index() => RedirectToAction("List");

        public IActionResult List() => View();

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command)
        {
            var newsEvent = await _newsEventService.GetNewsEventByUser(command.Page - 1, command.PageSize);
          
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
            var types = new SelectList(await GetNewsEventType(), "Value", "Text").ToList();
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
                    CMSEntityId=NewsEvent.NewsEventTenderId.ToString(),
                    SeoFilename=model.FileModel.File.FileName                    
                    };
                    NewsEvent.NewsEventFile = newseventFile;
                }
                NewsEvent.UserId = _workContext.CurrentCustomer.Id;
                NewsEvent.subMenus = await _subMenuService.GetSubMenuById(NewsEvent.SubMenu);
                NewsEvent.subSubMenus = await _subSubMenuService.GetSubSubMenuById(NewsEvent.SubSubMenu);
                NewsEvent.Mainmenu = await _mainMenu.GetMainMenuById(NewsEvent.Type);
                NewsEvent.CreatedDate = DateTime.Now;
                NewsEvent.CreatedBy = _workContext.CurrentCustomer.Id;

                if (NewsEvent.subSubMenus!=null)
                {
                    NewsEvent.TypeName = NewsEvent.subSubMenus.SubSubMenuName;
                }
                else if (NewsEvent.subMenus!=null)
                {
                    NewsEvent.TypeName = NewsEvent.subMenus.Name;
                }
                else
                {
                    NewsEvent.TypeName = NewsEvent.Mainmenu.MainMenuName;
                }

                await _newsEventService.InsertNewsEvent(NewsEvent);

                SuccessNotification(_localizationService.GetResource("Admin.NewsEvent.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = NewsEvent.Id }) : RedirectToAction("List");
            }
            var types= new SelectList(await GetNewsEventType(), "Value", "Text").ToList();
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
            model.FileModel = new NewsEventFileModel {
                PictureId = newsEvent.NewsEventFile.PictureId,
                FileName=newsEvent.NewsEventFile.FileName,
                CMSEntityId=newsEvent.NewsEventFile.CMSEntityId
            };
            ViewBag.Type = new SelectList(await GetNewsEventType(),"Value","Text",model.Type);
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

                if (model.FileModel!= null&&model.FileModel.File!=null)
                {
                    if (!Directory.Exists(_hostEnvironment.WebRootPath + "/uploads/newsEvent"))
                    {
                        Directory.CreateDirectory(_hostEnvironment.WebRootPath + "/uploads/newsEvent");
                    }

                    if (!Directory.Exists(_hostEnvironment.WebRootPath + "/uploads/newsEvent/" + newsEvent.NewsEventTenderId.ToString()))
                    {
                        Directory.CreateDirectory(_hostEnvironment.WebRootPath + "/uploads/newsEvent/" + newsEvent.NewsEventTenderId.ToString());
                    }

                    
                        if (System.IO.File.Exists(_hostEnvironment.WebRootPath + "/uploads/newsEvent/" + newsEvent.NewsEventTenderId.ToString() + "/" +newsEvent.NewsEventFile.FileName))
                        {
                            System.IO.File.Delete(_hostEnvironment.WebRootPath + "/uploads/newsEvent/" + newsEvent.NewsEventTenderId.ToString()+"/"+newsEvent.NewsEventFile.FileName);
                           
                            if (!Directory.Exists(_hostEnvironment.WebRootPath + "/uploads/newsEvent/" + newsEvent.NewsEventTenderId.ToString()))
                            {
                                Directory.CreateDirectory(_hostEnvironment.WebRootPath + "/uploads/newsEvent/" + newsEvent.NewsEventTenderId.ToString());
                            }
                        }
                    

                    string uploads = Path.Combine(_hostEnvironment.WebRootPath, "uploads/newsEvent/" + newsEvent.NewsEventTenderId.ToString());
                    string filePath = Path.Combine(uploads, model.FileModel.File.FileName);
                    using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.FileModel.File.CopyToAsync(fileStream);
                    }

                    var newseventFile = new NewsEventFile {
                        FileName = model.FileModel.File.FileName,
                        FileSize = model.FileModel.File.Length,
                        FilePath = "~/uploads/newsEvent/" + newsEvent.NewsEventTenderId.ToString() + "/" + model.FileModel.File.FileName,
                        MimeType = model.FileModel.File.ContentType,
                        Type = model.FileModel.File.ContentType,
                        AltAttribute = model.FileModel.File.FileName,
                        CMSEntityId = newsEvent.NewsEventTenderId.ToString(),
                        SeoFilename = model.FileModel.File.FileName
                    };
                    newsEvent.NewsEventFile = newseventFile;
                }
               
                var m = model.ToEntity(newsEvent);

                m.subMenus = await _subMenuService.GetSubMenuById(m.SubMenu);
                m.subSubMenus = await _subSubMenuService.GetSubSubMenuById(m.SubSubMenu);
                m.Mainmenu = await _mainMenu.GetMainMenuById(m.Type);
                m.UpdatedDate = DateTime.Now;
                m.UpdatedBy = _workContext.CurrentCustomer.Id;

                if (m.subSubMenus != null)
                {
                    m.TypeName = m.subSubMenus.SubSubMenuName;
                }
                else if (m.subMenus != null)
                {
                    m.TypeName = m.subMenus.Name;
                }
                else
                {
                    m.TypeName = m.Mainmenu.MainMenuName;
                }
               
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
            ViewBag.Type = new SelectList(await GetNewsEventType(), model.Type);
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
                return Json(true);
            }
            return Json(null);
        }
        
        public async Task<List<SelectListItem>> GetNewsEventType() {
            var user = _workContext.CurrentCustomer.Id;
            var types = await _mainMenu.GetByUser(user);
            var selectTypes = types.ToList().Select(m => new SelectListItem {Text=m.MainMenuName,Value=m.Id }).ToList();
            selectTypes.Insert(0, new SelectListItem { Text = _localizationService.GetResource("Admin.Common.Select"), Value = "" });
            return selectTypes;
        }
        public async Task<List<SelectListItem>> GetSubmenu(string mainMenuId)
        {
            var user = _workContext.CurrentCustomer.Id;
            var submenu = await _subMenuService.GetSubMenuByUser();
            var types = submenu.Where(m => m.MainMenuId == mainMenuId);
            var selectTypes = types.ToList().Select(m => new SelectListItem { Text = m.Name, Value = m.Id }).ToList();
            selectTypes.Insert(0, new SelectListItem { Text = _localizationService.GetResource("Admin.Common.Select"), Value = "" });
            return selectTypes;
        }
        public async Task<List<SelectListItem>> GetSubSubMenu(string subMenuId)
        {
            var user = _workContext.CurrentCustomer.Id;
            var submenu = await _subSubMenuService.GetSubSubMenuByUser();
            var types = submenu.Where(m => m.SubMenuId == subMenuId);
            var selectTypes = types.ToList().Select(m => new SelectListItem { Text = m.SubSubMenuName, Value = m.Id }).ToList();
            selectTypes.Insert(0, new SelectListItem { Text = _localizationService.GetResource("Admin.Common.Select"), Value = "" });
            return selectTypes;
        }
    }
}
