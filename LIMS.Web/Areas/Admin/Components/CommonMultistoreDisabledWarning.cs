using LIMS.Framework.Components;
using LIMS.Services.Configuration;
using LIMS.Services.Stores;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Components
{
    public class CommonMultistoreDisabledWarningViewComponent : BaseAdminViewComponent
    {
        private readonly ISettingService _settingService;
        private readonly IStoreService _storeService;
       
        public CommonMultistoreDisabledWarningViewComponent(ISettingService settingService, IStoreService storeService)
        {
            _settingService = settingService;
            _storeService = storeService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //action displaying notification (warning) to a store owner that "limit per store" feature is ignored
            //default setting
          

            return View();
        }
    }
}