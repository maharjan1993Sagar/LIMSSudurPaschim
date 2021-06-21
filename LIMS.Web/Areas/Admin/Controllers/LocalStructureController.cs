using LIMS.Services.LocalStructure;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class LocalStructureController : BaseAdminController
    {
        #region fields
        public readonly ILocalLevelService _localLevelService;
        public LocalStructureController(ILocalLevelService localLevelService)
        {
            _localLevelService = localLevelService;
        }
        #endregion
        [HttpGet]
        public async Task<IActionResult> GetProvince()
        {
            var province = await _localLevelService.GetProvience();
            return Json(province);
        }
        [HttpGet]
        public async Task<IActionResult> GetDistrict(string province)
        {
            var district = await _localLevelService.GetDistrict(province);
            return Json(district);
        }
        [HttpGet]
        public async Task<IActionResult> GetLocalLevel(string district)
        {
            var localLevel = await _localLevelService.GetLocalLevel(district);
            return Json(localLevel);
        }
    }
}
