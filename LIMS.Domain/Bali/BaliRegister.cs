using LIMS.Domain.BesicSetup;
using LIMS.Domain.Breed;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Bali
{
    public class BaliRegister:BaseEntity
    {
        public Species Species { get; set; }
        public string SpeciesId { get; set; }

        public BreedReg BreedReg { get; set; }
        public string BreedId{ get; set; }

        public string Area { get; set; }
        public string Productivity { get; set; }
        public string Production { get; set; }
        public FiscalYear FiscalYear { get; set; }
        public string FiscalYearId { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedAt { get; set; }
    }
}
