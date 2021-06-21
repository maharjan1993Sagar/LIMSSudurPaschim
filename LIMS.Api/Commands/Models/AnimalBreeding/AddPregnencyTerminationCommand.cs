using LIMS.Api.DTOs.AnimalBreeding;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Api.Commands.Models.AnimalBreeding
{
    public class AddPregnencyTerminationCommand: IRequest<PregnencyTerminationDto>
    {
        public PregnencyTerminationDto Model { get; set; }
    }
}
