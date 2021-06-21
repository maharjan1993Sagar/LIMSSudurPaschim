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
            this.Species = new Species();
        }
        public Guid AnimalTypeId { get; set; }
        public string Name { get; set; }
        public Species Species { get; set; }
        public string SpeciesId { get; set; }
        public string NepaliName { get; set; }
    }
}
