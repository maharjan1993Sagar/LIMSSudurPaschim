using LIMS.Domain.BesicSetup;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Bali
{
   public class AanudanKokaryakram:BaseEntity
    {
        public PujigatKharchaKharakram PujigatKharchaKharakram { get; set; }
        public string PujigatKharchaKaryakramId { get; set; }
        public Budget Budget { get; set; }
        public string BudgetId { get; set; }
        public FiscalYear FiscalYear { get; set; }
        public string FiscalyearId { get; set; }
        public string KrishakKoName { get; set; }
        public string PhoneNo { get; set; }
        public string Sex { get; set; }
        public string EthinicGroup { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string LocalLevel { get; set; }
        public string Ward { get; set; }
        public string Tole { get; set; }
        public string AanudanKokisim { get; set; }
        public decimal AanudanRakam { get; set; }
        public string Remaks { get; set; }
        public DateTime DateOfSubsidy { get; set; }
        public int? MaleMember { get; set; }
        public int? FemaleMember { get; set; }
        public int? DalitMember { get; set; }
        public int? JanajatiMember { get; set; }
        public int? Others { get; set; }
        public string ExpectedOutput { get; set; }
        public decimal FarmerContribution { get; set; }
        public string SubsidyCategory { get; set; }
        public string NameOfCategory { get; set; }
        public string ProposeOfSubsidy { get; set; }
        public string Area { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedAt { get; set; }
        public string AnudanReceiverType { get; set; }
    }
}
