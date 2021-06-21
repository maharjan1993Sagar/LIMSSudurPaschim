using LIMS.Core.ModelBinding;
using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.BasicSetup
{
    public class FiscalyearModel:BaseEntity
    {
        [LIMSResourceDisplayName("Admin.BasicSetup.NepaliFiscalYear")]
        public string NepaliFiscalYear { get; set; }
        [LIMSResourceDisplayName("Admin.BasicSetup.EnglishFiscalYear")]

        public string EnglishFiscalYear { get; set; }
        [LIMSResourceDisplayName("Admin.BasicSetup.CUrrentFiscalYear")]

        public bool CurrentFiscalYear { get; set; }

    }
}
