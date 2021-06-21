using LIMS.Core.ModelBinding;
using LIMS.Core.Models;

namespace LIMS.Web.Areas.Admin.Models.Directory
{
    public partial class MeasureDimensionModel : BaseEntityModel
    {
        [LIMSResourceDisplayName("Admin.Configuration.Measures.Dimensions.Fields.Name")]

        public string Name { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Measures.Dimensions.Fields.SystemKeyword")]

        public string SystemKeyword { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Measures.Dimensions.Fields.Ratio")]
        public decimal Ratio { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Measures.Dimensions.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Measures.Dimensions.Fields.IsPrimaryDimension")]
        public bool IsPrimaryDimension { get; set; }
    }
}