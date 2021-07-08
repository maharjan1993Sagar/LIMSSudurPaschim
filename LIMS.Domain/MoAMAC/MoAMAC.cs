using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.MoAMAC
{
  public partial class MoAMAC: BaseEntity
    {
        public MoAMAC()
        {
            this.MoAMACId = Guid.NewGuid();
           
        }

        public Guid MoAMACId { get; set; }
        public string NameEnglish { get; set; }
        public string NameNepali{ get; set; }
        public string UserEmail { get; set; }
        public string Provience { get; set; }
        public string District { get; set; }
        public string LocalLevel { get; set; }
    }
}
