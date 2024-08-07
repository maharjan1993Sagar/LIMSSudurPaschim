﻿using LIMS.Api.DTOs.Common;
using LIMS.Api.Queries.Models.Common;
using LIMS.Domain.Data;
using MediatR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LIMS.Api.Queries.Handlers.Common
{
    public class GetCountryQueryHandler : IRequestHandler<GetQuery<CountryDto>, IMongoQueryable<CountryDto>>
    {
        private readonly IMongoDBContext _mongoDBContext;

        public GetCountryQueryHandler(IMongoDBContext mongoDBContext)
        {
            _mongoDBContext = mongoDBContext;
        }
        public Task<IMongoQueryable<CountryDto>> Handle(GetQuery<CountryDto> request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Id))
                return Task.FromResult(_mongoDBContext.Database().GetCollection<CountryDto>(typeof(Domain.Directory.Country).Name).AsQueryable());
            else
                return Task.FromResult(_mongoDBContext.Database().GetCollection<CountryDto>(typeof(Domain.Directory.Country).Name).AsQueryable().Where(x => x.Id == request.Id));
        }
    }
}
