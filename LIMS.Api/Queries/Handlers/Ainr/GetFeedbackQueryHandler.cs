using LIMS.Api.DTOs.FeedBack;
using LIMS.Api.Queries.Models.Common;
using LIMS.Domain.Data;
using MediatR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LIMS.Api.Queries.Handlers.Ainr
{
    public class GetFeedbackQueryHandler: IRequestHandler<GetQuery<FeedbackDto>, IMongoQueryable<FeedbackDto>>
    {
        private readonly IMongoDBContext _mongoDBContext;

        public GetFeedbackQueryHandler(IMongoDBContext mongoDBContext)
        {
            _mongoDBContext = mongoDBContext;
        }
        public Task<IMongoQueryable<FeedbackDto>> Handle(GetQuery<FeedbackDto> request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Id))
                return Task.FromResult(
                    _mongoDBContext.Database()
                    .GetCollection<FeedbackDto>
                    (typeof(Domain.Feedback.Feedback).Name)
                    .AsQueryable());
            else
                return Task.FromResult(_mongoDBContext.Database()
                    .GetCollection<FeedbackDto>(typeof(Domain.Feedback.Feedback).Name)
                    .AsQueryable()
                    .Where(x => x.Id == request.Id));
        }

    }
}
