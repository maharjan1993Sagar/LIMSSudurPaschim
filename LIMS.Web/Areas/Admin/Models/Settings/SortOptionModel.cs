using LIMS.Core.ModelBinding;

namespace LIMS.Web.Areas.Admin.Models.Settings
{
    public partial class SortOptionModel
    {
        public virtual int Id { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Settings.Catalog.SortOptions.Name")]
        
        public string Name { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Settings.Catalog.SortOptions.IsActive")]        
        public bool IsActive { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Settings.Catalog.SortOptions.DisplayOrder")]
        public int DisplayOrder { get; set; }
    }
}