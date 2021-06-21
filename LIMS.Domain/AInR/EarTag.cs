using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.AInR
{
    public class EarTag : BaseEntity
    {
        public int SerialNo { get; set; }
        public string EarTagNo { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedAt { get; set; }
    }
}
