using LIMS.Framework.Localization;
using LIMS.Framework.Mapping;
using LIMS.Core.ModelBinding;
using LIMS.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LIMS.Framework.Mvc.Models;

namespace LIMS.Web.Areas.Admin.Models.Directory
{
    public partial class CurrencyModel : BaseEntityModel, ILocalizedModel<CurrencyLocalizedModel>, IStoreMappingModel
    {
        public CurrencyModel()
        {
            Locales = new List<CurrencyLocalizedModel>();
            AvailableStores = new List<StoreModel>();
        }
        [LIMSResourceDisplayName("Admin.Configuration.Currencies.Fields.Name")]

        public string Name { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Currencies.Fields.CurrencyCode")]

        public string CurrencyCode { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Currencies.Fields.DisplayLocale")]

        public string DisplayLocale { get; set; }

        [UIHint("DecimalN4")]
        [LIMSResourceDisplayName("Admin.Configuration.Currencies.Fields.Rate")]
        public decimal Rate { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Currencies.Fields.CustomFormatting")]
        public string CustomFormatting { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Currencies.Fields.NumberDecimal")]
        public int NumberDecimal { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Currencies.Fields.Published")]
        public bool Published { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Currencies.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Currencies.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Currencies.Fields.IsPrimaryExchangeRateCurrency")]
        public bool IsPrimaryExchangeRateCurrency { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Currencies.Fields.IsPrimaryStoreCurrency")]
        public bool IsPrimaryStoreCurrency { get; set; }

        public IList<CurrencyLocalizedModel> Locales { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Currencies.Fields.RoundingType")]
        public int RoundingTypeId { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Currencies.Fields.MidpointRound")]
        public int MidpointRoundId { get; set; }

        //Store mapping
        [LIMSResourceDisplayName("Admin.Configuration.Currencies.Fields.LimitedToStores")]
        public bool LimitedToStores { get; set; }
        [LIMSResourceDisplayName("Admin.Configuration.Currencies.Fields.AvailableStores")]
        public List<StoreModel> AvailableStores { get; set; }
        public string[] SelectedStoreIds { get; set; }
    }

    public partial class CurrencyLocalizedModel : ILocalizedModelLocal
    {
        public string LanguageId { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.Currencies.Fields.Name")]

        public string Name { get; set; }
    }
}