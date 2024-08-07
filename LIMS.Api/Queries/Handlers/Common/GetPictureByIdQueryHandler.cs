﻿using LIMS.Api.DTOs.Common;
using LIMS.Api.Queries.Models.Common;
using LIMS.Domain.Data;
using MediatR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LIMS.Api.Queries.Handlers.Common
{
    public class GetPictureByIdQueryHandler : IRequestHandler<GetPictureByIdQuery, PictureDto>
    {
        private readonly IMongoDBContext _mongoDBContext;

        public GetPictureByIdQueryHandler(IMongoDBContext mongoDBContext)
        {
            _mongoDBContext = mongoDBContext;
        }

        public async Task<PictureDto> Handle(GetPictureByIdQuery request, CancellationToken cancellationToken)
        {
            return await _mongoDBContext.Database()
                .GetCollection<PictureDto>(typeof(Domain.Media.Picture).Name)
                .AsQueryable()
                .FirstOrDefaultAsync(x => x.Id == request.Id);
        }
    }
}
