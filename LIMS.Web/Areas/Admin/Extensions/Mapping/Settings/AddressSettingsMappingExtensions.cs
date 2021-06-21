using LIMS.Domain.Common;
using LIMS.Web.Areas.Admin.Models.Settings;

namespace LIMS.Web.Areas.Admin.Extensions
{
    public static class AddressSettingsMappingExtensions
    {
        public static CustomerUserSettingsModel.AddressSettingsModel ToModel(this AddressSettings entity)
        {
            return entity.MapTo<AddressSettings, CustomerUserSettingsModel.AddressSettingsModel>();
        }
        public static AddressSettings ToEntity(this CustomerUserSettingsModel.AddressSettingsModel model, AddressSettings destination)
        {
            return model.MapTo(destination);
        }
    }
}