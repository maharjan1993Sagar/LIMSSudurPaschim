using LIMS.Core.ModelBinding;
using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Breed
{
    public class LivestockSpeciesModel:BaseEntity
    {
        [LIMSResourceDisplayName("Admin.Species.NepaliName")]
        public string NepaliName { get; set; }
        [LIMSResourceDisplayName("Admin.Species.EnglishName")]

        public string EnglishName { get; set; }
        [LIMSResourceDisplayName("Admin.Species.Detail")]


        public string Detail { get; set; }
        [LIMSResourceDisplayName("Admin.Species.Purposes")]

        public List<string> Purposes { get; set; }

    }
}
