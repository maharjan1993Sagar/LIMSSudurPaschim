using LIMS.Api.DTOs.Common;
using MediatR;
using MongoDB.Driver.Linq;

namespace LIMS.Api.Queries.Models.Common
{
    public class GetMessageTemplateQuery : IRequest<IMongoQueryable<MessageTemplateDto>>
    {
        public string Id { get; set; }
        public string TemplateName { get; set; }
    }
}
