using LIMS.Core.ModelBinding;
using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Bali
{
    public class CropDiseasesModel:BaseEntity
    {
        [LIMSResourceDisplayName("Admin.Common.SpeciesId")]

        public string SpeciesId { get; set; }
        [LIMSResourceDisplayName("Admin.Common.BreedId")]

        public string BreedId { get; set; }

        [LIMSResourceDisplayName("Admin.CropDiseases.DiseaseName")]

        public string DiseaseName { get; set; }
        [LIMSResourceDisplayName("Admin.CropDiseases.Description")]

        public string Description { get; set; }
        [LIMSResourceDisplayName("Admin.CropDiseases.Medicine")]

        public string Medicine { get; set; }
        [LIMSResourceDisplayName("Admin.Common.FiscalYearId")]

        public string FiscalYearId { get; set; }
        [LIMSResourceDisplayName("Admin.Common.NepaliDate")]

        public string NepaliDate { get; set; }
        [LIMSResourceDisplayName("Admin.Common.EnglishDate")]

        public DateTime EnglishDate { get; set; }
        [LIMSResourceDisplayName("Admin.CropDiseases.LabName")]

        public string LabName { get; set; }
        [LIMSResourceDisplayName("Admin.CropDiseases.Technician")]
        public string Technician { get; set; }
        [LIMSResourceDisplayName("Admin.CropDiseases.TechDesignation")]
        public string TechDesignation { get; set; }
        [LIMSResourceDisplayName("Admin.CropDiseases.TechPhoneNo")]
        public string TechPhoneNo { get; set; }
        [LIMSResourceDisplayName("Admin.CropDiseases.FarmerId")]
        public string FarmerId { get; set; }
        [LIMSResourceDisplayName("Admin.CropDiseases.FarmerName")]
        public string FarmerName { get; set; }
        [LIMSResourceDisplayName("Admin.Common.PhoneNo")]
        public string PhoneNo { get; set; }
        [LIMSResourceDisplayName("Admin.Common.CitizenshipNo")]
        public string CitizenshipNo { get; set; }
        [LIMSResourceDisplayName("Admin.Common.IssuedDate")]
        public string IssuedDate { get; set; }
        [LIMSResourceDisplayName("Admin.Common.IssuedDistrict")]
        public string IssuedDistrict { get; set; }
        [LIMSResourceDisplayName("Admin.Common.LocalLevel")]
        public string LocalLevel { get; set; }
        [LIMSResourceDisplayName("Admin.Common.WardNo")]
        public string WardNo { get; set; }
        [LIMSResourceDisplayName("Admin.Common.District")]
        public string District { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Province")]
        public string Province { get; set; }
        [LIMSResourceDisplayName("Admin.Common.VerifiedBy")]
        public string VerifiedBy { get; set; }
        [LIMSResourceDisplayName("Admin.CropDiseases.Remarks")]
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
