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

namespace LIMS.Website1.Controllers
{
    public class PageContentController : Controller
    {
        private readonly IConfiguration _config;
        private readonly ILogger<PageContentController> _logger;
        private readonly DataContext _db;

        public PageContentController(ILogger<PageContentController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _db = new DataContext(_config);
        }

        public async Task<IActionResult> Index(string mainMenu, string subMenu, string subsubMenu,string id)
        {
            var newsEventTenders = await _db.GetNewsEventTenderByMenu(mainMenu, subMenu, subsubMenu, "");

            newsEventTenders
                .ForEach(m => m.Image.FilePath = GetPath(m.Image.FilePath));

            var pageContent = new PageContentModel();
            
            var objNews = newsEventTenders.FirstOrDefault(m => m.Id == id);
            pageContent.Title = objNews.Title;
            pageContent.Image = objNews.Image;
            pageContent.Description = objNews.Description;
            

            pageContent.Type = await _db.GetMainMenuName(mainMenu, subMenu, subsubMenu);

            return View(pageContent);
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
