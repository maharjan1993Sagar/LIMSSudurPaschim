using LIMS.Core.ModelBinding;
using LIMS.Core.Models;

namespace LIMS.Web.Areas.Admin.Models.Documents
{
    public partial class DocumentTypeModel : BaseEntityModel
    {
        [LIMSResourceDisplayName("Admin.Documents.Type.Fields.Name")]

        public string Name { get; set; }

        [LIMSResourceDisplayName("Admin.Documents.Type.Fields.Description")]

        public string Description { get; set; }

        [LIMSResourceDisplayName("Admin.Documents.Type.Fields.DisplayOrder")]

        public int DisplayOrder { get; set; }
    }
}
