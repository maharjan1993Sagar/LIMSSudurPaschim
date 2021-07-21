using LIMS.Core;
using LIMS.Framework.Components;
using LIMS.Services.DynamicMenu;
using LIMS.Website1.Data;
using LIMS.Website1.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Components
{
    public class DynamicMenuViewComponent : BaseViewComponent
    {
        private readonly IConfiguration _config;
        private readonly DataContext _db;
        public DynamicMenuViewComponent(IConfiguration config)
        {
            _config = config;
            _db = new DataContext(_config);
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var menus = await _db.GetMainMenuModel();

            return View(menus);
        }
    }
}
