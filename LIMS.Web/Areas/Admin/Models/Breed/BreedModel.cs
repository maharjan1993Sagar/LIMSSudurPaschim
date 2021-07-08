using LIMS.Core.ModelBinding;
using LIMS.Domain;
using LIMS.Web.Areas.Admin.Models.Breed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Breed
{
    public class BreedModel:BaseEntity
    {
        [LIMSResourceDisplayName("Admin.Crop.NepaliName")]
        public string NepaliName { get; set; }
        [LIMSResourceDisplayName("Admin.Crop.EnglishName")]

        public string EnglishName { get; set; }
        [LIMSResourceDisplayName("Admin.Crop.CategoryId")]

        public string SpeciesId { get; set; }
        [LIMSResourceDisplayName("Admin.Crop.OriginFrom")]

        public string OriginForm { get; set; }
        [LIMSResourceDisplayName("Admin.Crop.Detail")]

        public string Detail { get; set; }
        public SpeciesModel Species { get; set; }
        [LIMSResourceDisplayName("Admin.Crop.Type")]
        public string Type { get; set; }
        [LIMSResourceDisplayName("Admin.Crop.ScientificName")]

        public string ScientificName { get; set; }
        [LIMSResourceDisplayName("Admin.Crop.GrowingSeason")]
        
        public string GrowingSeason { get; set; }
        [LIMSResourceDisplayName("Admin.Crop.GrowingDuration")]
        [UIHint("Int32Nullable")]
        public string GrowingDuration { get; set; }

        //public SpeciesModel Species { get; set; }
    }
}
