using System.Threading.Tasks;

namespace LIMS.Services.Customers
{
    /// <summary>
    /// Customer report service interface
    /// </summary>
    public partial interface ICustomerReportService
    {
        /// Gets a report of customers registered in the last days
        /// <param name="storeId">Store ident</param>
        /// <param name="days">Customers registered in the last days</param>
        /// <returns>Number of registered customers</returns>
        Task<int> GetRegisteredCustomersReport(string storeId, int days);
    }
}