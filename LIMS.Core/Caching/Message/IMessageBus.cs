﻿using System.Threading.Tasks;

namespace LIMS.Core.Caching.Message
{
    public interface IMessageBus : IMessagePublisher, IMessageSubscriber
    {
        Task OnSubscriptionChanged(IMessageEvent message);
    }
}
