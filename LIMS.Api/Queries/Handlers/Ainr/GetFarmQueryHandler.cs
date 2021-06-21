using LIMS.Api.DTOs.AINR;
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
   public class GetFarmQueryHandler : IRequestHandler<GetQuery<FarmDto>, IMongoQueryable<FarmDto>>
    {
        private readonly IMongoDBContext _mongoDBContext;
        public GetFarmQueryHandler(IMongoDBContext mongoDBContext)
        {
            _mongoDBContext = mongoDBContext;
        }
        public Task<IMongoQueryable<FarmDto>> Handle(GetQuery<FarmDto> request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Id))
                return Task.FromResult(
                    _mongoDBContext.Database()
                    .GetCollection<FarmDto>
                    (typeof(Domain.AInR.Farm).Name)
                    .AsQueryable());
            else
                return Task.FromResult(_mongoDBContext.Database()
                    .GetCollection<FarmDto>(typeof(Domain.AInR.Farm).Name)
                    .AsQueryable()
                    .Where(x => x.Id == request.Id));
        }
    }
}
