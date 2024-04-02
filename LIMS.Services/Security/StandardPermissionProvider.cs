using LIMS.Domain.Customers;
using LIMS.Domain.Security;
using System.Collections.Generic;

namespace LIMS.Services.Security
{
    /// <summary>
    /// Standard permission provider
    /// </summary>
    public partial class StandardPermissionProvider : IPermissionProvider
    {
        //admin area permissions
        public static readonly PermissionRecord AccessAdminPanel = new PermissionRecord { Name = "Access admin area", SystemName = PermissionSystemName.AccessAdminPanel, Category = "Standard" };
        public static readonly PermissionRecord AllowCustomerImpersonation = new PermissionRecord { Name = "Admin area. Allow Customer Impersonation", SystemName = PermissionSystemName.AllowCustomerImpersonation, Category = "Customers" };
        public static readonly PermissionRecord ManageProducts = new PermissionRecord { Name = "Admin area. Manage Products", SystemName = PermissionSystemName.Products, Category = "Catalog", Actions = new List<string> { PermissionActionName.List, PermissionActionName.Create, PermissionActionName.Edit, PermissionActionName.Preview, PermissionActionName.Delete, PermissionActionName.Export, PermissionActionName.Import } };
        public static readonly PermissionRecord ManageCategories = new PermissionRecord { Name = "Admin area. Manage Categories", SystemName = PermissionSystemName.Categories, Category = "Catalog", Actions = new List<string> { PermissionActionName.List, PermissionActionName.Create, PermissionActionName.Edit, PermissionActionName.Preview, PermissionActionName.Delete, PermissionActionName.Export, PermissionActionName.Import } };
        public static readonly PermissionRecord ManageManufacturers = new PermissionRecord { Name = "Admin area. Manage Manufacturers", SystemName = PermissionSystemName.Manufacturers, Category = "Catalog", Actions = new List<string> { PermissionActionName.List, PermissionActionName.Create, PermissionActionName.Edit, PermissionActionName.Preview, PermissionActionName.Delete, PermissionActionName.Export, PermissionActionName.Import } };
        public static readonly PermissionRecord ManageProductReviews = new PermissionRecord { Name = "Admin area. Manage Product Reviews", SystemName = PermissionSystemName.ProductReviews, Category = "Catalog", Actions = new List<string> { PermissionActionName.List, PermissionActionName.Edit, PermissionActionName.Preview, PermissionActionName.Delete } };
        public static readonly PermissionRecord ManageProductTags = new PermissionRecord { Name = "Admin area. Manage Product Tags", SystemName = PermissionSystemName.ProductTags, Category = "Catalog", Actions = new List<string> { PermissionActionName.List, PermissionActionName.Edit, PermissionActionName.Preview, PermissionActionName.Delete } };

        public static readonly PermissionRecord ManageProductAttributes = new PermissionRecord { Name = "Admin area. Manage Product Attributes", SystemName = PermissionSystemName.ProductAttributes, Category = "Catalog", Actions = new List<string> { PermissionActionName.List, PermissionActionName.Create, PermissionActionName.Edit, PermissionActionName.Preview, PermissionActionName.Delete } };
        public static readonly PermissionRecord ManageSpecificationAttributes = new PermissionRecord { Name = "Admin area. Manage Specification Attributes", SystemName = PermissionSystemName.SpecificationAttributes, Category = "Catalog", Actions = new List<string> { PermissionActionName.List, PermissionActionName.Create, PermissionActionName.Edit, PermissionActionName.Preview, PermissionActionName.Delete } };
        public static readonly PermissionRecord ManageCheckoutAttribute = new PermissionRecord { Name = "Admin area. Manage Checkout Attributes", SystemName = PermissionSystemName.CheckoutAttributes, Category = "Catalog", Actions = new List<string> { PermissionActionName.List, PermissionActionName.Create, PermissionActionName.Edit, PermissionActionName.Preview, PermissionActionName.Delete } };
        public static readonly PermissionRecord ManageContactAttribute = new PermissionRecord { Name = "Admin area. Manage Contact Attribute", SystemName = PermissionSystemName.ContactAttributes, Category = "Catalog", Actions = new List<string> { PermissionActionName.List, PermissionActionName.Create, PermissionActionName.Edit, PermissionActionName.Preview, PermissionActionName.Delete } };

