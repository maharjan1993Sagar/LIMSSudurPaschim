using LIMS.Core.ModelBinding;
using LIMS.Core.Models;

namespace LIMS.Web.Areas.Admin.Models.Localization
{
    public partial class LanguageResourceFilterModel : BaseEntityModel
    {
        [LIMSResourceDisplayName("Admin.Configuration.Languages.ResourcesFilter.Fields.ResourceName")]
        public string ResourceName { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Languages.ResourcesFilter.Fields.ResourceValue")]
        public string ResourceValue { get; set; }

    }
}