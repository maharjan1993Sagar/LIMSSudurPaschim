using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Breed
{
  public  class Species:BaseEntity
    {
        public Species()
        {
            this.SpeciesId = Guid.NewGuid();
        }
        public Guid SpeciesId { get; set; }
        public string NepaliName { get; set; }
        public string EnglishName { get; set; }
        public List<string> Purposes { get; set; } 

        public string Detail { get; set; }

    }
}
