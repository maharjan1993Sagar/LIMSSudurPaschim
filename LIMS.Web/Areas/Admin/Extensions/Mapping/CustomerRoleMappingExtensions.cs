using LIMS.Domain.Customers;
using LIMS.Web.Areas.Admin.Models.Customers;

namespace LIMS.Web.Areas.Admin.Extensions
{
    public static class CustomerRoleMappingExtensions
    {
        public static CustomerRoleModel ToModel(this CustomerRole entity)
        {
            return entity.MapTo<CustomerRole, CustomerRoleModel>();
        }

        public static CustomerRole ToEntity(this CustomerRoleModel model)
        {
            return model.MapTo<CustomerRoleModel, CustomerRole>();
        }

        public static CustomerRole ToEntity(this CustomerRoleModel model, CustomerRole destination)
        {
            return model.MapTo(destination);
        }
    }
}