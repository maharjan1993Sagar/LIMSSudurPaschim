using LIMS.Domain.Breed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Reports
{
    public class SpeciesWithAnimalTypeModel
    {
        public string SpeciesName { get; set; }
        public List<string> AnimalTypes { get; set; }
    }
}
