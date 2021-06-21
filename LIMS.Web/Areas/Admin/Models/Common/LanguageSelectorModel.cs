using LIMS.Core.Models;
using LIMS.Web.Areas.Admin.Models.Localization;
using System.Collections.Generic;

namespace LIMS.Web.Areas.Admin.Models.Common
{
    public partial class LanguageSelectorModel : BaseModel
    {
        public LanguageSelectorModel()
        {
            AvailableLanguages = new List<LanguageModel>();
        }

        public IList<LanguageModel> AvailableLanguages { get; set; }

        public LanguageModel CurrentLanguage { get; set; }
    }
}