using LIMS.Core.ModelBinding;
using LIMS.Core.Models;

namespace LIMS.Web.Areas.Admin.Models.Settings
{
    public partial class SettingModel : BaseEntityModel
    {
        [LIMSResourceDisplayName("Admin.Configuration.Settings.AllSettings.Fields.Name")]

        public string Name { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Settings.AllSettings.Fields.Value")]

        public string Value { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Settings.AllSettings.Fields.StoreName")]
        public string Store { get; set; }
        public string StoreId { get; set; }
    }
}