using LIMS.Api.DTOs.PerformnceRecording;
using LIMS.Api.Extensions;
using LIMS.Api.Queries.Models.Common;
using LIMS.Services.Recording;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LIMS.Api.Queries.Handlers.PerFormanceRecording
{
    public class GetGrowthQueryHandler: IRequestHandler<GetQueryModels<GrowthMonitoringDto>, IList<GrowthMonitoringDto>>
    {
        private readonly IGrowthMonitoringService _growthMonitoringService;

        public GetGrowthQueryHandler(IGrowthMonitoringService growthMonitoringService)
        {
            _growthMonitoringService = growthMonitoringService;
        }

        public async Task<IList<GrowthMonitoringDto>> Handle(GetQueryModels<GrowthMonitoringDto> request, CancellationToken cancellationToken)
        {
            var growth = await _growthMonitoringService.GetGrowthMonitoring();
            var result = new List<GrowthMonitoringDto>();
            foreach (var item in growth)
            {
                result.Add(item.ToModel());
            }
            return result;
        }

    }
}
