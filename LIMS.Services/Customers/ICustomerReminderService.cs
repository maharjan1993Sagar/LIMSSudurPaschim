using LIMS.Domain;
using LIMS.Domain.Customers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LIMS.Services.Customers
{
    public partial interface ICustomerReminderService
    {


        /// <summary>
        /// Gets customer reminder
        /// </summary>
        /// <param name="id">Customer reminder identifier</param>
        /// <returns>Customer reminder</returns>
        Task<CustomerReminder> GetCustomerReminderById(string id);


        /// <summary>
        /// Gets all customer reminders
        /// </summary>
        /// <returns>Customer reminders</returns>
        Task<IList<CustomerReminder>> GetCustomerReminders();

        /// <summary>
        /// Inserts a customer reminder
        /// </summary>
        /// <param name="CustomerReminder">Customer reminder</param>
        Task InsertCustomerReminder(CustomerReminder customerReminder);

        /// <summary>
        /// Delete a customer reminder
        /// </summary>
        /// <param name="customerReminder">Customer reminder</param>
        Task DeleteCustomerReminder(CustomerReminder customerReminder);

        /// <summary>
        /// Updates the customer reminder
        /// </summary>
        /// <param name="CustomerReminder">Customer reminder</param>
        Task UpdateCustomerReminder(CustomerReminder customerReminder);

        /// <summary>
        /// Gets customer reminders history for reminder
        /// </summary>
        /// <returns>SerializeCustomerReminderHistory</returns>
        Task<IPagedList<SerializeCustomerReminderHistory>> GetAllCustomerReminderHistory(string customerReminderId, int pageIndex = 0, int pageSize = 2147483647);

        Task Task_RegisteredCustomer(string id = "");
        Task Task_LastActivity(string id = "");
        Task Task_Birthday(string id = "");

    }
}
