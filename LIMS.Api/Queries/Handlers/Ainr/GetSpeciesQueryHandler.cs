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
   public class GetSpeciesQueryHandler : IRequestHandler<GetQuery<SpeciesDto>, IMongoQueryable<SpeciesDto>>
    {
        private readonly IMongoDBContext _mongoDBContext;

        public GetSpeciesQueryHandler(IMongoDBContext mongoDBContext)
        {
            _mongoDBContext = mongoDBContext;
        }
        public Task<IMongoQueryable<SpeciesDto>> Handle(GetQuery<SpeciesDto> request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Id))
                return Task.FromResult(
                    _mongoDBContext.Database()
                    .GetCollection<SpeciesDto>
                    (typeof(Domain.Breed.Species).Name)
                    .AsQueryable());
            else
                return Task.FromResult(_mongoDBContext.Database()
                    .GetCollection<SpeciesDto>(typeof(Domain.Breed.Species).Name)
                    .AsQueryable()
                    .Where(x => x.Id == request.Id));
        }

    }
}
