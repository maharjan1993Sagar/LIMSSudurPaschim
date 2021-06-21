using LIMS.Core.Caching;
using LIMS.Domain.Common;
using LIMS.Domain.Knowledgebase;
using LIMS.Domain.News;
using LIMS.Services.Customers;
using LIMS.Services.Knowledgebase;
using LIMS.Services.Localization;
using LIMS.Services.Seo;
using LIMS.Services.Topics;
using LIMS.Web.Features.Models.Common;
using LIMS.Web.Infrastructure.Cache;

using LIMS.Web.Models.Common;
using LIMS.Web.Models.Knowledgebase;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LIMS.Web.Features.Handlers.Common
{
    public class GetSitemapHandler : IRequestHandler<GetSitemap, SitemapModel>
    {
        private readonly ICacheManager _cacheManager;

        private readonly ITopicService _topicService;
        private readonly IKnowledgebaseService _knowledgebaseService;

        private readonly CommonSettings _commonSettings;
        private readonly NewsSettings _newsSettings;
        private readonly KnowledgebaseSettings _knowledgebaseSettings;

        public GetSitemapHandler(ICacheManager cacheManager,
            ITopicService topicService,
            IKnowledgebaseService knowledgebaseService,
            CommonSettings commonSettings,
            NewsSettings newsSettings,
            KnowledgebaseSettings knowledgebaseSettings)
        {
            _cacheManager = cacheManager;

            _topicService = topicService;
            _knowledgebaseService = knowledgebaseService;

            _commonSettings = commonSettings;
            _newsSettings = newsSettings;
            _knowledgebaseSettings = knowledgebaseSettings;
        }

        public async Task<SitemapModel> Handle(GetSitemap request, CancellationToken cancellationToken)
        {
            string cacheKey = string.Format(ModelCacheEventConst.SITEMAP_PAGE_MODEL_KEY,
                request.Language.Id,
                string.Join(",", request.Customer.GetCustomerRoleIds()),
                request.Store.Id);
            var cachedModel = await _cacheManager.GetAsync(cacheKey, async () =>
            {
                var model = new SitemapModel {
                    NewsEnabled = _newsSettings.Enabled,
                    KnowledgebaseEnabled = _knowledgebaseSettings.Enabled
                };

                //knowledgebase
                var knowledgebasearticles = (await _knowledgebaseService.GetPublicKnowledgebaseArticles()).ToList();
                model.KnowledgebaseArticles = knowledgebasearticles.Select(knowledgebasearticle => new KnowledgebaseItemModel {
                    Id = knowledgebasearticle.Id,
                    SeName = knowledgebasearticle.GetSeName(request.Language.Id),
                    Name = knowledgebasearticle.GetLocalized(x => x.Name, request.Language.Id)
                }).ToList();

                return model;
            });
            return cachedModel;
        }
    }
}
