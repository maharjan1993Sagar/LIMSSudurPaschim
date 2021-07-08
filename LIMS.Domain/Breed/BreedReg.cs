using System;

namespace LIMS.Domain.Breed
{
    public partial class BreedReg : BaseEntity
    {
        public BreedReg()
        {
            this.BreedId = Guid.NewGuid();
            this.Species = new Species();
        }
        public Guid BreedId { get; set; }
        public string NepaliName { get; set; }
        public string EnglishName { get; set; }
        
        public Species Species { get; set; }
        public string ScientificName { get; set; }
        public string GrowingSeason { get; set; }
        public string GrowingDuration { get; set; }

        public string Detail { get; set; }
        public string Type { get; set; }
    }
}
