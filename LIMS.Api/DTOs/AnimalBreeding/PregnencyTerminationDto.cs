using LIMS.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Api.DTOs.AnimalBreeding
{
    public class PregnencyTerminationDto: BaseApiEntityModel
    {
        public string AnimalId { get; set; }
        
        public string TerminationDate { get; set; }

        public string TerminationType { get; set; }

        public string Reason { get; set; }

        public string TerminitedBy { get; set; }
        public string FiscalYearId { get; set; }

    }
}
