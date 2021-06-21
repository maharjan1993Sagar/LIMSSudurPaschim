using LIMS.Domain.AInR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Services
{
    public class HeatRecording:BaseEntity
    {
        public HeatRecording()
        {
            this.HeatRecordingGuid = Guid.NewGuid();
            this.AnimalRegistration = new AnimalRegistration();
        }
        public Guid HeatRecordingGuid { get; set; }
        public DateTime? HeatDate { get; set; }
        public string NoOfHeat { get; set; }
        public string Description { get; set; }
        public AnimalRegistration AnimalRegistration { get; set; }
        public string Source { get; set; }
        public string CreatedBy { get; set; }
        public string EntityId { get; set; }
    }
}
