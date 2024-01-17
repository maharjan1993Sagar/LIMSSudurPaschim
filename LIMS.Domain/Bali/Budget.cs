using LIMS.Domain.BesicSetup;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Bali
{
   public class Budget:BaseEntity
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public FiscalYear FiscalYear { get; set; }
        public string FiscalYearId { get; set; }
        public string Remarks { get; set; }
        public string ExpensesCategory { get; set; }
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
