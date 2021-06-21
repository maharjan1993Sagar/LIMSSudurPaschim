using LIMS.Api.DTOs.PerformnceRecording;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Api.Commands.Models.Performance
{
    public class UpdateMilkCOmmand: IRequest<MilkRecordingDto>
    {
        public MilkRecordingDto MilkDto { get; set; }
    }
}
