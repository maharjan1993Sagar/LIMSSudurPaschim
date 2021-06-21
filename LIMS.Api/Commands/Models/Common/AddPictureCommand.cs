using LIMS.Api.DTOs.Common;
using MediatR;

namespace LIMS.Api.Commands.Models.Common
{
    public class AddPictureCommand : IRequest<PictureDto>
    {
        public PictureDto PictureDto { get; set; }
    }
}
