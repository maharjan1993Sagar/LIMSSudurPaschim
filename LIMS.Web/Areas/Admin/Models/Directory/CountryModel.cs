using LIMS.Framework.Localization;
using LIMS.Framework.Mapping;
using LIMS.Core.ModelBinding;
using LIMS.Core.Models;
using System.Collections.Generic;
using LIMS.Framework.Mvc.Models;

namespace LIMS.Web.Areas.Admin.Models.Directory
{
    public partial class CountryModel : BaseEntityModel, ILocalizedModel<CountryLocalizedModel>, IStoreMappingModel
    {
        public CountryModel()
        {
            AvailableStores = new List<StoreModel>();
            Locales = new List<CountryLocalizedModel>();
        }
        [LIMSResourceDisplayName("Admin.Configuration.Countries.Fields.Name")]

        public string Name { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Countries.Fields.AllowsBilling")]
        public bool AllowsBilling { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Countries.Fields.AllowsShipping")]
        public bool AllowsShipping { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Countries.Fields.TwoLetterIsoCode")]

        public string TwoLetterIsoCode { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Countries.Fields.ThreeLetterIsoCode")]

        public string ThreeLetterIsoCode { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Countries.Fields.NumericIsoCode")]
        public int NumericIsoCode { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Countries.Fields.SubjectToVat")]
        public bool SubjectToVat { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Countries.Fields.Published")]
        public bool Published { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Countries.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }


        [LIMSResourceDisplayName("Admin.Configuration.Countries.Fields.NumberOfStates")]
        public int NumberOfStates { get; set; }

        public IList<CountryLocalizedModel> Locales { get; set; }


        //Store mapping
        [LIMSResourceDisplayName("Admin.Configuration.Countries.Fields.LimitedToStores")]
        public bool LimitedToStores { get; set; }
        [LIMSResourceDisplayName("Admin.Configuration.Countries.Fields.AvailableStores")]
        public List<StoreModel> AvailableStores { get; set; }
        public string[] SelectedStoreIds { get; set; }
    }

    public partial class CountryLocalizedModel : ILocalizedModelLocal
    {
        public string LanguageId { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Countries.Fields.Name")]

        public string Name { get; set; }
    }
}