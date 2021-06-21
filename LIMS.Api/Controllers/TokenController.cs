using LIMS.Api.Commands.Models.Common;
using LIMS.Api.Models.Common;
using LIMS.Services.Customers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Api.Controllers
{
    [ApiController]
    [Area("Api")]
    [Route("[area]/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = false)]
    [SwaggerTag(description:"Create token")]
    public class TokenController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ICustomerService _customerService;
        public TokenController(IMediator mediator, ICustomerService customerService)
        {
            _mediator = mediator;
            _customerService = customerService;
            
        }

        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LoginModel model)
        {
            var claims = new Dictionary<string, string>();
            claims.Add("Email", model.Email);
            var user = await _customerService.GetCustomerByEmail(model.Email);
            if(user!=null)
            {
                if (user.EntityId == "Nlbo")
                {
                    claims.Add("Pprs", "true");
                }
                else
                {
                    claims.Add("Pprs", "false");
                }
                var roles = user.CustomerRoles.Select(m=>m.Name);
                string role =string.Join(",",roles);
                
                claims.Add("Roles", role);
            }
            
            var token = await _mediator.Send(new GenerateTokenCommand() { Claims = claims });
            return Ok(new { token = token });
        }
    }
}
