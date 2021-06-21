using LIMS.Domain.Customers;
using LIMS.Domain.Stores;
using LIMS.Web.Models.Common;
using MediatR;
using System.Collections.Generic;

namespace LIMS.Web.Features.Models.Common
{
    public class GetPrivacyPreference : IRequest<IList<PrivacyPreferenceModel>>
    {
        public Customer Customer { get; set; }
        public Store Store { get; set; }
    }
}
