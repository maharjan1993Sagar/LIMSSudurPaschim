using LIMS.Domain.BasicSetup;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Bali
{
    public class FarmLabResources:BaseEntity
    {
        public string Type { get; set; }
        public string UnitId { get; set; }
        public Unit Unit { get; set; }
        public string AvailableQuantity { get; set; }
        public string UnitPrice { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
