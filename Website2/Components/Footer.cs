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
            var employee = await _db.GetEmployee();
            employee.ForEach(m => m.Image.PictureUrl = GetPath(m.Image.PictureUrl));
            var informationOfficer = employee.FirstOrDefault(m => m.IsInformationOfficer);
            var speaker = employee.FirstOrDefault(m => m.Type == "Speaker");

            var footerVM = new FooterViewModel {
                ContactUs = contactUs,
                ImportantLinks = model,
                Customer=customer,
                SpokePerson=speaker,
                InformationOfficer=informationOfficer
            };

            return footerVM;
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