using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Bali
{
    public class Farmer:BaseEntity
    {
        public string Name { get; set; }
        public string NameNepali { get; set; }

        public string Province { get; set; }

        public string District { get; set; }
        public string LocalLevel { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public Talim Talim { get; set; }
        public string TalimId { get; set; }
        public IncubationCenter Incubation { get; set; }
        public string IncuvationCenterId { get; set; }
        public string Remarks { get; set; }
        public string Gender { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
