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
    public class EmployeeController : Controller
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmployeeController> _logger;
        private readonly DataContext _db;

        public EmployeeController(ILogger<EmployeeController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _db = new DataContext(_config);
        }

        public async Task<IActionResult> Index()
        {
            var employee = await _db.GetEmployee();
            employee = employee.OrderBy(m => m.SerialNo).ToList();
            var minister = employee.Where(m => m.Type == "Hon. Minister" || m.Type == "Hon. State Minister").ToList();
            employee = employee.Except(minister).ToList();
            employee.ForEach(m=>m.Image.PictureUrl =GetPath(m.Image.PictureUrl));

            return View(employee);
        }
        public async Task<IActionResult> Details(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                var employee = await _db.GetEmployee();
                employee = employee.OrderBy(m => m.SerialNo).ToList();
                employee.ForEach(m => m.Image.PictureUrl = GetPath(m.Image.PictureUrl));

                var objEmployee = employee.FirstOrDefault(m => m.Id == id);
                if (objEmployee != null)
                {
                    return View(objEmployee);
                }
                else
                {
                    TempData["ErrorMessage"] = "Please try again.";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Please try again.";
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