        public static readonly PermissionRecord ManageCustomers = new PermissionRecord { Name = "Admin area. Manage Customers", SystemName = PermissionSystemName.Customers, Category = "Customers", Actions = new List<string> { PermissionActionName.List, PermissionActionName.Create, PermissionActionName.Edit, PermissionActionName.Preview, PermissionActionName.Delete, PermissionActionName.Export } };
        public static readonly PermissionRecord ManageCustomerRoles = new PermissionRecord { Name = "Admin area. Manage Customer Roles", SystemName = PermissionSystemName.CustomerRoles, Category = "Customers", Actions = new List<string> { PermissionActionName.List, PermissionActionName.Create, PermissionActionName.Edit, PermissionActionName.Preview, PermissionActionName.Delete } };
        public static readonly PermissionRecord ManageCustomerTags = new PermissionRecord { Name = "Admin area. Manage Customer Tags", SystemName = PermissionSystemName.CustomerTags, Category = "Customers", Actions = new List<string> { PermissionActionName.List, PermissionActionName.Create, PermissionActionName.Edit, PermissionActionName.Preview, PermissionActionName.Delete } };
        public static readonly PermissionRecord ManageActions = new PermissionRecord { Name = "Admin area. Manage Customers Actions", SystemName = PermissionSystemName.Actions, Category = "Customers" };
        public static readonly PermissionRecord ManageReminders = new PermissionRecord { Name = "Admin area. Manage Customers Reminders", SystemName = PermissionSystemName.Reminders, Category = "Customers" };
        public static readonly PermissionRecord ManageVendors = new PermissionRecord { Name = "Admin area. Manage Vendors", SystemName = PermissionSystemName.Vendors, Category = "Customers", Actions = new List<string> { PermissionActionName.List, PermissionActionName.Create, PermissionActionName.Edit, PermissionActionName.Preview, PermissionActionName.Delete } };
        public static readonly PermissionRecord ManageVendorReviews = new PermissionRecord { Name = "Admin area. Manage Vendor Reviews", SystemName = PermissionSystemName.VendorReviews, Category = "Customers", Actions = new List<string> { PermissionActionName.List, PermissionActionName.Create, PermissionActionName.Edit, PermissionActionName.Preview, PermissionActionName.Delete } };
        public static readonly PermissionRecord ManageCurrentCarts = new PermissionRecord { Name = "Admin area. Manage Current Carts", SystemName = PermissionSystemName.CurrentCarts, Category = "Orders" };
        public static readonly PermissionRecord ManageOrders = new PermissionRecord { Name = "Admin area. Manage Orders", SystemName = PermissionSystemName.Orders, Category = "Orders", Actions = new List<string> { PermissionActionName.List, PermissionActionName.Edit, PermissionActionName.Payments, PermissionActionName.Cancel, PermissionActionName.Preview, PermissionActionName.Delete, PermissionActionName.Export } };
        public static readonly PermissionRecord ManageOrderTags = new PermissionRecord { Name = "Admin area. Manage Order Tags", SystemName = PermissionSystemName.OrderTags, Category = "Orders", Actions = new List<string> { PermissionActionName.List, PermissionActionName.Edit, PermissionActionName.Preview, PermissionActionName.Delete } };
        public static readonly PermissionRecord ManageShipments = new PermissionRecord { Name = "Admin area. Manage Shipments", SystemName = PermissionSystemName.Shipments, Category = "Orders", Actions = new List<string> { PermissionActionName.List, PermissionActionName.Create, PermissionActionName.Edit, PermissionActionName.Preview, PermissionActionName.Delete, PermissionActionName.Export } };
        public static readonly PermissionRecord ManageRecurringPayments = new PermissionRecord { Name = "Admin area. Manage Recurring Payments", SystemName = PermissionSystemName.RecurringPayments, Category = "Orders", Actions = new List<string> { PermissionActionName.List, PermissionActionName.Create, PermissionActionName.Edit, PermissionActionName.Preview, PermissionActionName.Delete } };
        public static readonly PermissionRecord ManageGiftCards = new PermissionRecord { Name = "Admin area. Manage Gift Cards", SystemName = PermissionSystemName.GiftCards, Category = "Orders", Actions = new List<string> { PermissionActionName.List, PermissionActionName.Create, PermissionActionName.Edit, PermissionActionName.Preview, PermissionActionName.Delete } };
        public static readonly PermissionRecord ManageReturnRequests = new PermissionRecord { Name = "Admin area. Manage Return Requests", SystemName = PermissionSystemName.ReturnRequests, Category = "Orders", Actions = new List<string> { PermissionActionName.List, PermissionActionName.Create, PermissionActionName.Edit, PermissionActionName.Preview, PermissionActionName.Delete } };
        public static readonly PermissionRecord ManageDocuments = new PermissionRecord { Name = "Admin area. Manage Documents", SystemName = PermissionSystemName.Documents, Category = "Customers", Actions = new List<string> { PermissionActionName.List, PermissionActionName.Create, PermissionActionName.Edit, PermissionActionName.Preview, PermissionActionName.Delete } };
        public static readonly PermissionRecord ManageReports = new PermissionRecord { Name = "Admin area. Manage Reports", SystemName = PermissionSystemName.Reports, Category = "Reports" };
        public static readonly PermissionRecord ManageAffiliates = new PermissionRecord { Name = "Admin area. Manage Affiliates", SystemName = PermissionSystemName.Affiliates, Category = "Promo", Actions = new List<string> { PermissionActionName.List, PermissionActionName.Create, PermissionActionName.Edit, PermissionActionName.Preview, PermissionActionName.Delete } };
        public static readonly PermissionRecord ManagePushNotifications = new PermissionRecord { Name = "Admin area. Manage Push Notifications", SystemName = PermissionSystemName.PushNotifications, Category = "Promo", Actions = new List<string> { PermissionActionName.List, PermissionActionName.Create, PermissionActionName.Edit, PermissionActionName.Delete } };
        public static readonly PermissionRecord ManageCampaigns = new PermissionRecord { Name = "Admin area. Manage Campaigns", SystemName = PermissionSystemName.Campaigns, Category = "Promo", Actions = new List<string> { PermissionActionName.List, PermissionActionName.Create, PermissionActionName.Edit, PermissionActionName.Preview, PermissionActionName.Delete, PermissionActionName.Export } };
        public static readonly PermissionRecord ManageBanners = new PermissionRecord { Name = "Admin area. Manage Banners", SystemName = PermissionSystemName.Banners, Category = "Promo", Actions = new List<string> { PermissionActionName.List, PermissionActionName.Create, PermissionActionName.Edit, PermissionActionName.Preview, PermissionActionName.Delete } };
        public static readonly PermissionRecord ManageInteractiveForm = new PermissionRecord { Name = "Admin area. Manage Interactive Forms", SystemName = PermissionSystemName.InteractiveForms, Category = "Promo", Actions = new List<string> { PermissionActionName.List, PermissionActionName.Create, PermissionActionName.Edit, PermissionActionName.Preview, PermissionActionName.Delete } };
        public static readonly PermissionRecord ManageDiscounts = new PermissionRecord { Name = "Admin area. Manage Discounts", SystemName = PermissionSystemName.Discounts, Category = "Promo", Actions = new List<string> { PermissionActionName.List, PermissionActionName.Create, PermissionActionName.Preview, PermissionActionName.Edit, PermissionActionName.Delete } };
        public static readonly PermissionRecord ManageNewsletterSubscribers = new PermissionRecord { Name = "Admin area. Manage Newsletter Subscribers", SystemName = PermissionSystemName.NewsletterSubscribers, Category = "Promo", Actions = new List<string> { PermissionActionName.List, PermissionActionName.Edit, PermissionActionName.Export, PermissionActionName.Import, PermissionActionName.Delete } };
        public static readonly PermissionRecord ManageNewsletterCategories = new PermissionRecord { Name = "Admin area. Manage Newsletter Categories", SystemName = PermissionSystemName.NewsletterCategories, Category = "Promo", Actions = new List<string> { PermissionActionName.List, PermissionActionName.Create, PermissionActionName.Preview, PermissionActionName.Edit, PermissionActionName.Delete } };
        public static readonly PermissionRecord ManagePolls = new PermissionRecord { Name = "Admin area. Manage Polls", SystemName = PermissionSystemName.Polls, Category = "Content Management", Actions = new List<string> { PermissionActionName.List, PermissionActionName.Create, PermissionActionName.Edit, PermissionActionName.Preview, PermissionActionName.Delete } };
        public static readonly PermissionRecord ManageNews = new PermissionRecord { Name = "Admin area. Manage News", SystemName = PermissionSystemName.News, Category = "Content Management", Actions = new List<string> { PermissionActionName.List, PermissionActionName.Create, PermissionActionName.Edit, PermissionActionName.Preview, PermissionActionName.Delete } };
        public static readonly PermissionRecord ManageBlog = new PermissionRecord { Name = "Admin area. Manage Blog", SystemName = PermissionSystemName.Blog, Category = "Content Management", Actions = new List<string> { PermissionActionName.List, PermissionActionName.Create, PermissionActionName.Edit, PermissionActionName.Preview, PermissionActionName.Delete } };
        public static readonly PermissionRecord ManageWidgets = new PermissionRecord { Name = "Admin area. Manage Widgets", SystemName = PermissionSystemName.Widgets, Category = "Content Management", Actions = new List<string> { PermissionActionName.List, PermissionActionName.Edit } };
        public static readonly PermissionRecord ManageTopics = new PermissionRecord { Name = "Admin area. Manage Topics", SystemName = PermissionSystemName.Topics, Category = "Content Management", Actions = new List<string> { PermissionActionName.List, PermissionActionName.Create, PermissionActionName.Edit, PermissionActionName.Preview, PermissionActionName.Delete } };
        public static readonly PermissionRecord ManageForums = new PermissionRecord { Name = "Admin area. Manage Forums", SystemName = PermissionSystemName.Forums, Category = "Content Management", Actions = new List<string> { PermissionActionName.List, PermissionActionName.Create, PermissionActionName.Edit, PermissionActionName.Preview, PermissionActionName.Delete } };
        public static readonly PermissionRecord ManageKnowledgebase = new PermissionRecord { Name = "Admin area. Manage Knowledgebase", SystemName = PermissionSystemName.Knowledgebase, Category = "Content Management", Actions = new List<string> { PermissionActionName.List, PermissionActionName.Create, PermissionActionName.Edit, PermissionActionName.Preview, PermissionActionName.Delete } };
        public static readonly PermissionRecord ManageCourses = new PermissionRecord { Name = "Admin area. Manage Courses", SystemName = PermissionSystemName.Courses, Category = "Content Management", Actions = new List<string> { PermissionActionName.List, PermissionActionName.Create, PermissionActionName.Edit, PermissionActionName.Preview, PermissionActionName.Delete } };
        public static readonly PermissionRecord ManageMessageTemplates = new PermissionRecord { Name = "Admin area. Manage Message Templates", SystemName = PermissionSystemName.MessageTemplates, Category = "Content Management", Actions = new List<string> { PermissionActionName.List, PermissionActionName.Create, PermissionActionName.Edit, PermissionActionName.Preview, PermissionActionName.Delete } };
        public static readonly PermissionRecord ManageCountries = new PermissionRecord { Name = "Admin area. Manage Countries", SystemName = PermissionSystemName.Countries, Category = "Configuration", Actions = new List<string> { PermissionActionName.List, PermissionActionName.Create, PermissionActionName.Edit, PermissionActionName.Preview, PermissionActionName.Delete, PermissionActionName.Export, PermissionActionName.Import } };
        public static readonly PermissionRecord ManageLanguages = new PermissionRecord { Name = "Admin area. Manage Languages", SystemName = PermissionSystemName.Languages, Category = "Configuration", Actions = new List<string> { PermissionActionName.List, PermissionActionName.Create, PermissionActionName.Edit, PermissionActionName.Preview, PermissionActionName.Delete, PermissionActionName.Export, PermissionActionName.Import } };
        public static readonly PermissionRecord ManageSettings = new PermissionRecord { Name = "Admin area. Manage Settings", SystemName = PermissionSystemName.Settings, Category = "Configuration" };
        public static readonly PermissionRecord ManagePaymentMethods = new PermissionRecord { Name = "Admin area. Manage Payment Methods", SystemName = PermissionSystemName.PaymentMethods, Category = "Configuration" };
        public static readonly PermissionRecord ManageExternalAuthenticationMethods = new PermissionRecord { Name = "Admin area. Manage External Authentication Methods", SystemName = PermissionSystemName.ExternalAuthenticationMethods, Category = "Configuration" };
        public static readonly PermissionRecord ManageTaxSettings = new PermissionRecord { Name = "Admin area. Manage Tax Settings", SystemName = PermissionSystemName.TaxSettings, Category = "Configuration" };
        public static readonly PermissionRecord ManageShippingSettings = new PermissionRecord { Name = "Admin area. Manage Shipping Settings", SystemName = PermissionSystemName.ShippingSettings, Category = "Configuration" };
        public static readonly PermissionRecord ManageCurrencies = new PermissionRecord { Name = "Admin area. Manage Currencies", SystemName = PermissionSystemName.Currencies, Category = "Configuration", Actions = new List<string> { PermissionActionName.List, PermissionActionName.Create, PermissionActionName.Edit, PermissionActionName.Preview, PermissionActionName.Delete } };


