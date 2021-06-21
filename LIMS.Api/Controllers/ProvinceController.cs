using LIMS.Api.DTOs.LocalStructure;
using LIMS.Services.LocalStructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Api.Controllers
{
    [ApiController]
    [Area("Api")]
    [Route("[area]/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = false)]
    [SwaggerTag(description: "Get Province")]
    public class ProvinceController : Controller
    {
        private readonly ILocalLevelService _localLevelService;

        public ProvinceController(ILocalLevelService localLevelService)
        {
            _localLevelService = localLevelService;
        }

        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        [HttpGet]
        public async Task<IActionResult> GetProvince()
        {
            var locallevels = await _localLevelService.GetAllProvience();
            var localDtos = new List<LocalDto>();
            var provinces = locallevels.Select(m => m.Province).Distinct().Where(m => m != null && m != "");
            foreach (var item in provinces)
            {
                var districts = locallevels.Where(m => m.Province == item).Select(m => m.District).Distinct();
                var districtList = new List<Districts>();
                foreach (var district in districts)
                {
                    var locallevel = locallevels.Where(m => m.District == district).Select(m => m.Municipality).ToList();
                    var dis = new Districts();
                    dis.District = district;
                    dis.LocalLevel = locallevel;
                    districtList.Add(dis);
                }
                localDtos.Add(new LocalDto {
                    Province = item,
                    Districts = districtList
                });
            }
            return Ok(localDtos);
        }
    }
}
