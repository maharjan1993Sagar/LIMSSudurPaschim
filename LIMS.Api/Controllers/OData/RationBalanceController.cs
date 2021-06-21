using LIMS.Api.DTOs;
using LIMS.Api.Queries.Models.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Api.Controllers.OData
{
    public class RationBalanceController: BaseODataController
    {
        private readonly IMediator _mediator;

        public RationBalanceController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [SwaggerOperation(summary: "Get entities from RationBalance", OperationId = "GetRationBalance")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _mediator.Send(new GetQueryModels<RationBalanceDto>()));
        }
        
    }
}
