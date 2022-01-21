using LIMS.Domain.Breed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Breed
{
    public class CropsProductionModel
    {
        public List<CropsSeason> CropSeason { get; set; }
        public string Season { get; set; }
        public string CropName { get; set; }
        public string Yeald { get; set; }

        public string CropId { get; set; }
        public string GrowingSeasonId { get; set; }
        public string Area { get; set; }
        public string Production { get; set; }

        public string Provience { get; set; }


        public string District { get; set; }

        public string LocalLevel { get; set; }

        public string Ward { get; set; }

        public string FiscalYearId { get; set; }
        public string Tole { get; set; }
    }
}
