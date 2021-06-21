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
    [SwaggerTag(description: "Get species")]
    public class SpeciesController:BaseODataController
    {
            private readonly IMediator _mediator;
            public SpeciesController(IMediator mediator)
            {
                _mediator = mediator;
            }

            [AllowAnonymous]
            [IgnoreAntiforgeryToken]
            [HttpGet]
            public async Task<IActionResult> GetSpecies()
            {
                return Ok(await _mediator.Send(new GetQuery<SpeciesDto>()));
            }
        }
    }
