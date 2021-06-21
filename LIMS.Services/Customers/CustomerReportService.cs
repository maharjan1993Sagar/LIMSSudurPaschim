using LIMS.Domain;
using LIMS.Domain.Data;
using LIMS.Domain.Customers;
using LIMS.Services.Helpers;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Services.Customers
{
    /// <summary>
    /// Customer report service
    /// </summary>
    public partial class CustomerReportService : ICustomerReportService
    {
        #region Fields

        private readonly IRepository<Customer> _customerRepository;
        private readonly ICustomerService _customerService;
        private readonly IDateTimeHelper _dateTimeHelper;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="customerRepository">Customer repository</param>
        /// <param name="customerService">Customer service</param>
        /// <param name="dateTimeHelper">Date time helper</param>
        public CustomerReportService(IRepository<Customer> customerRepository, ICustomerService customerService,
            IDateTimeHelper dateTimeHelper)
        {
            _customerRepository = customerRepository;
            _customerService = customerService;
            _dateTimeHelper = dateTimeHelper;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Gets a report of customers registered in the last days
        /// </summary>
        /// <param name="storeId">Store ident</param>
        /// <param name="days">Customers registered in the last days</param>
        /// <returns>Number of registered customers</returns>
        public virtual async Task<int> GetRegisteredCustomersReport(string storeId, int days)
        {
            DateTime date = _dateTimeHelper.ConvertToUserTime(DateTime.Now).AddDays(-days);

            var registeredCustomerRole = await _customerService.GetCustomerRoleBySystemName(SystemCustomerRoleNames.Registered);
            if (registeredCustomerRole == null)
                return 0;

            var query = from c in _customerRepository.Table
                        where !c.Deleted &&
                        (string.IsNullOrEmpty(storeId) || c.StoreId == storeId) &&
                        c.CustomerRoles.Any(cr => cr.Id == registeredCustomerRole.Id) &&
                        c.CreatedOnUtc >= date
                        //&& c.CreatedOnUtc <= DateTime.UtcNow
                        select c;
            int count = query.Count();
            return count;
        }
        #endregion
    }
}