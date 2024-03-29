﻿using LIMS.Core.ModelBinding;
using LIMS.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace LIMS.Web.Areas.Admin.Models.Customers
{
    public partial class CustomerReminderModel : BaseEntityModel
    {

        [LIMSResourceDisplayName("Admin.Customers.CustomerReminder.Fields.Name")]
        public string Name { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.CustomerReminder.Fields.StartDate")]
        public DateTime StartDateTime { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.CustomerReminder.Fields.EndDate")]
        public DateTime EndDateTime { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.CustomerReminder.Fields.LastUpdateDate")]
        public DateTime LastUpdateDate { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.CustomerReminder.Fields.AllowRenew")]
        public bool AllowRenew { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.CustomerReminder.Fields.RenewedDay")]
        public int RenewedDay { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.CustomerReminder.Fields.Active")]
        public bool Active { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.CustomerReminder.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.CustomerReminder.Fields.ReminderRule")]
        public int ReminderRuleId { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.CustomerReminder.Fields.ConditionId")]
        public int ConditionId { get; set; }
        public int ConditionCount { get; set; }


        public partial class ConditionModel : BaseEntityModel
        {
            public ConditionModel()
            {
                this.ConditionType = new List<SelectListItem>();
            }

            [LIMSResourceDisplayName("Admin.Customers.CustomerReminder.Condition.Fields.Name")]
            public string Name { get; set; }

            [LIMSResourceDisplayName("Admin.Customers.CustomerReminder.Condition.Fields.ConditionTypeId")]
            public int ConditionTypeId { get; set; }
            public IList<SelectListItem> ConditionType { get; set; }

            [LIMSResourceDisplayName("Admin.Customers.CustomerReminder.Condition.Fields.ConditionId")]
            public int ConditionId { get; set; }

            public string CustomerReminderId { get; set; }

            public partial class AddProductToConditionModel
            {
                public AddProductToConditionModel()
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

                public string CustomerReminderId { get; set; }
                public string ConditionId { get; set; }

                public string[] SelectedProductIds { get; set; }
            }
            public partial class AddCategoryConditionModel
            {
                [LIMSResourceDisplayName("Admin.Catalog.Categories.List.SearchCategoryName")]

                public string SearchCategoryName { get; set; }

                public string CustomerReminderId { get; set; }
                public string ConditionId { get; set; }

                public string[] SelectedCategoryIds { get; set; }
            }
            public partial class AddManufacturerConditionModel
            {
                [LIMSResourceDisplayName("Admin.Catalog.Manufacturers.List.SearchManufacturerName")]

                public string SearchManufacturerName { get; set; }

                public string CustomerReminderId { get; set; }
                public string ConditionId { get; set; }

                public string[] SelectedManufacturerIds { get; set; }
            }
            public partial class AddCustomerRoleConditionModel
            {
                public string CustomerReminderId { get; set; }
                public string ConditionId { get; set; }

                public string CustomerRoleId { get; set; }
                public string Id { get; set; }
            }
            public partial class AddCustomerTagConditionModel
            {
                public string CustomerReminderId { get; set; }
                public string ConditionId { get; set; }

                public string CustomerTagId { get; set; }
                public string Id { get; set; }
            }
            public partial class AddCustomerRegisterConditionModel
            {
                public string CustomerReminderId { get; set; }
                public string ConditionId { get; set; }
                public string CustomerRegisterName { get; set; }
                public string CustomerRegisterValue { get; set; }
                public string Id { get; set; }
            }
            public partial class AddCustomCustomerAttributeConditionModel
            {
                public string Id { get; set; }
                public string CustomerReminderId { get; set; }
                public string ConditionId { get; set; }
                public string CustomerAttributeName { get; set; }
                public string CustomerAttributeValue { get; set; }
            }

        }

        public partial class ReminderLevelModel : BaseEntityModel
        {
            public ReminderLevelModel()
            {
                EmailAccounts = new List<SelectListItem>();
            }

            public string CustomerReminderId { get; set; }

            [LIMSResourceDisplayName("Admin.Customers.CustomerReminder.Level.Fields.SendDay")]
            public int Day { get; set; }
            [LIMSResourceDisplayName("Admin.Customers.CustomerReminder.Level.Fields.SendHour")]
            public int Hour { get; set; }

            [LIMSResourceDisplayName("Admin.Customers.CustomerReminder.Level.Fields.SendMinutes")]
            public int Minutes { get; set; }

            [LIMSResourceDisplayName("Admin.Customers.CustomerReminder.Level.Fields.Name")]
            public string Name { get; set; }

            [LIMSResourceDisplayName("Admin.Customers.CustomerReminder.Level.Fields.AllowedTokens")]
            public string[] AllowedTokens { get; set; }

            [LIMSResourceDisplayName("Admin.Customers.CustomerReminder.Level.Fields.Level")]
            public int Level { get; set; }

            [LIMSResourceDisplayName("Admin.Customers.CustomerReminder.Level.Fields.EmailAccountId")]
            public string EmailAccountId { get; set; }
            public IList<SelectListItem> EmailAccounts { get; set; }

            [LIMSResourceDisplayName("Admin.Customers.CustomerReminder.Level.Fields.BccEmailAddresses")]
            public string BccEmailAddresses { get; set; }

            [LIMSResourceDisplayName("Admin.Customers.CustomerReminder.Level.Fields.Subject")]
            public string Subject { get; set; }

            [LIMSResourceDisplayName("Admin.Customers.CustomerReminder.Level.Fields.Body")]

            public string Body { get; set; }
        }

    }



}