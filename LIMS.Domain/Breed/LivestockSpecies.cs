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
        }
       
        public Guid SpeciesId { get; set; }
        public string NepaliName { get; set; }
        public string EnglishName { get; set; }
        public List<string> Purposes { get; set; }
        public string Detail { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedAt { get; set; }
    }
}
