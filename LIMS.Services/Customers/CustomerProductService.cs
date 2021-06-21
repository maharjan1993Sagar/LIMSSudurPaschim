using LIMS.Domain;
using LIMS.Core.Caching;
using LIMS.Domain.Data;
using LIMS.Domain.Customers;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Driver.Linq;
using System;
using System.Threading.Tasks;

namespace LIMS.Services.Customers
{
    /// <summary>
    /// Customer product service interface
    /// </summary>
    public class CustomerProductService : ICustomerProductService
    {
        /// <summary>
        /// Key pattern to clear cache
        /// {0} customer id
        /// </summary>
        private const string CUSTOMER_PRODUCT_KEY = "LIMS.product.personal-{0}";

        /// <summary>
        /// Key for cache 
        /// {0} - customer id
        /// {1} - product id
        /// </summary>
        private const string CUSTOMER_PRODUCT_PRICE_KEY_ID = "LIMS.product.price-{0}-{1}";

        private readonly IRepository<CustomerProduct> _customerProductRepository;
        private readonly ICacheManager _cacheManager;
        private readonly IMediator _mediator;

        public CustomerProductService(
            IRepository<CustomerProduct> customerProductRepository,
            ICacheManager cacheManager,
            IMediator mediator)
        {
            _cacheManager = cacheManager;
            _customerProductRepository = customerProductRepository;
            _mediator = mediator;
        }        

        #region Personalize products

        /// <summary>
        /// Gets a customer product 
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Customer product</returns>
        public virtual async Task<CustomerProduct> GetCustomerProduct(string id)
        {
            return await _customerProductRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Gets a customer product 
        /// </summary>
        /// <param name="customerId">Customer Identifier</param>
        /// <param name="productId">Product Identifier</param>
        /// <returns>Customer product</returns>
        public virtual Task<CustomerProduct> GetCustomerProduct(string customerId, string productId)
        {
            var query = from pp in _customerProductRepository.Table
                        where pp.CustomerId == customerId && pp.ProductId == productId
                        select pp;

            return query.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Insert a customer product 
        /// </summary>
        /// <param name="customerProduct">Customer product</param>
        public virtual async Task InsertCustomerProduct(CustomerProduct customerProduct)
        {
            if (customerProduct == null)
                throw new ArgumentNullException("customerProduct");

            await _customerProductRepository.InsertAsync(customerProduct);

            //clear cache
            await _cacheManager.RemoveAsync(string.Format(CUSTOMER_PRODUCT_KEY, customerProduct.CustomerId));

            //event notification
            await _mediator.EntityInserted(customerProduct);
        }

        /// <summary>
        /// Updates the customer product
        /// </summary>
        /// <param name="customerProduct">Customer product </param>
        public virtual async Task UpdateCustomerProduct(CustomerProduct customerProduct)
        {
            if (customerProduct == null)
                throw new ArgumentNullException("customerProduct");

            await _customerProductRepository.UpdateAsync(customerProduct);

            //clear cache
            await _cacheManager.RemoveAsync(string.Format(CUSTOMER_PRODUCT_KEY, customerProduct.CustomerId));

            //event notification
            await _mediator.EntityUpdated(customerProduct);
        }

        /// <summary>
        /// Delete a customer product 
        /// </summary>
        /// <param name="customerProduct">Customer product</param>
        public virtual async Task DeleteCustomerProduct(CustomerProduct customerProduct)
        {
            if (customerProduct == null)
                throw new ArgumentNullException("customerProduct");

            await _customerProductRepository.DeleteAsync(customerProduct);

            //clear cache
            await _cacheManager.RemoveAsync(string.Format(CUSTOMER_PRODUCT_KEY, customerProduct.CustomerId));

            //event notification
            await _mediator.EntityDeleted(customerProduct);
        }

        public virtual async Task<IPagedList<CustomerProduct>> GetProductsByCustomer(string customerId, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = from pp in _customerProductRepository.Table
                        where pp.CustomerId == customerId
                        orderby pp.DisplayOrder
                        select pp;
            return await PagedList<CustomerProduct>.Create(query, pageIndex, pageSize);
        }

        #endregion

    }
}
