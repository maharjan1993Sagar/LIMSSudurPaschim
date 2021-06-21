using LIMS.Domain.Customers;
using LIMS.Web.Areas.Admin.Models.Customers;

namespace LIMS.Web.Areas.Admin.Extensions
{
    public static class CustomerActionTypeMappingExtensions
    {
        public static CustomerActionTypeModel ToModel(this CustomerActionType entity)
        {
            return entity.MapTo<CustomerActionType, CustomerActionTypeModel>();
        }
    }
}