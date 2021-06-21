using LIMS.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Api.DTOs.AnimalBreeding
{
    public class PregnencyDiagnosisDto : BaseApiEntityModel
    {
        public string Method { get; set; }
        
        public DateTime Date { get; set; }

        public string Result { get; set; }
        public string Technician { get; set; }

        public string FiscalYearId { get; set; }
        public string Reason { get; set; }
        public string FarmName { get; set; }
        public string SpeciesId { get; set; }
        public string BreedId { get; set; }
        public string AnimalName { get; set; }
        public string Eartag { get; set; }

        public string AnimalId { get; set; }

        public string MobileNo { get; set; }

        public string FarmId { get; set; }
    
    }
}
