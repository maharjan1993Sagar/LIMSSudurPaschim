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
    public class FilesController : Controller
    {
        private readonly IConfiguration _config;
        private readonly ILogger<FilesController> _logger;
        private readonly DataContext _db;

        public FilesController(ILogger<FilesController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _db = new DataContext(_config);
        }
        public async Task<IActionResult> Index(string mainmenu, string submenu, string subsubmenu, string id)
        {
            var FilesTenders = await _db.GetNewsEventTenderByMenu(mainmenu,submenu, subsubmenu,"");
                 
            FilesTenders
                .ForEach(m => m.Image.FilePath = GetPath(m.Image.FilePath));

            var FilesVM = new NewsEventViewModel();
            FilesVM.News = FilesTenders;// FilesTenders.Take(FilesTenders.ToList().Count > 10 ? 10 : FilesTenders.ToList().Count).ToList();

            FilesVM.Type =await _db.GetMainMenuName(mainmenu,submenu,subsubmenu);
          
            return View(FilesVM);
        }
        public async Task<IActionResult> Download(string id)
        {
            var Files = await _db.GetNewsEventTender("");
            var newsById = Files.FirstOrDefault(m => m.Id == id);

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
