using LIMS.Core.ModelBinding;
using LIMS.Domain.Bali;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Bali
{
    public class LabambitReport
    {
        public PujigatKharchaKharakram pujigatKharchaKharakram { get; set; }

        [LIMSResourceDisplayName("LIMS.common.FiscalYear")]

        public string Fiscalyear { get; set; }

        [LIMSResourceDisplayName("LIMS.Common.Workdone")]

        public string WorkDone { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.Female")]

        public int Female { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.Female")]

        public int Male { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.Female")]

        public int Dalit { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.Female")]

        public int Janajati { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.Female")]

        public int Aanya { get; set; }
    }
}



