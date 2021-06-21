using LIMS.Domain;
using LIMS.Domain.AInR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Domain.Recording
{
    public class MilkRecording:BaseEntity
    {

        public MilkRecording()
        {
            this.MilkRecordingGuid = Guid.NewGuid();    
        }
        public Guid MilkRecordingGuid { get; set; }
        public string MilkStatus { get; set; }

        public string RecordingDate { get; set; }

        public string RecordingPeriod { get; set; }
        public string MilkVolume { get; set; }
        public string SampleBoxNo { get; set; }
        public string CreatedBy { get; set; }
        public AnimalRegistration AnimalRegistration { get; set; }
        public string AnimalRegistrationId { get; set; }
        public string Source { get; set; }
        public string UpdatedBy { get; set; }

    }
}
