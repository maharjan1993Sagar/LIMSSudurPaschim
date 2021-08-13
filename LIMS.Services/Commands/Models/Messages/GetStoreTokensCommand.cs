using Grand.Services.Messages.DotLiquidDrops;
using LIMS.Domain.Localization;
using LIMS.Domain.Messages;
using LIMS.Domain.Stores;
using MediatR;

namespace Grand.Services.Commands.Models.Messages
{
    public class GetStoreTokensCommand : IRequest<LiquidStore>
    {
        public Store Store { get; set; }
        public Language Language { get; set; }
        public EmailAccount EmailAccount { get; set; }
    }
}
