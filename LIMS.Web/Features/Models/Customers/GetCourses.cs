using LIMS.Domain.Customers;
using LIMS.Domain.Stores;
using LIMS.Web.Models.Customer;
using MediatR;

namespace LIMS.Web.Features.Models.Customers
{
    public class GetCourses : IRequest<CoursesModel>
    {
        public Customer Customer { get; set; }
        public Store Store { get; set; }
    }
}