        public static readonly PermissionRecord AccessMasterSetup = new PermissionRecord { Name = "Access Master Setup", SystemName = PermissionSystemName.MasterSetup };
        public static readonly PermissionRecord AccessSubsidyRegister = new PermissionRecord { Name = "Access Subsidy Setup", SystemName = PermissionSystemName.AccessSubsidyRegister };
        public static readonly PermissionRecord AccessResourceRegister = new PermissionRecord { Name = "Access AccessResourceRegister Setup", SystemName = PermissionSystemName.AccessResourceRegister };



        //new permissions
        public static readonly PermissionRecord AnimalRegistration = new PermissionRecord { Name = "Access Animal Registration", SystemName = PermissionSystemName.AnimalRegistration };
        public static readonly PermissionRecord AnimalHealth = new PermissionRecord { Name = "Access Animal Health", SystemName = PermissionSystemName.AnimalHealth };
        public static readonly PermissionRecord AnimalNutrition = new PermissionRecord { Name = "Access Animal Nutrition", SystemName = PermissionSystemName.AnimalNutrition };
        public static readonly PermissionRecord PerformanceRecording = new PermissionRecord { Name = "Access Animal Performance", SystemName = PermissionSystemName.PerformanceRecording };
        public static readonly PermissionRecord AnimalBreeding = new PermissionRecord { Name = "Access Animal Breeding", SystemName = PermissionSystemName.Breeding };
        public static readonly PermissionRecord ModifyUser = new PermissionRecord { Name = "Access Organization User", SystemName = PermissionSystemName.User, Actions = new List<string> { PermissionActionName.List, PermissionActionName.Create, PermissionActionName.Edit, PermissionActionName.Preview, PermissionActionName.Delete, } };
        public static readonly PermissionRecord LivestockData = new PermissionRecord { Name = "Enter Livestock Data", SystemName = PermissionSystemName.LivestockData };
        public static readonly PermissionRecord ProductionData = new PermissionRecord { Name = "Enter Production Data ", SystemName = PermissionSystemName.ProductionData };
        public static readonly PermissionRecord ServiceData = new PermissionRecord { Name = "Enter Service Data", SystemName = PermissionSystemName.ServiceData };
        public static readonly PermissionRecord MedicineInventory = new PermissionRecord { Name = "Admin area. Manage Medicine Inventory", SystemName = PermissionSystemName.MedicineInventory, Actions = new List<string> { PermissionActionName.List, PermissionActionName.Create, PermissionActionName.Edit, PermissionActionName.Preview, PermissionActionName.Delete } };
        public static readonly PermissionRecord Activity = new PermissionRecord { Name = "Admin area. Manage Activity", SystemName = PermissionSystemName.Activity, Actions = new List<string> { PermissionActionName.List, PermissionActionName.Create, PermissionActionName.Edit, PermissionActionName.Preview, PermissionActionName.Delete } };
        public static readonly PermissionRecord Target = new PermissionRecord { Name = "Admin area. Manage Activity Target", SystemName = PermissionSystemName.Target, Actions = new List<string> { PermissionActionName.List, PermissionActionName.Create, PermissionActionName.Edit, PermissionActionName.Preview, PermissionActionName.Delete } };
        public static readonly PermissionRecord Progress = new PermissionRecord { Name = "Admin area. Manage Activity Progress", SystemName = PermissionSystemName.Progress, Actions = new List<string> { PermissionActionName.List, PermissionActionName.Create, PermissionActionName.Edit, PermissionActionName.Preview, PermissionActionName.Delete } };
        public static readonly PermissionRecord ActivityMenu = new PermissionRecord { Name = "Admin area. Access Activity Menu", SystemName = PermissionSystemName.ActivityMenu };
        public static readonly PermissionRecord LssReport = new PermissionRecord { Name = "Admin area. Access Lss Report", SystemName = PermissionSystemName.AccessLssReport };
        public static readonly PermissionRecord VhlsecReport = new PermissionRecord { Name = "Admin area. Access Vhlsec Report", SystemName = PermissionSystemName.AccessVhlsecReport };
        public static readonly PermissionRecord NlboOnly = new PermissionRecord { Name = "Admin area. Access by Nlbo", SystemName = PermissionSystemName.AccessByNlboOnly };
        public static readonly PermissionRecord DolfdOnly = new PermissionRecord { Name = "Admin area. Access by Dolfd", SystemName = PermissionSystemName.AccessByDolfdOnly };


        public static readonly PermissionRecord ManageMeasures = new PermissionRecord {
            Name = "Admin area. Manage Measures",
            SystemName = PermissionSystemName.Measures,
            Category = "Configuration",
            Actions = new List<string> {
                PermissionActionName.Weights_List, PermissionActionName.Weights_Add, PermissionActionName.Weights_Edit, PermissionActionName.Weights_Delete,
                PermissionActionName.Units_List, PermissionActionName.Units_Add, PermissionActionName.Units_Edit, PermissionActionName.Units_Delete,
                PermissionActionName.Dimensions_List, PermissionActionName.Dimensions_Add, PermissionActionName.Dimensions_Edit, PermissionActionName.Dimensions_Delete,
            }
        };

