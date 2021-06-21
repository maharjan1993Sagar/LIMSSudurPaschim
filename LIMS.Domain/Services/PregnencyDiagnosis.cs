using LIMS.Domain.AInR;
using LIMS.Domain.BesicSetup;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Services
{
    public class PregnencyDiagnosis:BaseEntity
    {
        public PregnencyDiagnosis()
        {
            this.PregnencyDiagnosisGuid = Guid.NewGuid();
            this.AnimalRegistration = new AnimalRegistration();
        }
        public Guid PregnencyDiagnosisGuid { get; set; }
        public AnimalRegistration AnimalRegistration { get; set; }
        public string AnimalRegistrationId { get; set; }
        public string FarmId { get; set; }
        public Farm Farm { get; set; }

        public string ServiceType { get; set; }
        public string Source { get; set; }


        public string ServiceName { get; set; }
 

        public DateTime Date { get; set; }
        

        public string Result { get; set; }
       
        public string Technician { get; set; }
 
        public FiscalYear FiscalYear { get; set; }
        public string NaturalService { get; set; }
        public string OtherServiceProviced { get; set; }
      

        public string Reason { get; set; }
        public string EntityId { get; set; }
        public string CreatedBy { get; set; }
        
    }
}
