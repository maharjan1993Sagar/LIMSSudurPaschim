using LIMS.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Api.DTOs
{
    public class AIDto: BaseApiEntityModel
    {
        public string AIDate { get; set; }


        public string SemenNo { get; set; }

        public string BullId { get; set; }

        public string AmountReceived { get; set; }



        public string FiscalYear { get; set; }
        public string Technician { get; set; }
        public string SpeciesId { get; set; }
        public string BreedId { get; set; }



        public string TypeOfAi { get; set; }
        public string FarmName { get; set; }
        public string AnimalName { get; set; }
        public string Eartag { get; set; }

        public string AnimalId { get; set; }

        public string MobileNo { get; set; }

        public string FarmId { get; set; }
    }
}
