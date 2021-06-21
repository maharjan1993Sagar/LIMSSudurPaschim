using LIMS.Api.DTOs.Common;
using LIMS.Api.Queries.Models.Common;
using LIMS.Services.Security;
using MediatR;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Api.Controllers.OData
{
    public partial class LanguageController : BaseODataController
    {
        private readonly IMediator _mediator;
        private readonly IPermissionService _permissionService;

        public LanguageController(IMediator mediator, IPermissionService permissionService)
        {
            _mediator = mediator;
            _permissionService = permissionService;
        }

        [SwaggerOperation(summary: "Get entity from Languages by key", OperationId = "GetLanguageById")]
        [HttpGet("{key}")]
        public async Task<IActionResult> Get(string key)
        {
            if (!await _permissionService.Authorize(PermissionSystemName.Languages))
                return Forbid();

            var language = await _mediator.Send(new GetQuery<LanguageDto>() { Id = key });
            if (!language.Any())
                return NotFound();

            return Ok(language.FirstOrDefault());
        }

        [SwaggerOperation(summary: "Get entities from Languages", OperationId = "GetLanguages")]
        [HttpGet]
        [EnableQuery(HandleNullPropagation = HandleNullPropagationOption.False)]
        public async Task<IActionResult> Get()
        {
            if (!await _permissionService.Authorize(PermissionSystemName.Languages))
                return Forbid();

            return Ok(await _mediator.Send(new GetQuery<LanguageDto>()));
        }
    }
}
