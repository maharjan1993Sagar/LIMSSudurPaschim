using LIMS.Core;
using LIMS.Core.Data;
using LIMS.Services.Installation;
using LIMS.Web.Models.Upgrade;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LIMS.Web.Controllers
{
    public partial class UpgradeController : BasePublicController
    {
        #region Fields

        private readonly IUpgradeService _upgradeService;

        #endregion

        #region Ctor

        public UpgradeController(IUpgradeService upgradeService)
        {
            _upgradeService = upgradeService;
        }
        #endregion

        public virtual IActionResult Index()
        {
            if (!DataSettingsHelper.DatabaseIsInstalled())
                return RedirectToRoute("Install");

            var model = new UpgradeModel {
                ApplicationVersion = LIMSVersion.FullVersion,
                ApplicationDBVersion = LIMSVersion.SupportedDBVersion,
                DatabaseVersion = _upgradeService.DatabaseVersion()
            };

            if (model.ApplicationDBVersion == model.DatabaseVersion)
                return RedirectToRoute("Homepage");

            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Index(UpgradeModel m, [FromServices] IWebHelper webHelper)
        {
            var model = new UpgradeModel {
                ApplicationDBVersion = LIMSVersion.SupportedDBVersion,
                DatabaseVersion = _upgradeService.DatabaseVersion()
            };

            if (model.ApplicationDBVersion != model.DatabaseVersion)
            {
                await _upgradeService.UpgradeData(model.DatabaseVersion, model.ApplicationDBVersion);
            }
            else
                return RedirectToRoute("HomePage");

            //restart application
            webHelper.RestartAppDomain();

            //Redirect to home page
            return RedirectToRoute("HomePage");

        }
    }
}