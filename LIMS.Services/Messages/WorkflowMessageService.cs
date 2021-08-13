using Grand.Services.Messages.DotLiquidDrops;
using LIMS.Core;
using LIMS.Domain.Common;
using LIMS.Domain.Customers;
using LIMS.Domain.Knowledgebase;
using LIMS.Domain.Localization;
using LIMS.Domain.Messages;
using LIMS.Domain.News;
using LIMS.Domain.Stores;
using LIMS.Services.Commands.Models.Common;
using LIMS.Services.Common;
using LIMS.Services.Customers;
using LIMS.Services.Events.Extensions;
using LIMS.Services.Localization;
using LIMS.Services.Queries.Models.Customers;
using LIMS.Services.Stores;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LIMS.Services.Messages
{
    public partial class WorkflowMessageService : IWorkflowMessageService
    {
        #region Fields

        private readonly IMessageTemplateService _messageTemplateService;
        private readonly IQueuedEmailService _queuedEmailService;
        private readonly ILanguageService _languageService;
        private readonly IEmailAccountService _emailAccountService;
        private readonly IMessageTokenProvider _messageTokenProvider;
        private readonly IStoreService _storeService;
        private readonly IMediator _mediator;

        private readonly EmailAccountSettings _emailAccountSettings;
        private readonly CommonSettings _commonSettings;

        #endregion

        #region Ctor

        public WorkflowMessageService(IMessageTemplateService messageTemplateService,
            IQueuedEmailService queuedEmailService,
            ILanguageService languageService,
            IEmailAccountService emailAccountService,
            IStoreService storeService,
            IMediator mediator,
            EmailAccountSettings emailAccountSettings,
            CommonSettings commonSettings,
            IMessageTokenProvider messageTokenProvider)
        {
            _messageTemplateService = messageTemplateService;
            _queuedEmailService = queuedEmailService;
            _languageService = languageService;
            _emailAccountService = emailAccountService;
            _storeService = storeService;
            _emailAccountSettings = emailAccountSettings;
            _commonSettings = commonSettings;
            _mediator = mediator;
            _messageTokenProvider = messageTokenProvider;
        }

        #endregion

        #region Utilities

        protected virtual async Task<Store> GetStore(string storeId)
        {
            return await _storeService.GetStoreById(storeId) ?? (await _storeService.GetAllStores()).FirstOrDefault();
        }

        protected virtual async Task<MessageTemplate> GetMessageTemplate(string messageTemplateName, string storeId)
        {
            var messageTemplate = await _messageTemplateService.GetMessageTemplateByName(messageTemplateName, storeId);

            //no template found
            if (messageTemplate == null)
                return null;

            //ensure it's active
            var isActive = messageTemplate.IsActive;
            if (!isActive)
                return null;

            return messageTemplate;
        }

        protected virtual async Task<EmailAccount> GetEmailAccountOfMessageTemplate(MessageTemplate messageTemplate, string languageId)
        {
            var emailAccounId = messageTemplate.GetLocalized(mt => mt.EmailAccountId, languageId);
            var emailAccount = await _emailAccountService.GetEmailAccountById(emailAccounId);
            if (emailAccount == null)
                emailAccount = await _emailAccountService.GetEmailAccountById(_emailAccountSettings.DefaultEmailAccountId);
            if (emailAccount == null)
                emailAccount = (await _emailAccountService.GetAllEmailAccounts()).FirstOrDefault();
            return emailAccount;

        }

        protected virtual async Task<Language> EnsureLanguageIsActive(string languageId, string storeId)
        {
            //load language by specified ID
            var language = await _languageService.GetLanguageById(languageId);

            if (language == null || !language.Published)
            {
                //load any language from the specified store
                language = (await _languageService.GetAllLanguages(storeId: storeId)).FirstOrDefault();
            }
            if (language == null || !language.Published)
            {
                //load any language
                language = (await _languageService.GetAllLanguages()).FirstOrDefault();
            }

            if (language == null)
                throw new Exception("No active language could be loaded");
            return language;
        }

        #endregion

        #region Methods

        #region Customer workflow

        /// <summary>
        /// Send a message to a customer
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="store">Store</param>
        /// <param name="languageId">Message language identifier</param>
        /// <param name="templateName">Message template name</param>
        /// <param name="toEmailAccount">Send email to email account</param>
        /// <param name="customerNote">Customer note</param>
        /// <returns>Queued email identifier</returns>
        protected virtual async Task<int> SendCustomerMessage(Customer customer, Store store, string languageId, string templateName, bool toEmailAccount = false, CustomerNote customerNote = null)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            var language = await EnsureLanguageIsActive(languageId, store.Id);

            var messageTemplate = await GetMessageTemplate(templateName, store.Id);
            if (messageTemplate == null)
                return 0;

            //email account
            var emailAccount = await GetEmailAccountOfMessageTemplate(messageTemplate, language.Id);

            LiquidObject liquidObject = new LiquidObject();
            await _messageTokenProvider.AddStoreTokens(liquidObject, store, language, emailAccount);
            await _messageTokenProvider.AddCustomerTokens(liquidObject, customer, store, language, customerNote);

            //event notification
            await _mediator.MessageTokensAdded(messageTemplate, liquidObject);

            var toEmail = toEmailAccount ? emailAccount.Email : customer.Email;
            var toName = toEmailAccount ? emailAccount.DisplayName : customer.GetFullName();
            return await SendNotification(messageTemplate, emailAccount,
                languageId, liquidObject,
                toEmail, toName);
        }

        /// <summary>
        /// Sends 'New customer' notification message to a store owner
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="store">Store identifier</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual async Task<int> SendCustomerRegisteredNotificationMessage(Customer customer, Store store, string languageId)
        {
            return await SendCustomerMessage(customer, store, languageId, MessageTemplateNames.CustomerRegistered, true);
        }

        /// <summary>
        /// Sends a welcome message to a customer
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="store">Store</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual async Task<int> SendCustomerWelcomeMessage(Customer customer, Store store, string languageId)
        {
            return await SendCustomerMessage(customer, store, languageId, MessageTemplateNames.CustomerWelcome);
        }

        /// <summary>
        /// Sends an email validation message to a customer
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="store">Store</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual async Task<int> SendCustomerEmailValidationMessage(Customer customer, Store store, string languageId)
        {
            return await SendCustomerMessage(customer, store, languageId, MessageTemplateNames.CustomerEmailValidation);
        }

        /// <summary>
        /// Sends password recovery message to a customer
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="store">Store</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual async Task<int> SendCustomerPasswordRecoveryMessage(Customer customer, Store store, string languageId)
        {
            return await SendCustomerMessage(customer, store, languageId, MessageTemplateNames.CustomerPasswordRecovery);
        }

        /// <summary>
        /// Sends a new customer note added notification to a customer
        /// </summary>
        /// <param name="customerNote">Customer note</param>
        /// <param name="customer">Customer</param>
        /// <param name="store">Store</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual async Task<int> SendNewCustomerNoteAddedCustomerNotification(CustomerNote customerNote, Customer customer, Store store, string languageId)
        {
            return await SendCustomerMessage(customer, store, languageId, MessageTemplateNames.CustomerNewCustomerNote, customerNote: customerNote);
        }

        /// <summary>
        /// Send an email token validation message to a customer
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="store">Store instance</param>
        /// <param name="token">Token</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual async Task<int> SendCustomerEmailTokenValidationMessage(Customer customer, Store store, string languageId)
        {
            return await SendCustomerMessage(customer, store, languageId, MessageTemplateNames.CustomerEmailTokenValidationMessage);
        }

        #endregion


        #region sendemail

        public virtual async Task<int> SendNotification(MessageTemplate messageTemplate,
           EmailAccount emailAccount, string languageId, LiquidObject liquidObject,
           string toEmailAddress, string toName,
           string attachmentFilePath = null, string attachmentFileName = null,
           IEnumerable<string> attachedDownloads = null,
           string replyToEmailAddress = null, string replyToName = null,
           string fromEmail = null, string fromName = null, string subject = null)
        {
            if (String.IsNullOrEmpty(toEmailAddress))
                return 0;

            //retrieve localized message template data
            var bcc = messageTemplate.GetLocalized(mt => mt.BccEmailAddresses, languageId);

            if (String.IsNullOrEmpty(subject))
                subject = messageTemplate.GetLocalized(mt => mt.Subject, languageId);

            var body = messageTemplate.GetLocalized(mt => mt.Body, languageId);
            var subjectReplaced = LiquidExtensions.Render(liquidObject, subject);
            var bodyReplaced = LiquidExtensions.Render(liquidObject, body);


            var attachments = new List<string>();
            if (attachedDownloads != null)
                attachments.AddRange(attachedDownloads);
            if (!string.IsNullOrEmpty(messageTemplate.AttachedDownloadId))
                attachments.Add(messageTemplate.AttachedDownloadId);

            //limit name length
            toName = CommonHelper.EnsureMaximumLength(toName, 300);
            var email = new QueuedEmail {
                Priority = QueuedEmailPriority.High,
                From = !string.IsNullOrEmpty(fromEmail) ? fromEmail : emailAccount.Email,
                FromName = !string.IsNullOrEmpty(fromName) ? fromName : emailAccount.DisplayName,
                To = toEmailAddress,
                ToName = toName,
                ReplyTo = replyToEmailAddress,
                ReplyToName = replyToName,
                CC = string.Empty,
                Bcc = bcc,
                Subject = subjectReplaced,
                Body = bodyReplaced,
                AttachmentFilePath = attachmentFilePath,
                AttachmentFileName = attachmentFileName,
                AttachedDownloads = attachments,
                CreatedOnUtc = DateTime.UtcNow,
                EmailAccountId = emailAccount.Id,
                DontSendBeforeDateUtc = !messageTemplate.DelayBeforeSend.HasValue ? null
                     : (DateTime?)(DateTime.UtcNow + TimeSpan.FromHours(messageTemplate.DelayPeriod.ToHours(messageTemplate.DelayBeforeSend.Value)))
            };

            await _queuedEmailService.InsertQueuedEmail(email);
            return 1;
        }

        public Task<int> SendNewsLetterSubscriptionActivationMessage(NewsLetterSubscription subscription, string languageId)
        {
            throw new NotImplementedException();
        }

        public Task<int> SendNewsLetterSubscriptionDeactivationMessage(NewsLetterSubscription subscription, string languageId)
        {
            throw new NotImplementedException();
        }

        public Task<int> SendWishlistEmailAFriendMessage(Customer customer, Store store, string languageId, string customerEmail, string friendsEmail, string personalMessage)
        {
            throw new NotImplementedException();
        }

        public Task<int> SendNewVatSubmittedStoreOwnerNotification(Customer customer, Store store, string vatName, string vatAddress, string languageId)
        {
            throw new NotImplementedException();
        }

        public Task<int> SendCustomerDeleteStoreOwnerNotification(Customer customer, string languageId)
        {
            throw new NotImplementedException();
        }

        public Task<int> SendArticleCommentNotificationMessage(KnowledgebaseArticle article, KnowledgebaseArticleComment articleComment, string languageId)
        {
            throw new NotImplementedException();
        }

        public Task<int> SendNewsCommentNotificationMessage(NewsItem newsItem, NewsComment newsComment, string languageId)
        {
            throw new NotImplementedException();
        }

        public Task<int> SendContactUsMessage(Customer customer, Store store, string languageId, string senderEmail, string senderName, string subject, string body, string attrInfo, string attrXml)
        {
            throw new NotImplementedException();
        }

        public Task<int> SendCustomerActionEvent_Notification(CustomerAction action, string languageId, Customer customer)
        {
            throw new NotImplementedException();
        }

        public Task<int> SendTestEmail(string messageTemplateId, string sendToEmail, LiquidObject liquidObject, string languageId)
        {
            throw new NotImplementedException();
        }



        #endregion

    }
}
#endregion