using LIMS.Core.ModelBinding;
using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Services
{
    public class HeatRecordingModel:BaseEntity
    {
        [LIMSResourceDisplayName("Admin.HeatRecording.HeatDate")]
        public DateTime? HeatDate { get; set; }
        [LIMSResourceDisplayName("Admin.HeatRecording.NoOfHeat")]

        public string NoOfHeat { get; set; }
        [LIMSResourceDisplayName("Admin.HeatRecording.Description")]

        public string Description { get; set; }

    }
}
