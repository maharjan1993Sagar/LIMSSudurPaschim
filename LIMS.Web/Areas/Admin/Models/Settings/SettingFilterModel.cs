using LIMS.Core.ModelBinding;
using LIMS.Core.Models;

namespace LIMS.Web.Areas.Admin.Models.Settings
{
    public partial class SettingFilterModel : BaseModel
    {
        [LIMSResourceDisplayName("Admin.Configuration.Settings.Filter.Name")]
        public string SettingFilterName { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Settings.Filter.Value")]
        public string SettingFilterValue { get; set; }

    }
}