using LIMS.Api.Commands.Models.AnimalHealth;
using LIMS.Api.DTOs.AnimalHealth;
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
    public class TreatmentController: BaseODataController
    {
        private readonly IMediator _mediator;

        public TreatmentController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [SwaggerOperation(summary: "Add new ", OperationId = "AddTreatMent")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TreatMentDto model)
        {
            if (ModelState.IsValid)
            {
                model = await _mediator.Send(new AddTreatMentModel() { Model = model });
                return Created(model);
            }
            return BadRequest(ModelState);
        }
        [SwaggerOperation(summary: "Get entities from TreatMent", OperationId = "GetTreatment")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _mediator.Send(new GetQueryModels<TreatMentDto>()));
        }

    }
}
