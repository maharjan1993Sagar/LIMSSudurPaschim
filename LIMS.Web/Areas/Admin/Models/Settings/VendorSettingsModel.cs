﻿using LIMS.Core.ModelBinding;
using LIMS.Core.Models;

namespace LIMS.Web.Areas.Admin.Models.Settings
{
    public partial class VendorSettingsModel : BaseModel
    {

        public VendorSettingsModel()
        {
            AddressSettings = new AddressSettingsModel();
        }

        public string ActiveStoreScopeConfiguration { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Settings.Vendor.VendorsBlockItemsToDisplay")]
        public int VendorsBlockItemsToDisplay { get; set; }
        public bool VendorsBlockItemsToDisplay_OverrideForStore { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Settings.Vendor.ShowVendorOnProductDetailsPage")]
        public bool ShowVendorOnProductDetailsPage { get; set; }
        public bool ShowVendorOnProductDetailsPage_OverrideForStore { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Settings.Vendor.AllowCustomersToContactVendors")]
        public bool AllowCustomersToContactVendors { get; set; }
        public bool AllowCustomersToContactVendors_OverrideForStore { get; set; }
        [LIMSResourceDisplayName("Admin.Configuration.Settings.Vendor.AllowCustomersToApplyForVendorAccount")]
        public bool AllowCustomersToApplyForVendorAccount { get; set; }
        public bool AllowCustomersToApplyForVendorAccount_OverrideForStore { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Settings.Vendor.AllowSearchByVendor")]
        public bool AllowSearchByVendor { get; set; }
        public bool AllowSearchByVendor_OverrideForStore { get; set; }


        [LIMSResourceDisplayName("Admin.Configuration.Settings.Vendor.AllowVendorsToEditInfo")]
        public bool AllowVendorsToEditInfo { get; set; }
        public bool AllowVendorsToEditInfo_OverrideForStore { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Settings.Vendor.NotifyStoreOwnerAboutVendorInformationChange")]
        public bool NotifyStoreOwnerAboutVendorInformationChange { get; set; }
        public bool NotifyStoreOwnerAboutVendorInformationChange_OverrideForStore { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Settings.Vendor.TermsOfServiceEnabled")]
        public bool TermsOfServiceEnabled { get; set; }
        public bool TermsOfServiceEnabled_OverrideForStore { get; set; }

        //review vendor
        [LIMSResourceDisplayName("Admin.Configuration.Settings.Vendor.VendorReviewsMustBeApproved")]
        public bool VendorReviewsMustBeApproved { get; set; }
        public bool VendorReviewsMustBeApproved_OverrideForStore { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Settings.Vendor.AllowAnonymousUsersToReviewVendor")]
        public bool AllowAnonymousUsersToReviewVendor { get; set; }
        public bool AllowAnonymousUsersToReviewVendor_OverrideForStore { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Settings.Vendor.VendorReviewPossibleOnlyAfterPurchasing")]
        public bool VendorReviewPossibleOnlyAfterPurchasing { get; set; }
        public bool VendorReviewPossibleOnlyAfterPurchasing_OverrideForStore { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Settings.Vendor.NotifyVendorAboutNewVendorReviews")]
        public bool NotifyVendorAboutNewVendorReviews { get; set; }
        public bool NotifyVendorAboutNewVendorReviews_OverrideForStore { get; set; }

        public AddressSettingsModel AddressSettings { get; set; }

        #region Nested classes

        public partial class AddressSettingsModel : BaseModel
        {
            [LIMSResourceDisplayName("Admin.Configuration.Settings.Vendor.AddressFormFields.CompanyEnabled")]
            public bool CompanyEnabled { get; set; }
            [LIMSResourceDisplayName("Admin.Configuration.Settings.Vendor.AddressFormFields.CompanyRequired")]
            public bool CompanyRequired { get; set; }

            [LIMSResourceDisplayName("Admin.Configuration.Settings.Vendor.AddressFormFields.StreetAddressEnabled")]
            public bool StreetAddressEnabled { get; set; }
            [LIMSResourceDisplayName("Admin.Configuration.Settings.Vendor.AddressFormFields.StreetAddressRequired")]
            public bool StreetAddressRequired { get; set; }

            [LIMSResourceDisplayName("Admin.Configuration.Settings.Vendor.AddressFormFields.StreetAddress2Enabled")]
            public bool StreetAddress2Enabled { get; set; }
            [LIMSResourceDisplayName("Admin.Configuration.Settings.Vendor.AddressFormFields.StreetAddress2Required")]
            public bool StreetAddress2Required { get; set; }

            [LIMSResourceDisplayName("Admin.Configuration.Settings.Vendor.AddressFormFields.ZipPostalCodeEnabled")]
            public bool ZipPostalCodeEnabled { get; set; }
            [LIMSResourceDisplayName("Admin.Configuration.Settings.Vendor.AddressFormFields.ZipPostalCodeRequired")]
            public bool ZipPostalCodeRequired { get; set; }

            [LIMSResourceDisplayName("Admin.Configuration.Settings.Vendor.AddressFormFields.CityEnabled")]
            public bool CityEnabled { get; set; }
            [LIMSResourceDisplayName("Admin.Configuration.Settings.Vendor.AddressFormFields.CityRequired")]
            public bool CityRequired { get; set; }

            [LIMSResourceDisplayName("Admin.Configuration.Settings.Vendor.AddressFormFields.CountryEnabled")]
            public bool CountryEnabled { get; set; }

            [LIMSResourceDisplayName("Admin.Configuration.Settings.Vendor.AddressFormFields.StateProvinceEnabled")]
            public bool StateProvinceEnabled { get; set; }

            [LIMSResourceDisplayName("Admin.Configuration.Settings.Vendor.AddressFormFields.PhoneEnabled")]
            public bool PhoneEnabled { get; set; }
            [LIMSResourceDisplayName("Admin.Configuration.Settings.Vendor.AddressFormFields.PhoneRequired")]
            public bool PhoneRequired { get; set; }

            [LIMSResourceDisplayName("Admin.Configuration.Settings.Vendor.AddressFormFields.FaxEnabled")]
            public bool FaxEnabled { get; set; }
            [LIMSResourceDisplayName("Admin.Configuration.Settings.Vendor.AddressFormFields.FaxRequired")]
            public bool FaxRequired { get; set; }
        }

        #endregion

    }



}