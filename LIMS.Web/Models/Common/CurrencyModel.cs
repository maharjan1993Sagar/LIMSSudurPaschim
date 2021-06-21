using LIMS.Core.Models;

namespace LIMS.Web.Models.Common
{
    public partial class CurrencyModel : BaseEntityModel
    {
        public string Name { get; set; }

        public string CurrencySymbol { get; set; }
        public string CurrencyCode { get; set; }
    }
}