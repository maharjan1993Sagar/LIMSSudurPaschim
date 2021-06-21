using LIMS.Web.Models.Customer;
using MediatR;
using System;

namespace LIMS.Web.Features.Models.Customers
{
    public class GetUserAgreement : IRequest<UserAgreementModel>
    {
        public Guid OrderItemId { get; set; }
    }
}
