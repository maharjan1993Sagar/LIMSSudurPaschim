using LIMS.Api.Commands.Models.Ainr;
using LIMS.Api.DTOs.AINR;
using LIMS.Api.Extensions;
using LIMS.Services.Ainr;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace LIMS.Api.Commands.Handlers.Ainr
{
    public class UpdateFarmCommandHandler : IRequestHandler<UpdateFarmCommand, FarmDto>
    {
        private readonly IFarmService _farmService;

        public UpdateFarmCommandHandler(
            IFarmService farmService)
        {
            _farmService = farmService;
        }

        public async Task<FarmDto> Handle(UpdateFarmCommand request, CancellationToken cancellationToken)
        {
            var farm = await _farmService.GetFarmById(request.FarmDto.Id);
            if (farm != null)
            {
                farm = request.FarmDto.ToEntity(farm);
                await _farmService.UpdateFarm(farm);

                //activity log

                return farm.ToModel();
            }
            return null;
        }
    }
}
