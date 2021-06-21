using LIMS.Core.ModelBinding;
using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.AnimalHealth
{
    public class FarmForPurnaKhopModel:BaseEntity
    {
        [LIMSResourceDisplayName("Admin.FarmForPurnaKhop.FarmId")]
        public string FarmId { get; set; }
        [LIMSResourceDisplayName("Admin.FarmForPurnaKhop.FarmName")]
        public string FarmName { get; set; }
        [LIMSResourceDisplayName("Admin.FarmForPurnaKhop.MobileNo")]
        public string MobileNo { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Province")]
        public string Province { get; set; }
        [LIMSResourceDisplayName("Admin.Common.District")]

        public string District { get; set; }
        [LIMSResourceDisplayName("Admin.Common.LocalLevel")]
        public string LocalLevel { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Ward")]
        public string Ward { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Tole")]
        public string Tole { get; set; }
        [LIMSResourceDisplayName("Admin.FarmForPurnaKhop.SpeciesId")]
        public string SpeciesId { get; set; }
        [LIMSResourceDisplayName("Admin.FarmForPurnaKhop.BreedType")]
        public string BreedType { get; set; }
        [LIMSResourceDisplayName("Admin.FarmForPurnaKhop.BreedId")]
        public string BreedId { get; set; }
        [LIMSResourceDisplayName("Admin.FarmForPurnaKhop.Age")]
        public string Age { get; set; }
        [LIMSResourceDisplayName("Admin.FarmForPurnaKhop.Eartag")]
        public string EarTag { get; set; }
        [LIMSResourceDisplayName("Admin.FarmForPurnaKhop.AnimalName")]
        public string AnimalName { get; set; }
        [LIMSResourceDisplayName("Admin.FarmForPurnaKhop.FiscalYearId")]
        public string FiscalYearId { get; set; }
        [LIMSResourceDisplayName("Admin.FarmForPurnaKhop.Date")]
        [UIHint("date")]
        public string Date { get; set; }
        [LIMSResourceDisplayName("Admin.FarmForPurnaKhop.VaccinationTypeId")]
        public string VaccinationTypeId { get; set; }
        public bool Disease { get; set; }
    }
}
