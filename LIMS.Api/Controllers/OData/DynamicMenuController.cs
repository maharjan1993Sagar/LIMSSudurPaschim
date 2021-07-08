using LIMS.Api.DTOs.GeneralCMS;
using LIMS.Api.DTOs.RationBalance;
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
    [SwaggerTag(description: "Get Dynamic Menu")]
    public class DynamicMenuController:BaseODataController
    {
        private readonly IMediator _mediator;

        public DynamicMenuController(IMediator mediator)
        {
            _mediator = mediator;
        }
        //[SwaggerOperation(summary: "Get Dynamic Menu by User Id", OperationId = "GetDynamicMenu")]
        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        [HttpGet("{userId}")]
        public async Task<IActionResult> Get(string userId)
        {
            return Ok(await _mediator.Send(new GetQueryCMS<MainMenuDto> {UserId=userId }));
        }
    }
}
