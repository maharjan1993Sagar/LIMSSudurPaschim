﻿using LIMS.Domain;
using LIMS.Domain.Data;
using LIMS.Domain.AdminSearch;
using LIMS.Domain.Customers;
using LIMS.Domain.Knowledgebase;
using LIMS.Domain.Localization;
using LIMS.Domain.Logging;
using LIMS.Domain.Messages;
using LIMS.Domain.PushNotifications;
using LIMS.Domain.Security;
using LIMS.Domain.Seo;
using LIMS.Domain.Tasks;
using LIMS.Domain.Topics;
using LIMS.Services.Commands.Models.Security;
using LIMS.Services.Configuration;
using LIMS.Services.Localization;
using LIMS.Services.Security;
using LIMS.Services.Stores;
using LIMS.Services.Topics;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LIMS.Core;

namespace LIMS.Services.Installation
{
    public partial class UpgradeService : IUpgradeService
    {
        #region Fields
        private readonly IServiceProvider _serviceProvider;
        private readonly IMediator _mediator;
        private readonly IRepository<Domain.Common.LIMSVersion> _versionRepository;

        private const string version_400 = "4.00";
        private const string version_410 = "4.10";
        private const string version_420 = "4.20";
        private const string version_430 = "4.30";
        private const string version_440 = "4.40";
        private const string version_450 = "4.50";
        private const string version_460 = "4.60";
        private const string version_470 = "4.70";
        private const string version_480 = "4.80";

        #endregion

        #region Ctor
        public UpgradeService(IServiceProvider serviceProvider,
            IMediator mediator,
            IRepository<Domain.Common.LIMSVersion> versionRepository)
        {
            _serviceProvider = serviceProvider;
            _mediator = mediator;
            _versionRepository = versionRepository;
        }
        #endregion

        public virtual string DatabaseVersion()
        {
            var version = version_400;
            var databaseversion = _versionRepository.Table.FirstOrDefault();
            if (databaseversion != null)
                version = databaseversion.DataBaseVersion;
            return version;
        }
        public virtual async Task UpgradeData(string fromversion, string toversion)
        {
            if (fromversion == version_400)
            {
                await From400To410();
                fromversion = version_410;
            }
            if (fromversion == version_410)
            {
                await From410To420();
                fromversion = version_420;
            }
            if (fromversion == version_420)
            {
                await From420To430();
                fromversion = version_430;
            }
            if (fromversion == version_430)
            {
                await From430To440();
                fromversion = version_440;
            }
            if (fromversion == version_440)
            {
                await From440To450();
                fromversion = version_450;
            }
            if (fromversion == version_450)
            {
                await From450To460();
                fromversion = version_460;
            }
            if (fromversion == version_460)
            {
                await From460To470();
                fromversion = version_470;
            }
            if (fromversion == version_470)
            {
                await From470To480();
                fromversion = version_480;
            }

            if (fromversion == toversion)
            {
                var databaseversion = _versionRepository.Table.FirstOrDefault();
                if (databaseversion != null)
                {
                    databaseversion.DataBaseVersion = Core.LIMSVersion.SupportedDBVersion;
                    await _versionRepository.UpdateAsync(databaseversion);
                }
                else
                {
                    databaseversion = new Domain.Common.LIMSVersion {
                        DataBaseVersion = Core.LIMSVersion.SupportedDBVersion
                    };
                    await _versionRepository.InsertAsync(databaseversion);
                }
            }
        }

