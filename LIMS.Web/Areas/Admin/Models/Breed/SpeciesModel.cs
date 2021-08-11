using LIMS.Core.ModelBinding;
using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Breed
{
    public class SpeciesModel:BaseEntity
    {
        [LIMSResourceDisplayName("Admin.Category.NepaliName")]
        public string NepaliName { get; set; }
        [LIMSResourceDisplayName("Admin.Category.EnglishName")]

        public string EnglishName { get; set; }
        [LIMSResourceDisplayName("Admin.Category.Detail")]


        public string Detail { get; set; }
        [LIMSResourceDisplayName("Admin.Category.Purposes")]

        public List<string> Purposes { get; set; }
    }
}
