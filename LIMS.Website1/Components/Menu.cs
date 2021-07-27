using LIMS.Core;
using LIMS.Domain.Common;
using LIMS.Domain.Knowledgebase;
using LIMS.Domain.News;
using LIMS.Domain.Stores;
using LIMS.Framework.Components;
using LIMS.Services.Localization;
using LIMS.Services.Security;
using LIMS.Services.Seo;
using LIMS.Services.Topics;
using LIMS.Website1.Data;
using LIMS.Website1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.ViewComponents
{
    public class MenuViewComponent : BaseViewComponent
    {
        private readonly IConfiguration _config;
        private readonly DataContext _db;      

        public MenuViewComponent(IConfiguration config)
        {
            _config = config;
            _db = new DataContext(_config);
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {   
            var model = await PrepareMainMenu();
            return View(model);
        }
        private async Task<List<MainMenuModel>> PrepareMainMenu()
        {
            var model =await  _db.GetMainMenuModel();

            return model;
        }
    }
}