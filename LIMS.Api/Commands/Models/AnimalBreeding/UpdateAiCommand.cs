using LIMS.Api.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Api.Commands.Models.AnimalBreeding
{
    public class UpdateAiCommand:IRequest<AIDto>
    {
        public AIDto Model { get; set; }

    }
}
