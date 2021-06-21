using LIMS.Core.ModelBinding;
using LIMS.Core.Models;

namespace LIMS.Web.Areas.Admin.Models.Customers
{
    public partial class BestCustomerReportLineModel : BaseModel
    {
        public string CustomerId { get; set; }
        [LIMSResourceDisplayName("Admin.Reports.Customers.BestBy.Fields.Customer")]
        public string CustomerName { get; set; }

        [LIMSResourceDisplayName("Admin.Reports.Customers.BestBy.Fields.OrderTotal")]
        public string OrderTotal { get; set; }

        [LIMSResourceDisplayName("Admin.Reports.Customers.BestBy.Fields.OrderCount")]
        public decimal OrderCount { get; set; }
    }
}