using Autofac;
using LIMS.Core.Configuration;
using LIMS.Core.Data;
using LIMS.Core.Infrastructure;
using LIMS.Core.Infrastructure.DependencyManagement;
using LIMS.Services.Authentication;
using LIMS.Services.Authentication.External;

using LIMS.Services.Common;
using LIMS.Services.Configuration;
using LIMS.Services.Customers;
using LIMS.Services.Directory;
using LIMS.Services.Documents;
using LIMS.Services.ExportImport;
using LIMS.Services.Helpers;
using LIMS.Services.Installation;
using LIMS.Services.Knowledgebase;
using LIMS.Services.Localization;
using LIMS.Services.Logging;
using LIMS.Services.MachineNameProvider;
using LIMS.Services.Media;
using LIMS.Services.Messages;
using LIMS.Services.News;

using LIMS.Services.Polls;
using LIMS.Services.PushNotifications;
using LIMS.Services.Security;
using LIMS.Services.Seo;

using LIMS.Services.Stores;
using LIMS.Services.Tasks;
using LIMS.Services.Topics;
using Microsoft.AspNetCore.StaticFiles;
using System;

namespace LIMS.Services.Infrastructure
{
    /// <summary>
    /// Dependency registrar
    /// </summary>
    public class DependencyRegistrar : IDependencyRegistrar
    {
        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="builder">Container builder</param>
        /// <param name="typeFinder">Type finder</param>
        /// <param name="config">Config</param>
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, LIMSConfig config)
        {
            RegisterMachineNameProvider(builder, config);

            RegisterConfigurationService(builder);

            RegisterAuthenticationService(builder);

            RegisterCommonService(builder);

            RegisterCustomerService(builder);

            RegisterDirectoryService(builder);

            RegisterDocumentsService(builder);

            RegisterExportImportService(builder);

            RegisterInstallService(builder);

            RegisterKnowledgebaseService(builder);

            RegisterLocalizationService(builder);

            RegisterLoggingService(builder);

            RegisterMediaService(builder, config);

            RegisterMessageService(builder);

            RegisterNewsService(builder);

            RegisterPollsService(builder);

            RegisterPushService(builder);

            RegisterSecurityService(builder);

            RegisterSeoService(builder);

            RegisterStoresService(builder);

            RegisterTopicsService(builder);

            RegisterTask(builder);

        }

        /// <summary>
        /// Gets order of this dependency registrar implementation
        /// </summary>
        public int Order {
            get { return 1; }
        }

        private void RegisterMachineNameProvider(ContainerBuilder builder, LIMSConfig config)
        {
            if (config.RunOnAzureWebApps)
            {
                builder.RegisterType<AzureWebAppsMachineNameProvider>().As<IMachineNameProvider>().SingleInstance();
            }
            else
            {
                builder.RegisterType<DefaultMachineNameProvider>().As<IMachineNameProvider>().SingleInstance();
            }
        }

        private void RegisterAuthenticationService(ContainerBuilder builder)
        {
            builder.RegisterType<CookieAuthenticationService>().As<ILIMSAuthenticationService>().InstancePerLifetimeScope();
            builder.RegisterType<ApiAuthenticationService>().As<IApiAuthenticationService>().InstancePerLifetimeScope();
            builder.RegisterType<TwoFactorAuthenticationService>().As<ITwoFactorAuthenticationService>().InstancePerLifetimeScope();
            builder.RegisterType<ExternalAuthenticationService>().As<IExternalAuthenticationService>().InstancePerLifetimeScope();
        }
        private void RegisterCommonService(ContainerBuilder builder)
        {
            builder.RegisterType<AddressAttributeFormatter>().As<IAddressAttributeFormatter>().InstancePerLifetimeScope();
            builder.RegisterType<AddressAttributeParser>().As<IAddressAttributeParser>().InstancePerLifetimeScope();
            builder.RegisterType<AddressAttributeService>().As<IAddressAttributeService>().InstancePerLifetimeScope();
            builder.RegisterType<AddressService>().As<IAddressService>().InstancePerLifetimeScope();
            builder.RegisterType<GenericAttributeService>().As<IGenericAttributeService>().InstancePerLifetimeScope();
            builder.RegisterType<HistoryService>().As<IHistoryService>().InstancePerLifetimeScope();
            builder.RegisterType<WkPdfService>().As<IPdfService>().InstancePerLifetimeScope();
            builder.RegisterType<ViewRenderService>().As<IViewRenderService>().InstancePerLifetimeScope();
            builder.RegisterType<SearchTermService>().As<ISearchTermService>().InstancePerLifetimeScope();
            builder.RegisterType<DateTimeHelper>().As<IDateTimeHelper>().InstancePerLifetimeScope();
            builder.RegisterType<CookiePreference>().As<ICookiePreference>().InstancePerLifetimeScope();
        }

