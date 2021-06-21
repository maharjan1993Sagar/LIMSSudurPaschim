using LIMS.Domain.Stores;
using LIMS.Framework.Mapping;
using LIMS.Core.Models;
using LIMS.Services.Stores;
using System.Linq;
using System.Threading.Tasks;
using LIMS.Framework.Mvc.Models;

namespace LIMS.Web.Areas.Admin.Extensions
{
    public static class StoresMappingExtension
    {
        public static async Task PrepareStoresMappingModel<T>(this T baseLIMSEntityModel, IStoreMappingSupported storeMapping, IStoreService _storeService, bool excludeProperties, string storeId = null) 
            where T : BaseEntityModel, IStoreMappingModel
        {
            baseLIMSEntityModel.AvailableStores = (await _storeService
               .GetAllStores()).Where(x=>x.Id == storeId || string.IsNullOrEmpty(storeId))
               .Select(s => new StoreModel { Id = s.Id, Name = s.Shortcut })
               .ToList();
            if (!excludeProperties)
            {
                if (storeMapping != null)
                {
                    baseLIMSEntityModel.SelectedStoreIds = storeMapping.Stores.ToArray();
                }
            }
            if (!string.IsNullOrEmpty(storeId))
            {
                baseLIMSEntityModel.LimitedToStores = true;
                baseLIMSEntityModel.SelectedStoreIds = new string[] { storeId };
            }
        }       
    }
}
