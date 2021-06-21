using LIMS.Core.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.AnimalHealth
{
    public class PurnaKhopModel
    {
        [LIMSResourceDisplayName("Admin.AnimalHealth.PurnaKhop.FarmId")]
        public string FarmId { get; set; }
        [LIMSResourceDisplayName("Admin.AnimalHealth.PurnaKhop.SpeciesId")]
        public string SpeciesId { get; set; }
        [LIMSResourceDisplayName("Admin.AnimalHealth.PurnaKhop.BreedType")]
        public string BreedType { get; set; }
        [LIMSResourceDisplayName("Admin.AnimalHealth.PurnaKhop.BreedId")]
        public string BreedId { get; set; }
        [LIMSResourceDisplayName("Admin.AnimalHealth.PurnaKhop.Age")]
        public string Age { get; set; }
        [LIMSResourceDisplayName("Admin.AnimalHealth.PurnaKhop.FiscalYearId")]
        public string FiscalYearId { get; set; }
        [LIMSResourceDisplayName("Admin.AnimalHealth.PurnaKhop.VaccinationTypeId")]
        public string VaccinationTypeId { get; set; }

        [LIMSResourceDisplayName("Admin.AnimalHealth.PurnaKhop.VaccinationDate")]
        [UIHint("Date")]
        public string VaccinationDate { get; set; }
        [LIMSResourceDisplayName("Admin.AnimalHealth.PurnaKhop.NextVaccinationDate")]
        [UIHint("Date")]
        public string NextVaccinationDate { get; set; }
        [LIMSResourceDisplayName("Admin.AnimalHealth.PurnaKhop.Remarks")]

        public string Remarks { get; set; }
    
    }
}
