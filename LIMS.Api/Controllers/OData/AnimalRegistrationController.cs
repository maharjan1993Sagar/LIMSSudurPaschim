using LIMS.Api.Commands.Models.Ainr;
using LIMS.Api.DTOs.AINR;
using LIMS.Api.DTOs.AnimalBreeding;
using LIMS.Api.Queries.Models.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace LIMS.Api.Controllers.OData
{
    public class AnimalRegistrationController : BaseODataController
    {
        private readonly IMediator _mediator;

        public AnimalRegistrationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [SwaggerOperation(summary: "Add new ", OperationId = "AddAnimalRegistration")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AnimalRegistrationDto model)
        {
            if (ModelState.IsValid)
            {
                model = await _mediator.Send(new AddAnimalCommand() { Model = model });
                return Created(model);
            }
            return BadRequest(ModelState);
        }

        [SwaggerOperation(summary: "Update entity in Animal", OperationId = "UpdateAnimal")]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] AnimalRegistrationDto model)
        {
            if (ModelState.IsValid)
            {
                model = await _mediator.Send(new UpdateAnimalCommand() { Model = model });
                return Updated(model);
            }
            return BadRequest(ModelState);
        }

        [SwaggerOperation(summary: "Get entities from Animal", OperationId = "GetAnimal")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _mediator.Send(new GetQueryModels<AnimalListDto>()));
        }
        [SwaggerOperation(summary: "Get entity from Animal with farmid", OperationId = "Farmid")]
        [ActionName("GetByAnimalId")]
        [Route("/[action]")]
        [HttpPost]
        public async Task<IActionResult> GetById(GetAnimals animals)
        {
            var customer = (dynamic)null;
            if (!string.IsNullOrEmpty(animals.FarmId))
            {
                customer = await _mediator.Send(new GetQueryModels<AnimalListDto>() { FarmId = animals.FarmId });
            }
            else
            {
                customer = await _mediator.Send(new GetQueryModels<AnimalListDto>() { AnimalId = animals.AnimalId });

            }
            if (customer == null)
                return NotFound();

            return Ok(customer);
        }
        [SwaggerOperation(summary: "Exit animal", OperationId = "exitanimal")]
        [ActionName("ExitAnimal")]
        [Route("/[action]")]
        [HttpPost]
        public async Task<IActionResult> exitAnimalId([FromBody]ExitDto exit)
        {
            
                var customer = await _mediator.Send(new ExitQueryHandler<ExitDto>() {Model=exit });
            
            
       

            return Ok(customer);
        }

    }
}
