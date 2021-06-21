using LIMS.Domain.Customers;
using LIMS.Web.Areas.Admin.Models.Customers;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Interfaces
{
    public interface ICustomerViewModelService
    {
        Task<CustomerListModel> PrepareCustomerListModel();

        Task<(IEnumerable<CustomerModel> customerModelList, int totalCount)> PrepareCustomerList(CustomerListModel model,
            string[] searchCustomerRoleIds, string[] searchCustomerTagIds, int pageIndex, int pageSize, string createdBy = "");

        Task PrepareCustomerModel(CustomerModel model, Customer customer, bool excludeProperties);

        string ValidateCustomerRoles(IList<CustomerRole> customerRoles);

        Task<Customer> InsertCustomerModel(CustomerModel model);
        
        Task<Customer> UpdateCustomerModel(Customer customer, CustomerModel model);
        
        Task DeleteCustomer(Customer customer);
        
        Task DeleteSelected(IList<string> selectedIds);
        
        Task SendEmail(Customer customer, CustomerModel.SendEmailModel model);

        Task<(IEnumerable<CustomerModel.ActivityLogModel> activityLogModels, int totalCount)> PrepareActivityLogModel(string customerId, int pageIndex, int pageSize);
    }
}