        private async Task From400To410()
        {
            #region Install String resources
            await InstallStringResources("EN_400_410.nopres.xml");
            #endregion

            #region Security settings
            var settingService = _serviceProvider.GetRequiredService<ISettingService>();
            var securitySettings = _serviceProvider.GetRequiredService<SecuritySettings>();
            securitySettings.AllowNonAsciiCharInHeaders = true;
            await settingService.SaveSetting(securitySettings, x => x.AllowNonAsciiCharInHeaders, "", false);
            #endregion

            #region MessageTemplates

            var eaGeneral = _serviceProvider.GetRequiredService<IRepository<EmailAccount>>().Table.FirstOrDefault();
            if (eaGeneral == null)
                throw new Exception("Default email account cannot be loaded");
            var messageTemplates = new List<MessageTemplate>
            {
                new MessageTemplate
                        {
                            Name = "AuctionEnded.CustomerNotificationWin",
                            Subject = "%Store.Name%. Auction ended.",
                            Body = "<p>Hello, %Customer.FullName%!</p><p></p><p>At %Auctions.EndTime% you have won <a href=\"%Store.URL%%Auctions.ProductSeName%\">%Auctions.ProductName%</a> for %Auctions.Price%. Visit <a href=\"%Store.URL%/cart\">cart</a> to finish checkout process. </p>",
                            IsActive = true,
                            EmailAccountId = eaGeneral.Id,
                        },
                new MessageTemplate
                        {
                            Name = "AuctionEnded.CustomerNotificationLost",
                            Subject = "%Store.Name%. Auction ended.",
                            Body = "<p>Hello, %Customer.FullName%!</p><p></p><p>Unfortunately you did not win the bid %Auctions.ProductName%</p> <p>End price:  %Auctions.Price% </p> <p>End date auction %Auctions.EndTime% </p>",
                            IsActive = true,
                            EmailAccountId = eaGeneral.Id,
                        },
                new MessageTemplate
                        {
                            Name = "AuctionEnded.CustomerNotificationBin",
                            Subject = "%Store.Name%. Auction ended.",
                            Body = "<p>Hello, %Customer.FullName%!</p><p></p><p>Unfortunately you did not win the bid %Product.Name%</p> <p>Product was bought by option Buy it now for price: %Product.Price% </p>",
                            IsActive = true,
                            EmailAccountId = eaGeneral.Id,
                        },
                new MessageTemplate
                        {
                            Name = "AuctionEnded.StoreOwnerNotification",
                            Subject = "%Store.Name%. Auction ended.",
                            Body = "<p>At %Auctions.EndTime% %Customer.FullName% have won <a href=\"%Store.URL%%Auctions.ProductSeName%\">%Auctions.ProductName%</a> for %Auctions.Price%.</p>",
                            IsActive = true,
                            EmailAccountId = eaGeneral.Id,
                        },
                new MessageTemplate
                        {
                            Name = "BidUp.CustomerNotification",
                            Subject = "%Store.Name%. Your offer has been outbid.",
                            Body = "<p>Hi %Customer.FullName%!</p><p>Your offer for product <a href=\"%Auctions.ProductSeName%\">%Auctions.ProductName%</a> has been outbid. Your price was %Auctions.Price%.<br />Raise a price by raising one's offer. Auction will be ended on %Auctions.EndTime%</p>",
                            IsActive = true,
                            EmailAccountId = eaGeneral.Id,
                        },
            };
            await _serviceProvider.GetRequiredService<IRepository<MessageTemplate>>().InsertAsync(messageTemplates);

            #endregion

            #region Tasks

            var keepliveTask = _serviceProvider.GetRequiredService<IRepository<ScheduleTask>>();

            var endtask = new ScheduleTask {
                ScheduleTaskName = "End of the auctions",
                Type = "LIMS.Services.Tasks.EndAuctionsTask, LIMS.Services",
                Enabled = false,
                StopOnError = false,
                TimeInterval = 1440
            };
            await keepliveTask.InsertAsync(endtask);

            var _keepAliveScheduleTask = keepliveTask.Table.Where(x => x.Type == "LIMS.Services.Tasks.KeepAliveScheduleTask").FirstOrDefault();
            if (_keepAliveScheduleTask != null)
                keepliveTask.Delete(_keepAliveScheduleTask);

            #endregion

            #region Insert activities

            var _activityLogTypeRepository = _serviceProvider.GetRequiredService<IRepository<ActivityLogType>>();
            await _activityLogTypeRepository.InsertAsync(new ActivityLogType() {
                SystemKeyword = "PublicStore.AddNewBid",
                Enabled = false,
                Name = "Public store. Add new bid"
            });
            await _activityLogTypeRepository.InsertAsync(new ActivityLogType() {
                SystemKeyword = "DeleteBid",
                Enabled = false,
                Name = "Delete bid"
            });


            #endregion
        }

