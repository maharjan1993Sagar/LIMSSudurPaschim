using LIMS.Domain.BesicSetup;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Bali
{
    public class BaliRegister:BaseEntity
    {
        public string Bali { get; set; }
        public string Area { get; set; }
        public string Productivity { get; set; }
        public string Production { get; set; }
        public FiscalYear FiscalYear { get; set; }
        public string FiscalYearId { get; set; }
    }
}
