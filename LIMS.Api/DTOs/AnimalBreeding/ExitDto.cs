using LIMS.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Api.DTOs.AnimalBreeding
{
    public class ExitDto: BaseApiEntityModel
    {
        public string AnimalRegistrationId { get; set; }
        public string DateOfExit { get; set; }

        public string ReasonForExit { get; set; }

        public string Comments { get; set; }
       
    }
}
