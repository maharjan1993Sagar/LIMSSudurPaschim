using LIMS.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Api.DTOs.GeneralCMS
{
    public class NewsEventTenderDto: BaseApiEntityModel
    {
        public Guid NewsEventTenderId { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public bool HasTitle { get; set; }
        public string Description { get; set; }
        public string UploadedBy { get; set; }
        public DateTime ActiveDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsScroll { get; set; }
        public bool IsActive { get; set; }
        public bool IsModalPopup { get; set; }
        public bool ShowText { get; set; }
        public DateTime UploadedDate { get; set; }
        public string FilePath { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public NewsEventFileDto Image { get; set; }
    }
}
