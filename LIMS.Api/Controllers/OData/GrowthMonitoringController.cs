using LIMS.Api.Commands.Models.Performance;
using LIMS.Api.DTOs.PerformnceRecording;
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
   public  class GrowthMonitoringController:BaseODataController
    {

        private readonly IMediator _mediator;

        public GrowthMonitoringController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [SwaggerOperation(summary: "Add new ", OperationId = "AddGrowthMonitoring")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GrowthMonitoringDto model)
        {
            if (ModelState.IsValid)
            {
                model = await _mediator.Send(new AddGrowthCommand() { Model = model });
                return Created(model);
            }
            return BadRequest(ModelState);
        }

        [SwaggerOperation(summary: "Update entity in Growth", OperationId = "UpdateGrowthMomitoring")]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] GrowthMonitoringDto model)
        {
            if (ModelState.IsValid)
            {
                model = await _mediator.Send(new UpdateGrowthCommand() { Model = model });
                return Updated(model);
            }
            return BadRequest(ModelState);
        }

        [SwaggerOperation(summary: "Get entities from Growth", OperationId = "GetGrowth")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _mediator.Send(new GetQueryModels<GrowthMonitoringDto>()));
        }
    }
}
