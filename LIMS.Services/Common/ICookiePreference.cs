using LIMS.Domain.Customers;
using LIMS.Domain.Stores;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LIMS.Services.Common
{
    public interface ICookiePreference
    {
        IList<IConsentCookie> GetConsentCookies();
        Task<bool?> IsEnable(Customer customer, Store store, string cookieSystemName);
    }
}