        private async Task From410To420()
        {
            var _settingService = _serviceProvider.GetRequiredService<ISettingService>();

            #region Install String resources
            await InstallStringResources("EN_410_420.nopres.xml");
            #endregion

            #region Update string resources

            var _localeStringResource = _serviceProvider.GetRequiredService<IRepository<LocaleStringResource>>();

            await _localeStringResource.Collection.Find(new BsonDocument()).ForEachAsync(async (e) =>
            {
                e.ResourceName = e.ResourceName.ToLowerInvariant();
                await _localeStringResource.UpdateAsync(e);
            });

            #endregion

            #region ActivityLog

            var _activityLogTypeRepository = _serviceProvider.GetRequiredService<IRepository<ActivityLogType>>();
            await _activityLogTypeRepository.InsertAsync(new ActivityLogType() {
                SystemKeyword = "PublicStore.DeleteAccount",
                Enabled = false,
                Name = "Public store. Delete account"
            });
            await _activityLogTypeRepository.InsertAsync(new ActivityLogType {
                SystemKeyword = "UpdateKnowledgebaseCategory",
                Enabled = true,
                Name = "Update knowledgebase category"
            });
            await _activityLogTypeRepository.InsertAsync(new ActivityLogType {
                SystemKeyword = "CreateKnowledgebaseCategory",
                Enabled = true,
                Name = "Create knowledgebase category"
            });
            await _activityLogTypeRepository.InsertAsync(new ActivityLogType {
                SystemKeyword = "DeleteKnowledgebaseCategory",
                Enabled = true,
                Name = "Delete knowledgebase category"
            });
            await _activityLogTypeRepository.InsertAsync(new ActivityLogType {
                SystemKeyword = "CreateKnowledgebaseArticle",
                Enabled = true,
                Name = "Create knowledgebase article"
            });
            await _activityLogTypeRepository.InsertAsync(new ActivityLogType {
                SystemKeyword = "UpdateKnowledgebaseArticle",
                Enabled = true,
                Name = "Update knowledgebase article"
            });
            await _activityLogTypeRepository.InsertAsync(new ActivityLogType {
                SystemKeyword = "DeleteKnowledgebaseArticle",
                Enabled = true,
                Name = "Delete knowledgebase category"
            });
            await _activityLogTypeRepository.InsertAsync(new ActivityLogType {
                SystemKeyword = "AddNewContactAttribute",
                Enabled = true,
                Name = "Add a new contact attribute"
            });
            await _activityLogTypeRepository.InsertAsync(new ActivityLogType {
                SystemKeyword = "EditContactAttribute",
                Enabled = true,
                Name = "Edit a contact attribute"
            });
            await _activityLogTypeRepository.InsertAsync(new ActivityLogType {
                SystemKeyword = "DeleteContactAttribute",
                Enabled = true,
                Name = "Delete a contact attribute"
            });

            #endregion

            #region MessageTemplates

            var emailAccount = _serviceProvider.GetRequiredService<IRepository<EmailAccount>>().Table.FirstOrDefault();
            if (emailAccount == null)
                throw new Exception("Default email account cannot be loaded");
            var messageTemplates = new List<MessageTemplate>
            {
                new MessageTemplate
                {
                    Name = "CustomerDelete.StoreOwnerNotification",
                    Subject = "%Store.Name%. Customer has been deleted.",
                    Body = "<p><a href=\"%Store.URL%\">%Store.Name%</a> ,<br />%Customer.FullName% (%Customer.Email%) has just deleted from your database. </p>",
                    IsActive = true,
                    EmailAccountId = emailAccount.Id,
                },
            };
            await _serviceProvider.GetRequiredService<IRepository<MessageTemplate>>().InsertAsync(messageTemplates);
            #endregion

            #region Install new Topics
            var defaultTopicTemplate = _serviceProvider.GetRequiredService<IRepository<TopicTemplate>>().Table.FirstOrDefault(tt => tt.Name == "Default template");
            if (defaultTopicTemplate == null)
                defaultTopicTemplate = _serviceProvider.GetRequiredService<IRepository<TopicTemplate>>().Table.FirstOrDefault();

            var knowledgebaseHomepageTopic = new Topic {
                SystemName = "KnowledgebaseHomePage",
                IncludeInSitemap = false,
                IsPasswordProtected = false,
                DisplayOrder = 1,
                Title = "",
                Body = "<p>Knowledgebase homepage. You can edit this in the admin site.</p>",
                TopicTemplateId = defaultTopicTemplate.Id
            };

            var topicService = _serviceProvider.GetRequiredService<ITopicService>();
            await topicService.InsertTopic(knowledgebaseHomepageTopic);
            #endregion

            #region Permisions

            IPermissionProvider provider = new StandardPermissionProvider();
            await _mediator.Send(new InstallPermissionsCommand() { PermissionProvider = provider });

            #endregion

            #region Knowledge settings

            var knowledgesettings = _serviceProvider.GetRequiredService<KnowledgebaseSettings>();
            knowledgesettings.Enabled = false;
            knowledgesettings.AllowNotRegisteredUsersToLeaveComments = true;
            knowledgesettings.NotifyAboutNewArticleComments = false;
            await _settingService.SaveSetting(knowledgesettings);

            #endregion

            #region Push notifications settings

            var pushNotificationSettings = _serviceProvider.GetRequiredService<PushNotificationsSettings>();
            pushNotificationSettings.Enabled = false;
            pushNotificationSettings.AllowGuestNotifications = true;
            await _settingService.SaveSetting(pushNotificationSettings);

            #endregion

            #region Knowledge table

            await _serviceProvider.GetRequiredService<IRepository<KnowledgebaseArticle>>().Collection.Indexes.CreateOneAsync(new CreateIndexModel<KnowledgebaseArticle>((Builders<KnowledgebaseArticle>.IndexKeys.Ascending(x => x.DisplayOrder)), new CreateIndexOptions() { Name = "DisplayOrder", Unique = false }));
            await _serviceProvider.GetRequiredService<IRepository<KnowledgebaseCategory>>().Collection.Indexes.CreateOneAsync(new CreateIndexModel<KnowledgebaseCategory>((Builders<KnowledgebaseCategory>.IndexKeys.Ascending(x => x.DisplayOrder)), new CreateIndexOptions() { Name = "DisplayOrder", Unique = false }));

            #endregion
        }

