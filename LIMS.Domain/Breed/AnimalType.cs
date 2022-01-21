using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Breed
{
   public class AnimalType:BaseEntity
    {
        public AnimalType()
        {
            this.AnimalTypeId = AnimalTypeId;
            this.Species = new LivestockSpecies();
        }
        public Guid AnimalTypeId { get; set; }
        public string Name { get; set; }
        public LivestockSpecies Species { get; set; }
        public string SpeciesId { get; set; }
        public string NepaliName { get; set; }
    }
}
