using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Website1.Models
{
    public class MarketViewModel
    {
        public string BreedId { get; set; }

        public string SpeciesId { get; set; }
        public string Month { get; set; }
        public string MinPrice { get; set; }
        public string MaxPrice { get; set; }
        public string WholesalePrice { get; set; }
        public string FarmGetPrice { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string NameOfMarket { get; set; }
        public string AddressBazar { get; set; }
        public string FiscalYearId { get; set; }
        public string UnitId { get; set; }
        public DateTime RecordingDate { get; set; }
    }
}
