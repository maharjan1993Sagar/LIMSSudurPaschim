using LIMS.Core.ModelBinding;
using LIMS.Core.Models;

namespace LIMS.Web.Areas.Admin.Models.Localization
{
    public partial class LanguageResourceModel : BaseEntityModel
    {
        [LIMSResourceDisplayName("Admin.Configuration.Languages.Resources.Fields.Name")]
        public string Name { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Languages.Resources.Fields.Value")]
        public string Value { get; set; }

        public string LanguageId { get; set; }
    }
}