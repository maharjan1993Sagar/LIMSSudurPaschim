using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Breed
{
    public class CropsSeason:BaseEntity
    {
        public BreedReg Species { get; set; }
        public string SpeciesId { get; set; }
        public string GrowingSeason { get; set; }
        public string NepaliName { get; set; }
    }
}
