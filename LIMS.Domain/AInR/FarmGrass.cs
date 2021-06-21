using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.AInR
{
    public class FarmGrass : SubBaseEntity
    {
        public string FarmId { get; set; }
        public string FarmgrassId { get; set; }
        public string Type { get; set; }
        public string TotalArea { get; set; }
        public string GrassName { get; set; }
        public string Season { get; set; }
        public string NoOfTree { get; set; }

    }
}
