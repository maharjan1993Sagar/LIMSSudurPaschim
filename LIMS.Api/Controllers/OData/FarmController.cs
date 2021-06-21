using LIMS.Api.Commands.Models.Ainr;
using LIMS.Api.DTOs.AINR;
using LIMS.Api.Queries.Models.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace LIMS.Api.Controllers.OData
{
    public class FarmController : BaseODataController
    {
        private readonly IMediator _mediator;

        public FarmController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [SwaggerOperation(summary: "Add new ", OperationId = "AddFarm")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] FarmDto model)
        {
            if (ModelState.IsValid)
            {
                model = await _mediator.Send(new AddFarmCommand() { FarmDto = model });
                return Created(model);
            }
            return BadRequest(ModelState);
        }

        [SwaggerOperation(summary: "Update entity in Farm", OperationId = "UpdateFarm")]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] FarmDto model)
        {
            if (ModelState.IsValid)
            {
                model = await _mediator.Send(new UpdateFarmCommand() { FarmDto = model });
                return Updated(model);
            }
            return BadRequest(ModelState);
        }

        [SwaggerOperation(summary: "Get entities from Farm", OperationId = "GetFarm")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _mediator.Send(new GetQuery<FarmDto>()));
        }
    }
}
