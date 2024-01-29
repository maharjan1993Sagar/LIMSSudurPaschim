using LIMS.Domain.BasicSetup;
using LIMS.Domain.BesicSetup;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Bali
{
    public class InputSupply:BaseEntity
    {
        public Category Category { get; set; }
        public string CategoryId { get; set; }
        public string Type { get; set; } //Training, Subsidy
        public FiscalYear FiscalYear { get; set; }
        public string FiscalYearId { get; set; }
        public Budget Budget { get; set; }
        public string BudgetId { get; set; }
        public string PhoneNumber { get; set; }
        public string NepaliDate { get; set; }
        public DateTime EnglishDate { get; set; }
        public string Month { get; set; }
        public string Purpose { get; set; }
        public string Quantity { get; set; }
        public string UnitId { get; set; }
        public Unit Unit { get; set; }
        public Decimal Price { get; set; }
        public string ProvidedBy { get; set; }
        public string ReceivedBy { get; set; }
        public string Bibaran { get; set; }
        public string LocalLevel { get; set; }
        public string WardNo { get; set; }
        public string District { get; set; }
        public string Province { get; set; }
        public string VerifiedBy { get; set; }
        public string Remarks { get; set; }
        public string EntityId { get; set; }  // 
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
