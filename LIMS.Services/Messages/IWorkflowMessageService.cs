using LIMS.Domain.Customers;
using LIMS.Domain.Knowledgebase;
using LIMS.Domain.Messages;
using LIMS.Domain.News;
using LIMS.Domain.Stores;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LIMS.Services.Messages
{
    public partial interface IWorkflowMessageService
    {
        #region Customer workflow

        /// <summary>
        /// Sends 'New customer' notification message to a store owner
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="store">Store</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        Task<int> SendCustomerRegisteredNotificationMessage(Customer customer, Store store, string languageId);

        /// <summary>
        /// Sends a welcome message to a customer
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="store">Store</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        Task<int> SendCustomerWelcomeMessage(Customer customer, Store store, string languageId);

        /// <summary>
        /// Sends an email validation message to a customer
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="store">Store</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        Task<int> SendCustomerEmailValidationMessage(Customer customer, Store store, string languageId);

        /// <summary>
        /// Sends password recovery message to a customer
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="store">Store</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        Task<int> SendCustomerPasswordRecoveryMessage(Customer customer, Store store, string languageId);

        /// <summary>
        /// Sends a new customer note added notification to a customer
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="customerNote">Customer note</param>
        /// <param name="store">Store</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        Task<int> SendNewCustomerNoteAddedCustomerNotification(CustomerNote customerNote, Customer customer, Store store, string languageId);

        /// <summary>
        /// Send an email token validation message to a customer
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="store">Store instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        Task<int> SendCustomerEmailTokenValidationMessage(Customer customer, Store store, string languageId);

        #endregion

        #region Newsletter workflow

        /// <summary>
        /// Sends a newsletter subscription activation message
        /// </summary>
        /// <param name="subscription">Newsletter subscription</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Queued email identifier</returns>
        Task<int> SendNewsLetterSubscriptionActivationMessage(NewsLetterSubscription subscription,
            string languageId);

        /// <summary>
        /// Sends a newsletter subscription deactivation message
        /// </summary>
        /// <param name="subscription">Newsletter subscription</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Queued email identifier</returns>
        Task<int> SendNewsLetterSubscriptionDeactivationMessage(NewsLetterSubscription subscription,
            string languageId);

        #endregion

        #region Send a message to a friend, ask question
        /// <summary>
        /// Sends wishlist "email a friend" message
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="store">Store</param>
        /// <param name="languageId">Message language identifier</param>
        /// <param name="customerEmail">Customer's email</param>
        /// <param name="friendsEmail">Friend's email</param>
        /// <param name="personalMessage">Personal message</param>
        /// <returns>Queued email identifier</returns>
        Task<int> SendWishlistEmailAFriendMessage(Customer customer, Store store, string languageId,
             string customerEmail, string friendsEmail, string personalMessage);
        #endregion

        #region Misc
        /// <summary>
        /// Sends a "new VAT sumitted" notification to a store owner
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="store">Store</param>
        /// <param name="vatName">Received VAT name</param>
        /// <param name="vatAddress">Received VAT address</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        Task<int> SendNewVatSubmittedStoreOwnerNotification(Customer customer, Store store, string vatName, string vatAddress, string languageId);

        /// <summary>
        /// Sends a "customer delete" notification to a store owner
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        Task<int> SendCustomerDeleteStoreOwnerNotification(Customer customer, string languageId);

        /// <summary>
        /// Sends an article comment notification message to a store owner
        /// </summary>
        /// <param name="articleComment">Article comment</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        Task<int> SendArticleCommentNotificationMessage(KnowledgebaseArticle article, KnowledgebaseArticleComment articleComment, string languageId);

        /// <summary>
        /// Sends a news comment notification message to a store owner
        /// </summary>
        /// <param name="newsComment">News comment</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        Task<int> SendNewsCommentNotificationMessage(NewsItem newsItem, NewsComment newsComment, string languageId);

        /// <summary>
        /// Sends "contact us" message
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="store">Store</param>
        /// <param name="languageId">Message language identifier</param>
        /// <param name="senderEmail">Sender email</param>
        /// <param name="senderName">Sender name</param>
        /// <param name="subject">Email subject. Pass null if you want a message template subject to be used.</param>
        /// <param name="body">Email body</param>
        /// <param name="attrInfo">Attr info</param>
        /// <param name="attrXml">Attr xml</param>
        /// <returns>Queued email identifier</returns>
        Task<int> SendContactUsMessage(Customer customer, Store store, string languageId, string senderEmail, string senderName, string subject, string body, string attrInfo, string attrXml);


        /// <summary>
        /// Sends a customer action event 
        /// </summary>
        /// <param name="CustomerAction">Customer action</param>
        /// <param name="languageId">Message language identifier</param>
        /// <param name="customerId">Customer identifier</param>
        /// <returns>Queued email identifier</returns>
        Task<int> SendCustomerActionEvent_Notification(CustomerAction action, string languageId, Customer customer);

        /// <summary>
        /// Send notification
        /// </summary>
        /// <param name="messageTemplate">Message template</param>
        /// <param name="emailAccount">Email account</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="liquidObject">LiquidObject</param>
        /// <param name="tokens">Tokens</param>
        /// <param name="toEmailAddress">Recipient email address</param>
        /// <param name="toName">Recipient name</param>
        /// <param name="attachmentFilePath">Attachment file path</param>
        /// <param name="attachmentFileName">Attachment file name</param>
        /// <param name="attachedDownloads">Attached downloads ident</param>
        /// <param name="replyToEmailAddress">"Reply to" email</param>
        /// <param name="replyToName">"Reply to" name</param>
        /// <param name="fromEmail">Sender email. If specified, then it overrides passed "emailAccount" details</param>
        /// <param name="fromName">Sender name. If specified, then it overrides passed "emailAccount" details</param>
        /// <param name="subject">Subject. If specified, then it overrides subject of a message template</param>
        /// <returns>Queued email identifier</returns>
        Task<int> SendNotification(MessageTemplate messageTemplate,
            EmailAccount emailAccount, string languageId, LiquidObject liquidObject,
            string toEmailAddress, string toName,
            string attachmentFilePath = null, string attachmentFileName = null,
            IEnumerable<string> attachedDownloads = null,
            string replyToEmailAddress = null, string replyToName = null,
            string fromEmail = null, string fromName = null, string subject = null);

        /// <summary>
        /// Sends a test email
        /// </summary>
        /// <param name="messageTemplateId">Message template identifier</param>
        /// <param name="sendToEmail">Send to email</param>
        /// <param name="tokens">Tokens</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        Task<int> SendTestEmail(string messageTemplateId, string sendToEmail, LiquidObject liquidObject, string languageId);

        #endregion
    }
}