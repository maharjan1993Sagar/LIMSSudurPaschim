using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Breed
{
    public class CropsSeasonModel:BaseEntity
    {
        public string SpeciesId { get; set; }
        public string GrowingSeason { get; set; }
        public string NepaliName { get; set; }
    }
}
