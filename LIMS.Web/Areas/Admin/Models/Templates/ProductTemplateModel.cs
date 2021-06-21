using LIMS.Core.ModelBinding;
using LIMS.Core.Models;

namespace LIMS.Web.Areas.Admin.Models.Templates
{
    public partial class ProductTemplateModel : BaseEntityModel
    {
        [LIMSResourceDisplayName("Admin.System.Templates.Product.Name")]

        public string Name { get; set; }

        [LIMSResourceDisplayName("Admin.System.Templates.Product.ViewPath")]

        public string ViewPath { get; set; }

        [LIMSResourceDisplayName("Admin.System.Templates.Product.DisplayOrder")]
        public int DisplayOrder { get; set; }
    }
}