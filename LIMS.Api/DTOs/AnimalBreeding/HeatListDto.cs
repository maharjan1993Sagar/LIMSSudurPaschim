using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Api.DTOs.AnimalBreeding
{
    public class HeatListDto
    {
        public string Species { get; set; }
        public string Breed { get; set; }
        public DateTime? HeatDate { get; set; }
        public string NoOfHeat { get; set; }
        public string Description { get; set; }
    }
}
