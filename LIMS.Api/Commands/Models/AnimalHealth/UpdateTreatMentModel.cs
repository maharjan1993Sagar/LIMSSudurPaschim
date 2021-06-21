using LIMS.Api.DTOs.AnimalHealth;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Api.Commands.Models.AnimalHealth
{
    public class UpdateTreatMentModel: IRequest<TreatMentDto>
    {
        public  TreatMentDto Model { get; set; }
    }
}
