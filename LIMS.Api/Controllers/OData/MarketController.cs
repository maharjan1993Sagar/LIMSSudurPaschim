using LIMS.Api.DTOs;
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
        [SwaggerTag(description: "Get Market")]
         [Area("Api")]
       [Route("[area]/[controller]")]
    public class MarketController : BaseODataController
        {
            private readonly IMediator _mediator;
            public MarketController(IMediator mediator)
            {
                _mediator = mediator;
            }

            [AllowAnonymous]
            [IgnoreAntiforgeryToken]
            [HttpGet]
            public async Task<IActionResult> GetMarket()
            {
                return Ok(await _mediator.Send(new GetQueryCMS<MarketDto>() ));
            }
        }
}
