using LIMS.Domain.Customers;
using LIMS.Domain.Localization;
using LIMS.Domain.Stores;
using LIMS.Web.Models.Common;
using MediatR;

namespace LIMS.Web.Features.Models.Common
{
    public class GetSitemap : IRequest<SitemapModel>
    {
        public Customer Customer { get; set; }
        public Store Store { get; set; }
        public Language Language { get; set; }
    }
}
