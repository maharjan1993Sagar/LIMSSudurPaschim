using LIMS.Core.ModelBinding;
using LIMS.Core.Models;

namespace LIMS.Web.Areas.Admin.Models.Common
{
    public partial class UrlRecordListModel : BaseModel
    {
        [LIMSResourceDisplayName("Admin.System.SeNames.Name")]
        
        public string SeName { get; set; }
    }
}