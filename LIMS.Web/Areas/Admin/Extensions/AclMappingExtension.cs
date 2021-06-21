using LIMS.Domain;
using LIMS.Domain.Security;
using LIMS.Domain.Stores;
using LIMS.Framework.Mapping;
using LIMS.Core.Models;
using LIMS.Services.Customers;
using System.Linq;
using System.Threading.Tasks;
using LIMS.Framework.Mvc.Models;

namespace LIMS.Web.Areas.Admin.Extensions
{
    public static class AclMappingExtension
    {
        public static async Task PrepareACLModel<T>(this T baseLIMSEntityModel, IAclSupported aclMapping, bool excludeProperties, ICustomerService customerService)
            where T : BaseEntityModel, IAclMappingModel
        {
            baseLIMSEntityModel.AvailableCustomerRoles = (await customerService
               .GetAllCustomerRoles(showHidden: true))
               .Select(s => new CustomerRoleModel { Id = s.Id, Name = s.Name })
               .ToList();
            if (!excludeProperties)
            {
                if (aclMapping != null)
                {
                    baseLIMSEntityModel.SelectedCustomerRoleIds = aclMapping.CustomerRoles.ToArray();
                }
            }
        }

        /// <summary>
        /// Authorize whether entity could be accessed in a store 
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="storeId">Store identifier</param>
        /// <returns>true - authorized; otherwise, false</returns>
        public static bool AccessToEntityByStore<T>(this T entity, string storeId) where T : BaseEntity, IStoreMappingSupported
        {
            if (entity == null)
                return false;

            if (string.IsNullOrEmpty(storeId))
                //return true if no store specified/found
                return true;

            if (entity.LimitedToStores && entity.Stores.Where(x => x == storeId).Any() && entity.Stores.Count == 1)
                //yes, we have such permission
                return true;

            //no permission found
            return false;
        }
    }
}