        public static readonly PermissionRecord ManageActivityLog = new PermissionRecord { Name = "Admin area. Manage Activity Log", SystemName = PermissionSystemName.ActivityLog, Category = "Configuration", Actions = new List<string> { PermissionActionName.List, PermissionActionName.Edit, PermissionActionName.Delete } };
        public static readonly PermissionRecord ManageAcl = new PermissionRecord { Name = "Admin area. Manage ACL", SystemName = PermissionSystemName.Acl, Category = "Configuration" };
        public static readonly PermissionRecord ManageEmailAccounts = new PermissionRecord { Name = "Admin area. Manage Email Accounts", SystemName = PermissionSystemName.EmailAccounts, Category = "Configuration", Actions = new List<string> { PermissionActionName.List, PermissionActionName.Edit, PermissionActionName.Create, PermissionActionName.Preview, PermissionActionName.Delete } };
        public static readonly PermissionRecord ManageStores = new PermissionRecord { Name = "Admin area. Manage Stores", SystemName = PermissionSystemName.Stores, Category = "Configuration", Actions = new List<string> { PermissionActionName.List, PermissionActionName.Edit, PermissionActionName.Create, PermissionActionName.Preview, PermissionActionName.Delete } };
        public static readonly PermissionRecord ManagePlugins = new PermissionRecord { Name = "Admin area. Manage Plugins", SystemName = PermissionSystemName.Plugins, Category = "Configuration" };
        public static readonly PermissionRecord ManageSystemLog = new PermissionRecord { Name = "Admin area. Manage System Log", SystemName = PermissionSystemName.SystemLog, Category = "Configuration" };
        public static readonly PermissionRecord ManageMessageQueue = new PermissionRecord { Name = "Admin area. Manage Message Queue", SystemName = PermissionSystemName.MessageQueue, Category = "Configuration", Actions = new List<string> { PermissionActionName.List, PermissionActionName.Create, PermissionActionName.Edit, PermissionActionName.Preview, PermissionActionName.Delete } };
        public static readonly PermissionRecord ManageMessageContactForm = new PermissionRecord { Name = "Admin area. Manage Message Contact form", SystemName = PermissionSystemName.MessageContactForm, Category = "Configuration", Actions = new List<string> { PermissionActionName.List, PermissionActionName.Preview, PermissionActionName.Delete } };
        public static readonly PermissionRecord ManageMaintenance = new PermissionRecord { Name = "Admin area. Manage Maintenance", SystemName = PermissionSystemName.Maintenance, Category = "Configuration" };
        public static readonly PermissionRecord ManageFiles = new PermissionRecord { Name = "Admin area. Manage Files", SystemName = PermissionSystemName.Files, Category = "Configuration" };
        public static readonly PermissionRecord ManagePictures = new PermissionRecord { Name = "Admin area. Manage Pictures", SystemName = PermissionSystemName.Pictures, Category = "Configuration" };
        public static readonly PermissionRecord ManageGenericAttributes = new PermissionRecord { Name = "Admin area. Manage Generic Attributes", SystemName = PermissionSystemName.GenericAttributes, Category = "Configuration" };
        public static readonly PermissionRecord HtmlEditorManagePictures = new PermissionRecord { Name = "Admin area. HTML Editor. Manage pictures", SystemName = PermissionSystemName.HtmlEditor, Category = "Configuration" };
        public static readonly PermissionRecord ManageScheduleTasks = new PermissionRecord { Name = "Admin area. Manage Schedule Tasks", SystemName = PermissionSystemName.ScheduleTasks, Category = "Configuration", Actions = new List<string> { PermissionActionName.List, PermissionActionName.Edit, PermissionActionName.Preview } };

        //public store permissions
        public static readonly PermissionRecord DisplayPrices = new PermissionRecord { Name = "Public store. Display Prices", SystemName = PermissionSystemName.DisplayPrices, Category = "PublicStore" };
        public static readonly PermissionRecord EnableShoppingCart = new PermissionRecord { Name = "Public store. Enable shopping cart", SystemName = PermissionSystemName.EnableShoppingCart, Category = "PublicStore" };
        public static readonly PermissionRecord EnableWishlist = new PermissionRecord { Name = "Public store. Enable wishlist", SystemName = PermissionSystemName.EnableWishlist, Category = "PublicStore" };
        public static readonly PermissionRecord PublicStoreAllowNavigation = new PermissionRecord { Name = "Public store. Allow navigation", SystemName = PermissionSystemName.PublicStoreAllowNavigation, Category = "PublicStore" };
        public static readonly PermissionRecord AccessClosedStore = new PermissionRecord { Name = "Public store. Access a closed store", SystemName = PermissionSystemName.AccessClosedStore, Category = "PublicStore" };


        public static readonly PermissionRecord AccessDynamicMenu = new PermissionRecord { Name = "Admin area. Manage Dynamic Menu. Access a Dynamic Menu", SystemName = PermissionSystemName.AccessDynamicMenu, Category = "Configuration" };
        public static readonly PermissionRecord AccessNewsEventTender = new PermissionRecord { Name = "Admin area. Manage News event tender publication. Access news event tender publication.", SystemName = PermissionSystemName.AccessNewsEventTender, Category = "Configuration" };
        public static readonly PermissionRecord AccessGeneralCMS = new PermissionRecord { Name = "Admin area. Manage CMS for Website. Access CMS for Website.", SystemName = PermissionSystemName.AccessGeneralCMS, Category = "Configuration" };
        public static readonly PermissionRecord AccessMolmacRegister = new PermissionRecord { Name = "Admin area. Manage MolmacRegister", SystemName = PermissionSystemName.AccessMolmacRegister };
        public static readonly PermissionRecord AccessCMS = new PermissionRecord { Name = "Admin area. Manage CDS", SystemName = PermissionSystemName.AccessCDS };
        public static readonly PermissionRecord AccessCropProduction = new PermissionRecord { Name = "Admin area. AccessCropsProduction", SystemName = PermissionSystemName.AccessCropsProduction };

        //Agriculture
        public static readonly PermissionRecord AgriUserAccess = new PermissionRecord { Name = "AgriUser Access", SystemName = PermissionSystemName.AgriUserAccess };
        public static readonly PermissionRecord LivestockUserAccess = new PermissionRecord { Name = "LivestockUser Access", SystemName = PermissionSystemName.LivestockUserAccess };
        public static readonly PermissionRecord OnlyDataEntryAccess = new PermissionRecord { Name = "DataEntryUser Access", SystemName = PermissionSystemName.OnlyDataEntryAccess };


        //New role for Tokha Agriculture System
        //Dashboard
        public static readonly PermissionRecord AccessDashboard = new PermissionRecord { Name = "Access Dashboard", SystemName = PermissionSystemName.AccessDashboard };
        public static readonly PermissionRecord AccessUser = new PermissionRecord { Name = "Access User", SystemName = PermissionSystemName.AccessUser };

        //organization
        public static readonly PermissionRecord AccessOrganization = new PermissionRecord { Name = "Access Organization", SystemName = PermissionSystemName.AccessOrganization };
        public static readonly PermissionRecord AccessOtherOrganization = new PermissionRecord { Name = "Access Other Organization", SystemName = PermissionSystemName.AccessOtherOrganization };
        public static readonly PermissionRecord AccessServiceProvider = new PermissionRecord { Name = "Access Service Provider", SystemName = PermissionSystemName.AccessServiceProvider };

