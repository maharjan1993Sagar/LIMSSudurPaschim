namespace LIMS.Services.Security
{
    public static class PermissionSystemName
    {
        public const string AccessAdminPanel = "AccessAdminPanel";
        public const string AllowCustomerImpersonation = "AllowCustomerImpersonation";
        public const string Products = "ManageProducts";
        public const string Categories = "ManageCategories";
        public const string ProductReviews = "ManageProductReviews";
        public const string Manufacturers = "ManageManufacturers";
        public const string ProductTags = "ManageProductTags";
        public const string ProductAttributes = "ManageProductAttributes";
        public const string SpecificationAttributes = "ManageSpecificationAttributes";
        public const string CheckoutAttributes = "ManageCheckoutAttributes";
        public const string ContactAttributes = "ManageContactAttributes";
        public const string Customers = "ManageCustomers";
        public const string CustomerTags = "ManageCustomerTags";
        public const string Documents = "ManageDocuments";
        public const string Reports = "ManageReports";
        public const string CustomerRoles = "ManageCustomerRoles";
        public const string Actions = "ManageActions";
        public const string Reminders = "ManageReminders";
        public const string Vendors = "ManageVendors";
        public const string VendorReviews = "ManageVendorReviews";
        public const string CurrentCarts = "ManageCurrentCarts";
        public const string Orders = "ManageOrders";
        public const string OrderTags = "ManageOrderTags";
        public const string Shipments = "ManageShipments";
        public const string RecurringPayments = "ManageRecurringPayments";
        public const string GiftCards = "ManageGiftCards";
        public const string ReturnRequests = "ManageReturnRequests";
        public const string Affiliates = "ManageAffiliates";
        public const string PushNotifications = "ManagePushNotifications";
        public const string Campaigns = "ManageCampaigns";
        public const string Banners = "ManageBanners";
        public const string InteractiveForms = "ManageInteractiveForms";
        public const string Discounts = "ManageDiscounts";
        public const string NewsletterSubscribers = "ManageNewsletterSubscribers";
        public const string NewsletterCategories = "ManageNewsletterCategories";
        public const string Polls = "ManagePolls";
        public const string News = "ManageNews";
        public const string Blog = "ManageBlog";
        public const string Widgets = "ManageWidgets";
        public const string Topics = "ManageTopics";
        public const string Forums = "ManageForums";
        public const string Knowledgebase = "ManageKnowledgebase";
        public const string Courses = "ManageCourses";
        public const string MessageTemplates = "ManageMessageTemplates";
        public const string Countries = "ManageCountries";
        public const string Languages = "ManageLanguages";
        public const string Settings = "ManageSettings";
        public const string PaymentMethods = "ManagePaymentMethods";
        public const string ExternalAuthenticationMethods = "ManageExternalAuthenticationMethods";
        public const string TaxSettings = "ManageTaxSettings";
        public const string ShippingSettings = "ManageShippingSettings";
        public const string Currencies = "ManageCurrencies";
        public const string Measures = "ManageMeasures";
        public const string ActivityLog = "ManageActivityLog";
        public const string Acl = "ManageACL";
        public const string EmailAccounts = "ManageEmailAccounts";
        public const string Stores = "ManageStores";
        public const string Plugins = "ManagePlugins";
        public const string SystemLog = "ManageSystemLog";
        public const string MessageQueue = "ManageMessageQueue";
        public const string MessageContactForm = "ManageMessageContactForm";
        public const string Maintenance = "ManageMaintenance";
        public const string Files = "ManageFiles";
        public const string Pictures = "ManagePictures";
        public const string GenericAttributes = "GenericAttributes";
        public const string HtmlEditor = "HtmlEditor.ManagePictures";
        public const string ScheduleTasks = "ManageScheduleTasks";

        //public store permissions
        public const string DisplayPrices = "DisplayPrices";
        public const string EnableShoppingCart = "EnableShoppingCart";
        public const string EnableWishlist = "EnableWishlist";
        public const string PublicStoreAllowNavigation = "PublicStoreAllowNavigation";
        public const string AccessClosedStore = "AccessClosedStore";


        //New Permission Added 
      
        public const string MasterSetup = "AccessMasterSetup";
        public const string AnimalRegistration = "AccessAnimalRegistration";
        public const string AnimalHealth = "AccessHealth";
        public const string AnimalNutrition = "AccessNutrition";
        public const string PerformanceRecording = "AccessPerformanceRecording";

        public const string Breeding = "AccessBreeding";
        public const string User = "User";
        public const string LivestockData = "LivestockData";
        public const string ProductionData = "ProductionData";
        public const string ServiceData = "ServiceData";
        public const string MedicineInventory = "AccessInventory";

        public const string Activity = "AccessActivity";
        public const string Target = "AccessActivityTarget";
        public const string Progress = "AccessActivityProgress";
        public const string ActivityMenu = "AccessActivityMenu";

        public const string AccessLssReport = "AccessLssReport";
        public const string AccessVhlsecReport = "AccessVhlsecReport";
        public const string AccessByNlboOnly = "NlboOnly";
        public const string AccessByDolfdOnly = "DolfdOnly";
        //public const string AgricultureUser = "AgricultureUser";
        //public const string LivestockUser = "LivestockUser";

        public const string AccessDynamicMenu = "AccessDynamicMenu";
        public const string AccessNewsEventTender = "AccessNewsEventTender";
       public const string AccessCDS = "AccessCDS";
       public const string AccessMolmacRegister = "AccessMolmacRegister";
        public const string AccessSubsidyRegister = "AccessSubsidyRegister";
        public const string AccessResourceRegister = "AccessResourceRegister";


        public const string AccessGeneralCMS = "AccessGeneralCMS";
        public const string AccessCropsProduction = "AccessCropsProduction";

        public const string AgriUserAccess = "AgriUserAccess";
        public const string LivestockUserAccess = "LivestockUserAccess";

        public const string OnlyDataEntryAccess = "OnlyDataEntryAccess";


        //New roles for Tokha Agriculture System
        //Dashboard
        public const string AccessDashboard = "AccessDashboard";
        public const string AccessUser = "AccessUser";

        //Organization
        public const string AccessOrganization = "AccessOrganization";
        public const string AccessOtherOrganization = "AccessOtherOrganization";
        public const string AccessServiceProvider = "AccessServiceProvider";

        //Service Provider
        public const string AccessHatchery = "AccessHatchery";
        public const string AccessFeedIndustry = "AccessFeedIndustry";
        public const string AccessFeedShop = "AccessFeedShop";
        public const string AccessMilkCoOperative = "AccessMilkCoOperative";
        public const string AccessDiaryIndustry = "AccessDiaryIndustry";
        public const string AccessDiaryShop = "AccessDiaryShop";
        public const string AccessChillingCenter = "AccessChillingCenter";
        public const string AccessAnimalCollection = "AccessAnimalCollection";
        public const string AccessMarket = "AccessMarket";
        public const string AccessMeatProcessing = "AccessMeatProcessing";
        public const string AccessMeatShop = "AccessMeatShop";
        public const string AccessFishSrot = "AccessFishSrot";
        public const string AccessNgo = "AccessNgo";
        public const string AccessTechSchool = "AccessTechSchool";
        public const string AccessCanelClub = "AccessCanelClub";
        public const string AccessVetClinic = "AccessVetClinic";
        public const string AccessAgricultureCoOperative = "AccessAgricultureCoOperative";
        public const string AccessFertilizerShop = "AccessFertilizerShop";

        //Farm
        public const string AccessFarm = "AccessFarm";

        //AI
        public const string AccessAI = "AccessAI";

        //Medicine
        public const string AccessMedicine = "AccessMedicine";
        public const string AccessReceivedMedicine = "AccessReceivedMedicine";
        public const string AccessMedicineDistribution = "AccessMedicineDistribution";
        public const string AccessReceivedVaccination = "AccessReceivedVaccination";
        public const string AccessDistributedVaccination = "AccessDistributedVaccination";

        //DataEntry
        public const string AccessPujigatKharchaKharyakram = "AccessPujigatKharchaKharyakram";
        public const string AccessMonthlyProgress = "AccessMonthlyProgress";
        public const string AccessFarmer = "AccessFarmer";
        public const string AccessAanudanKaryakram = "AccessAanudanKaryakram";
        public const string AccessAnugaman = "AccessAnugaman";
        public const string AccessInputSupply = "AccessInputSupply";
        public const string AccessDeathVerification = "AccessDeathVerification";
        public const string AccessTreatment = "AccessTreatment";
       // public const string AccessMarket = "AccessMarket";
        public const string AccessResources = "AccessResources";
        public const string AccessTalim = "AccessTalim";
        public const string AccessIncubationCenter = "AccessIncubationCenter";
      //  public const string AccessFarmer = "AccessFarmer";
        public const string AccessSoil = "AccessSoil";
        public const string AccessLivestock = "AccessLivestock";
        public const string AccessProduction = "AccessProduction";
        //public const string AccessCropsProduction = "AccessCropsProduction";
        public const string AccessFishProduction = "AccessFishProduction";
        public const string AccessCropDisease = "AccessCropDisease";
        public const string AccessFertilizerDistribution = "AccessFertilizerDistribution";
        public const string AccessSeedDistribution = "AccessSeedDistribution";


        //Report
        public const string AccessProgressReport = "AccessProgressReport";
        public const string AccessSubsidyReport = "AccessSubsidyReport";
        public const string AccessTrainingReport = "AccessTrainingReport";
        public const string AccessProductionReport = "AccessProductionReport";
        public const string AccessMedicineReport = "AccessMedicineReport";
        public const string AccessAiReport = "AccessAiReport";
        public const string AccessMonthlyProgressReport = "AccessMonthlyProgressReport";
        public const string AccessKirshakReport = "AccessKirshakReport";
        public const string AccessAanudanOrgReport = "AccessAanudanOrgReport";
        public const string AccessMonthlyProgressSummery = "AccessMonthlyProgressSummery";
        //public const string AccessMonthlyProgressSReport = "AccessMonthlyProgressSReport";
        public const string AccessCropProductionReport = "AccessCropProductionReport";
      //  public const string AccessProductionReport = "AccessProductionReport";
        public const string AccessLivestockReport = "AccessLivestockReport";
        public const string AccessAanudanKaryakramReport = "AccessAanudanKaryakramReport";
        public const string AccessDeathVerificationReport = "AccessDeathVerificationReport";
        public const string AccessAgricultureCoOperativeReport = "AccessAgricultureCoOperativeReport";
        public const string AccessFertilizerShopReport = "AccessFertilizerShopReport";


        //Mastersetup
        public const string AccessSpecies = "AccessSpecies";
        public const string AccessBreed = "AccessBreed";
        public const string AccessGrowingSeason = "AccessGrowingSeason";
        public const string AccessLivestockSpecies = "AccessLivestockSpecies";
        public const string AccessCategory = "AccessCategory";
        public const string AccessAnimalType = "AccessAnimalType";
        public const string AccessVaccineType = "AccessVaccineType";
        public const string AccessUnit = "AccessUnit";
        public const string AccessFiscalYear = "AccessFiscalYear";
        public const string AccessDisease = "AccessDisease";
    }
}
