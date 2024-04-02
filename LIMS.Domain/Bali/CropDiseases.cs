using LIMS.Domain.BasicSetup;
using LIMS.Domain.BesicSetup;
using LIMS.Domain.Breed;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Bali
{
    public class CropDiseases:BaseEntity
    {
        public string SpeciesId { get; set; }
        public Species Species { get; set; }
        public string BreedId { get; set; }
        public BreedReg Breed { get; set; }
        public string DiseaseName { get; set; }
        public string Description { get; set; }
        public string Medicine { get; set; }
        public FiscalYear FiscalYear { get; set; }
        public string FiscalYearId { get; set; }
        public string NepaliDate { get; set; }
        public DateTime EnglishDate { get; set; }
        public string LabName { get; set; }
        public string Technician { get; set; }
        public string TechDesignation { get; set; }
        public string TehPhoneNo { get; set; }
        public string FarmerId { get; set; }
        public Farmer Farmer { get; set; }
        public string FarmerName { get; set; }
        public string PhoneNo { get; set; }
        public string CitizenshipNo { get; set; }
        public string IssuedDate { get; set; }
        public string IssuedDistrict { get; set; }
        public string LocalLevel { get; set; }
        public string WardNo { get; set; }
        public string District { get; set; }
        public string Province { get; set; }
        public string VerifiedBy { get; set; }
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
