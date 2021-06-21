﻿using LIMS.Domain.Data;
using LIMS.Domain.Logging;
using LIMS.Services.Commands.Models.Common;
using MediatR;
using MongoDB.Bson;
using System.Threading;
using System.Threading.Tasks;

namespace LIMS.Services.Commands.Handlers.Common
{
    public class DeleteActivitylogCommandHandler : IRequestHandler<DeleteActivitylogCommand, bool>
    {
        private readonly IRepository<ActivityLog> _repositoryActivityLog;

        public DeleteActivitylogCommandHandler(IRepository<ActivityLog> repositoryActivityLog)
        {
            _repositoryActivityLog = repositoryActivityLog;
        }

        public async Task<bool> Handle(DeleteActivitylogCommand request, CancellationToken cancellationToken)
        {
            await _repositoryActivityLog.Collection.DeleteManyAsync(new BsonDocument());
            return true;
        }
    }
}
