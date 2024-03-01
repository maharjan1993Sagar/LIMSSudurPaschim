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
        [LIMSResourceDisplayName("Admin.Budget.ExpensesCategory")]
        public string ExpensesCategory { get; set; }
        [LIMSResourceDisplayName("Admin.Budget.FiscalYearId")]

        public string FiscalYearId { get; set; }
        [LIMSResourceDisplayName("Admin.Budget.IsNitiTathaKaryaKram")]

        public string IsNitiTathaKaryaKram { get; set; }
        [LIMSResourceDisplayName("Admin.Budget.IsTrainingKaryaKram")]

        public string IsTrainingKaryaKram { get; set; }
        [LIMSResourceDisplayName("Admin.Budget.ActivityName")]

        public string ActivityName { get; set; }
        [LIMSResourceDisplayName("Admin.Budget.PLIMBIS_No")]

        public string PLIMBIS_No { get; set; }
        [LIMSResourceDisplayName("Admin.Budget.SerialNO")]

        public string SerialNO { get; set; }
        [LIMSResourceDisplayName("Admin.Budget.KarchaSrishak")]

        public string KarchaSrishak { get; set; }
        [LIMSResourceDisplayName("Admin.Budget.KharchaUpaSirshak")]

        public string KharchaUpaSirshak { get; set; }
        [LIMSResourceDisplayName("Admin.Budget.SanketNO")]

        public string SanketNO { get; set; }
        [LIMSResourceDisplayName("Admin.Budget.Xetra")]

        public string Xetra { get; set; }
        [LIMSResourceDisplayName("Admin.Budget.SakhaAndKaryakram")]

        public string SakhaAndKaryakram { get; set; }
        [LIMSResourceDisplayName("Admin.Budget.UpaXetra")]

        public string UpaXetra { get; set; }
        [LIMSResourceDisplayName("Admin.Budget.MukhyaKaryakram")]

        public string MukhyaKaryakram { get; set; }
        [LIMSResourceDisplayName("Admin.Budget.SourceOfFund")]

        public string SourceOfFund { get; set; }
        [LIMSResourceDisplayName("Admin.Budget.BudgetBiniyojanType")]

        public string BudgetBiniyojanType { get; set; }
        [LIMSResourceDisplayName("Admin.Budget.PlanningProgram")]

        public string PlanningProgram { get; set; }
        [LIMSResourceDisplayName("Admin.Budget.TypeOfExpen")]

        public string TypeOfExpen { get; set; }
        [LIMSResourceDisplayName("Admin.Budget.Yearly")]

        public string Yearly { get; set; }
        [LIMSResourceDisplayName("Admin.Budget.FirstQuaterBudget")]

        public string FirstQuaterBudget { get; set; }
        [LIMSResourceDisplayName("Admin.Budget.FirstQuaterQuantity")]

        public string FirstQuaterQuantity { get; set; }
        [LIMSResourceDisplayName("Admin.Budget.SecondQuaterBudget")]

        public string SecondQuaterBudget { get; set; }
        [LIMSResourceDisplayName("Admin.Budget.SecondQuaterQuantity")]

        public string SecondQuaterQuantity { get; set; }
        [LIMSResourceDisplayName("Admin.Budget.ThirdQuaterQuantityBudget")]

        public string ThirdQuaterQuantityBudget { get; set; }
        [LIMSResourceDisplayName("Admin.Budget.ThirdQuaterQuantity")]

        public string ThirdQuaterQuantity { get; set; }
        [LIMSResourceDisplayName("Admin.Budget.FourthQuaterQuantityBudget")]

        public string FourthQuaterQuantityBudget { get; set; }
        [LIMSResourceDisplayName("Admin.Budget.FourthQuaterQuantity")]

        public string FourthQuaterQuantity { get; set; }
        [LIMSResourceDisplayName("Admin.Budget.TypeOfExecution")]

        public string TypeOfExecution { get; set; }
        [LIMSResourceDisplayName("Admin.Budget.SanchalanAAbadi")]

        public string SanchalanAAbadi { get; set; }
        [LIMSResourceDisplayName("Admin.Budget.Objective")]

        public string Objective { get; set; }
        [LIMSResourceDisplayName("Admin.Budget.ISSplited")]

        public bool ISSplited { get; set; } = false;
        [LIMSResourceDisplayName("Admin.Budget.BudgetId")]

        public string BudgetId { get; set; }
        [LIMSResourceDisplayName("Admin.Budget.OrganizationId")]

        public string OrganizationId { get; set; }
        [LIMSResourceDisplayName("Admin.Budget.OldName")]

        public string OldName { get; set; }
        [LIMSResourceDisplayName("Admin.Budget.OldBiniyojanType")]

        public string OldBiniyojanType { get; set; }
        [LIMSResourceDisplayName("Admin.Budget.WardNo")]

        public string WardNo { get; set; }
        [LIMSResourceDisplayName("Admin.Budget.ProjectLocation")]

        public string ProjectLocation { get; set; }
        [LIMSResourceDisplayName("Admin.Budget.IsMerged")]

        public bool IsMerged { get; set; } = false;
        [LIMSResourceDisplayName("Admin.Budget.MergedId")]

        public string MergedId { get; set; }
        [LIMSResourceDisplayName("Admin.Budget.SakhaId")]

        //public Sakha Sakha { get; set; }
        public string SakhaId { get; set; }
        [LIMSResourceDisplayName("Admin.Budget.IsAnugaman")]

        public bool IsAnugaman { get; set; }
    }

    
}
