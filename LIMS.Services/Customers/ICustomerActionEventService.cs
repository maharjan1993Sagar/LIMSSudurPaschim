using LIMS.Domain.Customers;
using System.Threading.Tasks;

namespace LIMS.Services.Customers
{
    public partial interface ICustomerActionEventService
    {
        /// <summary>
        /// Viewed
        /// </summary>
        Task Viewed(Customer customer, string currentUrl, string previousUrl);

        /// <summary>
        /// Run action url
        /// </summary>
        Task Url(Customer customer, string currentUrl, string previousUrl);


        /// <summary>
        /// Run action url
        /// </summary>
        Task Registration(Customer customer);
    }
}
