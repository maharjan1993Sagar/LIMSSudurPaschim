using LIMS.Core.ModelBinding;
using LIMS.Core.Models;

namespace LIMS.Web.Areas.Admin.Models.Documents
{
    public class DocumentListModel : BaseModel
    {
        [LIMSResourceDisplayName("Admin.Documents.Document.List.SearchName")]
        public string SearchName { get; set; }

        [LIMSResourceDisplayName("Admin.Documents.Document.List.SearchNumber")]
        public string SearchNumber { get; set; }

        [LIMSResourceDisplayName("Admin.Documents.Document.List.SearchEmail")]
        public string SearchEmail { get; set; }

        [LIMSResourceDisplayName("Admin.Documents.Document.List.DocumentStatus")]
        public int StatusId { get; set; }

        public int Reference { get; set; }

        public string ObjectId { get; set; }
        public string CustomerId { get; set; }

    }
}
