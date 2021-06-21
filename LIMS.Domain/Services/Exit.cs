using LIMS.Domain.AInR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Services
{
    public class Exit:BaseEntity
    {
        public AnimalRegistration AnimalRegistration { get; set; }
        public string AnimalRegistrationId { get; set; }
        public string DateOfExit { get; set; }

        public string ReasonForExit { get; set; }

        public string Comments { get; set; }
        public string CreatedBy { get; set; }
        public string source { get; set; }
        public string EntityId { get; set; }
    }
}
