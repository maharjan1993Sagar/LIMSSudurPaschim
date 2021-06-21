using LIMS.Core.ModelBinding;
using LIMS.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.NewsEvent
{
    public class NewsEventTenderModel:BaseEntity
    {
        [LIMSResourceDisplayName("Admin.NewsEvent.Title")]
        public string Title { get; set; }
        [LIMSResourceDisplayName("Admin.NewsEvent.Type")]

        public string Type { get; set; }
        [LIMSResourceDisplayName("Admin.NewsEvent.HasTitle")]

        public bool HasTitle { get; set; }
        [LIMSResourceDisplayName("Admin.NewsEvent.Description")]

        public string Description { get; set; }
        [LIMSResourceDisplayName("Admin.NewsEvent.ActiveDate")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]

        public DateTime ActiveDate { get; set; }
        [LIMSResourceDisplayName("Admin.NewsEvent.ExpiryDate")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]


        public DateTime ExpiryDate { get; set; }
        [LIMSResourceDisplayName("Admin.NewsEvent.IsScroll")]

        public bool IsScroll { get; set; }
        [LIMSResourceDisplayName("Admin.NewsEvent.IsActive")]

        public bool IsActive { get; set; }
        [LIMSResourceDisplayName("Admin.NewsEvent.IsModalPopup")]

        public bool IsModalPopup { get; set; }
        [LIMSResourceDisplayName("Admin.NewsEvent.ShowText")]

        public bool ShowText { get; set; }
        public NewsEventFileModel FileModel { get; set; }

    }

    public class NewsEventFileModel
    {
        public string Type { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public decimal FileSize { get; set; }
        public IFormFile File { get; set; }

    }
}
