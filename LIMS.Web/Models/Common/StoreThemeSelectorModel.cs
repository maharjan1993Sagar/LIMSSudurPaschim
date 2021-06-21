using LIMS.Core.Models;
using System.Collections.Generic;

namespace LIMS.Web.Models.Common
{
    public partial class StoreThemeSelectorModel : BaseModel
    {
        public StoreThemeSelectorModel()
        {
            AvailableStoreThemes = new List<StoreThemeModel>();
        }

        public IList<StoreThemeModel> AvailableStoreThemes { get; set; }

        public StoreThemeModel CurrentStoreTheme { get; set; }
    }
}