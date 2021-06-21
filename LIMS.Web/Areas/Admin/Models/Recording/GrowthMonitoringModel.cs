using LIMS.Core.ModelBinding;
using LIMS.Domain;
using LIMS.Web.Areas.Admin.Models.AInR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Recording
{
    public class GrowthMonitoringModel:BaseEntity
    {
        [LIMSResourceDisplayName("Admin.GrowthMonitoring.RecordingPeriod")]
        public string MonitoringDate { get; set; }
        [LIMSResourceDisplayName("Admin.GrowthMonitoring.Leangth")]
        public string Length { get; set; }
        [LIMSResourceDisplayName("Admin.GrowthMonitoring.LengthGrowth")]
        public string LengthGrowth { get; set; }
        [LIMSResourceDisplayName("Admin.GrowthMonitoring.Weight")]

        public string   Weight { get; set; }
        [LIMSResourceDisplayName("Admin.GrowthMonitoring.WeightGrowth")]

        public string Growth { get; set; }
        [LIMSResourceDisplayName("Admin.GrowthMonitoring.HeartGirth")]

        public string HeartGirth { get; set; }
        [LIMSResourceDisplayName("Admin.GrowthMonitoring.HeightWither")]

        public string HeightWither { get; set; }

        [LIMSResourceDisplayName("Admin.GrowthMonitoring.Note")]
        public string Note { get; set; }
        public string AnimalRegistrationId { get; set; }
        public AnimalRegistrationModel AnimalRegistrationModel { get; set; }

    }
}
