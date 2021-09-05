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

        public async Task<IActionResult> Index(string pageName)
        {
            if (!String.IsNullOrEmpty(pageName))
            {
                var pageContent = await _db.GetPageContent(pageName);
                if (pageContent != null)
                {
                    pageContent.Image.PictureUrl = GetPath(pageContent.Image.PictureUrl);

                    return View(pageContent);
                }
                else
                {
                    return View(null);
                }
               
            }
            else
            {
                return View(null);  
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
