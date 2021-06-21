using LIMS.Api.Models;
using LIMS.Core.ModelBinding;
using LIMS.Domain;

namespace LIMS.Api.DTOs.PerformnceRecording
{
    public class MilkRecordingDto: BaseApiEntityModel
    {
        public string MilkStatus { get; set; }
        public string RecordingDate { get; set; }

        public string RecordingPeriod { get; set; }
      
        public string MilkVolume { get; set; }
      
        public string SampleBoxNo { get; set; }
        public string AnimalRegistrationId { get; set; }
      

    }
}
