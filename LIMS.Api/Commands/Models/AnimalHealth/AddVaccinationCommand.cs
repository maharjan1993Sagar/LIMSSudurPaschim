using LIMS.Api.DTOs.AnimalHealth;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Api.Commands.Models
{
   public class AddVaccinationCommand:IRequest<VaccinationDto>
    {
        public VaccinationDto VaccinationDto { get; set; }
    }
}
