using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Api.DTOs.Common
{
    public class FiscalYearDto:BaseEntity
    {
        public Guid FiscalYearId { get; set; }
        public string NepaliFiscalYear { get; set; }
        public string EnglishFiscalYear { get; set; }
    }
}
