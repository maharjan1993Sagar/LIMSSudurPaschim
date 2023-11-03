using LIMS.Domain.AInR;
using LIMS.Domain.BesicSetup;
using LIMS.Domain.Breed;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.StatisticalData
{
    public class FishProduction:BaseEntity
    {
      
        public string NatureOfProduction { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string LocalLevel { get; set; }
        public string Ward { get; set; }
        public string FiscalYearId { get; set; }
     
        public string Date { get; set; }
        public string NumberOfPond { get; set; }

        public string NumberOfFish { get; set; }
        public string Area { get; set; }
        public string Quantity { get; set; }
        public string Remarks { get; set; }
        public FiscalYear FiscalYear { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }


    }
}
