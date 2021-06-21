﻿using LIMS.Core.ModelBinding;
using LIMS.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LIMS.Framework.Mvc.Models;

namespace LIMS.Web.Areas.Admin.Models.Customers
{
    public partial class CustomerModel : BaseEntityModel
    {
        public CustomerModel()
        {
            AvailableTimeZones = new List<SelectListItem>();
            SendEmail = new SendEmailModel() { SendImmediately = true };
            SendPm = new SendPmModel();
            AvailableCustomerRoles = new List<CustomerRoleModel>();
            AssociatedExternalAuthRecords = new List<AssociatedExternalAuthModel>();
            AvailableCountries = new List<SelectListItem>();
            AvailableStates = new List<SelectListItem>();
            AvailableVendors = new List<SelectListItem>();
            AvailableStores = new List<SelectListItem>();
            CustomerAttributes = new List<CustomerAttributeModel>();
            AvailableNewsletterSubscriptionStores = new List<StoreModel>();
            RewardPointsAvailableStores = new List<SelectListItem>();
        }

        public bool AllowUsersToChangeUsernames { get; set; }
        public bool UsernamesEnabled { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.Customers.Fields.Username")]
        public string Username { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.Customers.Fields.Email")]
        public string Email { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.Customers.Fields.Owner")]
        public string Owner { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.Customers.Fields.Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.Customers.Fields.CustomerTags")]
        public string CustomerTags { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.Customers.Fields.Vendor")]
        public string VendorId { get; set; }
        public IList<SelectListItem> AvailableVendors { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.Customers.Fields.StaffStore")]
        public string StaffStoreId { get; set; }
        public IList<SelectListItem> AvailableStores { get; set; }

        //form fields & properties
        public bool GenderEnabled { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.Customers.Fields.Gender")]
        public string Gender { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.Customers.Fields.FirstName")]
        public string FirstName { get; set; }
        [LIMSResourceDisplayName("Admin.Customers.Customers.Fields.LastName")]

        public string LastName { get; set; }
        [LIMSResourceDisplayName("Admin.Customers.Customers.Fields.FullName")]
        public string FullName { get; set; }

        public bool DateOfBirthEnabled { get; set; }
        [UIHint("DateNullable")]
        [LIMSResourceDisplayName("Admin.Customers.Customers.Fields.DateOfBirth")]
        public DateTime? DateOfBirth { get; set; }

        public bool CompanyEnabled { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.Customers.Fields.Company")]
        public string Company { get; set; }

        public bool StreetAddressEnabled { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.Customers.Fields.StreetAddress")]
        public string StreetAddress { get; set; }

        public bool StreetAddress2Enabled { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.Customers.Fields.StreetAddress2")]
        public string StreetAddress2 { get; set; }

        public bool ZipPostalCodeEnabled { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.Customers.Fields.ZipPostalCode")]
        public string ZipPostalCode { get; set; }

        public bool CityEnabled { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.Customers.Fields.City")]
        public string City { get; set; }

        public bool CountryEnabled { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.Customers.Fields.Country")]
        public string CountryId { get; set; }
        public IList<SelectListItem> AvailableCountries { get; set; }

        public bool StateProvinceEnabled { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.Customers.Fields.StateProvince")]
        public string StateProvinceId { get; set; }
        public IList<SelectListItem> AvailableStates { get; set; }

        public bool PhoneEnabled { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.Customers.Fields.Phone")]
        public string Phone { get; set; }

        public bool FaxEnabled { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.Customers.Fields.Fax")]
        public string Fax { get; set; }

        public List<CustomerAttributeModel> CustomerAttributes { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.Customers.Fields.AdminComment")]
        public string AdminComment { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.Customers.Fields.IsTaxExempt")]
        public bool IsTaxExempt { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.Customers.Fields.FreeShipping")]
        public bool FreeShipping { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.Customers.Fields.Active")]
        public bool Active { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.Customers.Fields.Affiliate")]
        public string AffiliateId { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.Customers.Fields.Affiliate")]
        public string AffiliateName { get; set; }
        public string CustomAttributes { get; set; }

        //time zone
        [LIMSResourceDisplayName("Admin.Customers.Customers.Fields.TimeZoneId")]
        public string TimeZoneId { get; set; }

        public bool AllowCustomersToSetTimeZone { get; set; }

        public IList<SelectListItem> AvailableTimeZones { get; set; }

        //EU VAT
        [LIMSResourceDisplayName("Admin.Customers.Customers.Fields.VatNumber")]

        public string VatNumber { get; set; }

        public string VatNumberStatusNote { get; set; }

        public bool DisplayVatNumber { get; set; }

        //registration date
        [LIMSResourceDisplayName("Admin.Customers.Customers.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }
        [LIMSResourceDisplayName("Admin.Customers.Customers.Fields.LastActivityDate")]
        public DateTime LastActivityDate { get; set; }
        [LIMSResourceDisplayName("Admin.Customers.Customers.Fields.LastPurchaseDate")]
        public DateTime? LastPurchaseDate { get; set; }

        //IP adderss
        [LIMSResourceDisplayName("Admin.Customers.Customers.Fields.IPAddress")]
        public string LastIpAddress { get; set; }

        //Url referrer
        [LIMSResourceDisplayName("Admin.Customers.Customers.Fields.URLReferrer")]
        public string UrlReferrer { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.Customers.Fields.LastVisitedPage")]
        public string LastVisitedPage { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.Customers.Fields.LastUrlReferrer")]
        public string LastUrlReferrer { get; set; }

        //customer roles
        [LIMSResourceDisplayName("Admin.Customers.Customers.Fields.CustomerRoles")]
        public string CustomerRoleNames { get; set; }
        public List<CustomerRoleModel> AvailableCustomerRoles { get; set; }
        public string[] SelectedCustomerRoleIds { get; set; }

        //newsletter subscriptions (per store)
        [LIMSResourceDisplayName("Admin.Customers.Customers.Fields.Newsletter")]
        public List<StoreModel> AvailableNewsletterSubscriptionStores { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.Customers.Fields.Newsletter")]
        public string[] SelectedNewsletterSubscriptionStoreIds { get; set; }

        //reward points history
        public bool DisplayRewardPointsHistory { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.Customers.RewardPoints.Fields.AddRewardPointsValue")]
        public int AddRewardPointsValue { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.Customers.RewardPoints.Fields.AddRewardPointsMessage")]
        public string AddRewardPointsMessage { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.Customers.RewardPoints.Fields.AddRewardPointsStore")]
        public string AddRewardPointsStoreId { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.Customers.RewardPoints.Fields.AddRewardPointsStore")]
        public IList<SelectListItem> RewardPointsAvailableStores { get; set; }

        //send email model
        public SendEmailModel SendEmail { get; set; }
        //send PM model
        public SendPmModel SendPm { get; set; }
        //send the welcome message
        public bool AllowSendingOfWelcomeMessage { get; set; }
        //re-send the activation message
        public bool AllowReSendingOfActivationMessage { get; set; }
        public bool ShowMessageContactForm { get; set; }

        //external auth
        [LIMSResourceDisplayName("Admin.Customers.Customers.AssociatedExternalAuth")]
        public IList<AssociatedExternalAuthModel> AssociatedExternalAuthRecords { get; set; }
        //customer notes
        [LIMSResourceDisplayName("Admin.Customers.CustomerNotes.Fields.DisplayToCustomer")]
        public bool AddCustomerNoteDisplayToCustomer { get; set; }
        [LIMSResourceDisplayName("Admin.Customers.CustomerNotes.Fields.Title")]
        public string AddCustomerTitle { get; set; }
        [LIMSResourceDisplayName("Admin.Customers.CustomerNotes.Fields.Note")]
        public string AddCustomerNoteMessage { get; set; }
        public bool AddCustomerNoteHasDownload { get; set; }
        [LIMSResourceDisplayName("Admin.Customers.CustomerNotes.Fields.Download")]
        [UIHint("Download")]
        public string AddCustomerNoteDownloadId { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.Fields.PhoneNo")]

        public string PhoneNo { get; set; }
        [LIMSResourceDisplayName("Admin.Customers.Fields.MobileNo")]

        public string MobileNo { get; set; }
        [LIMSResourceDisplayName("Admin.Customers.Fields.State")]

        public string State { get; set; }
        [LIMSResourceDisplayName("Admin.Customers.Fields.District")]

        public string District { get; set; }
        [LIMSResourceDisplayName("Admin.Customers.Fields.LocalLevel")]

        public string LocalLevel { get; set; }
        [LIMSResourceDisplayName("Admin.Customers.Fields.WardNo")]

        public string Wardno { get; set; }
        [LIMSResourceDisplayName("Admin.Customers.Fields.Image")]

        public string Image { get; set; }
        [LIMSResourceDisplayName("Admin.Customers.Fields.Document")]

        public string Document { get; set; }
        [LIMSResourceDisplayName("Admin.Customers.Fields.NepaliName")]
        public string NameNepali { get; set; }
        [LIMSResourceDisplayName("Admin.Customers.Fields.EnglishName")]
        public string NameEnglish { get; set; }
        [LIMSResourceDisplayName("Admin.Customers.Fields.IdCardNo")]
        public string IDCardNo { get; set; }
        [LIMSResourceDisplayName("Admin.Customers.Fields.Provience")]
        public string Province { get; set; }
        [LIMSResourceDisplayName("Admin.Customers.Fields.Ward")]

        public string Ward { get; set; }
        [LIMSResourceDisplayName("Admin.Customers.Fields.Tole")]

        public string Tole { get; set; }
        [LIMSResourceDisplayName("Admin.Customers.Fields.Position")]

        public string Position { get; set; }
        [LIMSResourceDisplayName("Admin.Customers.Fields.CreatedBy")]

        public string CreatedBy { get; set; }
        public string EntityId { get; set; }
        public string OrgName { get; set; }
        public string OrgAddress { get; set; }

        #region Nested classes

        public partial class AssociatedExternalAuthModel : BaseEntityModel
        {
            [LIMSResourceDisplayName("Admin.Customers.Customers.AssociatedExternalAuth.Fields.Email")]
            public string Email { get; set; }

            [LIMSResourceDisplayName("Admin.Customers.Customers.AssociatedExternalAuth.Fields.ExternalIdentifier")]
            public string ExternalIdentifier { get; set; }

            [LIMSResourceDisplayName("Admin.Customers.Customers.AssociatedExternalAuth.Fields.AuthMethodName")]
            public string AuthMethodName { get; set; }
        }

        public partial class RewardPointsHistoryModel : BaseEntityModel
        {
            [LIMSResourceDisplayName("Admin.Customers.Customers.RewardPoints.Fields.Store")]
            public string StoreName { get; set; }

            [LIMSResourceDisplayName("Admin.Customers.Customers.RewardPoints.Fields.Points")]
            public int Points { get; set; }

            [LIMSResourceDisplayName("Admin.Customers.Customers.RewardPoints.Fields.PointsBalance")]
            public int PointsBalance { get; set; }

            [LIMSResourceDisplayName("Admin.Customers.Customers.RewardPoints.Fields.Message")]
            public string Message { get; set; }

            [LIMSResourceDisplayName("Admin.Customers.Customers.RewardPoints.Fields.Date")]
            public DateTime CreatedOn { get; set; }
        }

        public partial class SendEmailModel : BaseModel
        {
            [LIMSResourceDisplayName("Admin.Customers.Customers.SendEmail.Subject")]
            public string Subject { get; set; }

            [LIMSResourceDisplayName("Admin.Customers.Customers.SendEmail.Body")]
            public string Body { get; set; }
            [LIMSResourceDisplayName("Admin.Customers.Customers.SendEmail.SendImmediately")]
            public bool SendImmediately { get; set; }

            [LIMSResourceDisplayName("Admin.Customers.Customers.SendEmail.DontSendBeforeDate")]
            [UIHint("DateTimeNullable")]
            public DateTime? DontSendBeforeDate { get; set; }
        }

        public partial class SendPmModel : BaseModel
        {
            [LIMSResourceDisplayName("Admin.Customers.Customers.SendPM.Subject")]
            public string Subject { get; set; }

            [LIMSResourceDisplayName("Admin.Customers.Customers.SendPM.Message")]
            public string Message { get; set; }
        }

        public partial class OrderModel : BaseEntityModel
        {
            [LIMSResourceDisplayName("Admin.Customers.Customers.Orders.ID")]
            public override string Id { get; set; }
            [LIMSResourceDisplayName("Admin.Customers.Customers.Orders.ID")]
            public int OrderNumber { get; set; }
            [LIMSResourceDisplayName("Admin.Customers.Customers.Orders.Code")]
            public string OrderCode { get; set; }
            [LIMSResourceDisplayName("Admin.Customers.Customers.Orders.OrderStatus")]
            public string OrderStatus { get; set; }
            public int OrderStatusId { get; set; }
            [LIMSResourceDisplayName("Admin.Customers.Customers.Orders.PaymentStatus")]
            public string PaymentStatus { get; set; }
            public int PaymentStatusId { get; set; }
            [LIMSResourceDisplayName("Admin.Customers.Customers.Orders.ShippingStatus")]
            public string ShippingStatus { get; set; }
            public int ShippingStatusId { get; set; }
            [LIMSResourceDisplayName("Admin.Customers.Customers.Orders.OrderTotal")]
            public string OrderTotal { get; set; }
            [LIMSResourceDisplayName("Admin.Customers.Customers.Orders.Store")]
            public string StoreName { get; set; }
            [LIMSResourceDisplayName("Admin.Customers.Customers.Orders.CreatedOn")]
            public DateTime CreatedOn { get; set; }
        }

        public partial class ActivityLogModel : BaseEntityModel
        {
            [LIMSResourceDisplayName("Admin.Customers.Customers.ActivityLog.ActivityLogType")]
            public string ActivityLogTypeName { get; set; }
            [LIMSResourceDisplayName("Admin.Customers.Customers.ActivityLog.Comment")]
            public string Comment { get; set; }
            [LIMSResourceDisplayName("Admin.Customers.Customers.ActivityLog.CreatedOn")]
            public DateTime CreatedOn { get; set; }
            [LIMSResourceDisplayName("Admin.Customers.Customers.ActivityLog.IpAddress")]
            public string IpAddress { get; set; }
        }
        public partial class ProductModel : BaseEntityModel
        {
            [LIMSResourceDisplayName("Admin.Customers.Customers.PersonalizedProduct.ProductName")]
            public string ProductName { get; set; }
            [LIMSResourceDisplayName("Admin.Customers.Customers.PersonalizedProduct.DisplayOrder")]
            public int DisplayOrder { get; set; }
            public string ProductId { get; set; }
        }
        public partial class ProductPriceModel : BaseEntityModel
        {
            [LIMSResourceDisplayName("Admin.Customers.Customers.ProductPrice.ProductName")]
            public string ProductName { get; set; }
            [LIMSResourceDisplayName("Admin.Customers.Customers.ProductPrice.Price")]
            public decimal Price { get; set; }
            public string ProductId { get; set; }
        }
        public partial class AddProductModel : BaseModel
        {
            public AddProductModel()
            {
                AvailableCategories = new List<SelectListItem>();
                AvailableManufacturers = new List<SelectListItem>();
                AvailableStores = new List<SelectListItem>();
                AvailableVendors = new List<SelectListItem>();
                AvailableProductTypes = new List<SelectListItem>();
            }

            [LIMSResourceDisplayName("Admin.Catalog.Products.List.SearchProductName")]

            public string SearchProductName { get; set; }
            [LIMSResourceDisplayName("Admin.Catalog.Products.List.SearchCategory")]
            public string SearchCategoryId { get; set; }
            [LIMSResourceDisplayName("Admin.Catalog.Products.List.SearchManufacturer")]
            public string SearchManufacturerId { get; set; }
            [LIMSResourceDisplayName("Admin.Catalog.Products.List.SearchStore")]
            public string SearchStoreId { get; set; }
            [LIMSResourceDisplayName("Admin.Catalog.Products.List.SearchVendor")]
            public string SearchVendorId { get; set; }
            [LIMSResourceDisplayName("Admin.Catalog.Products.List.SearchProductType")]
            public int SearchProductTypeId { get; set; }

            public IList<SelectListItem> AvailableCategories { get; set; }
            public IList<SelectListItem> AvailableManufacturers { get; set; }
            public IList<SelectListItem> AvailableStores { get; set; }
            public IList<SelectListItem> AvailableVendors { get; set; }
            public IList<SelectListItem> AvailableProductTypes { get; set; }

            public string CustomerId { get; set; }

            public string[] SelectedProductIds { get; set; }
        }
        public partial class BackInStockSubscriptionModel : BaseEntityModel
        {
            [LIMSResourceDisplayName("Admin.Customers.Customers.BackInStockSubscriptions.Store")]
            public string StoreName { get; set; }
            [LIMSResourceDisplayName("Admin.Customers.Customers.BackInStockSubscriptions.Product")]
            public string ProductId { get; set; }
            [LIMSResourceDisplayName("Admin.Customers.Customers.BackInStockSubscriptions.Product")]
            public string ProductName { get; set; }
            public string AttributeDescription { get; set; }
            [LIMSResourceDisplayName("Admin.Customers.Customers.BackInStockSubscriptions.CreatedOn")]
            public DateTime CreatedOn { get; set; }
        }

        public partial class CustomerAttributeModel : BaseEntityModel
        {
            public CustomerAttributeModel()
            {
                Values = new List<CustomerAttributeValueModel>();
            }

            public string Name { get; set; }

            public bool IsRequired { get; set; }

            /// <summary>
            /// Default value for textboxes
            /// </summary>
            public string DefaultValue { get; set; }

            public IList<CustomerAttributeValueModel> Values { get; set; }

        }

        public partial class CustomerAttributeValueModel : BaseEntityModel
        {
            public string Name { get; set; }

            public bool IsPreSelected { get; set; }
        }

        public partial class CustomerNote : BaseEntityModel
        {
            public string CustomerId { get; set; }
            [LIMSResourceDisplayName("Admin.Customers.CustomerNotes.Fields.DisplayToCustomer")]
            public bool DisplayToCustomer { get; set; }
            [LIMSResourceDisplayName("Admin.Customers.CustomerNotes.Fields.Title")]
            public string Title { get; set; }
            [LIMSResourceDisplayName("Admin.Customers.CustomerNotes.Fields.Note")]
            public string Note { get; set; }
            [LIMSResourceDisplayName("Admin.Customers.CustomerNotes.Fields.Download")]
            public string DownloadId { get; set; }
            public Guid DownloadGuid { get; set; }
            [LIMSResourceDisplayName("Admin.Customers.CustomerNotes.Fields.CreatedOn")]
            public DateTime CreatedOn { get; set; }
        }

        #endregion
    }
}