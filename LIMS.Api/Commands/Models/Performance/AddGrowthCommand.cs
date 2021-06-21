using LIMS.Api.DTOs.PerformnceRecording;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Api.Commands.Models.Performance
{
    public class AddGrowthCommand: IRequest<GrowthMonitoringDto>
    {
        public GrowthMonitoringDto Model { get; set; }
    }
}
