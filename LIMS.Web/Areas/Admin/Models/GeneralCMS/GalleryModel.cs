using LIMS.Core.ModelBinding;
using LIMS.Domain;
using LIMS.Web.Areas.Admin.Models.NewsEvent;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Web.Areas.Admin.Models.GeneralCMS

{
    public class GalleryModel : BaseEntity
    {
        //[LIMSResourceDisplayName("Admin.Gallery.Images")]
        //public List<IFormFile> Images { get; set; }
        
        public NewsEventFileModel ImageModel { get; set; }
        [LIMSResourceDisplayName("Admin.Gallery.Title")]
         public string Title { get; set; }
        [LIMSResourceDisplayName("Admin.Gallery.Type")]

        public string Type { get; set; }
        [LIMSResourceDisplayName("Admin.Gallery.VideoUrl")]

        public string VideoUrl { get; set; }

    }
}
