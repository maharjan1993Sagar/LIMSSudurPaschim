using LIMS.Domain.Customers;
using LIMS.Web.Areas.Admin.Models.Customers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Interfaces
{
    public interface ICustomerRoleViewModelService
    {
        CustomerRoleModel PrepareCustomerRoleModel(CustomerRole customerRole);
        CustomerRoleModel PrepareCustomerRoleModel();
        Task<CustomerRole> InsertCustomerRoleModel(CustomerRoleModel model);
        Task<CustomerRole> UpdateCustomerRoleModel(CustomerRole customerRole, CustomerRoleModel model);
        Task DeleteCustomerRole(CustomerRole customerRole);
    }
}
