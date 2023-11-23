using LIMS.Core.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Website1.Models
{
    public class HomeModel {
        public GalleryModel Gallery { get; set; }
        public GalleryModel Facebook { get; set; }

        public GalleryModel Video { get; set; }
        public List<GalleryModel> Galleries { get; set; }
        public List<GalleryModel> Videos{ get; set; }
        public List<NewsEventTenderModel> News { get; set; }
        public List<NewsEventTenderModel> NewsAndEvent { get; set; }

        public List<NewsEventTenderModel> NewsScroll { get; set; }
        public List<NewsEventTenderModel> Events { get; set; }
        public List<NewsEventTenderModel> Notices { get; set; }
        public List<NewsEventTenderModel> Tenders { get; set; }
        public List<NewsEventTenderModel> PressReleaseAndLetter { get; set; }

        public List<NewsEventTenderModel> PressRelease { get; set; }
        public List<NewsEventTenderModel> Letters { get; set; }
        public List<NewsEventTenderModel> RulesRegulation { get; set; }
        public List<NewsEventTenderModel> Directives { get; set; }
        public List<NewsEventTenderModel> Publication { get; set; }

        public List<NewsEventTenderModel> ActsPolices { get; set; }
        public List<NewsEventTenderModel> Reports { get; set; }
        public List<NewsEventTenderModel> OtherFiles { get; set; }
        public List<EmployeeModel> Employee { get; set; }
        public EmployeeModel Director { get; set; }
        public EmployeeModel InformationOfficer { get; set; }
        public EmployeeModel Minister { get; set; }
        public EmployeeModel StateMinister { get; set; }
        public EmployeeModel Secratory { get; set; }
        public EmployeeModel SpokePerson { get; set; }

        public PageContentModel PageContent { get; set; }
        public List<BannerModel> Banner { get; set; }
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
        public bool IsInformationOfficer { get; set; }

        public bool IsOfficeChief { get; set; }
    }
    public class NewsEventTenderModel
    {
        public string Id { get; set; }
        public Guid NewsEventTenderId { get; set; }
        public string Title { get; set; }
        public string TypeName { get; set; }
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
        public NewsEventFileModel Image { get; set; }
        public string SubMenu { get; set; }
        public string SubSubMenu { get; set; }

    }

    public class PageContentModel
    {
        public string Id { get; set; }
        public string Type { get; set; }

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
    public class NewsEventFileModel 
    {
        public string Id { get; set; }
        public string CMSEntityId { get; set; }

       [LIMSResourceDisplayName("LIMS.File.Fields.Picture")]
        public string PictureId { get; set; }

        [LIMSResourceDisplayName("LIMS.File.Fields.Picture")]
        public string PictureUrl { get; set; }

        [LIMSResourceDisplayName("LIMS.File.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [LIMSResourceDisplayName("LIMS.File.Fields.OverrideAltAttribute")]

        public string OverrideAltAttribute { get; set; }

        [LIMSResourceDisplayName("LIMS.File.Fields.OverrideTitleAttribute")]

        public string OverrideTitleAttribute { get; set; }
        public string Pic { get; set; }
        public string Type { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public decimal FileSize { get; set; }
      
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