
namespace LIMS.Core.Caching.Message
{
    public interface IMessageEventClient : IMessageEvent
    {
        string ClientId { get; set; }
    }
}
