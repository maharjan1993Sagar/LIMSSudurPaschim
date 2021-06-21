using System.Threading.Tasks;

namespace LIMS.Core.Caching.Message
{
    public interface IMessageSubscriber
    {
        Task SubscribeAsync();
    }
}
