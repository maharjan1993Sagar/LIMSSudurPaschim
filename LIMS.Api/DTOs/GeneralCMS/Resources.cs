using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Api.DTOs
{
    public class Resources
    {
        public string Type { get; set; }

        public string SubType { get; set; }

        public string ItemName { get; set; }


        public string UnitId { get; set; }
     

        public string AvailableQuantity { get; set; }
        

        public string UnitPrice { get; set; }
        public string Remarks { get; set; }
    }
}