        private async Task From420To430()
        {
            var _settingService = _serviceProvider.GetRequiredService<ISettingService>();

            await InstallStringResources("EN_420_430.nopres.xml");

            #region Settings

            var adminSearchSettings = _serviceProvider.GetRequiredService<AdminSearchSettings>();
            adminSearchSettings.BlogsDisplayOrder = 0;
            adminSearchSettings.CategoriesDisplayOrder = 0;
            adminSearchSettings.CustomersDisplayOrder = 0;
            adminSearchSettings.ManufacturersDisplayOrder = 0;
            adminSearchSettings.MaxSearchResultsCount = 10;
            adminSearchSettings.MinSearchTermLength = 3;
            adminSearchSettings.NewsDisplayOrder = 0;
            adminSearchSettings.OrdersDisplayOrder = 0;
            adminSearchSettings.ProductsDisplayOrder = 0;
            adminSearchSettings.SearchInBlogs = true;
            adminSearchSettings.SearchInCategories = true;
            adminSearchSettings.SearchInCustomers = true;
            adminSearchSettings.SearchInManufacturers = true;
            adminSearchSettings.SearchInNews = true;
            adminSearchSettings.SearchInOrders = true;
            adminSearchSettings.SearchInProducts = true;
            adminSearchSettings.SearchInTopics = true;
            adminSearchSettings.TopicsDisplayOrder = 0;
            adminSearchSettings.SearchInMenu = true;
            adminSearchSettings.MenuDisplayOrder = -1;
            await _settingService.SaveSetting(adminSearchSettings);

            var customerSettings = _serviceProvider.GetRequiredService<CustomerSettings>();
            customerSettings.HideNotesTab = true;
            await _settingService.SaveSetting(customerSettings);

            #endregion

            #region Emails

            var emailAccount = _serviceProvider.GetRequiredService<IRepository<EmailAccount>>().Table.FirstOrDefault();
            if (emailAccount == null)
                throw new Exception("Default email account cannot be loaded");
            var messageTemplates = new List<MessageTemplate>
            {
                new MessageTemplate
                {
                    Name = "Knowledgebase.ArticleComment",
                    Subject = "%Store.Name%. New article comment.",
                    Body = "<p><a href=\"%Store.URL%\">%Store.Name%</a> <br /><br />A new article comment has been created for article \"%Article.ArticleTitle%\".</p>",
                    IsActive = true,
                    EmailAccountId = emailAccount.Id,
                },
                new MessageTemplate
                {
                    Name = "Customer.NewCustomerNote",
                    Subject = "New customer note has been added",
                    Body = "<p><br />Hello %Customer.FullName%, <br />New customer note has been added to your account:<br />\"%Customer.NewTitleText%\".<br /></p>",
                    IsActive = true,
                    EmailAccountId = emailAccount.Id,
                },
            };
            await _serviceProvider.GetRequiredService<IRepository<MessageTemplate>>().InsertAsync(messageTemplates);

            #endregion

            #region Activity log
            var _activityLogTypeRepository = _serviceProvider.GetRequiredService<IRepository<ActivityLogType>>();
            await _activityLogTypeRepository.InsertAsync(new ActivityLogType {
                SystemKeyword = "PublicStore.AddArticleComment",
                Enabled = false,
                Name = "Public store. Add article comment"
            });
            #endregion

            #region Knowledgebase settings

            var knowledgebaseSettings = _serviceProvider.GetRequiredService<KnowledgebaseSettings>();
            knowledgebaseSettings.AllowNotRegisteredUsersToLeaveComments = true;
            knowledgebaseSettings.NotifyAboutNewArticleComments = false;
            await _settingService.SaveSetting(knowledgebaseSettings);
            #endregion

            #region Customer Personalize Product

            var _customerProductRepository = _serviceProvider.GetRequiredService<IRepository<CustomerProduct>>();
            await _customerProductRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<CustomerProduct>((Builders<CustomerProduct>.IndexKeys.Ascending(x => x.CustomerId).Ascending(x => x.DisplayOrder)), new CreateIndexOptions() { Name = "CustomerProduct", Unique = false }));
            await _customerProductRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<CustomerProduct>((Builders<CustomerProduct>.IndexKeys.Ascending(x => x.CustomerId).Ascending(x => x.ProductId)), new CreateIndexOptions() { Name = "CustomerProduct_Unique", Unique = true }));