        //Service Provider
        public static readonly PermissionRecord AccessHatchery = new PermissionRecord { Name = "Access Hatchery", SystemName = PermissionSystemName.AccessHatchery };
        public static readonly PermissionRecord AccessFeedIndustry = new PermissionRecord { Name = "Access Feed Industry", SystemName = PermissionSystemName.AccessFeedIndustry };
        public static readonly PermissionRecord AccessFeedShop = new PermissionRecord { Name = "Access Feed Shop", SystemName = PermissionSystemName.AccessFeedShop };
        public static readonly PermissionRecord AccessMilkCoOperative = new PermissionRecord { Name = "Access Milk Co Operative", SystemName = PermissionSystemName.AccessMilkCoOperative };
        public static readonly PermissionRecord AccessDiaryIndustry = new PermissionRecord { Name = "Access Diary Industry", SystemName = PermissionSystemName.AccessDiaryIndustry };
        public static readonly PermissionRecord AccessDiaryShop = new PermissionRecord { Name = "Access Diary Shop", SystemName = PermissionSystemName.AccessDiaryShop };
        public static readonly PermissionRecord AccessChillingCenter = new PermissionRecord { Name = "Access Chilling Center", SystemName = PermissionSystemName.AccessChillingCenter };
        public static readonly PermissionRecord AccessAnimalCollection = new PermissionRecord { Name = "Access Animal Collection", SystemName = PermissionSystemName.AccessAnimalCollection };
        public static readonly PermissionRecord AccessMarket = new PermissionRecord { Name = "Access Market", SystemName = PermissionSystemName.AccessMarket };
        public static readonly PermissionRecord AccessMeatProcessing = new PermissionRecord { Name = "Access Meat Processing", SystemName = PermissionSystemName.AccessMeatProcessing };
        public static readonly PermissionRecord AccessMeatShop = new PermissionRecord { Name = "Access Meat Shop", SystemName = PermissionSystemName.AccessMeatShop };
        public static readonly PermissionRecord AccessFishSrot = new PermissionRecord { Name = "Access Fish Srot", SystemName = PermissionSystemName.AccessFishSrot };
        public static readonly PermissionRecord AccessNgo = new PermissionRecord { Name = "Access Ngo", SystemName = PermissionSystemName.AccessNgo };
        public static readonly PermissionRecord AccessTechSchool = new PermissionRecord { Name = "Access Tech School", SystemName = PermissionSystemName.AccessTechSchool };
        public static readonly PermissionRecord AccessCanelClub = new PermissionRecord { Name = "Access Canel Club", SystemName = PermissionSystemName.AccessCanelClub };
        public static readonly PermissionRecord AccessVetClinic = new PermissionRecord { Name = "Access Vet Clinic", SystemName = PermissionSystemName.AccessVetClinic };
        public static readonly PermissionRecord AccessFertilizerShop = new PermissionRecord { Name = "Access Fertilizer Shop", SystemName = PermissionSystemName.AccessFertilizerShop };
        public static readonly PermissionRecord AccessAgricultureCoOperative = new PermissionRecord { Name = "Access Agriculture CoOperative", SystemName = PermissionSystemName.AccessAgricultureCoOperative };

        //Farm
        public static readonly PermissionRecord AccessFarm = new PermissionRecord { Name = "Access Farm", SystemName = PermissionSystemName.AccessFarm };

        //AI
        public static readonly PermissionRecord AccessAI = new PermissionRecord { Name = "Access AI", SystemName = PermissionSystemName.AccessAI };

        //Medicine
        public static readonly PermissionRecord AccessMedicine = new PermissionRecord { Name = "Access Medicine", SystemName = PermissionSystemName.AccessMedicine };
        public static readonly PermissionRecord AccessReceivedMedicine = new PermissionRecord { Name = "Access Received Medicine", SystemName = PermissionSystemName.AccessReceivedMedicine };
        public static readonly PermissionRecord AccessMedicineDistribution = new PermissionRecord { Name = "Access Medicine Distribution", SystemName = PermissionSystemName.AccessMedicineDistribution };
        public static readonly PermissionRecord AccessReceivedVaccination = new PermissionRecord { Name = "Access Received Vaccination", SystemName = PermissionSystemName.AccessReceivedVaccination };
        public static readonly PermissionRecord AccessDistributedVaccination = new PermissionRecord { Name = "Access Distributed Vaccination", SystemName = PermissionSystemName.AccessDistributedVaccination };

        //DataEntry
        public static readonly PermissionRecord AccessPujigatKharchaKharyakram = new PermissionRecord { Name = "Access PujigatKharcha Karyakram", SystemName = PermissionSystemName.AccessPujigatKharchaKharyakram };
        public static readonly PermissionRecord AccessMonthlyProgress = new PermissionRecord { Name = "Access Monthly Progress", SystemName = PermissionSystemName.AccessMonthlyProgress };
        public static readonly PermissionRecord AccessFarmer = new PermissionRecord { Name = "Access Farmer", SystemName = PermissionSystemName.AccessFarmer };
        public static readonly PermissionRecord AccessAanudanKaryakram = new PermissionRecord { Name = "Access Aanudan Karyakram", SystemName = PermissionSystemName.AccessAanudanKaryakram };
        public static readonly PermissionRecord AccessAnugaman = new PermissionRecord { Name = "Access Anugaman", SystemName = PermissionSystemName.AccessAnugaman };
        public static readonly PermissionRecord AccessInputSupply = new PermissionRecord { Name = "Access Input Supply", SystemName = PermissionSystemName.AccessInputSupply };
        public static readonly PermissionRecord AccessDeathVerification = new PermissionRecord { Name = "Access Death Verification", SystemName = PermissionSystemName.AccessDeathVerification };
        public static readonly PermissionRecord AccessTreatment = new PermissionRecord { Name = "Access Treatment", SystemName = PermissionSystemName.AccessTreatment };
        //public static readonly PermissionRecord AccessMarket = new PermissionRecord { Name = "AccessMarket", SystemName = PermissionSystemName.AccessMarket };
        public static readonly PermissionRecord AccessResources = new PermissionRecord { Name = "Access Resources", SystemName = PermissionSystemName.AccessResources };
        public static readonly PermissionRecord AccessTalim = new PermissionRecord { Name = "Access Talim", SystemName = PermissionSystemName.AccessTalim };
        public static readonly PermissionRecord AccessIncubationCenter = new PermissionRecord { Name = "Access Incubation Center", SystemName = PermissionSystemName.AccessIncubationCenter };
        //public static readonly PermissionRecord AccessFarmer = new PermissionRecord { Name = "AccessFarmer", SystemName = PermissionSystemName.AccessFarmer };
        public static readonly PermissionRecord AccessSoil = new PermissionRecord { Name = "Access Soil", SystemName = PermissionSystemName.AccessSoil };
        public static readonly PermissionRecord AccessLivestock = new PermissionRecord { Name = "Access Livestock", SystemName = PermissionSystemName.AccessLivestock };
        public static readonly PermissionRecord AccessProduction = new PermissionRecord { Name = "Access Production", SystemName = PermissionSystemName.AccessProduction };
        public static readonly PermissionRecord AccessCropsProduction = new PermissionRecord { Name = "Access Crops Production", SystemName = PermissionSystemName.AccessCropsProduction };
        public static readonly PermissionRecord AccessFishProduction = new PermissionRecord { Name = "Access Fish Production", SystemName = PermissionSystemName.AccessFishProduction };
        public static readonly PermissionRecord AccessSeedDistribution = new PermissionRecord { Name = "Access Seed Distribution", SystemName = PermissionSystemName.AccessSeedDistribution };
        public static readonly PermissionRecord AccessFertilizerDistribution = new PermissionRecord { Name = "Access Fertilizer Distribution", SystemName = PermissionSystemName.AccessFertilizerDistribution };
        public static readonly PermissionRecord AccessCropDisease = new PermissionRecord { Name = "Access Crop Disease", SystemName = PermissionSystemName.AccessCropDisease };

