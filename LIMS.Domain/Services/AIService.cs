using LIMS.Domain.AInR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Services
{
   public class AIService : BaseEntity
    {
        public AIService()
        {
            this.AIGuid = Guid.NewGuid();
            this.AnimalRegistration = new AnimalRegistration();
        }
        public Guid AIGuid { get; set; }
        public AnimalRegistration AnimalRegistration { get; set; }

        public Farm Farm { get; set; }
        public string AnimalRegistrationId { get; set; }
        public string FarmId { get; set; }

        public string AIDate { get; set; }
     
        public string SemenNo { get; set; }
       

        public string BullId { get; set; }
    
        public string AmountReceived { get; set; }
    
        public string ReceiptNo { get; set; }
       

        public string NoofAiDone { get; set; }
       
        public string FiscalYear { get; set; }
        
        public string Technician { get; set; }
       
        public string SpeciesId { get; set; }
       
        public string BreedId { get; set; }
       
        public string NoOfWastedSemenDose { get; set; }

        public string NoOfAIMade { get; set; }
       
        public string TypeOfAi { get; set; }
        public string CreatedBy { get; set; }
        public string EntityId { get; set; }
        public string Source { get; set; }

    }
}
