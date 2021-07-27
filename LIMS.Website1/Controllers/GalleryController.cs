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
    public class GalleryController : Controller
    {
        private readonly IConfiguration _config;
        private readonly ILogger<GalleryController> _logger;
        private readonly DataContext _db;

        public GalleryController(ILogger<GalleryController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _db = new DataContext(_config);
        }

        public async Task<IActionResult> Index()
        {
            var galleryVideo = await _db.GetGallery();
            galleryVideo
                .ForEach(m => m.Images
                .ForEach(n => n.PictureUrl = GetPath(n.PictureUrl)));
                

            var gallery = galleryVideo.OrderByDescending(m=>m.CreatedDate).Where(m => m.Type == "Photo");
            var video = galleryVideo.OrderByDescending(m=>m.CreatedDate).Where(m => m.Type == "Video");

            var galleryVM = new GalleryViewModel {
                Galleries=gallery.ToList(),
                Videos=video.ToList()
            };
            return View(galleryVM);
        }


        public async Task<IActionResult> Details(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                var galleryVideo = await _db.GetGallery();
                galleryVideo
                    .ForEach(m => m.Images
                    .ForEach(n => n.PictureUrl = GetPath(n.PictureUrl)));

                var objGallery = galleryVideo.FirstOrDefault(m => m.Id == id);
                if (objGallery != null)
                {
                    return View(objGallery);
                }
                else
                {
                    return View(null);
                }
            }
            else
            {
                return View();
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
