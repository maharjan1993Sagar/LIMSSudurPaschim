using LIMS.Domain.Customers;
using LIMS.Domain.Localization;
using System.Threading.Tasks;

namespace LIMS.Services.Authentication
{
    public interface ISMSVerificationService
    {
        Task<bool> Authenticate(string secretKey, string token, Customer customer);
        Task<TwoFactorCodeSetup> GenerateCode(string secretKey, Customer customer, Language language);
    }
}
