using MediatR;
using Microsoft.AspNetCore.Http;

namespace LIMS.Web.Features.Models.Common
{
    public class GetParseCustomAddressAttributes : IRequest<string>
    {
        public IFormCollection Form { get; set; }
    }
}
