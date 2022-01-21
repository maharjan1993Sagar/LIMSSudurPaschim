using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Breed
{
    public class SubsidyType:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedAt { get; set; }
    }
}
