using LIMS.Api.Commands.Handlers.AnimalBreeding;
using LIMS.Api.Commands.Models.AnimalBreeding;
using LIMS.Api.DTOs;
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
    public class AiController:BaseODataController
    {
        private readonly IMediator _mediator;

        public AiController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [SwaggerOperation(summary: "Add new ", OperationId = "AddAi")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AIDto model)
        {
            if (ModelState.IsValid)
            {
                model = await _mediator.Send(new AddAiCommand() { Model = model });
                return Created(model);
            }
            return BadRequest(ModelState);
        }
        [SwaggerOperation(summary: "Get entities from Ai", OperationId = "GetAi")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _mediator.Send(new GetQueryModels<AiListDto>()));
        }
        [SwaggerOperation(summary: "Get Ai by animalId", OperationId = "animalId")]
        [ActionName("GetAiByAnimalId")]
        [Route("/[action]")]
        [HttpPost]
        public async Task<IActionResult> GetById(string animalId)
        {
           
              var  customer = await _mediator.Send(new GetQueryModels<AiListDto>() { AnimalId = animalId });

            
            if (customer == null)
                return NotFound();

            return Ok(customer);
        }

    }
}
