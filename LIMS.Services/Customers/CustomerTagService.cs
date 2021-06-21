using LIMS.Domain;
using LIMS.Core.Caching;
using LIMS.Domain.Data;
using LIMS.Domain.Customers;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Services.Customers
{
    /// <summary>
    /// Customer tag service
    /// </summary>
    public partial class CustomerTagService : ICustomerTagService
    {
        #region Fields

        private readonly IRepository<CustomerTag> _customerTagRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IMediator _mediator;
        private readonly ICacheManager _cacheManager;
        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public CustomerTagService(IRepository<CustomerTag> customerTagRepository,
            IRepository<Customer> customerRepository,
            IMediator mediator,
            ICacheManager cacheManager
            )
        {
            _customerTagRepository = customerTagRepository;
            _mediator = mediator;
            _customerRepository = customerRepository;
            _cacheManager = cacheManager;
        }

        #endregion

        /// <summary>
        /// Gets all customer for tag id
        /// </summary>
        /// <returns>Customers</returns>
        public virtual async Task<IPagedList<Customer>> GetCustomersByTag(string customerTagId = "", int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = from c in _customerRepository.Table
                        where c.CustomerTags.Contains(customerTagId)
                        select c;
            return await PagedList<Customer>.Create(query, pageIndex, pageSize);
        }

        /// <summary>
        /// Delete a customer tag
        /// </summary>
        /// <param name="customerTag">Customer tag</param>
        public virtual async Task DeleteCustomerTag(CustomerTag customerTag)
        {
            if (customerTag == null)
                throw new ArgumentNullException("productTag");

            var builder = Builders<Customer>.Update;
            var updatefilter = builder.Pull(x => x.CustomerTags, customerTag.Id);
            await _customerRepository.Collection.UpdateManyAsync(new BsonDocument(), updatefilter);

            await _customerTagRepository.DeleteAsync(customerTag);

            //event notification
            await _mediator.EntityDeleted(customerTag);
        }

        /// <summary>
        /// Gets all customer tags
        /// </summary>
        /// <returns>Customer tags</returns>
        public virtual async Task<IList<CustomerTag>> GetAllCustomerTags()
        {
            var query = _customerTagRepository.Table;
            return await query.ToListAsync();
        }

        /// <summary>
        /// Gets customer tag
        /// </summary>
        /// <param name="customerTagId">Customer tag identifier</param>
        /// <returns>Customer tag</returns>
        public virtual Task<CustomerTag> GetCustomerTagById(string customerTagId)
        {
            return _customerTagRepository.GetByIdAsync(customerTagId);
        }

        /// <summary>
        /// Gets customer tag by name
        /// </summary>
        /// <param name="name">Customer tag name</param>
        /// <returns>Customer tag</returns>
        public virtual Task<CustomerTag> GetCustomerTagByName(string name)
        {
            var query = from pt in _customerTagRepository.Table
                        where pt.Name == name
                        select pt;

            return query.FirstOrDefaultAsync(); 
        }

        /// <summary>
        /// Gets customer tags search by name
        /// </summary>
        /// <param name="name">Customer tags name</param>
        /// <returns>Customer tags</returns>
        public virtual async Task<IList<CustomerTag>> GetCustomerTagsByName(string name)
        {
            var query = from pt in _customerTagRepository.Table
                        where pt.Name.ToLower().Contains(name.ToLower())
                        select pt;
            return await query.ToListAsync();
        }

        /// <summary>
        /// Inserts a customer tag
        /// </summary>
        /// <param name="customerTag">Customer tag</param>
        public virtual async Task InsertCustomerTag(CustomerTag customerTag)
        {
            if (customerTag == null)
                throw new ArgumentNullException("customerTag");

            await _customerTagRepository.InsertAsync(customerTag);

            //event notification
            await _mediator.EntityInserted(customerTag);
        }

        /// <summary>
        /// Insert tag to a customer
        /// </summary>
        public virtual async Task InsertTagToCustomer(string customerTagId, string customerId)
        {
            var updatebuilder = Builders<Customer>.Update;
            var update = updatebuilder.AddToSet(p => p.CustomerTags, customerTagId);
            await _customerRepository.Collection.UpdateOneAsync(new BsonDocument("_id", customerId), update);
        }

        /// <summary>
        /// Delete tag from a customer
        /// </summary>
        public virtual async Task DeleteTagFromCustomer(string customerTagId, string customerId)
        {
            var updatebuilder = Builders<Customer>.Update;
            var update = updatebuilder.Pull(p => p.CustomerTags, customerTagId);
            await _customerRepository.Collection.UpdateOneAsync(new BsonDocument("_id", customerId), update);
        }

        /// <summary>
        /// Updates the customer tag
        /// </summary>
        /// <param name="customerTag">Customer tag</param>
        public virtual async Task UpdateCustomerTag(CustomerTag customerTag)
        {
            if (customerTag == null)
                throw new ArgumentNullException("customerTag");

            await _customerTagRepository.UpdateAsync(customerTag);

            //event notification
            await _mediator.EntityUpdated(customerTag);
        }

        /// <summary>
        /// Get number of customers
        /// </summary>
        /// <param name="customerTagId">Customer tag identifier</param>
        /// <returns>Number of customers</returns>
        public virtual async Task<int> GetCustomerCount(string customerTagId)
        {
            var query = await _customerRepository.Table.
                Where(x => x.CustomerTags.Contains(customerTagId)).
                GroupBy(p => p, (k, s) => new { Counter = s.Count() }).ToListAsync();
            if(query.Count > 0)
                return query.FirstOrDefault().Counter;
            return 0;
        }
    }
}
