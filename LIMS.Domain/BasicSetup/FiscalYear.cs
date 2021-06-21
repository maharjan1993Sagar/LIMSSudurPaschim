using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.BesicSetup
{
   public class FiscalYear:BaseEntity
    {
        public FiscalYear()
        {
            this.FiscalYearId = Guid.NewGuid();
        }
        public Guid FiscalYearId { get; set; }
        public string NepaliFiscalYear { get; set; }
        public string EnglishFiscalYear { get; set; }
        public bool CurrentFiscalYear { get; set; }
    }
}
