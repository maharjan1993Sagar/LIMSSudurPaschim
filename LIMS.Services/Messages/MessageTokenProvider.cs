using Grand.Services.Commands.Models.Messages;
using Grand.Services.Messages.DotLiquidDrops;
using LIMS.Domain.Customers;
using LIMS.Domain.Localization;
using LIMS.Domain.Messages;
using LIMS.Domain.Stores;
using LIMS.Services.Events.Extensions;
using LIMS.Services.Messages;
using MediatR;
using System.Threading.Tasks;

namespace Grand.Services.Messages
{
    public partial class MessageTokenProvider : IMessageTokenProvider
    {
        #region Fields

        private readonly IMediator _mediator;

        #endregion

        #region Ctor

        public MessageTokenProvider(IMediator mediator)
        {
            _mediator = mediator;
        }

        #endregion


        #region Methods

        /// <summary>
        /// Gets list of allowed (supported) message tokens for campaigns
        /// </summary>
        /// <returns>List of allowed (supported) message tokens for campaigns</returns>

        public virtual string[] GetListOfAllowedTokens()
        {
            var allowedTokens = LiquidExtensions.GetTokens(

                typeof(LiquidCustomer),
                typeof(LiquidEmailAFriend),

                typeof(LiquidKnowledgebase));
              
            return allowedTokens.ToArray();
        }


        public async Task AddCustomerTokens(LiquidObject liquidObject, Customer customer, Store store, Language language, CustomerNote customerNote = null)
        {
            var liquidCustomer = new LiquidCustomer(customer, store, customerNote);
            liquidObject.Customer = liquidCustomer;

            await _mediator.EntityTokensAdded(customer, liquidCustomer, liquidObject);
            if (customerNote != null)
                await _mediator.EntityTokensAdded(customerNote, liquidCustomer, liquidObject);
        }

        public async Task AddStoreTokens(LiquidObject liquidObject, Store store, Language language, EmailAccount emailAccount)
        {
            var liquidStore = await _mediator.Send(new GetStoreTokensCommand() { Store = store, Language = language, EmailAccount = emailAccount });
            liquidObject.Store = liquidStore;
            await _mediator.EntityTokensAdded(store, liquidStore, liquidObject);
        }





















        #endregion
    }
}