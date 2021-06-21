using LIMS.Core.ModelBinding;
using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Breed
{
    public class AnimalTypeModel:BaseEntity
    {
        [LIMSResourceDisplayName("Admin.AnimalType.Name")]
        public string Name { get; set; }
        [LIMSResourceDisplayName("Admin.AnimalType.NepaliName")]

        public string NepaliName { get; set; }
        [LIMSResourceDisplayName("Admin.AnimalType.SpeciesName")]
        public string SpeciesId { get; set; }
    }
}
