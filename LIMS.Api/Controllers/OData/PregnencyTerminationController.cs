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
    public class PregnencyTerminationController:BaseODataController
    {
        private readonly IMediator _mediator;

        public PregnencyTerminationController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [SwaggerOperation(summary: "Add new ", OperationId = "AddPregnencyTermination")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PregnencyTerminationDto model)
        {
            if (ModelState.IsValid)
            {
                model = await _mediator.Send(new AddPregnencyTerminationCommand() { Model = model });
                return Created(model);
            }
            return BadRequest(ModelState);
        }
        [SwaggerOperation(summary: "Get entities from Pregnency termination ", OperationId = "GetPregnencyTermination")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _mediator.Send(new GetQueryModels<PregnencyTerminationListDto>()));
        }
        [SwaggerOperation(summary: "Get Pregnency termination by animalId", OperationId = "animalId")]
        [ActionName("GetPregnencyTerminationByAnimalId")]
        [Route("/[action]")]
        [HttpPost]
        public async Task<IActionResult> GetById(string animalId)
        {

            var customer = await _mediator.Send(new GetQueryModels<PregnencyTerminationListDto>() { AnimalId = animalId });


            if (customer == null)
                return NotFound();

            return Ok(customer);
        }

    }
}
