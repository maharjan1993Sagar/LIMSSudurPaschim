using LIMS.Api.DTOs.Common;
using MediatR;

namespace LIMS.Api.Queries.Models.Common
{
    public class GetPictureByIdQuery : IRequest<PictureDto>
    {
        public string Id { get; set; }
    }
}