            #endregion

            #region Update customer

            var dbContext = _serviceProvider.GetRequiredService<IMongoDatabase>();
            var customerRepository = _serviceProvider.GetRequiredService<IRepository<Customer>>();

            var builderCustomer = Builders<Customer>.Filter;
            var filterCustomer = builderCustomer.Eq("IsNewsItem", true) | builderCustomer.Eq("IsHasOrders", true) | builderCustomer.Eq("IsHasBlogComments", true) | builderCustomer.Eq("IsHasArticleComments", true)
                 | builderCustomer.Eq("IsHasProductReview", true) | builderCustomer.Eq("IsHasProductReviewH", true) | builderCustomer.Eq("IsHasVendorReview", true)
                 | builderCustomer.Eq("IsHasVendorReviewH", true) | builderCustomer.Eq("IsHasPoolVoting", true) | builderCustomer.Eq("IsHasForumPost", true) | builderCustomer.Eq("IsHasForumTopic", true);

            var updateCustomer = Builders<Customer>.Update
               .Set(x => x.HasContributions, true);

            await customerRepository.Collection.UpdateOneAsync(filterCustomer, updateCustomer);

            var removeFields = Builders<object>.Update
               .Unset("IsNewsItem")
               .Unset("IsHasOrders")
               .Unset("IsHasBlogComments")
               .Unset("IsHasArticleComments")
               .Unset("IsHasProductReview")
               .Unset("IsHasProductReviewH")
               .Unset("IsHasVendorReview")
               .Unset("IsHasVendorReviewH")
               .Unset("IsHasPoolVoting")
               .Unset("IsHasForumPost")
               .Unset("IsHasForumTopic")
               .Unset("HasShoppingCartItems");

            await dbContext.GetCollection<object>(typeof(Customer).Name).UpdateManyAsync(new BsonDocument(), removeFields);


            #endregion
            
            #region Customer note

            //customer note
            await _serviceProvider.GetRequiredService<IRepository<CustomerNote>>().Collection.Indexes.CreateOneAsync(new CreateIndexModel<CustomerNote>((Builders<CustomerNote>.IndexKeys.Ascending(x => x.CustomerId).Descending(x => x.CreatedOnUtc)), new CreateIndexOptions() { Name = "CustomerId", Unique = false, Background = true }));

