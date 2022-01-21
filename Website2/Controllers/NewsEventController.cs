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
        public async Task<IActionResult> Index(string type, string id)
        {
            ViewBag.Types = new List<SelectListItem>() {
                            new SelectListItem{Text="--Select--",Value="" },
                            new SelectListItem{Text="Tender",Value="Tender" },
                            new SelectListItem{Text="Press",Value="Press" },
                            new SelectListItem{Text="Letters",Value="Letters" },
                            new SelectListItem{Text="Rules & Regulation",Value="Rules & Regulation" },
                            new SelectListItem{Text="Directives",Value="Directives" },
                            new SelectListItem{Text="Act & Policies",Value="Act & Policies" },
                            new SelectListItem{Text="Reports",Value="Reports" },
                            new SelectListItem{Text="Other Files",Value="Other Files" },
                            };
            type = String.IsNullOrEmpty(type) ? "News" : type;

            var newsEventTenders = await _db.GetNewsEventTender("");

            newsEventTenders
                .ForEach(m => m.Image.FilePath = GetPath(m.Image.FilePath));

            var newsEventVM = new NewsEventViewModel();
            newsEventVM.NewsAndEvent = newsEventTenders.Take(newsEventTenders.ToList().Count > 10 ? 10 : newsEventTenders.ToList().Count).ToList();

            if (!String.IsNullOrEmpty(type))
            {
                newsEventTenders = newsEventTenders.Where(m => m.Type == type).ToList();
                newsEventVM.News = newsEventTenders;
                if (type == "News" || type == "Event" || type=="Notice")
                {
                    if (!String.IsNullOrEmpty(id))
                    {
                        var objNews = newsEventTenders.FirstOrDefault(m => m.Id == id);
                        newsEventVM.ObjNews = objNews;
                    }
                    else
                    {
                        newsEventVM.ObjNews = newsEventTenders.OrderByDescending(m => m.UploadedDate).FirstOrDefault();
                    }
                }
            }
            ViewBag.Type = type;
            newsEventVM.Type = type;
            return View(newsEventVM);
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

                return File(fileBytes,"application/octet-stream",fileName);
               
            }
            else
            {
                TempData["ErrorMessage"] = "Error downloading file.";
                return RedirectToAction("Index");
            }

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
