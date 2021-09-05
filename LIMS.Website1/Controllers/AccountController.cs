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
using System.Data;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using LIMS.Website1.Resources;
using System.Reflection;
using System.ComponentModel;

namespace LIMS.Website1.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _config;
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _db;
        private readonly IGetLabel _lbl;
        //private IHostingEnvironment _env;


        public AccountController(ILogger<HomeController> logger, IConfiguration config, IGetLabel lbl)
        {
            _logger = logger;
            _config = config;
            _lbl = lbl;
            _db = new DataContext(_config);
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM obj)
        {
            if (ModelState.IsValid)
            {
                string token = await _db.GetToken(obj);
                if (!String.IsNullOrEmpty(token)) 
                {
                    var claims = new List<Claim>() {
                        new Claim(ClaimTypes.Email,obj.Email),
                        new Claim(ClaimTypes.Role,"AKC")
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties() {
                        IsPersistent = true
                    });
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Please try again.");
                return View();
            }
            return View(obj);

        }
        public async  Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }
        [Authorize]
        public IActionResult Index()
        {
            var all = _lbl.GetAll();

            return View(all);
        }

        [Authorize]
        public IActionResult CreateUpdate(string id,string title)
        {
            if (!String.IsNullOrEmpty(id))
            {
                var obj = _lbl.GetById(id);
                if (obj != null)
                {
                    return View(obj);
                }
            }
            if (!String.IsNullOrEmpty(title))
            {
                var obj = new LabelModel {Title=title };
                return View(obj);
            }
            var model = new LabelModel();
            return View(model);
        }
        [Authorize]
        [HttpPost]
        public IActionResult CreateUpdate(LabelModel obj)
        {
            if (ModelState.IsValid)
            {
                var objById = _lbl.GetById(obj.Id);

                if (objById == null)
                {
                    var objByTitle = _lbl.GetByTitle(obj.Title.Trim());

                    if (objByTitle == null)
                    {
                        var result = _lbl.CreateUpdate(obj);

                        TempData["Message"] = "Record Created Successfully.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("error", "Record with same title already exists.");
                        //ViewBag.ErrorMessage = "Invalid Inputs";
                        return View(obj);
                    }

                }
                else
                {
                    var result = _lbl.CreateUpdate(obj);

                    TempData["Message"] = "Record Updated Successfully.";
                    return RedirectToAction("Index");
                }
               
            }
            ModelState.AddModelError("error", "Invalid inputs");
            //ViewBag.ErrorMessage = "Invalid Inputs";
            return View(obj);
        }
        [Authorize]
        public IActionResult Delete(string id)
        {
            string result = _lbl.Delete(id);
            if (result == "success")
            {
                TempData["ErrorMessage"] = "Record deleted successfully.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Please Try Again.";
                return RedirectToAction("Index");
            }

        }
        //[Authorize]
        public IActionResult IndexNames()
        {
            var lstNames = new List<string>();
            Assembly ass = typeof(LoginVM).Assembly;
           var types = ClassObjects.GetTypesInNamespace(ass, "LIMS.WebSite1.Models");
             foreach (var item in types)
            {
                var props = item.GetProperties();
                if (props.Any())
                {
                    foreach (var prop in props)
                    {
                        var name = ClassObjects.GetDisplayName(prop);
                        if (name!=null)
                        {
                            lstNames.Add(name);
                        }
                    }
                }
            }
            var lstLabelModel = lstNames.Select(m => new LabelModel {
                Title = m
            }).ToList();

            return View(lstLabelModel);
        }
    }
}
