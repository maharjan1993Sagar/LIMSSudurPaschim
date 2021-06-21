using LIMS.Api.DTOs.Common;
using LIMS.Api.Queries.Models.Common;
using LIMS.Domain.Data;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LIMS.Api.Queries.Handlers.Common
{
    class GetFiscalYearQueryHandler
    {
        private readonly IMongoDBContext _mongoDBContext;

        public GetFiscalYearQueryHandler(IMongoDBContext mongoDBContext)
        {
            _mongoDBContext = mongoDBContext;
        }
        public Task<IMongoQueryable<FiscalYearDto>> Handle(GetQuery<FiscalYearDto> request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Id))
                return Task.FromResult(
                    _mongoDBContext.Database()
                    .GetCollection<FiscalYearDto>
                    (typeof(Domain.BesicSetup.FiscalYear).Name)
                    .AsQueryable());
            else
                return Task.FromResult(_mongoDBContext.Database()
                    .GetCollection<FiscalYearDto>(typeof(Domain.BesicSetup.FiscalYear).Name)
                    .AsQueryable()
                    .Where(x => x.Id == request.Id ));
        }
    }
}
