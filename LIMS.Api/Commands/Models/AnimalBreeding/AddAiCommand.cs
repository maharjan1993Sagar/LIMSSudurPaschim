using LIMS.Api.DTOs;
using LIMS.Api.DTOs.AnimalHealth;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Api.Commands.Models.AnimalBreeding
{
   public class AddAiCommand: IRequest<AIDto>
    {
        public AIDto Model { get; set; }
    }
}
