using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Api.DTOs.AnimalBreeding
{
    public class AiListDto
    {
        public string species { get; set; }
        public string Breed { get; set; }
        public string AIDate { get; set; }

        public string SemenNo { get; set; }

        public string BullId { get; set; }

        public string AmountReceived { get; set; }



        public string FiscalYear { get; set; }
        public string Technician { get; set; }
        public string SpeciesId { get; set; }
        public string BreedId { get; set; }

        public string NoOfWastedSemenDose { get; set; }

        public string NoOfAIMade { get; set; }

        public string RepeatAi { get; set; }


     
    }
}
