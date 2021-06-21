using LIMS.Api.Queries.Models.Customers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace LIMS.Api.Controllers.OData
{
    public class VaccineController:BaseODataController
    {
        private readonly IMediator _mediator;

        public VaccineController(
            IMediator mediator
          )
        {
            _mediator = mediator;

        }

        [SwaggerOperation(summary: "GetVaccine", OperationId = "Vaccine")]
        [HttpGet]
        public async Task<IActionResult> GetVaccine() {
            return Ok(await _mediator.Send(new GetVaccinationQuery()));
        }
    }
}
