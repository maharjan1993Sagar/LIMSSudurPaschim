using LIMS.Website1.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Website1.Controllers
{
    public class ResourcesController : Controller
    {
        private readonly IConfiguration _config;
        private readonly ILogger<ResourcesController> _logger;
        private readonly DataContext _db;

        public ResourcesController(ILogger<ResourcesController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _db = new DataContext(_config);
        }

        public async Task<IActionResult> Index(string type)
        {

            var employee = await _db.GetResources();
            employee = employee.Where(m => m.Type==type).ToList();

            return View(employee);
            
        }
    }
}
