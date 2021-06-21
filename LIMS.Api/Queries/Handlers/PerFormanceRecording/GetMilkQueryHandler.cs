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
    public class GetMilkQueryHandler: IRequestHandler<GetQueryModels<MilkRecordingDto>, IList<MilkRecordingDto>>
    {
        private readonly IMilkRecordingService _milkRecordingService;

        public GetMilkQueryHandler(IMilkRecordingService milkRecording)
        {
            _milkRecordingService = milkRecording;
        }

        public async Task<IList<MilkRecordingDto>> Handle(GetQueryModels<MilkRecordingDto> request, CancellationToken cancellationToken)
        {
            var Milk = await _milkRecordingService.GetMilkRecording();
            var result = new List<MilkRecordingDto>();
            foreach (var item in Milk)
            {
                result.Add(item.ToModel());
            }
            return result;
        }

    }
}
