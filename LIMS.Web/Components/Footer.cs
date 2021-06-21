using LIMS.Core;
using LIMS.Domain.Common;
using LIMS.Domain.Knowledgebase;
using LIMS.Domain.News;
using LIMS.Domain.Stores;
using LIMS.Framework.Components;
using LIMS.Services.Localization;
using LIMS.Services.Security;
using LIMS.Services.Seo;
using LIMS.Services.Topics;
using LIMS.Web.Models.Common;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.ViewComponents
{
    public class FooterViewComponent : BaseViewComponent
    {
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly ITopicService _topicService;
        private readonly IPermissionService _permissionService;

        private readonly StoreInformationSettings _storeInformationSettings;
        private readonly KnowledgebaseSettings _knowledgebaseSettings;
        private readonly NewsSettings _newsSettings;
        private readonly CommonSettings _commonSettings;

        public FooterViewComponent(
            IWorkContext workContext,
            IStoreContext storeContext,
            ITopicService topicService,
            IPermissionService permissionService,
            StoreInformationSettings storeInformationSettings,
            KnowledgebaseSettings knowledgebaseSettings,
            NewsSettings newsSettings,
            CommonSettings commonSettings)
        {
            _workContext = workContext;
            _storeContext = storeContext;
            _topicService = topicService;
            _permissionService = permissionService;

            _storeInformationSettings = storeInformationSettings;
            _knowledgebaseSettings = knowledgebaseSettings;
            _newsSettings = newsSettings;
            _commonSettings = commonSettings;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await PrepareFooter();
            return View(model);
        }
        private async Task<FooterModel> PrepareFooter()
        {
            var topicModel = (await _topicService.GetAllTopics(_storeContext.CurrentStore.Id))
                .Where(t => (t.IncludeInFooterRow1 || t.IncludeInFooterRow2 || t.IncludeInFooterRow3) && t.Published)
                .Select(t => new FooterModel.FooterTopicModel {
                    Id = t.Id,
                    Name = t.GetLocalized(x => x.Title, _workContext.WorkingLanguage.Id),
                    SeName = t.GetSeName(_workContext.WorkingLanguage.Id),
                    IncludeInFooterRow1 = t.IncludeInFooterRow1,
                    IncludeInFooterRow2 = t.IncludeInFooterRow2,
                    IncludeInFooterRow3 = t.IncludeInFooterRow3
                }).ToList();

            //model
            var currentstore = _storeContext.CurrentStore;
            var model = new FooterModel {
                StoreName = currentstore.GetLocalized(x => x.Name, _workContext.WorkingLanguage.Id),
                CompanyName = currentstore.CompanyName,
                CompanyEmail = currentstore.CompanyEmail,
                CompanyAddress = currentstore.CompanyAddress,
                CompanyPhone = currentstore.CompanyPhoneNumber,
                CompanyHours = currentstore.CompanyHours,
                PrivacyPreference = _storeInformationSettings.DisplayPrivacyPreference,
                WishlistEnabled = await _permissionService.Authorize(StandardPermissionProvider.EnableWishlist),
                ShoppingCartEnabled = await _permissionService.Authorize(StandardPermissionProvider.EnableShoppingCart),
                SitemapEnabled = _commonSettings.SitemapEnabled,
                WorkingLanguageId = _workContext.WorkingLanguage.Id,
                FacebookLink = _storeInformationSettings.FacebookLink,
                TwitterLink = _storeInformationSettings.TwitterLink,
                YoutubeLink = _storeInformationSettings.YoutubeLink,
                InstagramLink = _storeInformationSettings.InstagramLink,
                LinkedInLink = _storeInformationSettings.LinkedInLink,
                PinterestLink = _storeInformationSettings.PinterestLink,
                KnowledgebaseEnabled = _knowledgebaseSettings.Enabled,
                NewsEnabled = _newsSettings.Enabled,
                Topics = topicModel
            };

            return model;
        }


    }
}