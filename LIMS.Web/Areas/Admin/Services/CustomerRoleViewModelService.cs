using LIMS.Domain.Customers;
using LIMS.Framework.Extensions;
using LIMS.Services.Customers;
using LIMS.Services.Helpers;
using LIMS.Services.Localization;
using LIMS.Services.Logging;
using LIMS.Services.Stores;
using LIMS.Web.Areas.Admin.Extensions;
using LIMS.Web.Areas.Admin.Interfaces;
using LIMS.Web.Areas.Admin.Models.Customers;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Services
{
    public partial class CustomerRoleViewModelService : ICustomerRoleViewModelService
    {
        private readonly ICustomerService _customerService;
        private readonly ILocalizationService _localizationService;
        private readonly ICustomerActivityService _customerActivityService;

        #region Constructors

        public CustomerRoleViewModelService(ICustomerService customerService,
            ILocalizationService localizationService,
            ICustomerActivityService customerActivityService)
        {
            _customerService = customerService;
            _localizationService = localizationService;
            _customerActivityService = customerActivityService;
        }

        #endregion

        public virtual CustomerRoleModel PrepareCustomerRoleModel(CustomerRole customerRole)
        {
            var model = customerRole.ToModel();
            return model;
        }

        public virtual CustomerRoleModel PrepareCustomerRoleModel()
        {
            var model = new CustomerRoleModel();
            //default values
            model.Active = true;
            return model;
        }

        public virtual async Task<CustomerRole> InsertCustomerRoleModel(CustomerRoleModel model)
        {
            var customerRole = model.ToEntity();
            await _customerService.InsertCustomerRole(customerRole);
            //activity log
            await _customerActivityService.InsertActivity("AddNewCustomerRole", customerRole.Id, _localizationService.GetResource("ActivityLog.AddNewCustomerRole"), customerRole.Name);
            return customerRole;
        }
        public virtual async Task<CustomerRole> UpdateCustomerRoleModel(CustomerRole customerRole, CustomerRoleModel model)
        {
            customerRole = model.ToEntity(customerRole);
            await _customerService.UpdateCustomerRole(customerRole);

            //activity log
            await _customerActivityService.InsertActivity("EditCustomerRole", customerRole.Id, _localizationService.GetResource("ActivityLog.EditCustomerRole"), customerRole.Name);
            return customerRole;
        }
        public virtual async Task DeleteCustomerRole(CustomerRole customerRole)
        {
            await _customerService.DeleteCustomerRole(customerRole);

            //activity log
            await _customerActivityService.InsertActivity("DeleteCustomerRole", customerRole.Id, _localizationService.GetResource("ActivityLog.DeleteCustomerRole"), customerRole.Name);
        }
    }
}
