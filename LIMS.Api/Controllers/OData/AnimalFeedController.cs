using LIMS.Api.DTOs.RationBalance;
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
    public class AnimalFeedController:BaseODataController
    {
        private readonly IMediator _mediator;

        public AnimalFeedController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [SwaggerOperation(summary: "Get Feed from AnimalFeed", OperationId = "GetFeed")]
        [HttpGet]
        public async Task<IActionResult> GetFeed()
        {
            return Ok(await _mediator.Send(new GetQueryModels<AnimalFeedDto>()));
        }
    }
}
