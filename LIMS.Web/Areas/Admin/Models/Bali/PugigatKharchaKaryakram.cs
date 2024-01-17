using LIMS.Core.ModelBinding;
using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Bali
{
    public class PugigatKharchaKaryakramModel: BaseEntity
    {

        [LIMSResourceDisplayName("LIMS.Common.Limbis_Code")]
        public string Limbis_Code { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.Remarks")]
        public string Remarks { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.Program")]
        public string Program { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.ProgramSummery")]
        public string ProgramSummery { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.kharchaCode")]
        public string kharchaCode { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.Unit")]
        public string Unit { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.YearlyParinam")]
        public string YearlyParinam { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.YearlyVar")]
        public string YearlyVar { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.BarsikBajet")]
        public string BarsikBajet { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.BarshikBhar")]
        public string BarshikBhar { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.BarshikParinam")]
        public string BarshikParinam { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.PrathamChaumasikParimam")]
        public string PrathamChaumasikParimam { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.PrathamChaumasikVar")]
        public string PrathamChaumasikVar { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.PrathamChaumasikBadjet")]
        public string PrathamChaumasikBadjet { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.DorsoChaumasikParimam")]
        public string DorsoChaumasikParimam { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.DosroChaumasikVar")]
        public string DosroChaumasikVar { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.DosroChaumasikBadjet")]
        public string DosroChaumasikBadjet { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.TesroChaumasikParimam")]
        public string TesroChaumasikParimam { get; set; }
            public string TesroChaumasikVar { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.TesroChaumasikBadjet")]
        public string TesroChaumasikBadjet { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.Expenses_category")]
        public string Expenses_category { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.FiscalYearId")]
        public string FiscalYearId { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.Type")]
        public string Type { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.ProgramType")]
        public string ProgramType { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.IsNitiTathaKaryaKram")]
        public string IsNitiTathaKaryaKram { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.IsTrainingKaryaKram")]
        public string IsTrainingKaryaKram { get; set; }



        }

    public class BudgetModel : BaseEntity
    {
        public string ExpensesCategory { get; set; }
        public string FiscalYearId { get; set; }
        public string IsNitiTathaKaryaKram { get; set; }
        public string IsTrainingKaryaKram { get; set; }
        public string ActivityName { get; set; }
        public string PLIMBIS_No { get; set; }
        public string SerialNO { get; set; }
        public string KarchaSrishak { get; set; }
        public string KharchaUpaSirshak { get; set; }
        public string SanketNO { get; set; }
        public string Xetra { get; set; }
        public string SakhaAndKaryakram { get; set; }
        public string UpaXetra { get; set; }
        public string MukhyaKaryakram { get; set; }
        public string SourceOfFund { get; set; }
        public string BudgetBiniyojanType { get; set; }
        public string PlanningProgram { get; set; }
        public string TypeOfExpen { get; set; }
        public string Yearly { get; set; }
        public string FirstQuaterBudget { get; set; }
        public string FirstQuaterQuantity { get; set; }
        public string SecondQuaterBudget { get; set; }
        public string SecondQuaterQuantity { get; set; }
        public string ThirdQuaterQuantityBudget { get; set; }
        public string ThirdQuaterQuantity { get; set; }
        public string FourthQuaterQuantityBudget { get; set; }
        public string FourthQuaterQuantity { get; set; }
        public string TypeOfExecution { get; set; }
        public string SanchalanAAbadi { get; set; }
        public string Objective { get; set; }
        public bool ISSplited { get; set; } = false;
        public string BudgetId { get; set; }
        public string OrganizationId { get; set; }
        public string OldName { get; set; }
        public string OldBiniyojanType { get; set; }
        public string WardNo { get; set; }
        public string ProjectLocation { get; set; }
        public bool IsMerged { get; set; } = false;
        public string MergedId { get; set; }
        //public Sakha Sakha { get; set; }
        public string SakhaId { get; set; }
        public bool IsAnugaman { get; set; }
    }

    
}
