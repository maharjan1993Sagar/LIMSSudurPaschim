using LIMS.Api.DTOs.Common;
using MediatR;

namespace LIMS.Api.Commands.Models.Common
{
    public class DeletePictureCommand : IRequest<bool>
    {
        public PictureDto PictureDto { get; set; }
    }
}
