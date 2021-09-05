using LIMS.Domain.StatisticalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Breed
{
    public class LivestockSpecies:BaseEntity
    {
        public LivestockSpecies()
        {
            this.SpeciesId = Guid.NewGuid();
            this.Livestock = new Livestock();
        }
        public Livestock Livestock { get; set; }
        public string LivestockId { get; set; }

        public Guid SpeciesId { get; set; }
        public string NepaliName { get; set; }
        public string EnglishName { get; set; }
        public List<string> Purposes { get; set; }

        public string Detail { get; set; }
    }
}
