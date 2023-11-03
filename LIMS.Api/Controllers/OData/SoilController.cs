using LIMS.Api.DTOs.GeneralCMS;
using LIMS.Api.Queries.Models.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Api.Controllers.OData
{


        [ApiController]
        [Area("Api")]
        [Route("[area]/[controller]/[action]")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [SwaggerTag(description: "Get resources")]
        public class SoilController : BaseODataController
        {
            private readonly IMediator _mediator;
            public SoilController(IMediator mediator)
            {
                _mediator = mediator;
            }

            [AllowAnonymous]
            [IgnoreAntiforgeryToken]
            [HttpGet("{userId}")]
            public async Task<IActionResult> GetResources(string userId)
            {
                return Ok(await _mediator.Send(new GetQueryCMS<SoilDto>() { UserId = userId }));
            }
        }
    }

