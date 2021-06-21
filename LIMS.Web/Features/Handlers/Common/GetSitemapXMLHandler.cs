using LIMS.Core.Caching;
using LIMS.Services.Customers;
using LIMS.Services.Seo;
using LIMS.Web.Features.Models.Common;
using LIMS.Web.Infrastructure.Cache;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace LIMS.Web.Features.Handlers.Common
{
    public class GetSitemapXMLHandler : IRequestHandler<GetSitemapXml, string>
    {
        private readonly ISitemapGenerator _sitemapGenerator;
        private readonly ICacheManager _cacheManager;

        public GetSitemapXMLHandler(ISitemapGenerator sitemapGenerator, ICacheManager cacheManager)
        {
            _sitemapGenerator = sitemapGenerator;
            _cacheManager = cacheManager;
        }

        public async Task<string> Handle(GetSitemapXml request, CancellationToken cancellationToken)
        {
            string cacheKey = string.Format(ModelCacheEventConst.SITEMAP_SEO_MODEL_KEY, request.Id,
                request.Language.Id,
                string.Join(",", request.Customer.GetCustomerRoleIds()),
                request.Store.Id);
            var siteMap = await _cacheManager.GetAsync(cacheKey, () => _sitemapGenerator.Generate(request.UrlHelper, request.Id, request.Language.Id, request.Store.Id));
            return siteMap;
        }
    }
}
