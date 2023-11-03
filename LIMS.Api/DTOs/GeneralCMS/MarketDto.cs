using System;
 namespace LIMS.Api.DTOs
    {
        public class MarketDto
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


