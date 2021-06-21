using LIMS.Core.ModelBinding;
using LIMS.Framework.Mvc.Models;
using System.Collections.Generic;

namespace LIMS.Framework.Mapping
{
    public interface IStoreMappingModel
    {
        [LIMSResourceDisplayName("Admin.Catalog.Categories.Fields.LimitedToStores")]
        bool LimitedToStores { get; set; }
        [LIMSResourceDisplayName("Admin.Catalog.Categories.Fields.AvailableStores")]
        List<StoreModel> AvailableStores { get; set; }
        string[] SelectedStoreIds { get; set; }
    }
}
