using LIMS.Framework.Components;
using LIMS.Services.Security;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Components
{
    public class CommonPopularSearchTermsReportViewComponent : BaseAdminViewComponent
    {
        private readonly IPermissionService _permissionService;

        public CommonPopularSearchTermsReportViewComponent(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (!await _permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return Content("");

            return View();
        }
    }
}