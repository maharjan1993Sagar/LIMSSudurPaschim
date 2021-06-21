using System.Threading.Tasks;

namespace LIMS.Core.Caching.Message
{
    public interface IMessagePublisher
    {
        Task PublishAsync<TMessage>(TMessage msg) where TMessage : IMessageEvent;
    }
}