            #endregion
        }

        private async Task From430To440()
        {
            #region Install String resources
            await InstallStringResources("430_440.nopres.xml");
            #endregion

            #region Permisions
            IPermissionProvider provider = new StandardPermissionProvider();
            await _mediator.Send(new InstallPermissionsCommand() { PermissionProvider = provider });
            #endregion

            #region User api

            _serviceProvider.GetRequiredService<IRepository<UserApi>>().Collection.Indexes.CreateOne(new CreateIndexModel<UserApi>((Builders<UserApi>.IndexKeys.Ascending(x => x.Email)), new CreateIndexOptions() { Name = "Email", Unique = true, Background = true }));

            #endregion

            #region Update message templates (tokens)

            string ReplaceValue(string value)
            {
                string Evaluator(Match match)
                {
                    return $"{{{{{match.Value.Replace("%", "")}}}}}";
                }
                var evaluator = new MatchEvaluator(Evaluator);
                return Regex.Replace(value, @"%([A-Za-z0-9_.]*?)%", new MatchEvaluator(Evaluator));
            }

            var orderProducts = File.ReadAllText(CommonHelper.MapPath("~/App_Data/Upgrade/Order.Products.txt"));
            var shipmentProducts = File.ReadAllText(CommonHelper.MapPath("~/App_Data/Upgrade/Shipment.Products.txt"));

            var messagetemplateService = _serviceProvider.GetRequiredService<LIMS.Services.Messages.IMessageTemplateService>();
            var messagetemplates = await messagetemplateService.GetAllMessageTemplates(string.Empty);
            foreach (var messagetemplate in messagetemplates)
            {
                messagetemplate.Subject = ReplaceValue(messagetemplate.Subject);
                if (messagetemplate.Body.Contains("%Order.Product(s)%"))
                {
                    messagetemplate.Body = messagetemplate.Body.Replace("%Order.Product(s)%", orderProducts);
                }
                if (messagetemplate.Body.Contains("%Shipment.Product(s)%"))
                {
                    messagetemplate.Body = messagetemplate.Body.Replace("%Shipment.Product(s)%", shipmentProducts);
                }
                messagetemplate.Body = ReplaceValue(messagetemplate.Body);
                await messagetemplateService.UpdateMessageTemplate(messagetemplate);
            }
            #endregion

            #region Insert message template

            var eaGeneral = _serviceProvider.GetRequiredService<IRepository<EmailAccount>>().Table.FirstOrDefault();
            if (eaGeneral == null)
                throw new Exception("Default email account cannot be loaded");
            var messageTemplates = new List<MessageTemplate>
            {
                new MessageTemplate
                {
                    Name = "AuctionExpired.StoreOwnerNotification",
                    Subject = "Your auction to product {{Product.Name}}  has expired.",
                    Body = "Hello, <br> Your auction to product {{Product.Name}} has expired without bid.",
                    //this template is disabled by default
                    IsActive = false,
                    EmailAccountId = eaGeneral.Id,
                }
            };
            await _serviceProvider.GetRequiredService<IRepository<MessageTemplate>>().InsertAsync(messageTemplates);
            #endregion

        }

        private async Task From440To450()
        {
            #region Install String resources
            await InstallStringResources("EN_440_450.nopres.xml");
            #endregion

            #region Update task
            var tasks = _serviceProvider.GetRequiredService<IRepository<ScheduleTask>>();
            foreach (var task in tasks.Table)
            {
                if (task.TimeInterval == 0)
                {
                    task.TimeInterval = 1440;
                    await tasks.UpdateAsync(task);
                }
                if (task.Type == "LIMS.Services.Tasks.ClearLogScheduleTask")
                {
                    task.Type = "LIMS.Services.Tasks.ClearLogScheduleTask, LIMS.Services";
                    await tasks.UpdateAsync(task);
                }
            }
            #endregion

            #region Update topics - rename fields

            var renameFields = Builders<object>.Update
                .Rename("IncludeInFooterColumn1", "IncludeInFooterRow1")
                .Rename("IncludeInFooterColumn2", "IncludeInFooterRow2")
                .Rename("IncludeInFooterColumn3", "IncludeInFooterRow3");

            var dbContext = _serviceProvider.GetRequiredService<IMongoDatabase>();
            await dbContext.GetCollection<object>(typeof(Topic).Name).UpdateManyAsync(new BsonDocument(), renameFields);

            #endregion

            #region Insert new system customer role - staff

            var crStaff = new CustomerRole {
                Name = "Staff",
                Active = true,
                IsSystemRole = true,
                SystemName = SystemCustomerRoleNames.Staff,
            };
            await _serviceProvider.GetRequiredService<IRepository<CustomerRole>>().InsertAsync(crStaff);

            #endregion

            #region Permisions

            IPermissionProvider provider = new StandardPermissionProvider();
            await _mediator.Send(new InstallNewPermissionsCommand() { PermissionProvider = provider });

            #endregion
        }
        private async Task From450To460()
        {

            #region Install String resources

            await InstallStringResources("EN_450_460.nopres.xml");

            #endregion

            #region Add new customer action - paid order

            var customerActionType = _serviceProvider.GetRequiredService<IRepository<CustomerActionType>>();
            await customerActionType.InsertAsync(
            new CustomerActionType() {
                Name = "Paid order",
                SystemKeyword = "PaidOrder",
                Enabled = false,
                ConditionType = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 13 }
            });

            #endregion

            #region Permisions

            IPermissionProvider provider = new StandardPermissionProvider();
            await _mediator.Send(new InstallNewPermissionsCommand() { PermissionProvider = provider });
            #endregion

            #region Activity Log Type

            var _activityLogTypeRepository = _serviceProvider.GetRequiredService<IRepository<ActivityLogType>>();
            await _activityLogTypeRepository.InsertAsync(
                new ActivityLogType {
                    SystemKeyword = "AddNewDocument",
                    Enabled = false,
                    Name = "Add a new document"
                });
            await _activityLogTypeRepository.InsertAsync(
                new ActivityLogType {
                    SystemKeyword = "AddNewDocumentType",
                    Enabled = false,
                    Name = "Add a new document type"
                });
            await _activityLogTypeRepository.InsertAsync(
                new ActivityLogType {
                    SystemKeyword = "EditDocument",
                    Enabled = false,
                    Name = "Edit document"
                });
            await _activityLogTypeRepository.InsertAsync(
                new ActivityLogType {
                    SystemKeyword = "EditDocumentType",
                    Enabled = false,
                    Name = "Edit document type"
                });
            await _activityLogTypeRepository.InsertAsync(
                new ActivityLogType {
                    SystemKeyword = "DeleteDocument",
                    Enabled = false,
                    Name = "Delete document"
                });
            await _activityLogTypeRepository.InsertAsync(
                new ActivityLogType {
                    SystemKeyword = "DeleteDocumentType",
                    Enabled = false,
                    Name = "Delete document type"
                });
            await _activityLogTypeRepository.InsertAsync(
                new ActivityLogType {
                    SystemKeyword = "PublicStore.ViewCourse",
                    Enabled = false,
                    Name = "Public store. View a course"
                });
            await _activityLogTypeRepository.InsertAsync(
                new ActivityLogType {
                    SystemKeyword = "PublicStore.ViewLesson",
                    Enabled = false,
                    Name = "Public store. View a lesson"
                });
            #endregion

            #region Update customer settings

            var _settingService = _serviceProvider.GetRequiredService<ISettingService>();
            var customerSettings = _serviceProvider.GetRequiredService<CustomerSettings>();
            customerSettings.HideDocumentsTab = true;
            customerSettings.HideReviewsTab = false;
            customerSettings.HideCoursesTab = true;
            await _settingService.SaveSetting(customerSettings);

            #endregion

            #region Update topics

            IRepository<Topic> _topicRepository = _serviceProvider.GetRequiredService<IRepository<Topic>>();
            foreach (var topic in _topicRepository.Table)
            {
                topic.Published = true;
                _topicRepository.Update(topic);
            }

            #endregion

            #region Update url seo to lowercase

            IRepository<UrlRecord> _urlRecordRepository = _serviceProvider.GetRequiredService<IRepository<UrlRecord>>();
            foreach (var urlrecord in _urlRecordRepository.Table)
            {
                urlrecord.Slug = urlrecord.Slug.ToLowerInvariant();
                _urlRecordRepository.Update(urlrecord);
            }

            #endregion
        }

        private async Task From460To470()
        {
            #region Install String resources
            await InstallStringResources("EN_460_470.nopres.xml");
            #endregion

            #region MessageTemplates

            var emailAccount = _serviceProvider.GetRequiredService<IRepository<EmailAccount>>().Table.FirstOrDefault();
            if (emailAccount == null)
                throw new Exception("Default email account cannot be loaded");
            var messageTemplates = new List<MessageTemplate>
            {
                new MessageTemplate
                {
                    Name = "Customer.EmailTokenValidationMessage",
                    Subject = "{{Store.Name}} - Email Verification Code",
                    Body = "Hello {{Customer.FullName}}, <br /><br />\r\n Enter this 6 digit code on the sign in page to confirm your identity:<br /><br /> \r\n <b>{{Customer.Token}}</b><br /><br />\r\n Yours securely, <br /> \r\n Team",
                    IsActive = true,
                    EmailAccountId = emailAccount.Id,
                },
                new MessageTemplate
                {
                    Name = "OrderCancelled.VendorNotification",
                    Subject = "{{Store.Name}}. Order #{{Order.OrderNumber}} cancelled",
                    Body = "<p><a href=\"{{Store.URL}}\">{{Store.Name}}</a> <br /><br />Order #{{Order.OrderNumber}} has been cancelled. <br /><br />Order Number: {{Order.OrderNumber}}<br />   Date Ordered: {{Order.CreatedOn}} <br /><br /> ",
                    IsActive = false,
                    EmailAccountId = emailAccount.Id,
                },

            };

            await _serviceProvider.GetRequiredService<IRepository<MessageTemplate>>().InsertAsync(messageTemplates);
            #endregion

            #region Update store

            var storeService = _serviceProvider.GetRequiredService<IStoreService>();
            foreach (var store in await storeService.GetAllStores())
            {
                store.Shortcut = "Store";
                await storeService.UpdateStore(store);
            }

            #endregion

            #region Update media settings

            var settingsService = _serviceProvider.GetRequiredService<ISettingService>();
            var storeInDB = settingsService.GetSettingByKey("Media.Images.StoreInDB", true);
            await settingsService.SetSetting("MediaSettings.StoreInDb", storeInDB);

            #endregion
        }

        private async Task From470To480()
        {
            #region Install String resources

            await InstallStringResources("EN_470_480.nopres.xml");

            #endregion

            #region Update customer settings

            var _settingService = _serviceProvider.GetRequiredService<ISettingService>();
            var customerSettings = _serviceProvider.GetRequiredService<CustomerSettings>();
            customerSettings.HideSubAccountsTab = true;
            await _settingService.SaveSetting(customerSettings);

            #endregion

            #region Update permissions - Actions

            IPermissionProvider provider = new StandardPermissionProvider();
            //install new permissions
            await _mediator.Send(new InstallNewPermissionsCommand() { PermissionProvider = provider });

            var permissions = provider.GetPermissions();
            var permissionService = _serviceProvider.GetRequiredService<IPermissionService>();
            foreach (var permission in permissions)
            {
                var p = await permissionService.GetPermissionRecordBySystemName(permission.SystemName);
                if (p != null)
                {
                    p.Actions = permission.Actions;
                    await permissionService.UpdatePermissionRecord(p);
                }
            }

            #endregion

            #region Update cancel order Scheduled Task

            var tasks = _serviceProvider.GetRequiredService<IRepository<ScheduleTask>>();
            var cancelOrderTask = new ScheduleTask {
                ScheduleTaskName = "Cancel unpaid and pending orders",
                Type = "LIMS.Services.Tasks.CancelOrderScheduledTask, LIMS.Services",
                Enabled = false,
                StopOnError = false,
                TimeInterval = 1440
            };
            await tasks.InsertAsync(cancelOrderTask);

            #endregion
        }

        private async Task InstallStringResources(string filenames)
        {
            //'English' language            
            var langRepository = _serviceProvider.GetRequiredService<IRepository<Language>>();
            var language = langRepository.Table.FirstOrDefault(l => l.Name == "English");

            if (language == null)
                language = langRepository.Table.FirstOrDefault();

            //save resources
            foreach (var filePath in System.IO.Directory.EnumerateFiles(CommonHelper.MapPath("~/App_Data/Localization/Upgrade"), "*" + filenames, SearchOption.TopDirectoryOnly))
            {
                var localesXml = File.ReadAllText(filePath);
                var localizationService = _serviceProvider.GetRequiredService<ILocalizationService>();
                await localizationService.ImportResourcesFromXmlInstall(language, localesXml);
            }
        }

        /// <summary>
        /// Used to convert from 4.20 to 4.30
        /// </summary>
        class OldReturnRequest : BaseEntity
        {
            public int ReturnNumber { get; set; }
            public string StoreId { get; set; }
            public string OrderId { get; set; }
            public string OrderItemId { get; set; }
            public string CustomerId { get; set; }
            public int Quantity { get; set; }
            public string ReasonForReturn { get; set; }
            public string RequestedAction { get; set; }
            public string CustomerComments { get; set; }
            public string StaffNotes { get; set; }
            public int ReturnRequestStatusId { get; set; }
            public DateTime CreatedOnUtc { get; set; }
            public DateTime UpdatedOnUtc { get; set; }
        }
    }
}