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
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;

namespace LIMS.Website1.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _config;
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _db;


        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _db = new DataContext(_config);
        }

        public async Task<IActionResult> Index()
        {
            string baseUrl = _config.GetValue<string>("Constants:FileBaseUrl");

            var newsEventTenders = await _db.GetNewsEventTender("");

            newsEventTenders
                .ForEach(m => m.Image.FilePath = GetPath(m.Image.FilePath));


            var banner = await _db.GetBanner();

            banner
                .ForEach(m => m.Image.PictureUrl = GetPath(m.Image.PictureUrl));

            var employee = await _db.GetEmployee();
            employee.ForEach(m => m.Image.PictureUrl = GetPath(m.Image.PictureUrl));


            var director = employee.FirstOrDefault(m => m.Designation == "Director");
            var informationOfficer = employee.FirstOrDefault(m => m.Designation == "Information Officer");

            var pageContent = await _db.GetPageContent("Home");
            if (pageContent != null)
            {
                pageContent.Image.PictureUrl = GetPath(pageContent.Image.PictureUrl);

            }
            var newsScroll = newsEventTenders.Where(m => m.IsScroll);
            var news = newsEventTenders.Where(m => m.Type == "News");
            var events = newsEventTenders.Where(m => m.Type == "Event");
            var tenders = newsEventTenders.Where(m => m.Type == "Tender");
            var notices = newsEventTenders.Where(m => m.Type == "Notice");
            var rules = newsEventTenders.Where(m => m.Type == "Rules");
            var directives = newsEventTenders.Where(m => m.Type == "Directives");
            var acts = newsEventTenders.Where(m => m.Type == "Act");
            var reports = newsEventTenders.Where(m => m.Type == "Report");
            var otherFiles = newsEventTenders.Where(m => m.Type == "OtherFiles");
            var pressRelease = newsEventTenders.Where(m => m.Type == "PressRelease");
            var letters = newsEventTenders.Where(m => m.Type == "Letter");

            var galleryVideo = await _db.GetGallery();
            galleryVideo
                .ForEach(m => m.Images
                .ForEach(n => n.PictureUrl = GetPath(n.PictureUrl)));


            var gallery = galleryVideo.OrderByDescending(m => m.CreatedDate).Where(m => m.Type == "Photo");
            var video = galleryVideo.OrderByDescending(m => m.CreatedDate).Where(m => m.Type == "Video");

            var homeVM = new HomeModel {
                NewsScroll = newsScroll.ToList(),
                Banner = banner,
                Employee = employee,
                PageContent = pageContent,
                Notices = notices.Take(notices.ToList().Count > 4 ? 4 : notices.ToList().Count).ToList(),
                News = news.Take(news.ToList().Count > 4 ? 4 : news.ToList().Count).ToList(),
                Tenders = tenders.Take(tenders.ToList().Count > 4 ? 4 : tenders.ToList().Count).ToList(),
                Events = events.Take(events.ToList().Count > 4 ? 4 : events.ToList().Count).ToList(),
                InformationOfficer = informationOfficer,
                Director = director,
                ActsPolices = acts.Take(acts.ToList().Count > 4 ? 4 : acts.ToList().Count).ToList(),
                Directives = directives.Take(directives.ToList().Count > 4 ? 4 : directives.ToList().Count).ToList(),
                Letters = letters.Take(letters.ToList().Count > 4 ? 4 : letters.ToList().Count).ToList(),
                PressRelease = pressRelease.Take(pressRelease.ToList().Count > 4 ? 4 : pressRelease.ToList().Count).ToList(),
                OtherFiles = otherFiles.Take(otherFiles.ToList().Count > 4 ? 4 : otherFiles.ToList().Count).ToList(),
                Reports = reports.Take(reports.ToList().Count > 4 ? 4 : reports.ToList().Count).ToList(),
                RulesRegulation = rules.Take(rules.ToList().Count > 4 ? 4 : rules.ToList().Count).ToList(),
                Gallery = gallery.FirstOrDefault(),
                Video = video.FirstOrDefault(),
                Galleries = gallery.Take(gallery.ToList().Count > 4 ? 4 : gallery.ToList().Count).ToList(),
                Videos = video.Take(video.ToList().Count > 4 ? 4 : video.ToList().Count).ToList(),
            };
            return View(homeVM);
        }

        //public List<Type> TakeFour(List<Type> list )
        //{
        //    return list.Take(list.Count > 4 ? 4 : list.Count).ToList();
        //}

        public async Task<IActionResult> ContactUs()
        {
            var contactUs = await _db.GetContactUsModel();

            return View(contactUs);
        }
        public List<string> Menus()
        {
            var lst = new List<string> { "A", "B", "C", "D" };
            return lst;
        }

        //[HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }
        public async Task<IActionResult> Download(string id)
        {
            var newsEvent = await _db.GetNewsEventTender("");
            var newsById = newsEvent.FirstOrDefault(m => m.Id == id);

            if (newsById != null)
            {
                if (!string.IsNullOrEmpty(newsById.Image.FilePath))
                {
                    string filePathById = newsById.Image.FilePath.Substring(2, newsById.Image.FilePath.Length - 2);
                    string filePath = _config.GetValue<string>("Constants:PhysicalPath") + newsById.Image.FilePath.Substring(2, newsById.Image.FilePath.Length - 2);

                    // filePath = "http:/localhost:16595/uploads/newsEvent/2c26dd24-07a8-4200-8724-67ca8d3b520b/logo.png";
                    int idx = filePathById.LastIndexOf('/');
                    string fileName = "Download.jpg";

                    if (idx != -1)
                    {
                        fileName = filePathById.Substring(idx + 1);
                    }


                    byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

                    return File(fileBytes, "application/octet-stream", fileName);
                }
                else
                {
                    TempData["ErrorMessage"] = "Error downloading file.";
                    return RedirectToAction("Index");
                }

                //var content = new System.IO.MemoryStream(data);
                //var contentType = "APPLICATION/octet-stream";
                //var fileName = "something.bin";
                //return File(content, contentType, fileName);

                // byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

                // string remoteUri = "http://www.contoso.com/library/homepage/images/";
                //  string fileName = "ms-banner.gif", myStringWebResource = null;

                // Create a new WebClient instance.
                //using (WebClient myWebClient = new WebClient())
                //{
                //   /// myStringWebResource = remoteUri + fileName;
                //    // Download the Web resource and save it into the current filesystem folder.
                //    myWebClient.DownloadFile(filePath, fileName);
                //}

                //TempData["Message"] = "File downloaded successfully.";
                //return RedirectToAction("Index");
                //return File(fileBytes, "application/force-download", fileName);
                // return File(fileBytes, "application/octet-stream", fileName);
            }
            else
            {
                TempData["ErrorMessage"] = "Error downloading file.";
                return RedirectToAction("Index");
            }

        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