        //Report
        public static readonly PermissionRecord AccessProgressReport = new PermissionRecord { Name = "Access Progress Report", SystemName = PermissionSystemName.AccessProgressReport };
        public static readonly PermissionRecord AccessSubsidyReport = new PermissionRecord { Name = "Access Subsidy Report", SystemName = PermissionSystemName.AccessSubsidyReport };
        public static readonly PermissionRecord AccessTrainingReport = new PermissionRecord { Name = "Access Training Report", SystemName = PermissionSystemName.AccessTrainingReport };
        public static readonly PermissionRecord AccessProductionReport = new PermissionRecord { Name = "Access Production Report", SystemName = PermissionSystemName.AccessProductionReport };
        public static readonly PermissionRecord AccessMedicineReport = new PermissionRecord { Name = "Access Medicine Report", SystemName = PermissionSystemName.AccessMedicineReport };
        public static readonly PermissionRecord AccessAiReport = new PermissionRecord { Name = "Access Ai Report", SystemName = PermissionSystemName.AccessAiReport };
        public static readonly PermissionRecord AccessMonthlyProgressReport = new PermissionRecord { Name = "Access Monthly Progress Report", SystemName = PermissionSystemName.AccessMonthlyProgressReport };
        public static readonly PermissionRecord AccessKirshakReport = new PermissionRecord { Name = "Access Kirshak Report", SystemName = PermissionSystemName.AccessKirshakReport };
        public static readonly PermissionRecord AccessAanudanOrgReport = new PermissionRecord { Name = "Access Aanudan Org Report", SystemName = PermissionSystemName.AccessAanudanOrgReport };
        public static readonly PermissionRecord AccessMonthlyProgressSummery = new PermissionRecord { Name = "Access Monthly Progress Summery", SystemName = PermissionSystemName.AccessMonthlyProgressSummery };
        // public static readonly PermissionRecord AccessMonthlyProgressSReport = new PermissionRecord { Name = "Access Monthly Progress Report", SystemName = PermissionSystemName.AccessMonthlyProgressSReport };
        public static readonly PermissionRecord AccessCropProductionReport = new PermissionRecord { Name = "Access Crop Production Report", SystemName = PermissionSystemName.AccessCropProductionReport };
        //public static readonly PermissionRecord AccessProductionReport = new PermissionRecord { Name = "AccessProductionReport", SystemName = PermissionSystemName.AccessProductionReport };
        public static readonly PermissionRecord AccessLivestockReport = new PermissionRecord { Name = "Access Livestock Report", SystemName = PermissionSystemName.AccessLivestockReport };
        public static readonly PermissionRecord AccessAanudanKaryakramReport = new PermissionRecord { Name = "Access Aanudan Karyakram Report", SystemName = PermissionSystemName.AccessAanudanKaryakramReport };
        public static readonly PermissionRecord AccessDeathVerificationReport = new PermissionRecord { Name = "Access Death Verification Report", SystemName = PermissionSystemName.AccessDeathVerificationReport };
        public static readonly PermissionRecord AccessAgricultureCoOperativeReport = new PermissionRecord { Name = "Access Agriculture CoOperative Report", SystemName = PermissionSystemName.AccessAgricultureCoOperativeReport };
        public static readonly PermissionRecord AccessFertilizerShopReport = new PermissionRecord { Name = "Access Fertilizer Shop Report", SystemName = PermissionSystemName.AccessFertilizerShopReport };

        //Mastersetup
        public static readonly PermissionRecord AccessSpecies = new PermissionRecord { Name = "Access Species", SystemName = PermissionSystemName.AccessSpecies };
        public static readonly PermissionRecord AccessBreed = new PermissionRecord { Name = "Access Breed", SystemName = PermissionSystemName.AccessBreed };
        public static readonly PermissionRecord AccessGrowingSeason = new PermissionRecord { Name = "Access Growing Season", SystemName = PermissionSystemName.AccessGrowingSeason };
        public static readonly PermissionRecord AccessLivestockSpecies = new PermissionRecord { Name = "Access Livestock Species", SystemName = PermissionSystemName.AccessLivestockSpecies };
        public static readonly PermissionRecord AccessCategory = new PermissionRecord { Name = "Access Category", SystemName = PermissionSystemName.AccessCategory };
        public static readonly PermissionRecord AccessAnimalType = new PermissionRecord { Name = "Access Animal Type", SystemName = PermissionSystemName.AccessAnimalType };
        public static readonly PermissionRecord AccessVaccineType = new PermissionRecord { Name = "Access Vaccine Type", SystemName = PermissionSystemName.AccessVaccineType };
        public static readonly PermissionRecord AccessUnit = new PermissionRecord { Name = "Access Unit", SystemName = PermissionSystemName.AccessUnit };
        public static readonly PermissionRecord AccessFiscalYear = new PermissionRecord { Name = "Access Fiscal Year", SystemName = PermissionSystemName.AccessFiscalYear };
        public static readonly PermissionRecord AccessDisease = new PermissionRecord { Name = "Access Disease", SystemName = PermissionSystemName.AccessDisease };


        public virtual IEnumerable<PermissionRecord> GetPermissions()
        {
            return new[]
            {
                AccessAdminPanel,
                AllowCustomerImpersonation,
                ManageProducts,
                ManageCategories,
                ManageManufacturers,
                ManageProductReviews,
                ManageProductTags,
                ManageProductAttributes,
                ManageSpecificationAttributes,
                ManageCheckoutAttribute,
                ManageContactAttribute,
                ManageCustomers,
                ManageCustomerRoles,
                ManageCustomerTags,
                ManageActions,
                ManageReminders,
                ManageBanners,
                ManageInteractiveForm,
                ManageVendors,
                ManageVendorReviews,
                ManageCurrentCarts,
                ManageOrders,
                ManageShipments,
                ManageRecurringPayments,
                ManageGiftCards,
                ManageReturnRequests,
                ManageDocuments,
                ManageReports,
                ManageAffiliates,
                ManagePushNotifications,
                ManageCampaigns,
                ManageDiscounts,
                ManageNewsletterSubscribers,
                ManageNewsletterCategories,
                ManagePolls,
                ManageNews,
                ManageBlog,
                ManageWidgets,
                ManageTopics,
                ManageForums,
                ManageKnowledgebase,
                ManageCourses,
                ManageMessageTemplates,
                ManageCountries,
                ManageLanguages,
                ManageSettings,
                ManagePaymentMethods,
                ManageExternalAuthenticationMethods,
                ManageTaxSettings,
                ManageShippingSettings,
                ManageCurrencies,
                ManageMeasures,
                ManageActivityLog,
                ManageAcl,
                ManageEmailAccounts,
                ManageStores,
                ManagePlugins,
                ManageSystemLog,
                ManageMessageQueue,
                ManageMessageContactForm,
                ManageMaintenance,
                ManageFiles,
                ManagePictures,
                ManageGenericAttributes,
                HtmlEditorManagePictures,
                ManageScheduleTasks,
                DisplayPrices,
                EnableShoppingCart,
                EnableWishlist,
                PublicStoreAllowNavigation,
                AccessClosedStore,
                ManageOrderTags,
                AccessMasterSetup,
                AnimalRegistration,
                AnimalBreeding,
                AnimalHealth,
                AnimalNutrition,
                PerformanceRecording,
                ModifyUser,
                LivestockData,
                ProductionData,
                ServiceData,
                MedicineInventory,
                Activity,
                Target,
                Progress,
                ActivityMenu,
                LssReport,
                VhlsecReport,
                NlboOnly,DolfdOnly,
                AccessDynamicMenu,
                AccessNewsEventTender,
                AccessGeneralCMS,
                AccessCMS,
                AccessMolmacRegister,
                AccessCropProduction,
                AccessSubsidyRegister,
                AccessResourceRegister,
                //Agriculture Access
                AgriUserAccess,
                LivestockUserAccess,
                OnlyDataEntryAccess,
                //New roles for Tokha Agriculture System
                //Dashboard
                AccessDashboard,
                AccessUser,
                         
                          //Organization 
                AccessOrganization  ,
                AccessOtherOrganization ,
                AccessServiceProvider,

                                              
                          //Service Provider
                AccessHatchery,
                AccessFeedIndustry,
                AccessFeedShop,
                AccessMilkCoOperative ,
                AccessDiaryIndustry  ,
                AccessDiaryShop      ,
                AccessChillingCenter ,
                AccessAnimalCollection  ,
                AccessMarket         ,
                AccessMeatProcessing ,
                AccessMeatShop       ,
                AccessFishSrot       ,
                AccessNgo            ,
                AccessTechSchool     ,
                AccessCanelClub      ,
                AccessVetClinic      ,
                AccessAgricultureCoOperative      ,
                AccessFertilizerShop      ,

                                              
                          //Farm        ,
                AccessFarm           ,

                                              
                         //AI          ,
                AccessAI             ,

                                              
                          //Medicine    ,
                AccessMedicine       ,
                AccessReceivedMedicine     ,
                AccessMedicineDistribution ,
                AccessReceivedVaccination  ,
                AccessDistributedVaccination        ,

                                              
                         //DataEntry   ,
                AccessPujigatKharchaKharyakram      ,
                AccessMonthlyProgress,
                AccessFarmer         ,
                AccessAanudanKaryakram     ,
                AccessAnugaman       ,
                AccessInputSupply    ,
                AccessDeathVerification    ,
                AccessTreatment,
                        // AccessMarket,
                AccessResources      ,
                AccessTalim          ,
                AccessIncubationCenter,
                         //AccessFarmer,
                AccessSoil           ,
                AccessLivestock      ,
                AccessProduction     ,
                AccessCropsProduction   ,
                AccessFishProduction,
                AccessCropDisease,
                AccessFertilizerDistribution,
                AccessSeedDistribution,
                          //Report      ,
                AccessProgressReport ,
                AccessSubsidyReport  ,
                AccessTrainingReport ,
                AccessProductionReport     ,
                AccessMedicineReport ,
                AccessAiReport       ,
                AccessMonthlyProgressReport      ,
                AccessKirshakReport  ,
                AccessAanudanOrgReport     ,
                AccessMonthlyProgressSummery        ,
                        // AccessMonthlyProgressSReport        ,
                AccessCropProductionReport         ,
                AccessProductionReport,
                AccessLivestockReport,
                AccessAanudanKaryakramReport        ,
                AccessDeathVerificationReport,
                AccessAgricultureCoOperativeReport  ,
                AccessFertilizerShopReport,
                          //Mastersetup ,
                AccessSpecies        ,
                AccessBreed          ,
                AccessGrowingSeason  ,
                AccessLivestockSpecies ,
                AccessCategory       ,
                AccessAnimalType     ,
                AccessVaccineType    ,
                AccessUnit           ,
                AccessFiscalYear     ,
                AccessDisease
            };
        }

