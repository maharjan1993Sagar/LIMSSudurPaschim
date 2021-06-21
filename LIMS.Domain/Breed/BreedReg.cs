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
        public string OriginForm { get; set; }
        public string Detail { get; set; }
        public string Type { get; set; }
    }
}
