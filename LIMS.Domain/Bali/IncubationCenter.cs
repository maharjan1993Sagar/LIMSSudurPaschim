using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Bali
{
    public class IncubationCenter:BaseEntity
    {
        public string OrganizationNameNepali { get; set; }
        public string OrganizationNameEnglish { get; set; }
        public string Province { get; set; }

        public string District { get; set; }
        public string LocalLevel { get; set; }
        public string Address { get; set; }
        public DateTime? RegisteredDate { get; set; }
        public string RegisteredAddress { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string NatureOfWork { get; set; }
        public string ValueChain { get; set; }
        public bool OrganizationStatus { get; set; }
        
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
