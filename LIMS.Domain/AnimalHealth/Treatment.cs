using LIMS.Domain.AInR;
using LIMS.Domain.BasicSetup;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.AnimalHealth
{
    public class TreatMent:BaseEntity
    {
        public TreatMent()
        {
            this.AnimalRegistration = new AnimalRegistration();
            this.Farm = new Farm();
            this.Disease = new Disease();

        }
        public AnimalRegistration AnimalRegistration { get; set; }
        public string AnimalRegistrationId { get; set; }
        public Farm Farm { get; set; }
        public string FarmId { get; set; }
        public Disease Disease { get; set; }
        public string DiseaseName { get; set; }
        public string Symptoms { get; set; }
        public string Medicine { get; set; }
        public DateTime? Date { get; set; }
     
        public string Description { get; set; }
     
        public string TechnicianName { get; set; }

        
        public string Temp { get; set; }
   
        public string Pulse { get; set; }

     
        public string Respiration { get; set; }

        public string ProvisionalDiagnosis { get; set; }
        public string Source { get; set; }
        public string CreatedBy { get; set; }
    }
}
