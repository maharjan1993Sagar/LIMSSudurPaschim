using LIMS.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Api.DTOs.GeneralCMS
{
    public class GalleryDto: BaseApiEntityModel
    {
        public Guid GalleryId { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string VideoUrl { get; set; }
        public string UserId { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public List<NewsEventFileDto> Images { get; set; }
    }
}
