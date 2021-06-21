using LIMS.Domain;
using LIMS.Domain.AInR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Domain.Recording
{
    public class GrowthMonitoring : BaseEntity
    {
        public GrowthMonitoring()
        {
            this.GrowthMonitoringGuid = Guid.NewGuid();
        }
        public Guid GrowthMonitoringGuid { get; set; }
        public string MonitoringDate { get; set; }
        public string Length { get; set; }
        public string LengthGrowth { get; set; }
        public string Weight { get; set; }
        public string Growth { get; set; }
        public string HeartGirth { get; set; }
        public string HeightWither { get; set; }
        public string Note { get; set; }
        public AnimalRegistration AnimalRegistration { get; set; }
        public string CreatedBy { get; set; }
        public string Source { get; set; }
        public string Updatedby { get; set; }
    }
}
