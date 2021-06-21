using System.Threading.Tasks;

namespace LIMS.Services.Common
{
    public interface IConsentCookie
    {
        string SystemName { get; }
        bool AllowToDisable { get; }
        bool? DefaultState { get; }
        int DisplayOrder { get; }
        Task<string> Name();
        Task<string> FullDescription();
    }
}