        private void RegisterCustomerService(ContainerBuilder builder)
        {
            builder.RegisterType<CustomerAttributeFormatter>().As<ICustomerAttributeFormatter>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerAttributeParser>().As<ICustomerAttributeParser>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerAttributeService>().As<ICustomerAttributeService>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerService>().As<ICustomerService>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerRegistrationService>().As<ICustomerRegistrationService>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerReportService>().As<ICustomerReportService>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerTagService>().As<ICustomerTagService>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerActionService>().As<ICustomerActionService>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerActionEventService>().As<ICustomerActionEventService>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerReminderService>().As<ICustomerReminderService>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerProductService>().As<ICustomerProductService>().InstancePerLifetimeScope();
            builder.RegisterType<UserApiService>().As<IUserApiService>().InstancePerLifetimeScope();

        }
        private void RegisterDirectoryService(ContainerBuilder builder)
        {
            builder.RegisterType<GeoLookupService>().As<IGeoLookupService>().InstancePerLifetimeScope();
            builder.RegisterType<CountryService>().As<ICountryService>().InstancePerLifetimeScope();

            builder.RegisterType<CurrencyService>().As<ICurrencyService>().InstancePerLifetimeScope();
            builder.RegisterType<MeasureService>().As<IMeasureService>().InstancePerLifetimeScope();
            builder.RegisterType<StateProvinceService>().As<IStateProvinceService>().InstancePerLifetimeScope();

        }
        private void RegisterDocumentsService(ContainerBuilder builder)
        {
            builder.RegisterType<DocumentTypeService>().As<IDocumentTypeService>().InstancePerLifetimeScope();
            builder.RegisterType<DocumentService>().As<IDocumentService>().InstancePerLifetimeScope();

        }

        private void RegisterConfigurationService(ContainerBuilder builder)
        {
            builder.RegisterType<SettingService>().As<ISettingService>().InstancePerLifetimeScope();
            builder.RegisterType<GoogleAnalyticsService>().As<IGoogleAnalyticsService>().InstancePerLifetimeScope();
        }

        private void RegisterExportImportService(ContainerBuilder builder)
        {
            builder.RegisterType<ExportManager>().As<IExportManager>().InstancePerLifetimeScope();
            builder.RegisterType<ImportManager>().As<IImportManager>().InstancePerLifetimeScope();
        }

        private void RegisterInstallService(ContainerBuilder builder)
        {
            var databaseInstalled = DataSettingsHelper.DatabaseIsInstalled();
            if (!databaseInstalled)
            {
                //installation service
                builder.RegisterType<InstallationLocalizationService>().As<IInstallationLocalizationService>().InstancePerLifetimeScope();
                builder.RegisterType<CodeFirstInstallationService>().As<IInstallationService>().InstancePerLifetimeScope();
            }
            else
            {
                builder.RegisterType<UpgradeService>().As<IUpgradeService>().InstancePerLifetimeScope();
            }
        }
        private void RegisterKnowledgebaseService(ContainerBuilder builder)
        {
            builder.RegisterType<KnowledgebaseService>().As<IKnowledgebaseService>().InstancePerLifetimeScope();
        }

        private void RegisterLocalizationService(ContainerBuilder builder)
        {
            builder.RegisterType<LocalizationService>().As<ILocalizationService>().InstancePerLifetimeScope();
            builder.RegisterType<LanguageService>().As<ILanguageService>().InstancePerLifetimeScope();
        }

        private void RegisterLoggingService(ContainerBuilder builder)
        {
            builder.RegisterType<CustomerActivityService>().As<ICustomerActivityService>().InstancePerLifetimeScope();
            builder.RegisterType<ActivityKeywordsProvider>().As<IActivityKeywordsProvider>().InstancePerLifetimeScope();
            builder.RegisterType<DefaultLogger>().As<ILogger>().InstancePerLifetimeScope();
        }

        private void RegisterMessageService(ContainerBuilder builder)
        {
            builder.RegisterType<BannerService>().As<IBannerService>().InstancePerLifetimeScope();
            builder.RegisterType<PopupService>().As<IPopupService>().InstancePerLifetimeScope();
            builder.RegisterType<InteractiveFormService>().As<IInteractiveFormService>().InstancePerLifetimeScope();
            builder.RegisterType<NewsLetterSubscriptionService>().As<INewsLetterSubscriptionService>().InstancePerLifetimeScope();
            builder.RegisterType<NewsletterCategoryService>().As<INewsletterCategoryService>().InstancePerLifetimeScope();
            builder.RegisterType<MessageTemplateService>().As<IMessageTemplateService>().InstancePerLifetimeScope();
            builder.RegisterType<QueuedEmailService>().As<IQueuedEmailService>().InstancePerLifetimeScope();
            builder.RegisterType<EmailAccountService>().As<IEmailAccountService>().InstancePerLifetimeScope();
            builder.RegisterType<Tokenizer>().As<ITokenizer>().InstancePerLifetimeScope();
            builder.RegisterType<EmailSender>().As<IEmailSender>().InstancePerLifetimeScope();

            builder.RegisterType<ContactAttributeFormatter>().As<IContactAttributeFormatter>().InstancePerLifetimeScope();
            builder.RegisterType<ContactAttributeParser>().As<IContactAttributeParser>().InstancePerLifetimeScope();
            builder.RegisterType<ContactAttributeService>().As<IContactAttributeService>().InstancePerLifetimeScope();

            builder.RegisterType<ContactUsService>().As<IContactUsService>().InstancePerLifetimeScope();

        }

