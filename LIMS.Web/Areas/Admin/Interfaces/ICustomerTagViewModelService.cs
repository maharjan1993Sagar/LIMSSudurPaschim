using LIMS.Domain.Customers;
using LIMS.Web.Areas.Admin.Models.Customers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Interfaces
{
    public interface ICustomerTagViewModelService
    {
        CustomerModel PrepareCustomerModelForList(Customer customer);
        CustomerTagModel PrepareCustomerTagModel();
        Task<CustomerTag> InsertCustomerTagModel(CustomerTagModel model);
        Task<CustomerTag> UpdateCustomerTagModel(CustomerTag customerTag, CustomerTagModel model);
        Task DeleteCustomerTag(CustomerTag customerTag);
        Task<CustomerTagProductModel.AddProductModel> PrepareProductModel(string customerTagId);
        Task InsertProductModel(CustomerTagProductModel.AddProductModel model);
    }
}
