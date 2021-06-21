using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Api.DTOs.AnimalBreeding
{
    public class PregnencyTerminationListDto
    {
        public string Species { get; set; }
        public string Breed { get; set; }

        public string TerminationDate { get; set; }

        public string TerminationType { get; set; }

        public string Reason { get; set; }

        public string TerminitedBy { get; set; }
    }
}
