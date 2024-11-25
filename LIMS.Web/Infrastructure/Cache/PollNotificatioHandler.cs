﻿using LIMS.Core.Caching;
using LIMS.Domain.Polls;
using LIMS.Core.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace LIMS.Web.Infrastructure.Cache
{
    public class PollNotificatioHandler :
        INotificationHandler<EntityInserted<Poll>>,
        INotificationHandler<EntityUpdated<Poll>>,
        INotificationHandler<EntityDeleted<Poll>>
    {

        private readonly ICacheManager _cacheManager;

        public PollNotificatioHandler(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public async Task Handle(EntityInserted<Poll> eventMessage, CancellationToken cancellationToken)
        {
            await _cacheManager.RemoveByPrefix(ModelCacheEventConst.POLLS_PATTERN_KEY);
        }
        public async Task Handle(EntityUpdated<Poll> eventMessage, CancellationToken cancellationToken)
        {
            await _cacheManager.RemoveByPrefix(ModelCacheEventConst.POLLS_PATTERN_KEY);
        }
        public async Task Handle(EntityDeleted<Poll> eventMessage, CancellationToken cancellationToken)
        {
            await _cacheManager.RemoveByPrefix(ModelCacheEventConst.POLLS_PATTERN_KEY);
        }
    }
}