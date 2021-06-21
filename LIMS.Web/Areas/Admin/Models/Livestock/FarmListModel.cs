using LIMS.Core.ModelBinding;
using LIMS.Core.Models;

namespace LIMS.Web.Areas.Admin.Models.Livestock
{
    public partial class FarmListModel : BaseModel
    {
        [LIMSResourceDisplayName("Admin.Livestock.Farm.Keyword")]

        public string Keyword { get; set; }

    }
}