        public virtual IEnumerable<DefaultPermissionRecord> GetDefaultPermissions()
        {
            return new[]
            {
                new DefaultPermissionRecord
                {
                    CustomerRoleSystemName = SystemCustomerRoleNames.Administrators,
                    PermissionRecords = new[]
                    {
                        AccessAdminPanel,
                        AllowCustomerImpersonation,
                        ManageProducts,
                        ManageCategories,
                        ManageManufacturers,
                        ManageProductReviews,
                        ManageProductTags,
                        ManageOrderTags,
                        ManageProductAttributes,
                        ManageSpecificationAttributes,
                        ManageCheckoutAttribute,
                        ManageContactAttribute,
                        ManageCustomers,
                        ManageCustomerRoles,
                        ManageCustomerTags,
                        ManageVendors,
                        ManageVendorReviews,
                        ManageCurrentCarts,
                        ManageOrders,
                        ManageShipments,
                        ManageRecurringPayments,
                        ManageGiftCards,
                        ManageReturnRequests,
                        ManageDocuments,
                        ManageReports,
                        ManageAffiliates,
                        ManagePushNotifications,
                        ManageCampaigns,
                        ManageDiscounts,
                        ManageNewsletterSubscribers,
                        ManageNewsletterCategories,
                        ManagePolls,
                        ManageNews,
                        ManageBlog,
                        ManageWidgets,
                        ManageTopics,
                        ManageForums,
                        ManageKnowledgebase,
                        ManageCourses,
                        ManageMessageTemplates,
                        ManageMessageQueue,
                        ManageMessageContactForm,
                        ManageMaintenance,
                        ManageFiles,
                        ManagePictures,
                        ManageGenericAttributes,
                        HtmlEditorManagePictures,
                        ManageScheduleTasks,
                        DisplayPrices,
                        EnableShoppingCart,
                        EnableWishlist,
                        PublicStoreAllowNavigation,
                        AccessClosedStore,
                        ManageBanners,
                        ManageInteractiveForm,
                        ManageActions,
                        ManageReminders,
                        AccessMasterSetup,
                        AccessCMS,
                        //Agriculture Access
                        AgriUserAccess,
                        LivestockUserAccess,
                        OnlyDataEntryAccess,
                        //New roles for Tokha Agriculture System
                         //Dashboard
                         AccessDashboard,
                         AccessUser,
                         
                          //Organization 
                         AccessOrganization  ,
                         AccessOtherOrganization ,
                         AccessServiceProvider,

                                              
                          //Service Provider
                         AccessHatchery,
                         AccessFeedIndustry,
                         AccessFeedShop,
                         AccessMilkCoOperative ,
                         AccessDiaryIndustry  ,
                         AccessDiaryShop      ,
                         AccessChillingCenter ,
                         AccessAnimalCollection  ,
                         AccessMarket         ,
                         AccessMeatProcessing ,
                         AccessMeatShop       ,
                         AccessFishSrot       ,
                         AccessNgo            ,
                         AccessTechSchool     ,
                         AccessCanelClub      ,
                         AccessVetClinic      ,
                         AccessAgricultureCoOperative      ,
                        AccessFertilizerShop      ,
                                              
                          //Farm        ,
                         AccessFarm           ,

                                              
                         //AI          ,
                         AccessAI             ,

                                              
                          //Medicine    ,
                         AccessMedicine       ,
                         AccessReceivedMedicine     ,
                         AccessMedicineDistribution ,
                         AccessReceivedVaccination  ,
                         AccessDistributedVaccination        ,

                                              
                         //DataEntry   ,
                         AccessPujigatKharchaKharyakram      ,
                         AccessMonthlyProgress,
                         AccessFarmer         ,
                         AccessAanudanKaryakram     ,
                         AccessAnugaman       ,
                         AccessInputSupply    ,
                         AccessDeathVerification    ,
                         AccessTreatment,
                        // AccessMarket,
                         AccessResources      ,
                         AccessTalim          ,
                         AccessIncubationCenter,
                         //AccessFarmer,
                         AccessSoil           ,
                         AccessLivestock      ,
                         AccessProduction     ,
                         AccessCropsProduction   ,
                         AccessFishProduction,
                           AccessCropDisease,
                        AccessFertilizerDistribution,
                        AccessSeedDistribution,

                          //Report      ,
                         AccessProgressReport ,
                         AccessSubsidyReport  ,
                         AccessTrainingReport ,
                         AccessProductionReport     ,
                         AccessMedicineReport ,
                         AccessAiReport       ,
                         AccessMonthlyProgressReport      ,
                         AccessKirshakReport  ,
                         AccessAanudanOrgReport     ,
                         AccessMonthlyProgressSummery        ,
                        // AccessMonthlyProgressSReport        ,
                         AccessCropProductionReport         ,
                         AccessProductionReport,
                         AccessLivestockReport,
                         AccessAanudanKaryakramReport        ,
                         AccessDeathVerificationReport,
                           AccessAgricultureCoOperativeReport  ,
                        AccessFertilizerShopReport,   
                        
                          //Mastersetup ,
                         AccessSpecies        ,
                         AccessBreed          ,
                         AccessGrowingSeason  ,
                         AccessLivestockSpecies ,
                         AccessCategory       ,
                         AccessAnimalType     ,
                         AccessVaccineType    ,
                         AccessUnit           ,
                         AccessFiscalYear     ,
                         AccessDisease


                    }




                },
                new DefaultPermissionRecord
                {
                    CustomerRoleSystemName = SystemCustomerRoleNames.Developer,
                    PermissionRecords = new[]
                    {
                        AccessAdminPanel,
                        AllowCustomerImpersonation,
                        ManageProducts,
                        ManageCategories,
                        ManageManufacturers,
                        ManageProductReviews,
                        ManageProductTags,
                        ManageOrderTags,
                        ManageProductAttributes,
                        ManageSpecificationAttributes,
                        ManageCheckoutAttribute,
                        ManageContactAttribute,
                        ManageCustomers,
                        ManageCustomerRoles,
                        ManageCustomerTags,
                        ManageVendors,
                        ManageVendorReviews,
                        ManageCurrentCarts,
                        ManageOrders,
                        ManageShipments,
                        ManageRecurringPayments,
                        ManageGiftCards,
                        ManageReturnRequests,
                        ManageDocuments,
                        ManageReports,
                        ManageAffiliates,
                        ManagePushNotifications,
                        ManageCampaigns,
                        ManageDiscounts,
                        ManageNewsletterSubscribers,
                        ManageNewsletterCategories,
                        ManagePolls,
                        ManageNews,
                        ManageBlog,
                        ManageWidgets,
                        ManageTopics,
                        ManageForums,
                        ManageKnowledgebase,
                        ManageCourses,
                        ManageMessageTemplates,
                        ManageCountries,
                        ManageLanguages,
                        ManageSettings,
                        ManagePaymentMethods,
                        ManageExternalAuthenticationMethods,
                        ManageTaxSettings,
                        ManageShippingSettings,
                        ManageCurrencies,
                        ManageMeasures,
                        ManageActivityLog,
                        ManageAcl,
                        ManageEmailAccounts,
                        ManageStores,
                        ManagePlugins,
                        ManageSystemLog,
                        ManageMessageQueue,
                        ManageMessageContactForm,
                        ManageMaintenance,
                        ManageFiles,
                        ManagePictures,
                        ManageGenericAttributes,
                        HtmlEditorManagePictures,
                        ManageScheduleTasks,
                        DisplayPrices,
                        EnableShoppingCart,
                        EnableWishlist,
                        PublicStoreAllowNavigation,
                        AccessClosedStore,
                        ManageBanners,
                        ManageInteractiveForm,
                        ManageActions,
                        ManageReminders,
                        //Agriculture Access
                        AgriUserAccess,
                        LivestockUserAccess,
                        OnlyDataEntryAccess,
                        //New roles for Tokha Agriculture System
                         //Dashboard
                         AccessDashboard,
                         AccessUser,
                         
                          //Organization 
                         AccessOrganization  ,
                         AccessOtherOrganization ,
                         AccessServiceProvider,

                                              
                          //Service Provider
                         AccessHatchery,
                         AccessFeedIndustry,
                         AccessFeedShop,
                         AccessMilkCoOperative ,
                         AccessDiaryIndustry  ,
                         AccessDiaryShop      ,
                         AccessChillingCenter ,
                         AccessAnimalCollection  ,
                         AccessMarket         ,
                         AccessMeatProcessing ,
                         AccessMeatShop       ,
                         AccessFishSrot       ,
                         AccessNgo            ,
                         AccessTechSchool     ,
                         AccessCanelClub      ,
                         AccessVetClinic      ,
                         AccessAgricultureCoOperative      ,
                         AccessFertilizerShop      ,
                                              
                          //Farm        ,
                         AccessFarm           ,

                                              
                         //AI          ,
                         AccessAI             ,

                                              
                          //Medicine    ,
                         AccessMedicine       ,
                         AccessReceivedMedicine     ,
                         AccessMedicineDistribution ,
                         AccessReceivedVaccination  ,
                         AccessDistributedVaccination        ,

                                              
                         //DataEntry   ,
                         AccessPujigatKharchaKharyakram      ,
                         AccessMonthlyProgress,
                         AccessFarmer         ,
                         AccessAanudanKaryakram     ,
                         AccessAnugaman       ,
                         AccessInputSupply    ,
                         AccessDeathVerification    ,
                         AccessTreatment,
                        // AccessMarket,
                         AccessResources      ,
                         AccessTalim          ,
                         AccessIncubationCenter,
                         //AccessFarmer,
                         AccessSoil           ,
                         AccessLivestock      ,
                         AccessProduction     ,
                         AccessCropsProduction   ,
                         AccessFishProduction,
                          AccessCropDisease,
                        AccessFertilizerDistribution,
                        AccessSeedDistribution,
                          //Report      ,
                         AccessProgressReport ,
                         AccessSubsidyReport  ,
                         AccessTrainingReport ,
                         AccessProductionReport     ,
                         AccessMedicineReport ,
                         AccessAiReport       ,
                         AccessMonthlyProgressReport      ,
                         AccessKirshakReport  ,
                         AccessAanudanOrgReport     ,
                         AccessMonthlyProgressSummery        ,
                        // AccessMonthlyProgressSReport        ,
                         AccessCropProductionReport         ,
                         AccessProductionReport,
                         AccessLivestockReport,
                         AccessAanudanKaryakramReport        ,
                         AccessDeathVerificationReport,
                         AccessAgricultureCoOperativeReport  ,
                         AccessFertilizerShopReport,    
                         
                          //Mastersetup ,
                         AccessSpecies        ,
                         AccessBreed          ,
                         AccessGrowingSeason  ,
                         AccessLivestockSpecies ,
                         AccessCategory       ,
                         AccessAnimalType     ,
                         AccessVaccineType    ,
                         AccessUnit           ,
                         AccessFiscalYear     ,
                         AccessDisease

                    }
                },
                };
            //    new DefaultPermissionRecord
            //    {
            //        CustomerRoleSystemName = SystemCustomerRoleNames.ForumModerators,
            //        PermissionRecords = new[]
            //        {
            //            DisplayPrices,
            //            EnableShoppingCart,
            //            EnableWishlist,
            //            PublicStoreAllowNavigation
            //        }
            //    },
            //    new DefaultPermissionRecord
            //    {
            //        CustomerRoleSystemName = SystemCustomerRoleNames.Guests,
            //        PermissionRecords = new[]
            //        {
            //            DisplayPrices,
            //            EnableShoppingCart,
            //            EnableWishlist,
            //            PublicStoreAllowNavigation
            //        }
            //    },
            //    new DefaultPermissionRecord
            //    {
            //        CustomerRoleSystemName = SystemCustomerRoleNames.Registered,
            //        PermissionRecords = new[]
            //        {
            //            DisplayPrices,
            //            EnableShoppingCart,
            //            EnableWishlist,
            //            PublicStoreAllowNavigation
            //        }
            //    },
            //    new DefaultPermissionRecord
            //    {
            //        CustomerRoleSystemName = SystemCustomerRoleNames.Vendors,
            //        PermissionRecords = new[]
            //        {
            //            AccessAdminPanel,
            //            ManageProducts,
            //            ManageFiles,
            //            ManagePictures,
            //            ManageOrders,
            //            ManageVendorReviews,
            //            ManageShipments
            //        }
            //    },
            //    new DefaultPermissionRecord
            //    {
            //        CustomerRoleSystemName = SystemCustomerRoleNames.Staff,
            //        PermissionRecords = new[]
            //        {
            //            AccessAdminPanel,
            //            ManageProducts,
            //            ManageFiles,
            //            ManagePictures,
            //            ManageCategories,
            //            ManageManufacturers,
            //            ManageOrders,
            //            ManageShipments,
            //            ManageReturnRequests,
            //            ManageReports

            //        }
            //    }
            //};
        }
    }
}