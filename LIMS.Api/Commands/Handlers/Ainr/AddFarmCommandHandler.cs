using LIMS.Api.Commands.Models.Ainr;
using LIMS.Api.DTOs.AINR;
using LIMS.Api.Extensions;
using LIMS.Core;
using LIMS.Services.Ainr;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace LIMS.Api.Commands.Handlers.Ainr
{
    public class AddFarmCommandHandler : IRequestHandler<AddFarmCommand,FarmDto>
    {
        private readonly IFarmService _farmService;
        private readonly IWorkContext _workContext;

        public AddFarmCommandHandler(
            IFarmService farmService,
            IWorkContext workContext)
        {
            _farmService = farmService;
            _workContext = workContext;
        }

        public async Task<FarmDto> Handle(AddFarmCommand request, CancellationToken cancellationToken)
        {
            var farm = request.FarmDto.ToEntity();
            farm.Source = _workContext.CurrentCustomer.OrgName + "From mobile";
            farm.CreatedBy = _workContext.CurrentCustomer.Id;
            await _farmService.InsertFarm(farm);

            //activity log
         
            return farm.ToModel();
        }
    }
}

