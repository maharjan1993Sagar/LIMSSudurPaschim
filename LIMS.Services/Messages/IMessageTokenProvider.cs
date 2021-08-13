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
        Task AddCustomerTokens(LiquidObject liquidObject, Customer customer, Store store, Language language, CustomerNote customerNote = null);
        string[] GetListOfAllowedTokens();
        Task AddStoreTokens(LiquidObject liquidObject, Store store, Language language, EmailAccount emailAccount);

    }
}