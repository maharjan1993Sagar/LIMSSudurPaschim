using LIMS.Core.ModelBinding;
using LIMS.Domain;
using LIMS.Web.Areas.Admin.Models.NewsEvent;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Web.Areas.Admin.Models.GeneralCMS

{
    public class BannerModel : BaseEntity
    {
        public NewsEventFileModel ImageModel { get; set; }

        [LIMSResourceDisplayName("Admin.Banner.Title")]
        public string Title { get; set; }
        [LIMSResourceDisplayName("Admin.Banner.Description")]

        public string Description { get; set; }
        [LIMSResourceDisplayName("Admin.Banner.IsActive")]

        public bool IsActive { get; set; }

    }
}
