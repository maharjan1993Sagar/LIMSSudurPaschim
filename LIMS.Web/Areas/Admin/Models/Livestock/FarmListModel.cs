using LIMS.Core.ModelBinding;
using LIMS.Core.Models;

namespace LIMS.Web.Areas.Admin.Models.Livestock
{
    public partial class FarmListModel : BaseModel
    {
        [LIMSResourceDisplayName("Admin.Livestock.Farm.Keyword")]

        public string Keyword { get; set; }

        [LIMSResourceDisplayName("Admin.Common.Province")]

        public string Province { get; set; }
        public string CurrentFiscalYear { get; set; }
        [LIMSResourceDisplayName("Admin.Livestock.Farm.Fiscalyear")]
        public string Fiscalyear { get; set; }
        [LIMSResourceDisplayName("Admin.Livestock.Species")]
        public string Species { get; set; }
        [LIMSResourceDisplayName("Admin.Livestock.District")]
        public string District { get; set; }
        [LIMSResourceDisplayName("Admin.Livestock.LocalLevel")]
        public string LocalLevel { get; set; }
        [LIMSResourceDisplayName("Admin.Livestock.LocalLevel")]
        public string Ward { get; set; }
        [LIMSResourceDisplayName("Admin.Livestock.LocalLevel")]
        public string Type { get; set; }
    }
}
