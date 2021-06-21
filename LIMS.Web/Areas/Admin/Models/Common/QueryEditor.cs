
using LIMS.Core.ModelBinding;

namespace LIMS.Web.Areas.Admin.Models.Common
{
    public partial class QueryEditor
    {
        [LIMSResourceDisplayName("Admin.System.Field.QueryEditor")]
        public string Query { get; set; }
    }
}
