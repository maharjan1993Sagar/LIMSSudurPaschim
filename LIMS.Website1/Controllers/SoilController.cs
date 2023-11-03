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
    public class SoilController:Controller
    {
        private readonly IConfiguration _config;
        private readonly ILogger<SoilController> _logger;
        private readonly DataContext _db;

        public SoilController(ILogger<SoilController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _db = new DataContext(_config);
        }

        public async Task<IActionResult> Index(string type)
        {

            var employee = await _db.GetSoil();

            return View(employee);

        }
    }
}
