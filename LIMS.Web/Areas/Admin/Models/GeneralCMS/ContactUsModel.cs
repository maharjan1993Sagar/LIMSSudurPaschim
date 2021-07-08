using LIMS.Core.ModelBinding;
using LIMS.Domain;
using LIMS.Domain.NewsEvent;
using LIMS.Web.Areas.Admin.Models.NewsEvent;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Web.Areas.Admin.Models.GeneralCMS

{
   public class ContactUsModel:BaseEntity
    {
        [LIMSResourceDisplayName("Admin.ContactUs.OfficeName")]
        public string OfficeName { get; set; }
        [LIMSResourceDisplayName("Admin.ContactUs.MinistryName")]

        public string MinistryName { get; set; }
        [LIMSResourceDisplayName("Admin.ContactUs.Department")]

        public string Department { get; set; }
        [LIMSResourceDisplayName("Admin.ContactUs.Address")]

        public string Address { get; set; }
        [LIMSResourceDisplayName("Admin.ContactUs.Phone1")]

        public string  Phone1 { get; set; }
        [LIMSResourceDisplayName("Admin.ContactUs.Phone2")]

        public string  Phone2 { get; set; }
        [LIMSResourceDisplayName("Admin.ContactUs.Fax")]

        public string  Fax { get; set; }
        [LIMSResourceDisplayName("Admin.ContactUs.Email1")]

        public string  Email1 { get; set; }
        [LIMSResourceDisplayName("Admin.ContactUs.Email2")]

        public string  Email2 { get; set; }
        [LIMSResourceDisplayName("Admin.ContactUs.Extension")]

        public string  Extension { get; set; }
        [LIMSResourceDisplayName("Admin.ContactUs.OpeningHours")]

        public string OpeningHours{ get; set; }
        [LIMSResourceDisplayName("Admin.ContactUs.FacebookUrl")]

        public string FacebookUrl { get; set; }
        [LIMSResourceDisplayName("Admin.ContactUs.TweeterUrl")]

        public string TweeterUrl { get; set; }
        [LIMSResourceDisplayName("Admin.ContactUs.WebsiteUrl")]

        public string WebsiteUrl { get; set; }
      
    }
}
