using LIMS.Framework.Localization;
using LIMS.Core.ModelBinding;
using LIMS.Core.Models;
using System.Collections.Generic;

namespace LIMS.Web.Areas.Admin.Models.Directory
{
    public partial class StateProvinceModel : BaseEntityModel, ILocalizedModel<StateProvinceLocalizedModel>
    {
        public StateProvinceModel()
        {
            Locales = new List<StateProvinceLocalizedModel>();
        }
        public string CountryId { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Countries.States.Fields.Name")]

        public string Name { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Countries.States.Fields.Abbreviation")]

        public string Abbreviation { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Countries.States.Fields.Published")]
        public bool Published { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Countries.States.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        public IList<StateProvinceLocalizedModel> Locales { get; set; }
    }

    public partial class StateProvinceLocalizedModel : ILocalizedModelLocal
    {
        public string LanguageId { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Countries.States.Fields.Name")]

        public string Name { get; set; }
    }
}