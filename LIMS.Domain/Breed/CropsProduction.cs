using LIMS.Domain.BesicSetup;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Breed
{
    public class CropsProduction:BaseEntity
    {
        public BreedReg CropName { get; set; }
        public string CropId { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedAt { get; set; }
        public CropsSeason GrowingSeason { get; set; }
        public string GrowingSeasonId { get; set; }
        public string Area { get; set; }
        public string Production { get; set; }
        public FiscalYear FiscalYear { get; set; }

        public string Provience { get; set; }


        public string District { get; set; }

        public string LocalLevel { get; set; }

        public string Ward { get; set; }


        public string Tole { get; set; }
    }
}
