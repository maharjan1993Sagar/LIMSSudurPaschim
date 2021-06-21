using LIMS.Core.ModelBinding;
using LIMS.Core.Models;

namespace LIMS.Web.Areas.Admin.Models.Directory
{
    public partial class MeasureWeightModel : BaseEntityModel
    {
        [LIMSResourceDisplayName("Admin.Configuration.Measures.Weights.Fields.Name")]

        public string Name { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Measures.Weights.Fields.SystemKeyword")]

        public string SystemKeyword { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Measures.Weights.Fields.Ratio")]
        public decimal Ratio { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Measures.Weights.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Measures.Weights.Fields.IsPrimaryWeight")]
        public bool IsPrimaryWeight { get; set; }
    }
}