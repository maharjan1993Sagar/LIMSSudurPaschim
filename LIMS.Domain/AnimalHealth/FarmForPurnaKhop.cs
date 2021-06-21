using LIMS.Domain.AInR;
using LIMS.Domain.BasicSetup;
using LIMS.Domain.BesicSetup;
using LIMS.Domain.Breed;
using LIMS.Domain.Vaccination;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.AnimalHealth
{
    public class FarmForPurnaKhop:BaseEntity
    {
      public Farm Farm { get; set; }
        public string FarmId { get; set; }
        public string MobileNo { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string LocalLevel { get; set; }
        public string Ward { get; set; }
        public string Tole { get; set; }
        public Species Species { get; set; }
        public string SpeciesId { get; set; }
        public string BreedType { get; set; }
        public BreedReg Breed { get; set; }
        public string BreedId { get; set; }
        public string Age { get; set; }
        public string EarTag { get; set; }
        public string AnimalName { get; set; }
        public string FiscalYearId { get; set; }
        public FiscalYear FiscalYear { get; set; }
        public string Date { get; set; }
        public VaccinationType Vaccination { get; set; }
        public string VaccinationTypeId { get; set; }
        public string Source { get; set; }
        public bool Disease { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
