using LIMS.Core.ModelBinding;
using LIMS.Core.Models;

namespace LIMS.Web.Areas.Admin.Models.Common
{
    public partial class UrlRecordModel : BaseEntityModel
    {
        [LIMSResourceDisplayName("Admin.System.SeNames.Name")]
        
        public string Name { get; set; }

        [LIMSResourceDisplayName("Admin.System.SeNames.EntityId")]
        public string EntityId { get; set; }

        [LIMSResourceDisplayName("Admin.System.SeNames.EntityName")]
        public string EntityName { get; set; }

        [LIMSResourceDisplayName("Admin.System.SeNames.IsActive")]
        public bool IsActive { get; set; }

        [LIMSResourceDisplayName("Admin.System.SeNames.Language")]
        public string Language { get; set; }

        [LIMSResourceDisplayName("Admin.System.SeNames.Details")]
        public string DetailsUrl { get; set; }
    }
}