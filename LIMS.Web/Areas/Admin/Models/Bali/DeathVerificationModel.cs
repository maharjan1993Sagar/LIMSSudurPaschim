using LIMS.Core.ModelBinding;
using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Bali
{
    public class DeathVerificationModel:BaseEntity
    {
        //General Info
        [LIMSResourceDisplayName("Admin.Common.FiscalYear")]
        public string FiscalYearId { get; set; }
        [LIMSResourceDisplayName("Admin.Common.NepaliDate")]

        public string NepaliDate { get; set; }
        [LIMSResourceDisplayName("Admin.Common.EnglishDate")]

        public DateTime EnglishDate { get; set; }
        [LIMSResourceDisplayName("Admin.DeathVerification.IsFarmRegistered")]

        public bool IsFarmRegistered { get; set; }
        [LIMSResourceDisplayName("Admin.DeathVerification.OrganizationId")]

        public string OrganizationId { get; set; }
        [LIMSResourceDisplayName("Admin.DeathVerification.OrganizationName")]

        public string OrganizationName { get; set; }
        [LIMSResourceDisplayName("Admin.DeathVerification.FarmerName")]

        public string FarmerName { get; set; }
        [LIMSResourceDisplayName("Admin.DeathVerification.FarmerId")]


        public string FarmerId { get; set; }
        [LIMSResourceDisplayName("Admin.DeathVerification.WardNo")]

        public string WardNo { get; set; }

        [LIMSResourceDisplayName("Admin.DeathVerification.LivestockSpeciesId")]


        public string LivestockSpeciesId { get; set; }

        //Animal Related
        [LIMSResourceDisplayName("Admin.DeathVerification.LivestockBreedId")]

        public string LivestockBreedId { get; set; }
        [LIMSResourceDisplayName("Admin.Common.TagNo")]

        public string TagNo { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Color")]

        public string Color { get; set; }

        [LIMSResourceDisplayName("Admin.Common.Height")]

        public decimal Height { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Age")]

        public decimal Age { get; set; }
        [LIMSResourceDisplayName("Admin.DeathVerification.InsuranceCompany")]

        public string InsuranceCompany { get; set; }
        [LIMSResourceDisplayName("Admin.DeathVerification.InsuranceCompanyAddress")]

        public string InsuranceCompanyAddress { get; set; }
        [LIMSResourceDisplayName("Admin.DeathVerification.InsuranceNo")]

        public string InsuranceNo { get; set; }
        [LIMSResourceDisplayName("Admin.DeathVerification.CauseOfDeath")]

        public string CauseOfDeath { get; set; }
        [LIMSResourceDisplayName("Admin.DeathVerification.InsuranceAmount")]

        public Decimal InsuranceAmount { get; set; }
        [LIMSResourceDisplayName("Admin.DeathVerification.DeathDate")]

        public DateTime DeathDate { get; set; }
        [LIMSResourceDisplayName("Admin.DeathVerification.Remarks")]

        public string Remarks { get; set; }
        [LIMSResourceDisplayName("Admin.DeathVerification.VerifiedBy")]


        //Official Info
        public string VerifiedBy { get; set; }
        [LIMSResourceDisplayName("Admin.Common.PhoneNumber")]

        public string PhoneNumber { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Designation")]

        public string Designation { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }
        [LIMSResourceDisplayName("Admin.Common.LocalLevel")]


        //LocalLevel
        public string LocalLevel { get; set; }
        [LIMSResourceDisplayName("Admin.Common.District")]

        public string District { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Country")]

        public string Country { get; set; }

    }
}
