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
    public class MilkRecordingController:BaseODataController
    {
        private readonly IMediator _mediator;

        public MilkRecordingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [SwaggerOperation(summary: "Add new ", OperationId = "AddMilkRecording")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MilkRecordingDto model)
        {
            if (ModelState.IsValid)
            {
                model = await _mediator.Send(new AddMilkCommand() { MilkDto = model });
                return Created(model);
            }
            return BadRequest(ModelState);
        }

        [SwaggerOperation(summary: "Update entity in Milk", OperationId = "UpdateMilkRecording")]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] MilkRecordingDto model)
        {
            if (ModelState.IsValid)
            {
                model = await _mediator.Send(new UpdateMilkCOmmand() { MilkDto = model });
                return Updated(model);
            }
            return BadRequest(ModelState);
        }

        [SwaggerOperation(summary: "Get entities from Milk", OperationId = "GetMilk")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _mediator.Send(new GetQueryModels<MilkRecordingDto>()));
        }

    }
}
