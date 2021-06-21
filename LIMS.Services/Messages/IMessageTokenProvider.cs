using LIMS.Domain.Customers;
using LIMS.Domain.Knowledgebase;
using LIMS.Domain.Localization;
using LIMS.Domain.Messages;
using LIMS.Domain.News;
using LIMS.Domain.Stores;
using System.Threading.Tasks;

namespace LIMS.Services.Messages
{
    public partial interface IMessageTokenProvider
    {
        Task AddStoreTokens(LiquidObject liquidObject, Store store, Language language, EmailAccount emailAccount);
        Task AddCustomerTokens(LiquidObject liquidObject, Customer customer, Store store, Language language, CustomerNote customerNote = null);
        Task AddShoppingCartTokens(LiquidObject liquidObject, Customer customer, Store store, Language language, string personalMessage = "", string customerEmail = "");
        Task AddNewsLetterSubscriptionTokens(LiquidObject liquidObject, NewsLetterSubscription subscription, Store store);
        Task AddArticleCommentTokens(LiquidObject liquidObject, KnowledgebaseArticle article, KnowledgebaseArticleComment articleComment, Store store, Language language);
        Task AddNewsCommentTokens(LiquidObject liquidObject, NewsItem newsItem, NewsComment newsComment, Store store, Language language);
        string[] GetListOfCampaignAllowedTokens();
        string[] GetListOfAllowedTokens();
        string[] GetListOfCustomerReminderAllowedTokens(CustomerReminderRuleEnum rule);
    }
}