using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.MoAMAC
{
    public class Vhlsec:BaseEntity
    {
        public Vhlsec()
        {
            this.VhlsecId = Guid.NewGuid();
            this.Dolfd = new Dolfd();
        }
        public Guid VhlsecId { get; set; }
        public string NameNepali { get; set; }
        public string NameEnglish { get; set; }
        public string Provience { get; set; }
        public string District { get; set; }
        public string LocalLevel { get; set; }
        public string Municipility { get; set; }
        public Dolfd Dolfd { get; set; }
        public string DolfdId { get; set; }
        public string UserEmail { get; set; }
    }
}
