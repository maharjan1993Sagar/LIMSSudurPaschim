using LIMS.Core.Models;

namespace LIMS.Web.Models.Common
{
    public partial class HeaderLinksModel : BaseModel
    {
        public bool IsAuthenticated { get; set; }
        public string CustomerEmailUsername { get; set; }
        public bool AllowPrivateMessages { get; set; }
        public string UnreadPrivateMessages { get; set; }
        public string AlertMessage { get; set; }
    }
}