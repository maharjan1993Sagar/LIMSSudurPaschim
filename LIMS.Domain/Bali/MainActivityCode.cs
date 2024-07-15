using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Bali
{
    public class MainActivityCode:BaseEntity
    {
        public string Limbis_Code { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedAt { get; set; }
    }
}
