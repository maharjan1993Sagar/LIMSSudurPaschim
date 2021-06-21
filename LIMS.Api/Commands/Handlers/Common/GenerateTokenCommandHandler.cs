using LIMS.Api.Commands.Models.Common;
using LIMS.Api.Infrastructure.Extensions;
using LIMS.Api.Jwt;
using LIMS.Core.Configuration;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace LIMS.Api.Commands.Handlers.Common
{
    public class GenerateTokenCommandHandler : IRequestHandler<GenerateTokenCommand, string>
    {
        private readonly ApiConfig _apiConfig;

        public GenerateTokenCommandHandler(ApiConfig apiConfig)
        {
            _apiConfig = apiConfig;
        }
        public async Task<string> Handle(GenerateTokenCommand request, CancellationToken cancellationToken)
        {
            var token = new JwtTokenBuilder();
            token.AddSecurityKey(JwtSecurityKey.Create(_apiConfig.SecretKey));

            if (_apiConfig.ValidateIssuer)
                token.AddIssuer(_apiConfig.ValidIssuer);
            if (_apiConfig.ValidateAudience)
                token.AddAudience(_apiConfig.ValidAudience);

            token.AddClaims(request.Claims);
            token.AddExpiry(_apiConfig.ExpiryInMinutes);
            token.Build();

            return await Task.FromResult(token.Build().Value);
        }
    }
}
