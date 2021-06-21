using LIMS.Api.Commands.Models;
using LIMS.Api.Commands.Models.AnimalHealth;
using LIMS.Api.DTOs.AnimalHealth;
using LIMS.Api.Queries.Models.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;

namespace LIMS.Api.Controllers.OData
{
    public class VaccinationController : BaseODataController
    {
        private readonly IMediator _mediator;

        public VaccinationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [SwaggerOperation(summary: "GetVaccination", OperationId = "Vaccination")]
        [HttpGet]
        public async Task<IActionResult> GetVaccination()
        {
            try
            {
                var vaccinations = await _mediator.Send(new GetQueryModels<VaccinationDto>());
                return Ok(vaccinations);
            }
            catch (Exception e)
            {
                var f = e;
                return Ok();
            }
        }

        [SwaggerOperation(summary: "Add new ", OperationId = "AddVaccination")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] VaccinationDto model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                   model = await _mediator.Send(new AddVaccinationCommand() { VaccinationDto = model });
                    return Ok(model);
                }
                catch (Exception e)
                {
                    return Ok(e);
                }
            }
            return BadRequest(ModelState);
        }

        [SwaggerOperation(summary: "Update entity in Vaccination", OperationId = "UpdateVaccination")]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] VaccinationDto model)
        {
            if (ModelState.IsValid)
            {
                model = await _mediator.Send(new UpdateVaccinationCommand() { vaccinationDto = model });
                return Updated(model);
            }
            return BadRequest(ModelState);
        }
    }
}
