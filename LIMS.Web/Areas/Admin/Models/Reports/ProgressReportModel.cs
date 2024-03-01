using LIMS.Core.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Reports
{
    public class ProgressReportModel
    {
        [LIMSResourceDisplayName("Admin.Common.Address")]
        public string Address { get; set; }
        [LIMSResourceDisplayName("Admin.Common.LocalLevel")]

        public string LocalLevel { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Level")]

        public string Level { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Ward")]

        public string Ward { get; set; }
        [LIMSResourceDisplayName("Admin.Common.FiscalYear")]

        public string FiscalYear { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Xetra")]

        public string Xetra { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Month")]

        public string Month { get; set; }
        public List<ProgressRowData> RowsPragatiVayeka { get; set; }
        public List<ProgressRowData> RowsPragatiNavayeka { get; set; }
    }
    
    public class ProgressRowData
    {
        [LIMSResourceDisplayName("Admin.Common.SN")]

        public string SN { get; set; }
        [LIMSResourceDisplayName("Admin.Common.BudgetTitle")]

        public string BudgetTitle { get; set; }
        [LIMSResourceDisplayName("Admin.Progress.VautikPragati")]

        public string VautikPragati { get; set; }
        [LIMSResourceDisplayName("Admin.Progress.BitiyaPragati")]

        public string BitiyaPragati { get; set; }
        [LIMSResourceDisplayName("Admin.Progress.KharchaKoSwrot")]

        public string KharchaKoSwrot { get; set; }
        [LIMSResourceDisplayName("Admin.Progress.SuchanaPrakashan")]

        public string SuchanaPrakashan { get; set; }
        [LIMSResourceDisplayName("Admin.Progress.FieldVerification")]

        public string FieldVerification { get; set; }
        [LIMSResourceDisplayName("Admin.Progress.Samzauta")]

        public string Samzauta { get; set; }
        [LIMSResourceDisplayName("Admin.Progress.Anugaman")]

        public string Anugaman { get; set; }
        [LIMSResourceDisplayName("Admin.Progress.UpalbdiHaru")]

        public string UpalbdiHaru { get; set; }
        [LIMSResourceDisplayName("Admin.Progress.VuktaniPauneKoNam")]

        public string VuktaniPauneKoNam { get; set; }
        [LIMSResourceDisplayName("Admin.Progress.Remarks")]

        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
       
}
