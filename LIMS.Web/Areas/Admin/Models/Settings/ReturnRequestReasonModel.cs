using LIMS.Framework.Localization;
using LIMS.Core.ModelBinding;
using LIMS.Core.Models;
using System.Collections.Generic;

namespace LIMS.Web.Areas.Admin.Models.Settings
{
    public partial class ReturnRequestReasonModel : BaseEntityModel, ILocalizedModel<ReturnRequestReasonLocalizedModel>
    {
        public ReturnRequestReasonModel()
        {
            Locales = new List<ReturnRequestReasonLocalizedModel>();
        }

        [LIMSResourceDisplayName("Admin.Configuration.Settings.Order.ReturnRequestReasons.Name")]

        public string Name { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Settings.Order.ReturnRequestReasons.DisplayOrder")]
        public int DisplayOrder { get; set; }

        public IList<ReturnRequestReasonLocalizedModel> Locales { get; set; }
    }

    public partial class ReturnRequestReasonLocalizedModel : ILocalizedModelLocal
    {
        public string LanguageId { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Settings.Order.ReturnRequestReasons.Name")]

        public string Name { get; set; }

    }
}