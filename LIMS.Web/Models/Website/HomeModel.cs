using LIMS.Web.Areas.Admin.Models.NewsEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Models.Website
{
    public class HomeModel {
        public List<NewsEventTenderModel> News { get; set; }
        public List<NewsEventTenderModel> Events { get; set; }
        public List<NewsEventTenderModel> Notices { get; set; }
        public List<NewsEventTenderModel> Tenders { get; set; }
        public List<EmployeeModel> BoardMembers { get; set; }
        public PageContentModel PageContent { get; set; }
        public BannerModel Banner { get; set; }
        public ContactUsModel ContactUs { get; set; }
        public List<ImportantLinks> ImportantLinks { get; set; }        
    }
    public class EmployeeModel
    {
        public string Id { get; set; }
        public Guid EmployeeId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Designation { get; set; }
        public string Abbrebiation { get; set; }
        public string Address { get; set; }
        public string PIS { get; set; }
        public string OfficePhone { get; set; }
        public string Phone2 { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string Extension { get; set; }
        public string Description { get; set; }
        public string FacebookUrl { get; set; }
        public string TweeterUrl { get; set; }
        public string WebsiteUrl { get; set; }
        public string UserId { get; set; }
        public int SerialNo { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; }
        public NewsEventFileModel Image { get; set; }
    }
    public class NewsEventTenderModel
    {
        public string Id { get; set; }

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

    }

    public class PageContentModel
    {
        public string Id { get; set; }

        public Guid PageContentId { get; set; }
        public string PageName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool ShowImage { get; set; }
        public string UserId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public NewsEventFileModel Image { get; set; }
    }

    public class BannerModel
    {
        public string Id { get; set; }

        public string BannerId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public bool IsActive { get; set; }
        public NewsEventFileModel Image { get; set; }
    }

    public class ContactUsModel
    {
        public string Id { get; set; }

        public Guid ContactUsId { get; set; }
        public string OfficeName { get; set; }
        public string MinistryName { get; set; }
        public string Department { get; set; }
        public string Address { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Fax { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string Extension { get; set; }
        public string OpeningHours { get; set; }
        public string FacebookUrl { get; set; }
        public string TweeterUrl { get; set; }
        public string WebsiteUrl { get; set; }
        public string UserId { get; set; }
    }
    public class ImportantLinks
    {
        public string Id { get; set; }
        public Guid ImportantLinkId { get; set; }
        public string LinkName { get; set; }
        public int SerialNo { get; set; }
        public string URL { get; set; }
        public string UserId { get; set; }
    }




}