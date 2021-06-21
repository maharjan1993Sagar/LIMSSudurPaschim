using LIMS.Domain.AInR;
using LIMS.Domain.BesicSetup;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Services
{
    public class PregnencyTermination:BaseEntity
    {
        public PregnencyTermination()
        {
            this.PregnencyTerminationId = Guid.NewGuid();
            this.AnimalRegistration = new AnimalRegistration();
            this.FiscalYear = new FiscalYear();
        }
        public Guid PregnencyTerminationId { get; set; }
        public AnimalRegistration AnimalRegistration { get; set; }
       
    
        public string TerminationDate { get; set; }
       

        public string TerminationType { get; set; }

        public string Reason { get; set; }

        public string TerminitedBy { get; set; }
       
        public FiscalYear FiscalYear { get; set; }
        public string Source { get; set; }
        public string CreatedBy { get; set; }
        public string EntityId { get; set; }
    }
}