        private void RegisterNewsService(ContainerBuilder builder)
        {
            builder.RegisterType<NewsService>().As<INewsService>().InstancePerLifetimeScope();
        }

        private void RegisterPollsService(ContainerBuilder builder)
        {
            builder.RegisterType<PollService>().As<IPollService>().InstancePerLifetimeScope();
        }

        private void RegisterPushService(ContainerBuilder builder)
        {
            builder.RegisterType<PushNotificationsService>().As<IPushNotificationsService>().InstancePerLifetimeScope();
        }

        private void RegisterSecurityService(ContainerBuilder builder)
        {
            builder.RegisterType<PermissionService>().As<IPermissionService>().InstancePerLifetimeScope();
            builder.RegisterType<AclService>().As<IAclService>().InstancePerLifetimeScope();
            builder.RegisterType<EncryptionService>().As<IEncryptionService>().InstancePerLifetimeScope();
        }

        private void RegisterSeoService(ContainerBuilder builder)
        {
            builder.RegisterType<SitemapGenerator>().As<ISitemapGenerator>().InstancePerLifetimeScope();
            builder.RegisterType<UrlRecordService>().As<IUrlRecordService>().InstancePerLifetimeScope();
        }

        private void RegisterStoresService(ContainerBuilder builder)
        {
            builder.RegisterType<StoreService>().As<IStoreService>().InstancePerLifetimeScope();
            builder.RegisterType<StoreMappingService>().As<IStoreMappingService>().InstancePerLifetimeScope();
        }

        private void RegisterTopicsService(ContainerBuilder builder)
        {
            builder.RegisterType<TopicTemplateService>().As<ITopicTemplateService>().InstancePerLifetimeScope();
            builder.RegisterType<TopicService>().As<ITopicService>().InstancePerLifetimeScope();
        }

        private void RegisterMediaService(ContainerBuilder builder, LIMSConfig config)
        {
            var provider = new FileExtensionContentTypeProvider();
            builder.RegisterType<MimeMappingService>().As<IMimeMappingService>()
                .WithParameter(new TypedParameter(typeof(FileExtensionContentTypeProvider), provider))
                .InstancePerLifetimeScope();

            //picture service
            var useAzureBlobStorage = !String.IsNullOrEmpty(config.AzureBlobStorageConnectionString);
            var useAmazonBlobStorage = (!String.IsNullOrEmpty(config.AmazonAwsAccessKeyId) && !String.IsNullOrEmpty(config.AmazonAwsSecretAccessKey) && !String.IsNullOrEmpty(config.AmazonBucketName) && !String.IsNullOrEmpty(config.AmazonRegion));

            if (useAzureBlobStorage)
            {
                //Windows Azure BLOB
                builder.RegisterType<AzurePictureService>().As<IPictureService>().InstancePerLifetimeScope();
            }
            else if (useAmazonBlobStorage)
            {
                //Amazon S3 Simple Storage Service
                builder.RegisterType<AmazonPictureService>().As<IPictureService>().InstancePerLifetimeScope();
            }
            else
            {
                //standard file system
                builder.RegisterType<PictureService>().As<IPictureService>().InstancePerLifetimeScope();
            }

            builder.RegisterType<DownloadService>().As<IDownloadService>().InstancePerLifetimeScope();
        }

        private void RegisterTask(ContainerBuilder builder)
        {
            builder.RegisterType<ScheduleTaskService>().As<IScheduleTaskService>().InstancePerLifetimeScope();

            builder.RegisterType<QueuedMessagesSendScheduleTask>().As<IScheduleTask>().InstancePerLifetimeScope();
            builder.RegisterType<ClearLogScheduleTask>().As<IScheduleTask>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerReminderBirthdayScheduleTask>().As<IScheduleTask>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerReminderLastActivityScheduleTask>().As<IScheduleTask>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerReminderRegisteredCustomerScheduleTask>().As<IScheduleTask>().InstancePerLifetimeScope();
            builder.RegisterType<DeleteGuestsScheduleTask>().As<IScheduleTask>().InstancePerLifetimeScope();
            builder.RegisterType<UpdateExchangeRateScheduleTask>().As<IScheduleTask>().InstancePerLifetimeScope();
        }
    }
}
