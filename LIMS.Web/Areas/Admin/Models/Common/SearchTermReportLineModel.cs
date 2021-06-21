using LIMS.Core.ModelBinding;
using LIMS.Core.Models;

namespace LIMS.Web.Areas.Admin.Models.Common
{
    public partial class SearchTermReportLineModel : BaseModel
    {
        [LIMSResourceDisplayName("Admin.SearchTermReport.Keyword")]
        public string Keyword { get; set; }

        [LIMSResourceDisplayName("Admin.SearchTermReport.Count")]
        public int Count { get; set; }
    }
}
