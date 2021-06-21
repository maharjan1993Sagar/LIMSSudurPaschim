using LIMS.Core.ModelBinding;
using LIMS.Core.Models;

namespace LIMS.Web.Areas.Admin.Models.Templates
{
    public partial class TopicTemplateModel : BaseEntityModel
    {
        [LIMSResourceDisplayName("Admin.System.Templates.Topic.Name")]

        public string Name { get; set; }

        [LIMSResourceDisplayName("Admin.System.Templates.Topic.ViewPath")]

        public string ViewPath { get; set; }

        [LIMSResourceDisplayName("Admin.System.Templates.Topic.DisplayOrder")]
        public int DisplayOrder { get; set; }
    }
}