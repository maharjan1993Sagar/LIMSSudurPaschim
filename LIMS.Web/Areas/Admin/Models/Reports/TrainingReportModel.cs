using LIMS.Core.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Reports
{
    public class TrainingReportModel
    {
        [LIMSResourceDisplayName("Admin.Common.Address")]
        public string Address { get; set; }
        [LIMSResourceDisplayName("Admin.Common.LocalLevel")]

        public string LocalLevel { get; set; }
        [LIMSResourceDisplayName("Admin.Common.BudgetId")]

        public string BudgetId { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Level")]

        public string Level { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Ward")]

        public string Ward { get; set; }
        [LIMSResourceDisplayName("Admin.Common.FiscalYear")]

        public string FiscalYear { get; set; }
        [LIMSResourceDisplayName("Admin.Common.TalimId")]

        public string TalimId { get; set; }
        [LIMSResourceDisplayName("Admin.Common.StartDate")]

        public string StartDate { get; set; }
        [LIMSResourceDisplayName("Admin.Common.EndDate")]

        public string EndDate { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Xetra")]

        public string Xetra { get; set; }
        public List<TrainingRowData> Rows { get; set; }
    }
    
    public class TrainingRowData
    {
        [LIMSResourceDisplayName("Admin.Common.SN")]

        public string SN { get; set; }
        [LIMSResourceDisplayName("Admin.Common.BudgetTitle")]

        public string BudgetTitle { get; set; }
        [LIMSResourceDisplayName("Admin.Common.TrainingTitle")]

        public string TrainingTitle { get; set; }
        [LIMSResourceDisplayName("Admin.Common.MainActivity")]

        public string MainActivity { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Remarks")]

        public string Remarks { get; set; }
        [LIMSResourceDisplayName("Admin.Common.StartDate")]

        public string StartDate { get; set; }
        [LIMSResourceDisplayName("Admin.Common.EndDate")]

        public string EndDate { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Male")]

        public string Male { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Female")]

        public string Female { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Dalit")]

        public string Dalit { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Janajati")]

        public string Janajati { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Others")]

        public string Others { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Total")]

        public string Total { get; set; }
        [LIMSResourceDisplayName("Admin.Training.Purpose")]

        public string Purpose { get; set; }
        [LIMSResourceDisplayName("Admin.Training.Upalabdhiharu")]

        public string Upalabdhiharu { get; set; }
        [LIMSResourceDisplayName("Admin.Trainging.Logistics")]

        public string Logistics { get; set; }
        [LIMSResourceDisplayName("Admin.Training.TotalExpense")]

        public string TotalExpense { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Address")]

        public string Address { get; set; }
    }
       
}
