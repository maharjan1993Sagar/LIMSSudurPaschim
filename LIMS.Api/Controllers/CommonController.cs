using LIMS.Api.DTOs.AINR;
using LIMS.Services.Ainr;
using LIMS.Services.Customers;
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
    [SwaggerTag(description: "Get Role")]
    public class CommonController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IFarmService _farmService;

        public CommonController(ICustomerService customerService, IFarmService farmService)
        {
            _customerService = customerService;
            _farmService = farmService;
        }

        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        [HttpGet]
        public async Task<IActionResult> GetRole()
        {
            return Ok(await _customerService.GetAllCustomerRoles(showHidden: true));
        }
        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        [HttpGet]
        public async Task<IActionResult> GetFarm()
        {
            var farm = await _farmService.SearchFarm();
            var farmandfarmer = farm.Where(n=>n.Category!=null).Where(m => m.Category.ToLower() == "farm" || m.Category.ToLower() == "farmer");
            var FarmList = new List<FarmListDto>();
           foreach(var item in farmandfarmer)
            {
                FarmList.Add(new FarmListDto() {
                    Id=item.Id,
                    Name=item.NameEnglish,
                    Address=item.Provience+" "+item.District+" "+item.LocalLevel
                });
            }
            return Ok(FarmList);
        }
    }
}
