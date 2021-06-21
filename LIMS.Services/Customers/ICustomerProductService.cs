using LIMS.Domain;
using LIMS.Domain.Customers;
using System.Threading.Tasks;

namespace LIMS.Services.Customers
{
    /// <summary>
    /// Customer product service interface
    /// </summary>
    public partial interface ICustomerProductService
    {
        #region Customer product

        /// <summary>
        /// Inserts a customer product 
        /// </summary>
        /// <param name="customerProduct">Customer product</param>
        Task InsertCustomerProduct(CustomerProduct customerProduct);

        /// <summary>
        /// Updates the customer product
        /// </summary>
        /// <param name="customerProduct">Customer product </param>
        Task UpdateCustomerProduct(CustomerProduct customerProduct);

        /// <summary>
        /// Delete a customer product 
        /// </summary>
        /// <param name="customerProduct">Customer product </param>
        Task DeleteCustomerProduct(CustomerProduct customerProduct);

        Task<IPagedList<CustomerProduct>> GetProductsByCustomer(string customerId, int pageIndex = 0, int pageSize = int.MaxValue);

        #endregion
    }
}
