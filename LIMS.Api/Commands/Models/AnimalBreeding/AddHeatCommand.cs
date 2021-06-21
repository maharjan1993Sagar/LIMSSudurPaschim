using LIMS.Api.DTOs.AnimalBreeding;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Api.Commands.Models.AnimalBreeding
{
   public class AddHeatCommand : IRequest<HeatRecordingDto>
    {
        public HeatRecordingDto Model { get; set; }
    }
}
