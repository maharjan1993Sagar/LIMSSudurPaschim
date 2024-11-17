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
        public List<MonthlyPragati> pujigatKharchaKharakram { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.PujigatKharchaId")]

        public string PujigatKharchaId { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.Fiscalyear")]

        public string FiscalYearId { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.Month")]

        public string Month { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.Month")]

        public string ToMonth { get; set; }
        public string VautikPragati { get; set; }
        public string BitiyaPragati { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.Type")]

        public string Type { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.ProgramType")]

        public string ProgramType { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.ExpensesCategory")]

        public string ExpensesCategory { get; set; }

        [LIMSResourceDisplayName("LIMS.Common.BudgetSource")]

        public string BudgetSourceId { get; set; }

        [LIMSResourceDisplayName("LIMS.Common.SubSector")]

        public string SubSectorId { get; set; }

        [LIMSResourceDisplayName("LIMS.Common.Dolfd")]

        public string DolfdId { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.Vhlsec")]




        public string KharchaKoSwrot { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.SuchanaPrakashan")]

        public string SuchanaPrakashan { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.FieldVerification")]

        public string FieldVerification { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.Samzauta")]

        public string Samzauta { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.Anugaman")]

        public string Anugaman { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.UpalbdiHaru")]

        public string UpalbdiHaru { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.VuktaniPauneKoNam")]

        public string VuktaniPauneKoNam { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.Remarks")]

        public string Remarks { get; set; }








        [LIMSResourceDisplayName("LIMS.Common.Vhlsec")]

        public string VhlsecId { get; set; }


    }
}
