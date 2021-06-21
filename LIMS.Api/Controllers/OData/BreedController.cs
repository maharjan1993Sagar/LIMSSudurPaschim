using LIMS.Api.DTOs.AINR;
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
    [SwaggerTag(description: "Get Breed")]
    public class BreedController : BaseODataController
    {
        private readonly IMediator _mediator;
        public BreedController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        [HttpGet]
        public async Task<IActionResult> GetBreed()
        {
            return Ok(await _mediator.Send(new GetQuery<BreedDto>()));
        }
    }
}
