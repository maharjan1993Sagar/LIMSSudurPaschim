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
    public class GetBreedQueryHandler: IRequestHandler<GetQuery<BreedDto>, IMongoQueryable<BreedDto>>
    {
        private readonly IMongoDBContext _mongoDBContext;

        public GetBreedQueryHandler(IMongoDBContext mongoDBContext)
        {
            _mongoDBContext = mongoDBContext;
        }
        public Task<IMongoQueryable<BreedDto>> Handle(GetQuery<BreedDto> request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Id))
                return Task.FromResult(
                    _mongoDBContext.Database()
                    .GetCollection<BreedDto>
                    (typeof(Domain.Breed.BreedReg).Name)
                    .AsQueryable());
            else
                return Task.FromResult(_mongoDBContext.Database()
                    .GetCollection<BreedDto>(typeof(Domain.Breed.BreedReg).Name)
                    .AsQueryable()
                    .Where(x => x.Id == request.Id));
        }
    }
}

