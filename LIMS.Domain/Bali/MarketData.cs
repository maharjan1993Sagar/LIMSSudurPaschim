using LIMS.Domain.BasicSetup;
using LIMS.Domain.BesicSetup;
using LIMS.Domain.Breed;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Bali
{
    public class MarketData:BaseEntity
    {
        public string BreedId { get; set; }
        public BreedReg Breed { get; set; }
        public string SpeciesId { get; set; }
        public Species   Species { get; set; }
        public string UnitId { get; set; }
        public Unit Unit { get; set; }
        public string Month { get; set; }
        public string MinPrice { get; set; }
        public string MaxPrice { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string NameOfMarket { get; set; }

        public string AddressBazar { get; set; }
        public FiscalYear FiscalYear { get; set; }
        public string FiscalYearId { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedAt { get; set; }
        public string RecordingDate { get; set; }

    }
}
