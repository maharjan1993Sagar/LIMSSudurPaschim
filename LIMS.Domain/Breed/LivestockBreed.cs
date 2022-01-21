using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Breed
{
    public class LivestockBreed:BaseEntity
    {
        public LivestockBreed()
        {
            this.BreedId = Guid.NewGuid();
            this.Species = new LivestockSpecies();
        }
        public Guid BreedId { get; set; }
        public string NepaliName { get; set; }
        public string EnglishName { get; set; }

        public LivestockSpecies Species { get; set; }
        public string ScientificName { get; set; }
      
        public string Detail { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedAt { get; set; }

    }
}
