using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.MoAMAC
{
    public partial class Dolfd:BaseEntity
    {
        public Dolfd()
        {
            this.DolfdId = Guid.NewGuid();
            this.MoAMAC = new MoAMAC();
        }
        public Guid DolfdId { get; set; }
        public string NameNepali { get; set; }
        public string NameEnglish { get; set; }
        public string Provience { get; set; }
        public string Address { get; set; }
        public string AddressNepali { get; set; }
        public string District { get; set; }
        public string LocalLevel { get; set; }
        public string Municipility { get; set; }
        public MoAMAC MoAMAC { get; set; }
        public string MoamacId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string UserEmail { get; set; }
        public string Type { get; set; }

    }
}
