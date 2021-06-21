using LIMS.Framework.Localization;
using LIMS.Core.ModelBinding;
using LIMS.Core.Models;
using System.Collections.Generic;

namespace LIMS.Web.Areas.Admin.Models.Settings
{
    public partial class ReturnRequestActionModel : BaseEntityModel, ILocalizedModel<ReturnRequestActionLocalizedModel>
    {
        public ReturnRequestActionModel()
        {
            Locales = new List<ReturnRequestActionLocalizedModel>();
        }

        [LIMSResourceDisplayName("Admin.Configuration.Settings.Order.ReturnRequestActions.Name")]

        public string Name { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Settings.Order.ReturnRequestActions.DisplayOrder")]
        public int DisplayOrder { get; set; }

        public IList<ReturnRequestActionLocalizedModel> Locales { get; set; }
    }

    public partial class ReturnRequestActionLocalizedModel : ILocalizedModelLocal
    {
        public string LanguageId { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Settings.Order.ReturnRequestActions.Name")]

        public string Name { get; set; }

    }
}