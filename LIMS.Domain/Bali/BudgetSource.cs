using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Bali
{
    public class BudgetSource:BaseEntity
    {
        public string Name { get; set; }
        public string NameNepali { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedAt { get; set; }
    }
}
