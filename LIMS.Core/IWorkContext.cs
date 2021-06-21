using LIMS.Domain.Customers;
using LIMS.Domain.Directory;
using LIMS.Domain.Localization;
using System.Threading.Tasks;

namespace LIMS.Core
{
    /// <summary>
    /// Work context
    /// </summary>
    public interface IWorkContext
    {
        /// <summary>
        /// Gets the current customer
        /// </summary>
        Customer CurrentCustomer { get; }

        /// <summary>
        /// Set the current customer by Middleware
        /// </summary>
        /// <returns></returns>
        Task<Customer> SetCurrentCustomer();

        /// <summary>
        /// Gets or sets the original customer (in case the current one is impersonated)
        /// </summary>
        Customer OriginalCustomerIfImpersonated { get; }

        /// <summary>
        /// Get or set current user working language
        /// </summary>
        Language WorkingLanguage { get; }

        /// <summary>
        /// Set current user working language by Middleware
        /// </summary>
        Task<Language> SetWorkingLanguage(Customer customer);

        /// <summary>
        /// Set current user working language
        /// </summary>
        Task<Language> SetWorkingLanguage(Language language);

        /// <summary>
        /// Get or set current user working currency
        /// </summary>
        Currency WorkingCurrency { get; }

        /// <summary>
        /// Set current user working currency by Middleware
        /// </summary>
        Task<Currency> SetWorkingCurrency(Customer customer);

        /// <summary>
        /// Set user working currency
        /// </summary>
        Task<Currency> SetWorkingCurrency(Currency currency);

    }
}
