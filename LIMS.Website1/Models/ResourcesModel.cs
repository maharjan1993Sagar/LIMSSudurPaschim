using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website1.Models
{
    public class ResourcesModel
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
