using LIMS.Services.Security;
using MediatR;

namespace LIMS.Services.Commands.Models.Security
{
    public class UninstallPermissionsCommand : IRequest<bool>
    {
        public IPermissionProvider PermissionProvider { get; set; }
    }
}
