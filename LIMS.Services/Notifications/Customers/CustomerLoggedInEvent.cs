﻿using LIMS.Domain.Customers;
using MediatR;

namespace LIMS.Services.Notifications.Customers
{
    /// <summary>
    /// Customer logged-in event
    /// </summary>
    public class CustomerLoggedInEvent : INotification
    {
        public CustomerLoggedInEvent(Customer customer)
        {
            Customer = customer;
        }

        /// <summary>
        /// Customer
        /// </summary>
        public Customer Customer {
            get; private set;
        }
    }

}
