using LIMS.Core.ModelBinding;
using LIMS.Domain;
using LIMS.Web.Areas.Admin.Models.AInR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Recording
{
    public class MilkRecordingModel:BaseEntity
    {

        [LIMSResourceDisplayName("Admin.MilkRecording.MilkStatus")]
        public string MilkStatus { get; set; }
        [LIMSResourceDisplayName("Admin.MilkRecording.RecordingDate")]

        public string RecordingDate { get; set; }
        [LIMSResourceDisplayName("Admin.MilkRecording.RecordingPeriod")]

        public string RecordingPeriod { get; set; }
        [LIMSResourceDisplayName("Admin.MilkRecording.MilkVolume")]
        public string MilkVolume { get; set; }
        [LIMSResourceDisplayName("Admin.MilkRecording.SampleBoxNo")]
        public string SampleBoxNo { get; set; }
        public string AnimalRegistrationId { get; set; }
        public AnimalRegistrationModel AnimalRegistrationModel { get; set; }

    }
}
