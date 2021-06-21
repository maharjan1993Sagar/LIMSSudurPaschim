using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Api.DTOs.AnimalBreeding
{
    public class HeatRecordingDto
    {
        public string AnimalId { get; set; }
        public DateTime? HeatDate { get; set; }
        public string NoOfHeat { get; set; }
        public string Description { get; set; }
    }
}
