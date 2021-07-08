using LIMS.Core.ModelBinding;
using LIMS.Domain;
using LIMS.Domain.NewsEvent;
using LIMS.Web.Areas.Admin.Models.NewsEvent;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Web.Areas.Admin.Models.GeneralCMS

{
   public class PageContentModel:BaseEntity
    {
        [LIMSResourceDisplayName("Admin.PageContent.PageName")]
        public string PageName { get; set; }
        [LIMSResourceDisplayName("Admin.PageContent.Title")]

        public string Title { get; set; }
        [LIMSResourceDisplayName("Admin.PageContent.Description")]

        public string Description { get; set; }
        [LIMSResourceDisplayName("Admin.PageContent.ShowImage")]

        public bool ShowImage { get; set; }
        [LIMSResourceDisplayName("Admin.PageContent.Image")]

        public IFormFile Image { get; set; }
        [LIMSResourceDisplayName("Admin.PageContent.ImageFile")]

        public NewsEventFileModel ImageModel { get; set; }
        [LIMSResourceDisplayName("Admin.PageContent.ImageUrl")]

        public string ImageUrl { get; set; }
    }

 
}
