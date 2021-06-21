using LIMS.Domain.Breed;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.AInR
{
    public class AnimalRegistration : BaseEntity
    {
        public AnimalRegistration()
        {
            this.AnimalRegistrationId = Guid.NewGuid();
            this.Farm = new Farm();
        }
        public Guid AnimalRegistrationId { get; set; }
        public Species Species { get; set; }
        public string SpeciesId { get; set; }
        public BreedReg Breed { get; set; }
        public string BreedId { get; set; }
        public string Name { get; set; }
        public Farm Farm { get; set; }
        public string FarmId { get; set; }
        public string EarTagNo { get; set; }
        public string Gender { get; set; }
        public int? Age { get; set; }
        public string SireId { get; set; }
        public string DamId { get; set; }
        public int? Weight { get; set; }
        public string NoOFCalving { get; set; }
        public string PregencyStatus { get; set; }
        public DateTime? DOB { get; set; }
        public string MilkStatus { get; set; }
        public string PhysicalDefact { get; set; }
        public string AnimalColor { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedAt { get; set; }
        public string EntryType { get; set; }
        public string Source { get; set; }
        public string BreedType { get; set; }
        public bool IsDeleted { get; set; }
    }
}
