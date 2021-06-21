using LIMS.Api.Commands.Models.AnimalBreeding;
using LIMS.Api.DTOs.AnimalBreeding;
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
    public class HeatRecordingController:BaseODataController
    {
        private readonly IMediator _mediator;

        public HeatRecordingController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [SwaggerOperation(summary: "Add new ", OperationId = "AddHeatRecording")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] HeatRecordingDto model)
        {
            if (ModelState.IsValid)
            {
                model = await _mediator.Send(new AddHeatCommand() { Model = model });
                return Created(model);
            }
            return BadRequest(ModelState);
        }
        [SwaggerOperation(summary: "Get entities from Heat recording", OperationId = "GetHeatRecording")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _mediator.Send(new GetQueryModels<HeatRecordingDto>()));
        }
        [SwaggerOperation(summary: "Get HeatRecording by animalId", OperationId = "animalId")]
        [ActionName("GetHeatByAnimalId")]
        [Route("/[action]")]
        [HttpPost]
        public async Task<IActionResult> GetById(string animalId)
        {

            var customer = await _mediator.Send(new GetQueryModels<HeatRecordingDto>() { AnimalId = animalId });


            if (customer == null)
                return NotFound();

            return Ok(customer);
        }

    }
}
