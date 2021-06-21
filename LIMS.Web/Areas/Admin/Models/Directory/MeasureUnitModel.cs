using LIMS.Core.ModelBinding;
using LIMS.Core.Models;

namespace LIMS.Web.Areas.Admin.Models.Directory
{
    public partial class MeasureUnitModel : BaseEntityModel
    {
        [LIMSResourceDisplayName("Admin.Configuration.Measures.Units.Fields.Name")]

        public string Name { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Measures.Units.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

    }
}