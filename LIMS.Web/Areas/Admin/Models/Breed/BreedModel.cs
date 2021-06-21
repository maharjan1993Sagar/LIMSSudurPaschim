using LIMS.Core.ModelBinding;
using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Breed
{
    public class BreedModel:BaseEntity
    {
        [LIMSResourceDisplayName("Admin.Breed.NepaliName")]
        public string NepaliName { get; set; }
        [LIMSResourceDisplayName("Admin.Breed.EnglishName")]

        public string EnglishName { get; set; }
        [LIMSResourceDisplayName("Admin.Breed.SpeciesId")]

        public string SpeciesId { get; set; }
        [LIMSResourceDisplayName("Admin.Breed.OriginFrom")]

        public string OriginForm { get; set; }
        [LIMSResourceDisplayName("Admin.Breed.Detail")]

        public string Detail { get; set; }
        public SpeciesModel Species { get; set; }
        [LIMSResourceDisplayName("Admin.Breed.Type")]
        public string Type { get; set; }
        //public SpeciesModel Species { get; set; }
    }
}
