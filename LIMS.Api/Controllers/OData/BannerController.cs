using LIMS.Api.DTOs.AINR;
using LIMS.Api.DTOs.GeneralCMS;
using LIMS.Api.Queries.Models.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace LIMS.Api.Controllers.OData
{
    [ApiController]
    [Area("Api")]
    [Route("[area]/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = false)]
    [SwaggerTag(description: "Get Banner")]
    public class BannerController : BaseODataController
    {
        private readonly IMediator _mediator;
        public BannerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetBanner(string userId)
        {
            return Ok(await _mediator.Send(new GetQueryCMS<BannerDto>() {UserId=userId }));
        }
    }
}
