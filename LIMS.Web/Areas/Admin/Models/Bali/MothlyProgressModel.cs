using LIMS.Core.ModelBinding;
using LIMS.Domain;
using LIMS.Domain.Bali;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Bali
{
    public class MonthlyProgressModel:BaseEntity
    {
        public List<PujigatKharchaKharakram> pujigatKharchaKharakram { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.PujigatKharchaId")]

        public string PujigatKharchaId { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.Fiscalyear")]

        public string FiscalYearId { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.Month")]

        public string Month { get; set; }
        public string VautikPragati { get; set; }
        public string BitiyaPragati { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.Type")]

        public string Type { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.ProgramType")]

        public string ProgramType { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.Dolfd")]

        public string DolfdId { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.Vhlsec")]

        public string VhlsecId { get; set; }

    }
}
