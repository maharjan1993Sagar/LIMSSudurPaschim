using LIMS.Core.ModelBinding;
using LIMS.Core.Models;

namespace LIMS.Web.Areas.Admin.Models.Templates
{
    public partial class ManufacturerTemplateModel : BaseEntityModel
    {
        [LIMSResourceDisplayName("Admin.System.Templates.Manufacturer.Name")]

        public string Name { get; set; }

        [LIMSResourceDisplayName("Admin.System.Templates.Manufacturer.ViewPath")]

        public string ViewPath { get; set; }

        [LIMSResourceDisplayName("Admin.System.Templates.Manufacturer.DisplayOrder")]
        public int DisplayOrder { get; set; }
    }
}