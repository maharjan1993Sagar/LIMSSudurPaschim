using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LIMS.Website1.Models;
using Microsoft.Extensions.Configuration;
using LIMS.Website1.Data;
using System.Net;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LIMS.Website1.Controllers
{
    public class NewsEventController : Controller
    {
        private readonly IConfiguration _config;
        private readonly ILogger<NewsEventController> _logger;
        private readonly DataContext _db;

        public NewsEventController(ILogger<NewsEventController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _db = new DataContext(_config);
        }
        public async Task<IActionResult> Index(string mainmenu, string submenu, string subsubmenu, string id)
        {
            var newsEventTenders = await _db.GetNewsEventTender("");
            var lstMainMenu = await _db.GetMainMenuModel();


            var lstSinglePageTypes = _config.GetSection("Constants:SingleTypes").Get<List<string>>();

            var lstNewsTypes = _config.GetSection("Constants:NewsTypes").Get<List<string>>();

            var lstFileTypes = _config.GetSection("Constants:FileTypes").Get<List<string>>();

            string type = "home";

            if (!String.IsNullOrEmpty(id))
            {
                var objNews1 = newsEventTenders.FirstOrDefault(m => m.Id == id);
                if (objNews1 != null)
                {
                    var objMainMenu1 = lstMainMenu.FirstOrDefault(m => m.MainMenuId == objNews1.Type);
                    if (objMainMenu1 != null)
                    {
                        type = objMainMenu1.MainMenuName;
                        if (lstSinglePageTypes.Contains(type.Trim().ToLower()))
                        {
                            return RedirectToAction("SPageContent", new { type = type, mainmenu = mainmenu, submenu = submenu, subsubmenu = subsubmenu, id = id });
                        }
                        else if (lstNewsTypes.Contains(type.Trim().ToLower()))
                        {
                            return RedirectToAction("NPageContent", new { type = type, mainmenu = mainmenu, submenu = submenu, subsubmenu = subsubmenu, id = id });

                        }
                        else if (lstFileTypes.Contains(type.Trim().ToLower()))
                        {
                            return RedirectToAction("FPageContent", new { type = type, mainmenu = mainmenu, submenu = submenu, subsubmenu = subsubmenu });
                        }
                        else
                        {
                            return RedirectToAction("PageNotFound");
                        }
                    }
                }

            }

            if (!String.IsNullOrEmpty(subsubmenu))
            {
                newsEventTenders = newsEventTenders.Where(m => m.SubSubMenu == subsubmenu).ToList();
            }

            if (!String.IsNullOrEmpty(submenu))
            {
                newsEventTenders = newsEventTenders.Where(m => m.SubMenu == submenu).ToList();
            }

            if (!string.IsNullOrEmpty(mainmenu))
            {
                newsEventTenders = newsEventTenders.Where(m => m.Type == mainmenu).ToList();
            }

            var objNews = newsEventTenders.FirstOrDefault();
            if (objNews != null)
            {
                var objMainMenu = lstMainMenu.FirstOrDefault(m => m.MainMenuId == objNews.Type);
                if (objMainMenu != null)
                {
                    type = objMainMenu.MainMenuName;
                    if (lstSinglePageTypes.Contains(type.Trim().ToLower()))
                    {
                        return RedirectToAction("SPageContent", new { type = type, mainmenu = mainmenu, submenu = submenu, subsubmenu = subsubmenu, id = id });
                    }
                    else if (lstNewsTypes.Contains(type.Trim().ToLower()))
                    {
                        return RedirectToAction("NPageContent", new { type = type, mainmenu = mainmenu, submenu = submenu, subsubmenu = subsubmenu, id = id });
                    }
                    else if (lstFileTypes.Contains(type.Trim().ToLower()))
                    {
                        return RedirectToAction("FPageContent", new { type = type, mainmenu = mainmenu, submenu = submenu, subsubmenu = subsubmenu });
                    }
                    else
                    {
                        return RedirectToAction("PageNotFound");
                    }
                }
            }
            return RedirectToAction("PageNotFound");
        }
        public async Task<IActionResult> PageNotFound()
        {
            return View();
        }

        public async Task<IActionResult> SPageContent(string type, string mainmenu, string submenu, string subsubmenu, string id)
        {
            var newsEventTenders = await _db.GetNewsEventTender("");
            newsEventTenders
                .ForEach(m => m.Image.FilePath = GetPath(m.Image.FilePath));
            var objNews = new NewsEventTenderModel();

            if (!String.IsNullOrEmpty(id))
            {
                objNews = newsEventTenders.FirstOrDefault(m => m.Id == id);
                return View(objNews);
            }
            else
            {
                if (!String.IsNullOrEmpty(subsubmenu))
                {
                    newsEventTenders = newsEventTenders.Where(m => m.SubSubMenu == subsubmenu).ToList();
                }
                if (!String.IsNullOrEmpty(submenu))
                {
                    newsEventTenders = newsEventTenders.Where(m => m.SubMenu == submenu).ToList();
                }
                if (!String.IsNullOrEmpty(mainmenu))
                {
                    newsEventTenders = newsEventTenders.Where(m => m.Type == mainmenu).ToList();
                }
                objNews = newsEventTenders.FirstOrDefault();
                return View(objNews);
            }

            return RedirectToAction("PageNotFound");
        }

        public async Task<IActionResult> NPageContent(string type, string mainmenu, string submenu, string subsubmenu, string id)
        {

            var newsEventTenders = await _db.GetNewsEventTender("");
            var mainMenus = await _db.GetMainMenuModel();
            var newsModel = new NewsEventViewModel();
            var objNews = new NewsEventTenderModel();

            newsEventTenders
                .ForEach(m => m.Image.FilePath = GetPath(m.Image.FilePath));

            if (!String.IsNullOrEmpty(id))
            {
                objNews = newsEventTenders.FirstOrDefault(m => m.Id == id);
                var objMainMenu = mainMenus.FirstOrDefault(m => m.MainMenuId == objNews.Type);
                type = (objMainMenu != null && !string.IsNullOrEmpty(type)) ? objMainMenu.MainMenuName : "News & Events";
            }

            if (!String.IsNullOrEmpty(subsubmenu))
            {
                newsEventTenders = newsEventTenders.Where(m => m.SubSubMenu == subsubmenu).ToList();
            }
            if (!String.IsNullOrEmpty(submenu))
            {
                newsEventTenders = newsEventTenders.Where(m => m.SubMenu == submenu).ToList();
            }
            if (!String.IsNullOrEmpty(mainmenu))
            {
                newsEventTenders = newsEventTenders.Where(m => m.Type == mainmenu).ToList();
            }

            if (!string.IsNullOrEmpty(type))
            {
                if (newsEventTenders.Any())
                {
                    if (String.IsNullOrEmpty(id))
                    {
                        objNews = newsEventTenders.FirstOrDefault();
                    }
                    var objMainMenu = mainMenus.FirstOrDefault(m => m.MainMenuId == objNews.Type);
                    type = (objMainMenu != null && !string.IsNullOrEmpty(type)) ? objMainMenu.MainMenuName : "News & Events";
                }
            }

            newsModel.ObjNews = objNews;
            newsModel.News = newsEventTenders;
            newsModel.Type = type;

            return View(newsModel);

        }

        public async Task<IActionResult> FPageContent(string type, string mainmenu, string submenu, string subsubmenu)
        {
            var newsEventTenders = await _db.GetNewsEventTender("");
            var mainMenus = await _db.GetMainMenuModel();
            var newsModel = new NewsEventViewModel();
            var objNews = new NewsEventTenderModel();
            //string type = "";
            newsEventTenders
                .ForEach(m => m.Image.FilePath = GetPath(m.Image.FilePath));



            if (!String.IsNullOrEmpty(subsubmenu))
            {
                newsEventTenders = newsEventTenders.Where(m => m.SubSubMenu == subsubmenu).ToList();
            }
            if (!String.IsNullOrEmpty(submenu))
            {
                newsEventTenders = newsEventTenders.Where(m => m.SubMenu == submenu).ToList();
            }
            if (!String.IsNullOrEmpty(mainmenu))
            {
                newsEventTenders = newsEventTenders.Where(m => m.Type == mainmenu).ToList();
            }
            if (String.IsNullOrEmpty(type))
            {
                mainmenu = newsEventTenders.FirstOrDefault().Type;

                var objMainMenu = mainMenus.FirstOrDefault(m => m.MainMenuId == mainmenu);
                type = (objMainMenu != null && !string.IsNullOrEmpty(type)) ? objMainMenu.MainMenuName : "Publications";
            }
         
            newsModel.News = newsEventTenders;
            newsModel.Type = type;

            return View(newsModel);
        }




        public async Task<IActionResult> Download(string id)
        {
            var newsEvent = await _db.GetNewsEventTender("");
            var newsById = newsEvent.FirstOrDefault(m => m.Id == id);

            if (newsById != null)
            {
                string fileNameById = newsById.Image.FilePath.Substring(2, newsById.Image.FilePath.Length - 2);
                string filePath = _config.GetValue<string>("Constants:PhysicalPath") + newsById.Image.FilePath.Substring(2, newsById.Image.FilePath.Length - 2);

                string fileName = "Download.jpg";

                int idx = fileNameById.IndexOf('/');

                if (idx > -1)
                {
                    fileName = fileNameById.Substring(idx + 1);
                }

                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

                return File(fileBytes, "application/octet-stream", fileName);

            }
            else
            {
                TempData["ErrorMessage"] = "Error downloading file.";
                return RedirectToAction("Index");
            }

        }

        //public async Task<IActionResult> AboutUs()
        //{
        //    var newsEventTenders = await _db.GetNewsEventTender("");

        //    newsEventTenders
        //        .ForEach(m => m.Image.FilePath = GetPath(m.Image.FilePath));

        //    var newsEventVM = new NewsEventViewModel();
        //    if (!string.IsNullOrEmpty(type))
        //    {
        //        newsEventTenders = newsEventTenders.Where(m => m.Type == type).ToList();
        //        if (newsEventTenders.FirstOrDefault() != null)
        //        {
        //            newsEventVM.TypeName = newsEventTenders.FirstOrDefault().TypeName;
        //        }
        //        else
        //        {
        //            newsEventVM.TypeName = "";
        //        }
        //        newsEventVM.News = newsEventTenders;

        //        if (!String.IsNullOrEmpty(id))
        //        {
        //            var objNews = newsEventTenders.FirstOrDefault(m => m.Id == id);
        //            newsEventVM.ObjNews = objNews;
        //        }
        //        else
        //        {
        //            newsEventVM.ObjNews = newsEventTenders.OrderByDescending(m => m.UploadedDate).FirstOrDefault();
        //        }

        //    }
        //    if (!string.IsNullOrEmpty(submenu))
        //    {
        //        newsEventTenders = newsEventTenders.Where(m => m.SubMenu == submenu).ToList();
        //        newsEventVM.News = newsEventTenders;
        //        try
        //        {
        //            newsEventVM.TypeName = newsEventTenders.FirstOrDefault().TypeName;
        //        }
        //        catch
        //        {
        //            newsEventVM.TypeName = "";
        //        }
        //        if (!String.IsNullOrEmpty(id))
        //        {
        //            var objNews = newsEventTenders.FirstOrDefault(m => m.Id == id);
        //            newsEventVM.ObjNews = objNews;
        //        }
        //        else
        //        {
        //            newsEventVM.ObjNews = newsEventTenders.OrderByDescending(m => m.UploadedDate).FirstOrDefault();
        //        }

        //    }
        //    if (!string.IsNullOrEmpty(subsubmenu))
        //    {
        //        newsEventTenders = newsEventTenders.Where(m => m.SubSubMenu == subsubmenu).ToList();

        //        newsEventVM.News = newsEventTenders;
        //        try
        //        {
        //            newsEventVM.TypeName = newsEventTenders.FirstOrDefault().TypeName;
        //        }
        //        catch
        //        {
        //            newsEventVM.TypeName = "";
        //        }
        //        if (!String.IsNullOrEmpty(id))
        //        {
        //            var objNews = newsEventTenders.FirstOrDefault(m => m.Id == id);
        //            newsEventVM.ObjNews = objNews;
        //        }
        //        else
        //        {
        //            newsEventVM.ObjNews = newsEventTenders.OrderByDescending(m => m.UploadedDate).FirstOrDefault();
        //        }

        //    }
        //    if (newsEventVM != null)
        //    {
        //        //newsEventVM.Type = ViewBag.type;
        //        return View(newsEventVM);
        //    }
        //    else
        //    {
        //        return View(new NewsEventViewModel());
        //    }

        //}
        public async Task<IActionResult> Details(string id)
        {
            var newsEvent = await _db.GetNewsEventTender("");
            var newsById = newsEvent.FirstOrDefault(m => m.Id == id);
            return View(newsById);


        }
        public string GetPath(string path)
        {
            string basePath = _config.GetValue<string>("Constants:FileBaseUrl");
            if (!string.IsNullOrEmpty(path))
            {
                if (path.Contains("~"))
                {
                    return basePath + path.Substring(2, path.Length - 2);
                }
                else
                {
                    return basePath + path.Substring(1, path.Length - 1);
                }
            }
            else
            {
                return path;
            }
        }
    }
}
