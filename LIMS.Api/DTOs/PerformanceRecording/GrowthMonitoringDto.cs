using LIMS.Api.Models;
using LIMS.Domain;

namespace LIMS.Api.DTOs.PerformnceRecording
{
    public class GrowthMonitoringDto: BaseApiEntityModel
    {
        public string MonitoringDate { get; set; }
       
        public string Length { get; set; }
       
        public string LengthGrowth { get; set; }
 
        public string   Weight { get; set; }

        public string Growth { get; set; }
        
        public string HeartGirth { get; set; }
       
        public string HeightWither { get; set; }
        public string Note { get; set; }
        public string AnimalRegistrationId { get; set; }
       

    }
}
