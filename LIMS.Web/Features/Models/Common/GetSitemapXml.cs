using LIMS.Domain.Customers;
using LIMS.Domain.Localization;
using LIMS.Domain.Stores;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LIMS.Web.Features.Models.Common
{
    public class GetSitemapXml : IRequest<string>
    {
        public int? Id { get; set; }
        public Customer Customer { get; set; }
        public Store Store { get; set; }
        public Language Language { get; set; }
        public IUrlHelper UrlHelper { get; set; }
    }
}
