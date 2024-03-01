using LIMS.Domain.BasicSetup;
using LIMS.Domain.BesicSetup;
using LIMS.Domain.Breed;
using LIMS.Domain.Organizations;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Bali
{
    public class DeathVerification:BaseEntity
    {
#region General_Info
        public FiscalYear FiscalYear { get; set; }
        public string FiscalYearId { get; set; }
        public string WardNo { get; set; }
        public string NepaliDate { get; set; }
        public DateTime EnglishDate { get; set; }
        public bool IsFarmRegistered { get; set; }
        public OtherOrganization Organization { get; set; }
        public string OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string FarmerName { get; set; }
        public Farmer Farmer { get; set; }
        public string FarmerId { get; set; } 
        public LivestockSpecies LivestockSpecies { get; set; }
        public string LivestockSpeciesId { get; set; }
#endregion
        #region Animal_Related
        public LivestockBreed LivestockBreed { get; set; } 
        public string LivestockBreedId { get; set; }
        public string  TagNo { get; set; }                 
        public string  Color { get; set; }
        public decimal Height { get; set; }
        public decimal Age { get; set; }
        public string InsuranceCompany { get; set; }
        public string InsuranceCompanyAddress { get; set; }
        public string InsuranceNo { get; set; }
        public string CauseOfDeath { get; set; }
        public Decimal InsuranceAmount { get; set; }
        public DateTime DeathDate { get; set; }
        public string Remarks { get; set; }
        #endregion
        #region Official_Info
        public string VerifiedBy { get; set; }
        public string PhoneNumber { get; set; }
        public string Designation { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
#endregion
#region LocalLevel
        public string LocalLevel { get; set; }
        public string District { get; set; }
        public string Country { get; set; }
#endregion
    }
}
