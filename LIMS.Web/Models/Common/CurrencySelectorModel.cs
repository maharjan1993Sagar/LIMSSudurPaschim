using LIMS.Core.Models;
using System.Collections.Generic;

namespace LIMS.Web.Models.Common
{
    public partial class CurrencySelectorModel : BaseModel
    {
        public CurrencySelectorModel()
        {
            AvailableCurrencies = new List<CurrencyModel>();
        }

        public IList<CurrencyModel> AvailableCurrencies { get; set; }

        public string CurrentCurrencyId { get; set; }
    }
}