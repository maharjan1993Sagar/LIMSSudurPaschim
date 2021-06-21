using LIMS.Domain.Customers;
using LIMS.Web.Areas.Admin.Models.Settings;

namespace LIMS.Web.Areas.Admin.Extensions
{
    public static class CustomerSettingsMappingExtensions
    {
        public static CustomerUserSettingsModel.CustomerSettingsModel ToModel(this CustomerSettings entity)
        {
            return entity.MapTo<CustomerSettings, CustomerUserSettingsModel.CustomerSettingsModel>();
        }
        public static CustomerSettings ToEntity(this CustomerUserSettingsModel.CustomerSettingsModel model, CustomerSettings destination)
        {
            return model.MapTo(destination);
        }
    }
}