using LIMS.Api.Commands.Models.Ainr;
using LIMS.Api.DTOs.FeedBack;
using LIMS.Api.Queries.Models.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace LIMS.Api.Controllers
{
    [AllowAnonymous]
    [IgnoreAntiforgeryToken]
    public class FeedbackController : BaseODataController
    {
        private readonly IMediator _mediator;

        public FeedbackController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [SwaggerOperation(summary: "Add new ", OperationId = "Feedback")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FeedbackDto model)
        {
            if (ModelState.IsValid)
            {
                model = await _mediator.Send(new AddFeedBackCommand() { FeedBack = model });
                return Created(model);
            }
            return BadRequest(ModelState);
        }

        [SwaggerOperation(summary: "Update feedback ", OperationId = "Feedback")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] FeedbackDto model)
        {
            if (ModelState.IsValid)
            {
                model = await _mediator.Send(new UpdateFeedbackCommand() { FeedBack = model });
                return Updated(model);
            }
            return BadRequest(ModelState);
        }

        [SwaggerOperation(summary: "Get all feedback ", OperationId = "Feedback")]
        [HttpGet]
        public async Task<IActionResult> GetFeedback ()
        {
            return Ok(await _mediator.Send(new GetQuery<FeedbackDto>()));
        }
    }
}
