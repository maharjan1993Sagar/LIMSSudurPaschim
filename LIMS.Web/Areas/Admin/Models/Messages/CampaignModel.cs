using LIMS.Core.ModelBinding;
using LIMS.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LIMS.Web.Areas.Admin.Models.Messages
{
    public partial class CampaignModel : BaseEntityModel
    {
        public CampaignModel()
        {
            AvailableStores = new List<SelectListItem>();
            AvailableLanguages = new List<SelectListItem>();
            AvailableCustomerTags = new List<SelectListItem>();
            CustomerTags = new List<string>();
            NewsletterCategories = new List<string>();
            AvailableCustomerRoles = new List<SelectListItem>();
            CustomerRoles = new List<string>();
            AvailableEmailAccounts = new List<EmailAccountModel>();
        }

        [LIMSResourceDisplayName("Admin.Promotions.Campaigns.Fields.Name")]

        public string Name { get; set; }

        [LIMSResourceDisplayName("Admin.Promotions.Campaigns.Fields.Subject")]

        public string Subject { get; set; }

        [LIMSResourceDisplayName("Admin.Promotions.Campaigns.Fields.Body")]

        public string Body { get; set; }

        [LIMSResourceDisplayName("Admin.Promotions.Campaigns.Fields.Store")]
        public string StoreId { get; set; }
        public IList<SelectListItem> AvailableStores { get; set; }

        [LIMSResourceDisplayName("Admin.Promotions.Campaigns.Fields.Language")]
        public string LanguageId { get; set; }
        public IList<SelectListItem> AvailableLanguages { get; set; }


        [LIMSResourceDisplayName("Admin.Promotions.Campaigns.Fields.CustomerCreatedDateFrom")]
        [UIHint("DateTimeNullable")]
        public DateTime? CustomerCreatedDateFrom { get; set; }

        [LIMSResourceDisplayName("Admin.Promotions.Campaigns.Fields.CustomerCreatedDateTo")]
        [UIHint("DateTimeNullable")]
        public DateTime? CustomerCreatedDateTo { get; set; }

        [LIMSResourceDisplayName("Admin.Promotions.Campaigns.Fields.CustomerLastActivityDateFrom")]
        [UIHint("DateTimeNullable")]
        public DateTime? CustomerLastActivityDateFrom { get; set; }
        [LIMSResourceDisplayName("Admin.Promotions.Campaigns.Fields.CustomerLastActivityDateTo")]
        [UIHint("DateTimeNullable")]
        public DateTime? CustomerLastActivityDateTo { get; set; }

        [LIMSResourceDisplayName("Admin.Promotions.Campaigns.Fields.CustomerLastPurchaseDateFrom")]
        [UIHint("DateTimeNullable")]
        public DateTime? CustomerLastPurchaseDateFrom { get; set; }
        [LIMSResourceDisplayName("Admin.Promotions.Campaigns.Fields.CustomerLastPurchaseDateTo")]
        [UIHint("DateTimeNullable")]
        public DateTime? CustomerLastPurchaseDateTo { get; set; }

        [LIMSResourceDisplayName("Admin.Promotions.Campaigns.Fields.CustomerHasOrders")]
        public int CustomerHasOrders { get; set; }

        [LIMSResourceDisplayName("Admin.Promotions.Campaigns.Fields.CustomerHasShoppingCart")]
        public int CustomerHasShoppingCart { get; set; }

        [LIMSResourceDisplayName("Admin.Promotions.Campaigns.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        [LIMSResourceDisplayName("Admin.Promotions.Campaigns.Fields.CustomerTags")]
        public IList<SelectListItem> AvailableCustomerTags { get; set; }

        [LIMSResourceDisplayName("Admin.Promotions.Campaigns.Fields.CustomerTags")]
        [UIHint("MultiSelect")]
        public IList<string> CustomerTags { get; set; }

        [LIMSResourceDisplayName("Admin.Promotions.Campaigns.Fields.NewsletterCategory")]
        [UIHint("MultiSelect")]
        public IList<string> NewsletterCategories { get; set; }
        [LIMSResourceDisplayName("Admin.Promotions.Campaigns.Fields.NewsletterCategory")]
        public IList<SelectListItem> AvailableNewsletterCategories { get; set; }

        [LIMSResourceDisplayName("Admin.Promotions.Campaigns.Fields.CustomerRoles")]
        public IList<SelectListItem> AvailableCustomerRoles { get; set; }

        [LIMSResourceDisplayName("Admin.Promotions.Campaigns.Fields.CustomerRoles")]
        [UIHint("MultiSelect")]
        public IList<string> CustomerRoles { get; set; }


        [LIMSResourceDisplayName("Admin.Promotions.Campaigns.Fields.AllowedTokens")]
        public string[] AllowedTokens { get; set; }

        [LIMSResourceDisplayName("Admin.Promotions.Campaigns.Fields.EmailAccount")]
        public string EmailAccountId { get; set; }
        public IList<EmailAccountModel> AvailableEmailAccounts { get; set; }

        [LIMSResourceDisplayName("Admin.Promotions.Campaigns.Fields.TestEmail")]

        public string TestEmail { get; set; }
    }
}