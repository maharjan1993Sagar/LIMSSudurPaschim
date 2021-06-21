using LIMS.Core.Models;
using LIMS.Web.Areas.Admin.Models.Common;

namespace LIMS.Web.Areas.Admin.Models.Customers
{
    public partial class CustomerReviewModel
    {
        public string CustomerId { get; set; }

        public ReviewModel Review { get; set; }
    }
}