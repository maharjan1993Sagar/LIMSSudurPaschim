using LIMS.Core.ModelBinding;
using LIMS.Core.Models;

namespace LIMS.Web.Areas.Admin.Models.Templates
{
    public partial class CategoryTemplateModel : BaseEntityModel
    {
        [LIMSResourceDisplayName("Admin.System.Templates.Category.Name")]

        public string Name { get; set; }

        [LIMSResourceDisplayName("Admin.System.Templates.Category.ViewPath")]

        public string ViewPath { get; set; }

        [LIMSResourceDisplayName("Admin.System.Templates.Category.DisplayOrder")]
        public int DisplayOrder { get; set; }
    }
}