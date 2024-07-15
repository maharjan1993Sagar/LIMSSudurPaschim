using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Bali
{
    public class SubSector:BaseEntity
    {
        public string BudgetSourceId { get; set; }
        public BudgetSource BudgetSource { get; set; }
        public string Name { get; set; }
        public string NameNepali { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedAt { get; set; }
    }
}
