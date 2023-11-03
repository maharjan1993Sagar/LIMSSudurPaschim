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
    public class FooterViewComponent : BaseViewComponent
    {
        private readonly IConfiguration _config;
        private readonly DataContext _db;      

        public FooterViewComponent(IConfiguration config)
        {
            _config = config;
            _db = new DataContext(_config);
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await PrepareFooter();
            return View(model);
        }
        private async Task<FooterViewModel> PrepareFooter()
        {
            var model =await _db.GetImportantLinks();

            var contactUs = await _db.GetContactUsModel();
            var customer =await _db.GetCustomer();
            var galleryVideo = await _db.GetGallery();

            string map = "";
           var Facebook = galleryVideo.OrderByDescending(m => m.CreatedDate).Where(m => m.Type == "Map").FirstOrDefault();
            if(Facebook!=null)
            {
                map = Facebook.VideoUrl; 
            }
            var footerVM = new FooterViewModel {
                ContactUs = contactUs,
                ImportantLinks = model,
                Customer=customer,
                Map=map
            };

            return footerVM;
        }
    }
}