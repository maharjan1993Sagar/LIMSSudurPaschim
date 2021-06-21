using LIMS.Core.Models;

namespace LIMS.Web.Areas.Admin.Models.Customers
{
    public partial class CustomerReportsModel : BaseModel
    {
        public BestCustomersReportModel BestCustomersByOrderTotal { get; set; }
        public BestCustomersReportModel BestCustomersByNumberOfOrders { get; set; }
    }
}