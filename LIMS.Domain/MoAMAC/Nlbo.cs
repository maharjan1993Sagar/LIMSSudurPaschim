using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.MoAMAC
{
    public class Nlbo:BaseEntity
    {
        public Nlbo()
        {
            this.NlboId = Guid.NewGuid();
          
        }
        public Guid NlboId { get; set; }
        public MoAMAC Moald { get; set; }
        public string MOALDId { get; set; }
        public string NameNepali { get; set; }
        public string NameEnglish { get; set; }
        public string Provience { get; set; }
        public string Address { get; set; }
        public string District { get; set; }
        public string LocalLevel { get; set; }
        public string Municipility { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string UserEmail { get; set; }

    }
}
