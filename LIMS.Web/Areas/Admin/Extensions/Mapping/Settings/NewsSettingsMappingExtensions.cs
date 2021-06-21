using LIMS.Domain.News;
using LIMS.Web.Areas.Admin.Models.Settings;

namespace LIMS.Web.Areas.Admin.Extensions
{
    public static class NewsSettingsMappingExtensions
    {
        public static NewsSettingsModel ToModel(this NewsSettings entity)
        {
            return entity.MapTo<NewsSettings, NewsSettingsModel>();
        }
        public static NewsSettings ToEntity(this NewsSettingsModel model, NewsSettings destination)
        {
            return model.MapTo(destination);
        }
    }
}