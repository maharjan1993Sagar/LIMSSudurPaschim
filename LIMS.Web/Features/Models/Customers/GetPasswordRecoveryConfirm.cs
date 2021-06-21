using LIMS.Domain.Customers;
using LIMS.Web.Models.Customer;
using MediatR;

namespace LIMS.Web.Features.Models.Customers
{
    public class GetPasswordRecoveryConfirm : IRequest<PasswordRecoveryConfirmModel>
    {
        public Customer Customer { get; set; }
        public string Token { get; set; }
    }
}
