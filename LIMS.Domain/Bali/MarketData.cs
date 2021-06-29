using LIMS.Domain.Breed;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Bali
{
    public class MarketData:BaseEntity
    {
        public string SpeciesId { get; set; }
        public Species   Species { get; set; }
        public string Month { get; set; }
        public string MinPrice { get; set; }
        public string MaxPrice { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string NameOfMarket { get; set; }

        public string ProvinceBazar { get; set; }
        public string DistrictBazar { get; set; }

    }
}
