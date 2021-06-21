using LIMS.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Api.DTOs.AnimalHealth
{
    public class TreatMentDto: BaseApiEntityModel
    {
        public string AnimalRegistrationId { get; set; }
        public string FarmId { get; set; }
        public string DiseaseName { get; set; }
        public string Symptoms { get; set; }
        public string Medicine { get; set; }
        public DateTime? Date { get; set; }

        public string Description { get; set; }



        public string Temp { get; set; }

        public string Pulse { get; set; }


        public string Respiration { get; set; }

        public string ProvisionalDiagnosis { get; set; }

    }
}
