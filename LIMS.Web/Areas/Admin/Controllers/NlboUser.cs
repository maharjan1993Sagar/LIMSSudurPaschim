﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class NlboUser : BaseAdminController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
